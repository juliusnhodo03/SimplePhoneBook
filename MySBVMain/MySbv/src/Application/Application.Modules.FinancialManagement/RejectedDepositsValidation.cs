using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using Application.Dto.FinancialManagement;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Utility.Core;

namespace Application.Modules.FinancialManagement
{
    public class RejectedDepositsValidation : IRejectedDepositsValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public RejectedDepositsValidation(IRepository repository, ILookup lookup)
        {
            _repository = repository;
            _lookup = lookup;
        }

        #endregion

        #region IRejected Deposits

        /// <summary>
        ///     Get all rejected transactions.
        ///     NOTE: These are Payments, Multi deposits, and Single & Multidrop deposits
        /// </summary>
        /// <returns></returns>
        public MethodResult<IEnumerable<RejectedDepositDto>> All()
        {            
            int rejectedStatusId = _lookup.GetStatusId("SETTLEMENT_REJECTED");

            var transactions = new List<RejectedDepositDto>();

            // get rejected cash deposits
            var deposits = GetRejectedCashDeposits(rejectedStatusId);
            var collection = deposits as RejectedDepositDto[] ?? deposits.ToArray();
            transactions.AddRange(collection);

            // get rejected Multi Deposits
            var multiDeposits = GetRejectedMultiDeposits(rejectedStatusId);
            transactions.AddRange(multiDeposits);

            // get rejected payments
            var payments = GetRejectedPayments(rejectedStatusId);
            transactions.AddRange(payments);

            foreach (var transaction in transactions)
            {
                transaction.Reason = transaction.RejectionReason.Length > 20
                    ? transaction.RejectionReason.Substring(0, 20)
                    : transaction.RejectionReason;
            }

            return new MethodResult<IEnumerable<RejectedDepositDto>>(MethodStatus.Successful, transactions);
        }



        /// <summary>
        /// Find rejected Transaction by its ID
        /// </summary>
        /// <param name="cashDepositId"></param>
        /// <param name="containerDropId"></param>
        /// <param name="paymentId"></param>
        public MethodResult<RejectedDepositDto> FindRejectedRecord(int cashDepositId, int containerDropId, int paymentId)
        {
            int rejectedStatusId = _lookup.GetStatusId("SETTLEMENT_REJECTED");

            if (cashDepositId > 0)
            {
                var cashDeposit = GetRejectedCashDeposits(rejectedStatusId)
                    .FirstOrDefault(e => e.CashDepositId == cashDepositId);

                return cashDeposit == null
                    ? new MethodResult<RejectedDepositDto>(MethodStatus.Error, null, "Rejected Deposit Not Found.")
                    : new MethodResult<RejectedDepositDto>(MethodStatus.Successful, cashDeposit);
            }

            if (containerDropId > 0)
            {
                var multiDeposit = GetRejectedMultiDeposits(rejectedStatusId)
                    .FirstOrDefault(e => e.ContainerDropId == containerDropId);

                return multiDeposit == null
                    ? new MethodResult<RejectedDepositDto>(MethodStatus.Error, null, "Rejected Multi Deposit Not Found.")
                    : new MethodResult<RejectedDepositDto>(MethodStatus.Successful, multiDeposit);
            }

            if (paymentId > 0)
            {
                var payment = GetRejectedPayments(rejectedStatusId)
                    .FirstOrDefault(e => e.PaymentId == paymentId);

                return payment == null
                    ? new MethodResult<RejectedDepositDto>(MethodStatus.Error, null,
                        "Rejected Payment Not Found.")
                    : new MethodResult<RejectedDepositDto>(MethodStatus.Successful, payment);
            }
            return new MethodResult<RejectedDepositDto>(MethodStatus.Error, null, "Rejected Deposit Not Found.");
        }


