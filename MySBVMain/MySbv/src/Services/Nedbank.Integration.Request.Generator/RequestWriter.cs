using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Nedbank.Integration.FileUtilities;

namespace Nedbank.Integration.Request.Generator
{
    [Export(typeof(IRequestWriter))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class RequestWriter : IRequestWriter 
    {
        #region Fields
        private readonly IEnumerable<Status> _statuses;
        private readonly List<CashDeposit> _unsettledDeposits;
        private readonly List<VaultPartialPayment> _unsettledVaultPartialPayments;
        private readonly IEnumerable<NedbankClientType> _clientTypes;
        private readonly IEnumerable<NedbankServiceType> _serviceTypes;
        private IEnumerable<Site> _sites;
        private readonly IEnumerable<NedbankTransactionType> _transactionTypes;
        private readonly NedbankClientProfile _clientProfile;
        private IRepository _repository { get; set; }
        private IFileUtility _fileUtility { get; set; }
        public string cycleDate { get; set; }
        #endregion


        #region Constructor

        /// <summary>
        ///     Request Data Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="fileUtility"></param>
        [ImportingConstructor]
        public RequestWriter(IRepository repository, IFileUtility fileUtility) 
        {
            _repository = repository;
            _fileUtility = fileUtility;
            cycleDate = _fileUtility.GetDate(Format.YYMMDD);
            _statuses = _repository.All<Status>();
            _unsettledDeposits = new List<CashDeposit>();
            _unsettledVaultPartialPayments = new List<VaultPartialPayment>();

            _clientTypes = _repository.All<NedbankClientType>();
            _serviceTypes = _repository.All<NedbankServiceType>();
            _transactionTypes = _repository.All<NedbankTransactionType>();
            _clientProfile = _repository.Query<NedbankClientProfile>(e => e.LookupKey == "CLIENT_PROFILE_NUMBER").FirstOrDefault();
            _sites = _repository.All<Site>().ToList();
        }

        #endregion


        #region IRequestWriter Methods

        /// <summary>
        /// Get Deposits Awaiting settlement.
        /// This happens:
        /// 1.  When deposits captured on web are processed at a cash center.
        /// 2.  When a Payment message has been send from a device.
        /// 3.  When CIT message from device is send to close a bag.
        /// </summary>
        /// <param name="batch"></param>
        public void CollectRecordsToSettle(ref NedbankBatchFile batch)
        {
            try
            {
                var settlementRecords = new List<NedbankFileItem>();

                // get all mySBV unsettled deposits
                IEnumerable<NedbankFileItem> webDeposits = GetWebDeposits();

                if (webDeposits != null)
                {
                    settlementRecords.AddRange(webDeposits);
                }

                // Deposits Marked For Resubmission
                IEnumerable<NedbankFileItem> resubmittedDeposits = GetResubmissions();
                if (resubmittedDeposits != null)
                {
                    settlementRecords.AddRange(resubmittedDeposits);
                }

                // Filter Out Settled Multi-Deposits
                IEnumerable<NedbankFileItem> multiDeposits = GetMultiDeposits();
                if (multiDeposits != null)
                {
                    settlementRecords.AddRange(multiDeposits);
                }

                // Vault Settlement
                IEnumerable<NedbankFileItem> vaultDeposits = GetVaultDeposits();

                if (vaultDeposits != null)
                {
                    settlementRecords.AddRange(vaultDeposits);
                }

                //Calculate batch total
                batch.BatchTotal = CalculateBatchTotal(settlementRecords);

                var counter = 1;

                foreach (var record in settlementRecords)
                {
                    string sequentialNumber = counter.ToString().PadLeft(10, '0');
                    record.PaymentReferenceNumber = batch.NedbankHeaderRecord.FileSequenceNumber + sequentialNumber;
                    record.ActionDate = batch.BatchDate;
                    record.NominatedAccountNumber = batch.NedbankHeaderRecord.NominatedAccountNumber;
                    record.ChargesAccountNumber = batch.NedbankHeaderRecord.ChargesAccountNumber;
                    record.Amount = record.Amount.Replace(".", "").PadLeft(12, '0');
                    
                    counter++;
                }

                // add transactions to batch
                AddTransactionsToBatch(ref batch, settlementRecords);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET SETTLEMENT RECORDS]", ex);
                throw;
            }
        }


        /// <summary>
        /// This runs after an instruction file has been droped to Connect Direct.
        /// Update Deposits send to Nedbank as "Pending Settlement".
        /// </summary>
        public void UpdateSettlementStatus()
        {
            try
            {
                SetCashDepositsAsPendingSettlement();
                SetPaymentsAsPendingSettlement();
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE SETTLEMENT STATUS]", ex);
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="transactions"></param>
        /// <returns></returns>
        private void AddTransactionsToBatch(ref NedbankBatchFile batch, IEnumerable<NedbankFileItem> transactions)
        {
            foreach (var transaction in transactions)
            { 
                try
                {
                    transaction.CreatedById = 1;
                    transaction.CreateDate = DateTime.Now;
                    transaction.LastChangedById = 1;
                    transaction.LastChangedDate = DateTime.Now;
                    transaction.IsNotDeleted = true;
                    transaction.EntityState = State.Added;

                    var isAmountGreaterThanZero = Convert.ToDouble(transaction.Amount) > 0;

                    if (isAmountGreaterThanZero)
                    {
                        // add transactions to batch
                        batch.NedbankFileItems.Add(transaction);
                    }
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("Exception On Method : [ADD TRANSACTION TO BATCH]", ex);
                }
            }
        }



        /// <summary>
        /// Calculate Batch Total
        /// </summary>
        /// <param name="settlementRecords"></param>
        /// <returns></returns>
        private string CalculateBatchTotal(IEnumerable<NedbankFileItem> settlementRecords)
        {
            var total = settlementRecords.Sum(e => Convert.ToDecimal(e.Amount));
            return total.ToString().Replace(".", "").PadLeft(12, '0');
        }

        #endregion


        #region Set Status as Pending Settlement

        /// <summary>
        ///     Mark unsettle cash deposits as settled
        /// </summary>
        private void SetCashDepositsAsPendingSettlement()
        {
            if (_unsettledDeposits.Count <= 0) return;

            string query =
                "UPDATE CashDeposit SET LastChangedDate = '" + DateTime.Now +
                "', StatusId = (SELECT StatusId FROM Status WHERE LookUpKey = 'PENDING_SETTLEMENT'), SendDateTime = '" +
                DateTime.Now + "' WHERE CashDepositId IN ";

            string dropQuery =
                "UPDATE ContainerDrop SET LastChangedDate = '" + DateTime.Now +
                "',  StatusId = (SELECT StatusId FROM Status WHERE LookUpKey = 'PENDING_SETTLEMENT'), SendDateTime = '" +
                DateTime.Now + "' WHERE ContainerId IN (SELECT ContainerId FROM CONTAINER WHERE ContainerId IN ";

            const string filter = " AND StatusId <> (SELECT StatusId FROM Status WHERE LookUpKey = 'SETTLED')" +
                                  " AND StatusId <> (SELECT StatusId FROM Status WHERE LookUpKey = 'ACTIVE')" +
                                  " AND StatusId <> (SELECT StatusId FROM Status WHERE LookUpKey = 'SUBMITTED')" +
                                  " AND StatusId <> (SELECT StatusId FROM Status WHERE LookUpKey = 'SETTLEMENT_REJECTED')";

            string depositWhereClause = "(";
            string containerWhereClause = "(";

            for (int i = 0; i < _unsettledDeposits.Count; i++)
            {
                if (i == 0)
                    depositWhereClause += _unsettledDeposits[i].CashDepositId;
                else depositWhereClause += ", " + _unsettledDeposits[i].CashDepositId;
            }
            depositWhereClause += ")";

            for (int i = 0; i < _unsettledDeposits.Count; i++)
            {
                foreach (Container container in _unsettledDeposits[i].Containers)
                {
                    foreach (ContainerDrop containerDrop in container.ContainerDrops)
                    {
                        if (i == 0)
                        {
                            containerWhereClause += containerDrop.ContainerDropId;
                        }
                        else containerWhereClause += ", " + containerDrop.ContainerDropId;
                    }
                }
            }
            containerWhereClause += ")";

            string fullQuery = query + depositWhereClause + filter;
            string fullDropQuery = dropQuery + containerWhereClause + ")" + filter;

            _repository.ExecuteQuery(fullQuery);
            _repository.ExecuteQuery(fullDropQuery);
        }


        /// <summary>
        ///  This sets Payments as pending settlement.
        /// </summary>
        private void SetPaymentsAsPendingSettlement()
        {
            if (_unsettledVaultPartialPayments.Count <= 0) return;

            string query =
                "UPDATE VaultPartialPayment SET LastChangedDate = '" + DateTime.Now +
                "', StatusId = (SELECT StatusId FROM Status WHERE LookUpKey = 'PENDING_SETTLEMENT'), SendDateTime = '" +
                DateTime.Now + "' WHERE VaultPartialPaymentId IN ";

            string whereClause = "(";
            for (int i = 0; i < _unsettledVaultPartialPayments.Count; i++)
            {
                if (i == 0)
                    whereClause += _unsettledVaultPartialPayments[i].VaultPartialPaymentId;
                whereClause += ", " + _unsettledVaultPartialPayments[i].VaultPartialPaymentId;
            }
            whereClause += ")";
            string fullQuery = query + whereClause;
            _repository.ExecuteQuery(fullQuery);
        }

        #endregion
        

        #region Web Deposits

        /// <summary>
        ///     Get All processed unsettled mysbv cash deposits
        /// </summary>
        private IEnumerable<NedbankFileItem> GetWebDeposits() 
        {
            try
            {
                var nedbankAccountIds = GetNedbankAccountIds(); 

                // Get Confirmed Status
                var status = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED");
                var productType = _repository.Query<ProductType>(a => a.LookUpKey == "MYSBV_DEPOSIT").FirstOrDefault();

                // Get Processed NEDBANK deposits.
                // These deposits were captured on the Web.
                // The status of CONFIRMED.
                var cashDeposits = _repository.Query<CashDeposit>(
                                    a => a.IsProcessed.HasValue && a.IsProcessed.Value && 
                                    a.StatusId == status.StatusId && 
                                    a.ProductTypeId == productType.ProductTypeId &&
                                    nedbankAccountIds.Contains(a.AccountId.Value),
                                    deposit => deposit.Containers,
                                    containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops),
                                    containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops.Select(e => e.ContainerDropItems)),
                                    containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops.Select(e => e.Status))
                                    ).ToList();


                foreach (CashDeposit deposit in cashDeposits)
                {
                    _unsettledDeposits.Add(deposit);
                }

                return SeperateDeposits(cashDeposits);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET UNSETTLED WEB DEPOSITS]", ex);
                return null;
            }
        }



        /// <summary>
        ///     Get all mysbv cash deposits marked for resubmission
        /// </summary>
        private IEnumerable<NedbankFileItem> GetResubmissions()
        {
            try
            {
                // NEDBANK Accounts
                var nedbankAccountIds = GetNedbankAccountIds();  

                // Get RESUBMITTED Status
                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED");

                // Retrieve all single and multi-drop deposits that have been marked 
                // for resubmission.
                var cashDeposits = _repository.Query<CashDeposit>(
                                                a => a.SettledDateTime.HasValue &&
                                                a.StatusId == status.StatusId &&
                                                nedbankAccountIds.Contains(a.AccountId.Value),
                                    container => container.Containers,
                                    containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops),
                                    containerDropItems => containerDropItems.Containers.Select(c => c.ContainerDrops.Select(b => b.ContainerDropItems)))
                                .ToList();

                _unsettledDeposits.AddRange(cashDeposits);

                return SeperateDeposits(cashDeposits);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET ALL RESUBMISSIONS]", ex);
                return null;
            }
        }



        /// <summary>
        ///     Filter out settled multi deposits
        /// </summary>
        /// <returns></returns>
        private IEnumerable<NedbankFileItem> GetMultiDeposits()
        {
            try
            {
                // NEDBANK Accounts
                var nedbankAccountIds = GetNedbankAccountIds(); 

                // Get RESUBMITTED Status
                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED");

                // All required information for multi deposit is sitting on container drop table. 
                // retrieve all deposits that have been marked with resubmission but exclude all the other
                // multi deposits that have not been market with resubmit but belonging to the same cash deposit.
                var containerDrops = _repository.Query<ContainerDrop>(
                    a => a.SettlementDateTime.HasValue && a.StatusId == status.StatusId && 
                        nedbankAccountIds.Contains(a.Container.CashDeposit.AccountId.Value),
                    c => c.Container.CashDeposit,
                    c => c.Container.CashDeposit.Containers,
                    c => c.Container.CashDeposit.Containers.Select(b => b.ContainerDrops),
                    c =>
                        c.Container.CashDeposit.Containers.Select(
                            b => b.ContainerDrops.Select(v => v.ContainerDropItems)))
                    .ToList();

                foreach (ContainerDrop containerDrop in containerDrops)
                {
                    if (!_unsettledDeposits.Contains(containerDrop.Container.CashDeposit))
                        _unsettledDeposits.Add(containerDrop.Container.CashDeposit);
                }

                IEnumerable<CashDeposit> cashDeposits =
                    containerDrops.Select(containerDrop => containerDrop.Container.CashDeposit);
                return SeperateDeposits(cashDeposits);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [FILTER OUT SETTLED MULTI DEPOSITS]", ex);
                return null;
            }
        }



        /// <summary>
        ///     Separate deposits in to single, multi deposits
        /// </summary>
        /// <param name="cashDeposits"></param>
        private IEnumerable<NedbankFileItem> SeperateDeposits(IEnumerable<CashDeposit> cashDeposits)
        {
            var simpleDeposits = new List<CashDeposit>();
            var complexDeposits = new List<CashDeposit>();

            foreach (CashDeposit cashDeposit in cashDeposits)
            {
                switch (cashDeposit.DepositTypeId)
                {
                        // Copy all Single and Multi Drop deposits in to simple Deposits list
                    case 1:
                    case 3:
                        simpleDeposits.Add(cashDeposit);
                        break;
                        // Copy all Multi Deposits in to Complex Deposits list
                    case 2:
                        complexDeposits.Add(cashDeposit);
                        break;
                }
            }

            simpleDeposits = simpleDeposits.Select(a => a.RemoveDeleted()).ToList();
            complexDeposits = complexDeposits.Select(a => a.RemoveDeleted()).ToList();
            return GenerateSettlementFileList(simpleDeposits, complexDeposits);
        }


        /// <summary>
        ///     Generate settlement transaction file collection for supplied cash deposits
        /// </summary>
        /// <param name="simpleDeposits"></param>
        /// <param name="complexDeposits"></param>
        private IEnumerable<NedbankFileItem> GenerateSettlementFileList(IEnumerable<CashDeposit> simpleDeposits, IEnumerable<CashDeposit> complexDeposits)
        {
            var list = new List<NedbankFileItem>();

            var settlementFiles = GenerateFromSimpleFiles(simpleDeposits);
            var settlementFiles1 = GenerateFromComplexDeposit(complexDeposits);

            list.AddRange(settlementFiles);
            list.AddRange(settlementFiles1);

            return list;
        }



        /// <summary>
        ///     Generate settlement transaction file collection for multi deposits
        /// </summary>
        /// <param name="complexDeposits"></param>
        private IEnumerable<NedbankFileItem> GenerateFromComplexDeposit(IEnumerable<CashDeposit> complexDeposits)
        {
            try
            {
                var settlementFileList = new List<NedbankFileItem>();

                int resubmittedStatusId = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED").StatusId;
                int statusId = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED").StatusId;

                foreach (CashDeposit complexDeposit in complexDeposits)
                {
                    foreach (Container container in complexDeposit.Containers)
                    {
                        foreach (ContainerDrop drop in container.ContainerDrops)
                        {
                            if (drop.StatusId == statusId || drop.StatusId == resubmittedStatusId)
                            {
                                var settlement = GetSettlementDetails(complexDeposit, drop, null);
                                settlementFileList.Add(settlement);
                            }
                        }
                    }
                }
                
                return settlementFileList;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GENERATE FROM COMPLEX DEPOSITS]", ex);
                throw;
            }
        }



        /// <summary>
        ///     Generate settlement transaction file for single and multi drop deposits.
        /// </summary>
        /// <param name="simpleDeposits"></param>
        /// <returns></returns>
        private IEnumerable<NedbankFileItem> GenerateFromSimpleFiles(IEnumerable<CashDeposit> simpleDeposits)
        {
            try
            {
                return (from cashDeposit in simpleDeposits
                    let siteInfo = _sites.FirstOrDefault(a => a.SiteId == cashDeposit.SiteId)
                    join siteSettlementAccount in _repository.Query<Account>(a => a.IsNotDeleted, b => b.Bank, t => t.TransactionType,
                            tt => tt.AccountType) on cashDeposit.AccountId equals siteSettlementAccount.AccountId
                        where siteInfo != null
                    select GetSettlementDetails(cashDeposit, null, null)).ToList();
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GENERATE FROM SIMPLE DEPOSITS]", ex);
                throw;
            }
        }

        #endregion
        
        
        #region Vault Deposits

        /// <summary>
        ///     Get All unsettled vault cash deposits
        /// </summary>
        /// <returns></returns>
        private IEnumerable<NedbankFileItem> GetVaultDeposits()
        {
            try
            {
                var settlements = new List<NedbankFileItem>();

                //Vault Payments Deposits 
                var vaultPaymentsDeposits = GetVaultPayments();

                if (vaultPaymentsDeposits != null)
                {
                    settlements.AddRange(vaultPaymentsDeposits);
                }

                // Vault Cit Payment Deposit
                IEnumerable<NedbankFileItem> vaultCitPaymentDeposit = GetVaultCitPaymentDeposit();

                if (vaultCitPaymentDeposit != null)
                {
                    settlements.AddRange(vaultCitPaymentDeposit);
                }
                return settlements;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [VAULT SETTLEMENT]", ex);
                return null;
            }
        }


        /// <summary>
        ///     Get all unsettled vault cash deposits based on the CIT message
        /// </summary>
        /// <returns></returns>
        private IEnumerable<NedbankFileItem> GetVaultCitPaymentDeposit()
        {
            try
            {
                this.Log().Info(string.Format("Vault Cit Deposits method..."));

                var settlementRecords = new List<NedbankFileItem>();

                Status payUnConfirmedStatus = _statuses.FirstOrDefault(a => a.LookUpKey == "PAY_UNCONFIRMED");

                // NEDBANK Accounts
                var nedbankAccountIds = GetNedbankAccountIds();

                // Get all MYSBV_VAULT Deposits that have not been PROCESSED, SETTLED and NOT DELETED
                List<CashDeposit> payments =
                    _repository.Query<CashDeposit>(a => a.StatusId == payUnConfirmedStatus.StatusId && 
                        nedbankAccountIds.Contains(a.AccountId.Value),
                        a => a.Account,
                        a => a.Account.Bank,
                        a => a.Account.TransactionType,
                        a => a.Account.AccountType,
                        a => a.Containers,
                        a => a.Containers.Select(b => b.ContainerDrops),
                        a => a.Containers.Select(b => b.ContainerDrops.Select(e => e.Status)),
                        a => a.VaultBeneficiaries,
                        a => a.VaultBeneficiaries.Select(b => b.Account),
                        a => a.VaultBeneficiaries.Select(b => b.Account.Bank),
                        a => a.VaultBeneficiaries.Select(b => b.Account.TransactionType),
                        a => a.VaultBeneficiaries.Select(b => b.Account.AccountType),
                        a => a.VaultBeneficiaries.Select(b => b.CashDeposit),
                        a => a.VaultBeneficiaries.Select(b => b.ContainerDrop)).ToList();

                Status confirmedStatus = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED");

                ProductType productType =
                    _repository.Query<ProductType>(a => a.LookUpKey == "MYSBV_VAULT").FirstOrDefault();

                // Get All processed MYSBV VAULT ONLY deposits that have not been SETTLED.
                List<CashDeposit> processedVaultDeposits = (
                    from deposit in
                        _repository.Query<CashDeposit>(a => a.IsProcessed.HasValue && a.IsProcessed.Value &&
                                                            a.StatusId == confirmedStatus.StatusId &&
                                                            a.ProductTypeId == productType.ProductTypeId &&
                                                            a.SendDateTime == null,
                                                            a => a.Account,
                            a => a.Account.Bank,
                            a => a.Account.TransactionType,
                            a => a.Account.AccountType,
                            a => a.Containers,
                            a => a.Containers.Select(b => b.ContainerDrops),
                            a => a.Containers.Select(b => b.ContainerDrops.Select(e => e.Status)),
                            a => a.VaultBeneficiaries,
                            a => a.VaultBeneficiaries.Select(b => b.Account),
                            a => a.VaultBeneficiaries.Select(b => b.CashDeposit),
                            a => a.VaultBeneficiaries.Select(b => b.Account.Bank),
                            a => a.VaultBeneficiaries.Select(b => b.Account.TransactionType),
                            a => a.VaultBeneficiaries.Select(b => b.Account.AccountType),
                            a => a.VaultBeneficiaries.Select(b => b.ContainerDrop))
                    join
                        beneficiary in
                        _repository.Query<VaultBeneficiary>(a => nedbankAccountIds.Contains(a.AccountId.Value))
                        on deposit.CashDepositId equals beneficiary.CashDepositId
                    select deposit).ToList();

                // Add all deposits in to one collection for easy enumeration
                payments.AddRange(processedVaultDeposits.Distinct());

                _unsettledDeposits.AddRange(payments);

                payments.ForEach(a =>
                {
                    switch (a.VaultSource)
                    {
                        case "GPT":
                            NedbankFileItem gptResult = HandleGptDepositSettlement(a);
                            if (gptResult != null)
                                settlementRecords.Add(gptResult);
                            break;
                        case "GREYSTONE":
                            NedbankFileItem greystoneResult = HandleGreystoneDepositSettlement(a);
                            if (greystoneResult != null)
                                settlementRecords.Add(greystoneResult);
                            break;
                        case "WEBFLO":
                            IEnumerable<NedbankFileItem> webFlowResult = HandleWebFloDepositSettlement(a);
                            if (webFlowResult != null)
                                settlementRecords.AddRange(webFlowResult);
                            break;
                        default:
                            // Remove the cash deposit because it is not going to be settled
                            // as part of this batch.
                            _unsettledDeposits.Remove(a);
                            this.Log()
                                .Fatal(
                                    string.Format(
                                        "Invalid vault source supplied [{0}] for deposit with settlement identifier [{1}]",
                                        a.VaultSource, a.SettlementIdentifier),
                                    new ArgumentException("Invalid Vault Source"));
                            break;
                    }
                });

                return settlementRecords;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [VAULT CIT PAYMENT DEPOSIT]", ex);
                return null;
            }
        }


        /// <summary>
        ///     Get all unsettled vault cash deposits based on the Payment message
        /// </summary>
        private IEnumerable<NedbankFileItem> GetVaultPayments() 
        {
            try
            {
                this.Log().Info(string.Format("Vault Payments Deposits method..."));

                var settlementRecords = new List<NedbankFileItem>();

                Status status = _repository.Query<Status>(a => a.LookUpKey == "PAY_UNCONFIRMED").FirstOrDefault();

                // NEDBANK Accounts

                var nedbankBeneficiaryCodes = (from account in _repository.All<Account> 
                    (
                        b => b.Bank,
                        t => t.AccountType,
                        tt => tt.TransactionType
                    )
                    join bankType in _repository.All<TransactionType>() on account.TransactionTypeId equals
                        bankType.TransactionTypeId
                    where bankType.LookUpKey == "NED_HOST" || bankType.LookUpKey == "NED_CROSS_BANK"
                    select account.BeneficiaryCode).ToList(); 

                // Get all payments that have not been paid and not deleted
                var payments = _repository.Query<VaultPartialPayment>(a => a.IsNotDeleted && 
                                      a.StatusId == status.StatusId && 
                                      a.TotalToBePaid > 0 &&
                                      nedbankBeneficiaryCodes.Contains(a.BeneficiaryCode)).ToList();

                this.Log().Info(string.Format("Vault Payments found are = {0}", payments.Count));

                _unsettledVaultPartialPayments.AddRange(payments);

                foreach (VaultPartialPayment a in payments)
                {
                    try
                    {
                        // Get the account based on beneficiary code
                        Account account = _repository.Query<Account>(c => c.BeneficiaryCode == a.BeneficiaryCode,
                            c => c.Bank,
                            c => c.TransactionType,
                            c => c.AccountType).FirstOrDefault();

                        if (account != null)
                        {
                            // Get Container based on serial number
                            Container container =
                                _repository.Query<Container>(c => c.IsNotDeleted && c.SealNumber == a.BagSerialNumber,
                                    c => c.CashDeposit).FirstOrDefault();

                            if (container != null)
                            {
                                var settlement = GetSettlementDetails(null, null, a); 

                                // Create a new Settlement Transaction and add
                                // it to the list of settlement transactions.
                                settlementRecords.Add(settlement);
                            }
                            else
                            {
                                // Remove vault partial payment because it is not going
                                // to be settled
                                _unsettledVaultPartialPayments.Remove(a);

                                this.Log()
                                    .Error(
                                        string.Format(
                                            "IEnumerable<SettlementRecord> VaultPaymentsDeposits() => Container with serial number [{0}] not found.",
                                            a.BagSerialNumber));
                            }
                        }
                        else
                        {
                            // Remove vault partial payment because it is not going
                            // to be settled
                            //_unsettledVaultPartialPayments.Remove(a);
                            this.Log()
                                .Error(
                                    string.Format(
                                        "IEnumerable<SettlementRecord> VaultPaymentsDeposits() => Account with Beneficiary Code [{0}] not found.",
                                        a.BeneficiaryCode));
                        }
                    }
                    catch (Exception ex)
                    {
                        _unsettledVaultPartialPayments.Remove(a);
                        var exception =
                            new Exception(
                                string.Format("Error Occurred while processing a payment with id [{0}]",
                                    a.VaultPartialPaymentId), ex);
                        this.Log().Fatal("Exception On Method : [VAULT PAYMENTS DEPOSITS]", exception);
                    }
                }
                return settlementRecords;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [VAULT PAYMENTS DEPOSITS]", ex);
                return null;
            }
        }


        /// <summary>
        ///     Get data from cash deposit submitted by GREYSTONE
        /// </summary>
        /// <param name="cashDeposit"></param>
        private NedbankFileItem HandleGreystoneDepositSettlement(CashDeposit cashDeposit)
        {
            // NOTE : We always pay amount that has been sent by the client
            //        be it counted and confirmed or not. If there is a discrepancy
            //        normal discrepancy resolution process will be followed.
            //
            // NOTE : For GREYSTONE, We take the Vault Amount, 
            //        this amount will be the final amount to pay
            //        after all partial payments have been made. 
            //
            try
            {
                this.Log().Debug(cashDeposit);
                var settlementRecord = GetSettlementDetails(cashDeposit, null, null);

                return settlementRecord;
            }
            catch (Exception ex)
            {
                _unsettledDeposits.Remove(cashDeposit);
                var exception =
                    new Exception(
                        string.Format("Error Occurred while processing a deposit with id [{0}]",
                            cashDeposit.CashDepositId), ex);
                this.Log().Fatal("Exception On Method : [HANDLE GREYSTONE DEPOSIT SETTLEMENT]", exception);
            }
            return null;
        }

        

        /// <summary>
        ///     Get data from cash deposit submitted by GPT
        /// </summary>
        /// <param name="cashDeposit"></param>
        private NedbankFileItem HandleGptDepositSettlement(CashDeposit cashDeposit)
        {
            // NOTE : We always pay amount that has been sent by the client
            //        be it counted and confirmed or not. If there is a discrepancy
            //        normal discrepancy resolution process will be followed.
            //
            // NOTE : For GPT, We take the Vault Amount, 
            //        this amount will be the final amount to pay
            //        after all partial payments have been made. 
            //
            try
            {
                return GetSettlementDetails(cashDeposit, null, null);
            }
            catch (Exception ex)
            {
                _unsettledDeposits.Remove(cashDeposit);
                var exception =
                    new Exception(
                        string.Format("Error Occurred while processing a deposit with id [{0}]",
                            cashDeposit.CashDepositId), ex);
                this.Log().Fatal("Exception On Method : [HANDLE GPT DEPOSIT SETTLEMENT]", exception);
            }
            return null;
        }



        /// <summary>
        ///     Get data from a cash deposit submitted by Cash Connect
        /// </summary>
        /// <param name="cashDeposit"></param>
        private IEnumerable<NedbankFileItem> HandleWebFloDepositSettlement(CashDeposit cashDeposit)
        {
            var settlementRecords = new List<NedbankFileItem>();
            var items = new Dictionary<int, NedbankFileItem>();

            try
            {
                // NOTE : We must sum up the amounts based on the account number
                foreach (VaultBeneficiary a in cashDeposit.VaultBeneficiaries)
                {
                    // Check if the settlement transaction 
                    // exist in the dictionary and if it does
                    // only update the amount.
                    if (items.ContainsKey(a.AccountId.Value))
                    {
                        // Sum the amount
                        items[a.AccountId.Value].Amount =
                            (Convert.ToDecimal(items[a.AccountId.Value].Amount) +
                             Convert.ToDecimal(a.ContainerDrop.Amount)).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        // NOTE : We always pay amount that has been sent by the client
                        //        be it counted and confirmed or not. If there is a discrepancy
                        //        normal discrepancy resolution process will be followed.

                        // Create a settlement transaction for an accountID that
                        // does not exist in our items dictionary
                        var settlementRecord = GetSettlementDetails(cashDeposit, null, null);

                        // Add the settlement transaction to the dictionary
                        // and it will be referenced by ID next time.
                        items.Add(a.AccountId.Value, settlementRecord);
                    }
                }
            }
            catch (Exception ex)
            {
                _unsettledDeposits.Remove(cashDeposit);
                var exception =
                    new Exception(
                        string.Format("Error Occurred while processing a deposit with id [{0}]",
                            cashDeposit.CashDepositId), ex);
                this.Log().Fatal("Exception On Method : [HANDLE WEB FLO DEPOSIT SETTLEMENT]", exception);
            }

            // Get all settlement transactions from the items
            // dictionary and the add them to the settlement
            // transaction collection.
            items.ToList().ForEach(a => { settlementRecords.Add(a.Value); });

            return settlementRecords;
        }

        #endregion


        #region Helpers

        /// <summary>
        /// Create settlement record from Cash deposit.
        /// </summary>
        /// <param name="cashDeposit"></param>
        /// <param name="drop"></param>
        /// <param name="payment"></param>
        private NedbankFileItem GetSettlementDetails(CashDeposit cashDeposit, ContainerDrop drop, VaultPartialPayment payment)
        {
            try
            {
                string amount;
                string accountName;
                string branchCode;
                string accountNumber;
                string settlementIdentifier;
                int accountId;
                string clientReference;
                string nominatedReference;

                if (cashDeposit != null)
                {
                    var beneficiaryAccountId = 0;

                    //NOTE: Cash Connect deposits may not have Account IDs
                    //So get deposits from a related deposit beneficiary record from 
                    if (cashDeposit.AccountId.HasValue)
                    {
                        beneficiaryAccountId = cashDeposit.AccountId.Value;
                    }
                    else
                    {
                        var beneficiary = _repository.Query<VaultBeneficiary>(e => e.CashDepositId == cashDeposit.CashDepositId)
                                .FirstOrDefault();

                        beneficiaryAccountId = beneficiary.AccountId.HasValue ? beneficiary.AccountId.Value : 0;
                    }

                    var account = GetAccount(beneficiaryAccountId);
                    var site = account.Site;

                    if (drop != null)
                    {
                        // manage multi deposits
                        amount = drop.ActualAmount.ToString();
                        accountName = account.Site.Name.ToUpper();
                        branchCode = account.BranchCode;
                        settlementIdentifier = drop.SettlementIdentifier;
                        var hasNarrative = !string.IsNullOrWhiteSpace(drop.Narrative);
                        clientReference = hasNarrative ? drop.Narrative : site.DepositReference;
                        nominatedReference = drop.ReferenceNumber + "-" + drop.Narrative;
                    }
                    else
                    {
                        // manage single/multidrop deposits
                        amount = cashDeposit.ActualAmount.ToString();
                        accountName = account.AccountHolderName.ToUpper();
                        branchCode = account.BranchCode;
                        settlementIdentifier = cashDeposit.SettlementIdentifier;
                        var hasNarrative = !string.IsNullOrWhiteSpace(cashDeposit.Narrative);
                        clientReference = hasNarrative ? cashDeposit.Narrative : site.DepositReference;
                        nominatedReference = cashDeposit.TransactionReference + "-" + cashDeposit.Narrative;
                    }
                    accountId = beneficiaryAccountId;
                    accountNumber = account.AccountNumber;
                    amount = cashDeposit.DeviceId.HasValue ? cashDeposit.VaultAmount.ToString() : amount;
                }
                else
                {
                    // manage partial payments
                    var account = GetAccount(payment.BeneficiaryCode);
                    amount = payment.TotalToBePaid.ToString();
                    accountName = account.AccountHolderName.ToUpper();
                    branchCode = account.BranchCode;
                    accountNumber = account.AccountNumber;
                    settlementIdentifier = payment.SettlementIdentifier;
                    accountId = account.AccountId;
                    clientReference = payment.PaymentReference;
                    nominatedReference = GetVaultDepositReference(payment.BagSerialNumber);
                }
                var settlementRecord = CreateSettlementRecord(branchCode, accountNumber, amount, accountName, clientReference, nominatedReference);
                settlementRecord.AccountId = accountId;
                settlementRecord.SettlementIdentifier = settlementIdentifier;

                return PadFileProperties(settlementRecord);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (var error in entityValidationError.ValidationErrors)
                    {
                        this.Log()
                            .Fatal(
                                string.Format(
                                    "DbEntityValidationException On Method => CreateSettlementRecord: [REQUEST WRITER]\n[{0}]\nStacktrace\n",
                                    error.ErrorMessage), ex);
                    }
                }
                // throw new exception
                throw new DbEntityValidationException(
                    "DbEntityValidationException On Method => CreateSettlementRecord: [REQUEST WRITER]\n[{0}]\nStacktrace\n", ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception on Method: [CreateSettlementRecord] => NEDBANK_REQUEST_DATA", ex);
                throw;
            }
        }


        /// <summary>
        /// get Vault Transaction Reference number by Bag Serial.
        /// </summary>
        /// <param name="bagNumber"></param>
        /// <returns></returns>
        private string GetVaultDepositReference(string bagNumber)
        {
            var cashDeposit = (from v in _repository.All<VaultPartialPayment>()
                join c in _repository.All<Container>() on v.BagSerialNumber equals c.SerialNumber
                join d in _repository.All<CashDeposit>() on c.CashDepositId equals d.CashDepositId
                where c.SerialNumber.ToLower() == bagNumber.ToLower()
                select d).FirstOrDefault();

            if (cashDeposit != null)
            {
                return cashDeposit.TransactionReference + "-" + cashDeposit.Narrative;
            }
            return string.Empty;
        }


        /// <summary>
        /// Create settlement record.
        /// </summary>
        /// <param name="branchCode"></param>
        /// <param name="accountNumber"></param>
        /// <param name="amount"></param>
        /// <param name="accountName"></param>
        /// <param name="reference"></param>
        /// <param name="nominatedReference"></param>
        private NedbankFileItem CreateSettlementRecord(string branchCode, string accountNumber, string amount,
            string accountName, string reference, string nominatedReference)
        {
            var transaction = _transactionTypes.FirstOrDefault(e => e.LookUpKey == "CREDIT");
            var client = _clientTypes.FirstOrDefault(e => e.LookUpKey == "FINANCIAL_INSTITUTION");
            var service = _serviceTypes.FirstOrDefault(e => e.LookUpKey == "SDV");
            var accountHolderName = accountName.Length > 30 ? accountName.Substring(0, 30) : accountName;
            var clientReference = reference.Length > 30 ? reference.Substring(0, 30) : reference;

            var nominatedAccountReference = nominatedReference.Length > 30
                ? nominatedReference.Substring(0, 30)
                : nominatedReference;

            var settlementRecord = new NedbankFileItem
            {
                RecordIdentifier = Convert.ToInt32(RecordIdentifier.DETAIL).ToString(),
                DestinationBranchCode = branchCode,
                DestinationAccountNumber = accountNumber,
                Amount = amount,
                Reference = clientReference.Trim(),
                DestinationAccountHoldersName = accountHolderName,
                TransactionType = transaction.TransactionType,
                NedbankClientTypeId = client.NedbankClientTypeId,
                ServiceType = service.ServiceType,
                ChargesAccountNumber = _clientProfile.ChargesAccountNumber,
                NominatedAccountNumber = _clientProfile.NominatedAccountNumber,
                NominatedAccountReference = nominatedAccountReference.Trim()
            };
            return settlementRecord;
        }


        /// <summary>
        /// Padding of file properties to meet file specification.
        /// </summary>
        /// <param name="settlement"></param>
        private NedbankFileItem PadFileProperties(NedbankFileItem settlement)
        {
            settlement.RecordIdentifier = settlement.RecordIdentifier.PadLeft(2, '0');
            settlement.DestinationBranchCode = settlement.DestinationBranchCode.PadLeft(6, '0');
            settlement.DestinationAccountNumber = settlement.DestinationAccountNumber.PadLeft(16, '0');
            settlement.Reference = settlement.Reference.PadLeft(30, ' ');
            settlement.DestinationAccountHoldersName = settlement.DestinationAccountHoldersName.PadRight(30, ' ');
            settlement.TransactionType = settlement.TransactionType.PadLeft(4, '0');
            settlement.NedbankClientTypeId = settlement.NedbankClientTypeId.PadLeft(2, '0');
            settlement.ServiceType = settlement.ServiceType.PadLeft(2, '0');
            settlement.ChargesAccountNumber = _clientProfile.ChargesAccountNumber.PadLeft(16, '0');
            settlement.NominatedAccountNumber = _clientProfile.NominatedAccountNumber.PadLeft(16, '0');
            settlement.NominatedAccountReference = settlement.NominatedAccountReference.PadLeft(30, ' ');
            settlement.BDFIndicator = string.Empty.PadLeft(1, ' ');
            settlement.Filler = string.Empty.PadLeft(75, ' ');
            settlement.OriginalPaymentReferenceNumber = string.Empty.PadLeft(34, ' ');

            return settlement;
        }


        /// <summary>
        /// Get Account by ID
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        private Account GetAccount(int accountId)
        {
            var account = _repository.Query<Account>(a => a.AccountId == accountId,
                                a => a.Site,
                                a => a.Site.Merchant,
                                a => a.Site.Address,
                                a => a.Site.Accounts,
                                a => a.Site.Accounts.Select(b => b.Bank),
                                a => a.Site.Accounts.Select(b => b.AccountType),
                                a => a.Site.Accounts.Select(b => b.TransactionType),
                                a => a.Site.Accounts.Select(b => b.Status),
                                u => u.Site.SiteContainers,
                                o => o.Site.SiteContainers.Select(a => a.ContainerType),
                                cta => cta.Site.SiteContainers.Select(a => a.ContainerType.ContainerTypeAttributes)
                                ).FirstOrDefault();
            return account;
        }


        /// <summary>
        /// Get Account by Beneficiary Code
        /// </summary>
        /// <param name="beneficiaryCode"></param>
        /// <returns></returns>
        private Account GetAccount(string beneficiaryCode)
        {
            var account = _repository.Query<Account>(a => a.BeneficiaryCode == beneficiaryCode,
                                a => a.Site,
                                a => a.Site.Merchant,
                                a => a.Site.Address,
                                a => a.Site.Accounts,
                                a => a.Site.Accounts.Select(b => b.Bank),
                                a => a.Site.Accounts.Select(b => b.AccountType),
                                a => a.Site.Accounts.Select(b => b.TransactionType),
                                a => a.Site.Accounts.Select(b => b.Status),
                                u => u.Site.SiteContainers,
                                o => o.Site.SiteContainers.Select(a => a.ContainerType),
                                cta => cta.Site.SiteContainers.Select(a => a.ContainerType.ContainerTypeAttributes)
                                ).FirstOrDefault();
            return account;
        }


        /// <summary>
        /// get nedbank accounts
        /// </summary>
        /// <returns></returns>
        private List<int> GetNedbankAccountIds()
        {
            var bankTypeIds = (from account in _repository.All<Account>(b => b.Bank,
                                       t => t.AccountType,
                                       tt => tt.TransactionType)
                join bankType in _repository.All<TransactionType>() on account.TransactionTypeId equals
                    bankType.TransactionTypeId
                where bankType.LookUpKey == "NED_HOST" || bankType.LookUpKey == "NED_CROSS_BANK"
                select bankType.TransactionTypeId).ToList();

            var nedbankAccountIds = _repository.Query<Account>(a => bankTypeIds.Contains(a.TransactionTypeId),
                                       b => b.Bank,
                                       t => t.AccountType,
                                       tt => tt.TransactionType
                                   ).Select(e => e.AccountId);
            return nedbankAccountIds.ToList();
        }
        #endregion
    }
}