        /// <summary>
        ///     Resubmit a deposit or payment.
        /// </summary>
        /// <param name="cashDepositId"></param>
        /// <param name="paymentId"></param>
        /// <param name="userId"></param>
        /// <param name="containerDropId"></param>
        public MethodResult<bool> Submit(int cashDepositId, int containerDropId, int paymentId, int userId)
        {
            int resubmittedStatusId = _lookup.GetStatusId("RESUBMITTED");

            if (containerDropId > 0)
            {
                var multiDeposit =
                    _repository.Query<ContainerDrop>(a => a.ContainerDropId == containerDropId).FirstOrDefault();
                if (multiDeposit != null)
                {
                    multiDeposit.StatusId = resubmittedStatusId;
                    multiDeposit.LastChangedById = userId;
                    multiDeposit.LastChangedDate = DateTime.Now;
                    multiDeposit.EntityState = State.Modified;
                    _repository.Update(multiDeposit);
                    return new MethodResult<bool>(MethodStatus.Successful, true,
                        "Deposit was successfully resubmitted for settlement.");
                }
            }
            else if (cashDepositId > 0)
            {
                var cashDeposit = _repository.Query<CashDeposit>(c => c.CashDepositId == cashDepositId).FirstOrDefault();
                if (cashDeposit != null)
                {
                    cashDeposit.StatusId = resubmittedStatusId;
                    cashDeposit.LastChangedDate = DateTime.Now;
                    cashDeposit.LastChangedById = userId;
                    cashDeposit.EntityState = State.Modified;
                    _repository.Update(cashDeposit);
                    return new MethodResult<bool>(MethodStatus.Successful, true,
                        "Deposit was successfully resubmitted for settlement.");
                }
            }
            else
            {
                var payment =
                    _repository.Query<VaultPartialPayment>(c => c.VaultPartialPaymentId == paymentId).FirstOrDefault();
                if (payment != null)
                {
                    payment.StatusId = resubmittedStatusId;
                    payment.LastChangedDate = DateTime.Now;
                    payment.LastChangedById = userId;
                    payment.EntityState = State.Modified;
                    _repository.Update(payment);
                    return new MethodResult<bool>(MethodStatus.Successful, true,
                        "Payment was successfully resubmitted for settlement.");
                }
            }
            return new MethodResult<bool>(MethodStatus.Error, true, "Failed to resubmit the transaction!");
        }


        /// <summary>
        ///     Get NEDBABNK rejection reason.
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="errorCode"></param>
        /// <param name="settlementIdentifier"></param>
        private string GetReasonForRejection(Bank bank, ErrorCode errorCode, string settlementIdentifier)
        {
            switch (bank.LookUpKey)
            {
                case "NEDBANK":
                    return GetNedbankReason(settlementIdentifier);
                default:
                    var description = errorCode != null ? errorCode.Description : "Not Found";
                    return description;
            }
        }

        /// <summary>
        ///     Get NEDBABNK rejection reason.
        /// </summary>
        /// <param name="bank"></param>
        /// <param name="errorCode"></param>
        /// <param name="settlementIdentifier"></param>
        private string GetRejectionCode(Bank bank, ErrorCode errorCode, string settlementIdentifier) 
        {
            switch (bank.LookUpKey)
            {
                case "NEDBANK":
                    return GetNedbankErrorCode(settlementIdentifier);
                default:
                    var error = errorCode != null ? errorCode.Code : "Not Found";
                    return error;
            }
        }


        /// <summary>
        /// Get Nedbank error code.
        /// </summary>
        /// <param name="settlementIdentifier"></param>
        private string GetNedbankErrorCode(string settlementIdentifier)
        {
            NedbankResponseDetail responseDetail =
                (from item in _repository.Query<NedbankFileItem>(e => e.SettlementIdentifier == settlementIdentifier)
                 join response in _repository.All<NedbankResponseDetail>() on item.PaymentReferenceNumber equals
                     response.PaymentReferenceNumber
                 select response).FirstOrDefault();

            if (responseDetail != null)
            {
                return "Not Specified";
            }

            NedbankUnpaidOrNaedo unpaid =
                (from item in _repository.Query<NedbankFileItem>(e => e.SettlementIdentifier == settlementIdentifier)
                 join response in _repository.All<NedbankUnpaidOrNaedo>() on item.PaymentReferenceNumber equals
                     response.PaymentReferenceNumber
                 select response).FirstOrDefault();

            return (unpaid != null) ? unpaid.ReasonCode : "Not Found";
        }


        /// <summary>
        ///     get Nedbank rejection reason
        /// </summary>
        /// <param name="settlementIdentifier"></param>
        private string GetNedbankReason(string settlementIdentifier)
        {
            NedbankResponseDetail responseDetail =
                (from item in _repository.Query<NedbankFileItem>(e => e.SettlementIdentifier == settlementIdentifier)
                    join response in _repository.All<NedbankResponseDetail>() on item.PaymentReferenceNumber equals
                        response.PaymentReferenceNumber
                    select response).FirstOrDefault();

            if (responseDetail != null)
            {
                return responseDetail.Reason;
            }

            NedbankUnpaidOrNaedo unpaid =
                (from item in _repository.Query<NedbankFileItem>(e => e.SettlementIdentifier == settlementIdentifier)
                    join response in _repository.All<NedbankUnpaidOrNaedo>() on item.PaymentReferenceNumber equals
                        response.PaymentReferenceNumber
                    select response).FirstOrDefault();

            return (unpaid != null) ? unpaid.Reason : "Not Found";
        }


        /// <summary>
        ///     Get cash deposits with settlement rejected
        /// </summary>
        /// <param name="rejectedStatusId"></param>
        private IEnumerable<RejectedDepositDto> GetRejectedMultiDeposits(int rejectedStatusId)
        {
            var transactions = new List<RejectedDepositDto>();
            try
            {
                var drops = _repository.Query<ContainerDrop>(a => a.StatusId == rejectedStatusId,
                    o => o.Container,
                    o => o.Container.CashDeposit,
                    o => o.Container.CashDeposit.Account,
                    o => o.Container.CashDeposit.Account.AccountType,
                    o => o.Container.CashDeposit.Account.Bank,
                    o => o.Container.CashDeposit.Site,
                    o => o.Container.CashDeposit.Site.Merchant,
                    o => o.ErrorCode);

                foreach (var containerDrop in drops)
                {
                    var deposit = containerDrop.Container.CashDeposit;

                    var drop = new RejectedDepositDto
                    {
                        ContainerDropId = containerDrop.ContainerDropId,
                        IsContainerDrop = true,
                        CashDepositId = deposit.CashDepositId,
                        AccountNumber = deposit.Account.AccountNumber,
                        BranchCode = deposit.Account.Bank.BranchCode,
                        AccountType = deposit.Account.AccountType.Name,
                        Narrative = containerDrop.Narrative,
                        SbvReference = containerDrop.ReferenceNumber,
                        ErrorCode = GetRejectionCode(deposit.Account.Bank, containerDrop.ErrorCode,
                            containerDrop.SettlementIdentifier),
                        BankName = deposit.Account.Bank.Name,
                        CitCode = deposit.Site.CitCode,
                        DepositAmount = (containerDrop.DiscrepancyAmount == Convert.ToDecimal(0.00)
                            ? containerDrop.Amount
                            : containerDrop.DiscrepancyAmount).ToString(CultureInfo.InvariantCulture),
                        DepositDateTime = deposit.CreateDate.Value.ToString(CultureInfo.InvariantCulture),
                        DepositReference = containerDrop.ReferenceNumber,
                        MerchantName = deposit.Site.Merchant.Name,
                        RejectionDateTime = containerDrop.SettlementDateTime.ToString(),
                        RejectionReason = GetReasonForRejection(deposit.Account.Bank, containerDrop.ErrorCode,
                                containerDrop.SettlementIdentifier),
                        SiteName = deposit.Site.Name,
                        DepositType = "Multi Deposit"
                    };
                    transactions.Add(drop);
                }

                return transactions;
            }
            catch (DbEntityValidationException ex)
            {
                return transactions;  
            }
            catch (Exception ex)
            {
                return transactions;
            }
        }


        /// <summary>
        ///     Get cash deposits with settlement rejected
        /// </summary>
        /// <param name="rejectedStatusId"></param>
        private IEnumerable<RejectedDepositDto> GetRejectedCashDeposits(int rejectedStatusId)
        {
            var transactions = new List<RejectedDepositDto>();

            // get rejected deposits
            IEnumerable<CashDeposit> deposits = _repository.Query<CashDeposit>(a => a.StatusId == rejectedStatusId,
                o => o.Account,
                o => o.Account.AccountType,
                o => o.Account.Bank,
                o => o.Site,
                o => o.Site.Merchant,
                o => o.ErrorCode,
                o => o.DepositType,
                o => o.VaultBeneficiaries,
                o => o.VaultBeneficiaries.Select(e => e.Account),
                o => o.VaultBeneficiaries.Select(e => e.Account.Bank));

            foreach (CashDeposit cashDeposit in deposits)
            {
                var account = (cashDeposit.Account != null)
                    ? cashDeposit.Account
                    : cashDeposit.VaultBeneficiaries.FirstOrDefault().Account;

                var transaction = new RejectedDepositDto
                {                    
                    CashDepositId = cashDeposit.CashDepositId,
                    IsCashDeposit = true,
                    AccountNumber = account.AccountNumber,
                    BranchCode = account.Bank.BranchCode,
                    AccountType = account.AccountType.Name,
                    Narrative = cashDeposit.Narrative,
                    SbvReference = cashDeposit.TransactionReference,
                    ErrorCode = GetRejectionCode(account.Bank, cashDeposit.ErrorCode, cashDeposit.SettlementIdentifier),
                    BankName = account.Bank.Name,
                    CitCode = cashDeposit.Site.CitCode,
                    DepositAmount = cashDeposit.DepositedAmount.ToString(CultureInfo.InvariantCulture),
                    DepositDateTime = cashDeposit.CreateDate.Value.ToString(CultureInfo.InvariantCulture),
                    DepositReference = cashDeposit.TransactionReference,
                    MerchantName = cashDeposit.Site.Merchant.Name,
                    RejectionDateTime = cashDeposit.SettledDateTime.ToString(),
                    RejectionReason = GetReasonForRejection(account.Bank, cashDeposit.ErrorCode, cashDeposit.SettlementIdentifier),
                    SiteName = cashDeposit.Site.Name,
                    DepositType = cashDeposit.DepositType.Name
                };
                transactions.Add(transaction);
            }
            return transactions;
        }


        /// <summary>
        ///     Get rejected Payments
        /// </summary>
        /// <param name="rejectedStatusId"></param>
        /// <returns></returns>
        private IEnumerable<RejectedDepositDto> GetRejectedPayments(int rejectedStatusId)
        {
            try
            {
                var transactions = new List<RejectedDepositDto>();

                var payments = (
                    from payment in _repository.Query<VaultPartialPayment>(e => e.StatusId == rejectedStatusId)
                    join account in
                        _repository.All<Account>(e => e.Bank,
                            e => e.AccountType,
                            e => e.Site,
                            e => e.Site.Merchant)
                        on payment.BeneficiaryCode equals account.BeneficiaryCode 
                        join container in _repository.All<Container>(o => o.CashDeposit, o => o.CashDeposit.DepositType) 
                    on payment.BagSerialNumber equals container.SerialNumber
                    select new RejectedDepositDto
                    {
                        PaymentId = payment.VaultPartialPaymentId,
                        IsPayment = true,
                        AccountNumber = account.AccountNumber,
                        BranchCode = account.Bank.BranchCode,                
                        AccountType = account.AccountType.Name,
                        Narrative = payment.PaymentReference,
                        SbvReference = container.CashDeposit.TransactionReference,
                        BankName = account.Bank.Name,
                        CitCode = account.Site.CitCode,
                        DepositAmount = payment.TotalToBePaid.ToString(CultureInfo.InvariantCulture),
                        DepositDateTime = payment.CreateDate.Value.ToString(CultureInfo.InvariantCulture),
                        DepositReference = payment.PaymentReference,
                        MerchantName = account.Site.Merchant.Name,
                        RejectionDateTime = payment.SettlementDate.ToString(),
                        ErrorCode = GetRejectionCode(account.Bank, payment.ErrorCode, payment.SettlementIdentifier),
                        RejectionReason = GetReasonForRejection(account.Bank, payment.ErrorCode, payment.SettlementIdentifier),
                        SiteName = account.Site.Name,
                        DepositType = container.CashDeposit.DepositType.Name
                    });

                transactions.AddRange(payments);
                return transactions;
            }
            catch (DbEntityValidationException ex)
            {
                this.Log().Fatal(ex);
                return new List<RejectedDepositDto>();
            }
            catch (Exception ex)
            {
                this.Log().Fatal(ex);
                return new List<RejectedDepositDto>();
            }
        }

        #endregion
    }
}