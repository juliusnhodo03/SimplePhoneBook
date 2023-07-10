using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;

namespace Hyphen.Integration.Request.Data
{
    /// <summary>
    ///     Manages Business logic to get and update status
    ///     Before and after generating a Hyphen FACS File
    /// </summary>
    [Export(typeof (IRequestData))]
    public class RequestData : IRequestData
    {
        #region Fields

        /// <summary>
        ///     Instance of the repository
        /// </summary>
        public IRepository _repository { get; set; }

        /// <summary>
        ///     List of all statuses
        /// </summary>
        private readonly IEnumerable<Status> _statuses;

        /// <summary>
        ///     A list of all deposits
        /// </summary>
        private readonly List<CashDeposit> _unsettledDeposits;

        /// <summary>
        ///     A list of all partial payments
        /// </summary>
        private readonly List<VaultPartialPayment> _unsettledVaultPartialPayments;

        #endregion

        #region Constructor

        /// <summary>
        ///     Request Data Constructor
        /// </summary>
        /// <param name="repository"></param>
        [ImportingConstructor]
        public RequestData(IRepository repository)
        {
            _repository = repository;
            _statuses = _repository.All<Status>();
            _unsettledDeposits = new List<CashDeposit>();
            _unsettledVaultPartialPayments = new List<VaultPartialPayment>();
        }

        #endregion

        #region IRequest Data

        public IEnumerable<SettlementTransaction> GetSettlementTransactions()
        {
            try
            {
                var settlementTransactions = new List<SettlementTransaction>();

                // get all mySBV unsettled deposits
                IEnumerable<SettlementTransaction> mySbvUnprocessedDeposits = GetAllProcessedUnsettledMySbvDeposits();

                if (mySbvUnprocessedDeposits != null)
                {
                    settlementTransactions.AddRange(mySbvUnprocessedDeposits);
                }

                // Deposits Marked For Resubmission
                IEnumerable<SettlementTransaction> depositsMarkedForResubmission = GetAllDepositsMarkedForResubmission();
                if (depositsMarkedForResubmission != null)
                {
                    settlementTransactions.AddRange(depositsMarkedForResubmission);
                }

                // Filter Out Settled Multi-Deposits
                IEnumerable<SettlementTransaction> filterOutSettledMultiDeposits = FilterOutSettledMultiDeposits();
                if (filterOutSettledMultiDeposits != null)
                {
                    settlementTransactions.AddRange(filterOutSettledMultiDeposits);
                }

                // Vault Settlement
                IEnumerable<SettlementTransaction> vaultSettlement = VaultSettlement();
                if (vaultSettlement != null)
                {
                    settlementTransactions.AddRange(vaultSettlement);
                }


                var deposits = new List<SettlementTransaction>();

                foreach (var transaction in settlementTransactions)
                {
                    if (Convert.ToDouble(transaction.TransactionAmount) > 0)
                    {
                        deposits.Add(transaction);
                    }
                }

                return deposits;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET PROCESSED DEPOSITS]", ex);
                throw;
            }
        }

        /// <summary>
        ///     Mark all unsettled deposits as settled
        /// </summary>
        public void UpdateSettlementStatus()
        {
            try
            {
                HandleUnsettledDeposits();
                HandleUnsettledVaultPartialPayments();
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE SETTLEMENT STATUS]", ex);
            }
        }

        #endregion
        
        #region Update Settlement Status Helpers

        /// <summary>
        ///     Mark unsettle cash deposits as settled
        /// </summary>
        private void HandleUnsettledDeposits()
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
        ///     Marks unsettled vault partial payments as settled
        /// </summary>
        private void HandleUnsettledVaultPartialPayments()
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

        #region MySBV Deposit Helpers

        /// <summary>
        ///     Get All processed unsettled mysbv cash deposits
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> GetAllProcessedUnsettledMySbvDeposits()
        {
            try
            {
                var accountIds = GetNonNedbankAccountIds(); 

                // Get Confirmed Status
                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED");
                ProductType productType =
                    _repository.Query<ProductType>(a => a.LookUpKey == "MYSBV_DEPOSIT").FirstOrDefault();
                // Get a list of all deposits that are processed
                // and with a status of CONFIRMED
                List<CashDeposit> cashDeposits = _repository.Query<CashDeposit>(
                    a =>
                        (a.IsProcessed.HasValue && a.IsProcessed.Value) && 
                        (a.StatusId == status.StatusId) &&
                        (a.ProductTypeId == productType.ProductTypeId) &&
                        (accountIds.Contains(a.AccountId.Value) && a.AccountId.HasValue),
                    deposit => deposit.Containers,
                    containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops),
                    containerDrops =>
                        containerDrops.Containers.Select(c => c.ContainerDrops.Select(e => e.ContainerDropItems)),
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
                this.Log().Fatal("Exception On Method : [GET ALL PROCESSED UNSETTLED MYSBV DEPOSITS]", ex);
                return null;
            }
        }


        
        /// <summary>
        ///     Get all mysbv cash deposits marked for resubmission
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> GetAllDepositsMarkedForResubmission()
        {
            try
            {
                var accountIds = GetNonNedbankAccountIds();

                // Get RESUBMITTED Status
                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED");

                // Retrieve all single and multi-drop deposits that have been marked 
                // for resubmission.
                List<CashDeposit> cashDeposits = _repository.Query<CashDeposit>(a => 
                    (a.SettledDateTime.HasValue) &&
                    (a.StatusId == status.StatusId) &&
                    (accountIds.Contains(a.AccountId.Value) && 
                    a.AccountId.HasValue),
                    container => container.Containers,
                    containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops),
                    containerDropItems =>
                        containerDropItems.Containers.Select(c => c.ContainerDrops.Select(b => b.ContainerDropItems)))
                    .ToList();
                _unsettledDeposits.AddRange(cashDeposits);
                return SeperateDeposits(cashDeposits);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET ALL DEPOSITS MARKED FOR RESUBMISSION]", ex);
                return null;
            }
        }
        


        /// <summary>
        /// get other banks accounts
        /// </summary>
        private List<int> GetNonNedbankAccountIds()
        {
            var nedbankAccountIds = _repository.Query<Account>(a => a.Bank.LookUpKey != "NEDBANK",
                                       b => b.Bank,
                                       t => t.AccountType,
                                       tt => tt.TransactionType
                                   ).Select(e => e.AccountId);
            return nedbankAccountIds.ToList();
        }



        /// <summary>
        ///     Filter out settled multi deposits
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> FilterOutSettledMultiDeposits()
        {
            try
            {
                var accountIds = GetNonNedbankAccountIds(); 

                // Get RESUBMITTED Status
                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED");

                // All required information for multi deposit is sitting on container drop table. 
                // retrieve all deposits that have been marked with resubmission but exclude all the other
                // multi deposits that have not been market with resubmit but belonging to the same cash deposit.
                List<ContainerDrop> containerDrops = _repository.Query<ContainerDrop>(
                    a => a.SettlementDateTime.HasValue && a.StatusId == status.StatusId &&
                        (accountIds.Contains(a.Container.CashDeposit.AccountId.Value) && a.Container.CashDeposit.AccountId.HasValue),
                    c => c.Container.CashDeposit,
                    c => c.Container.CashDeposit.Containers,
                    c => c.Container.CashDeposit.Containers.Select(b => b.ContainerDrops),
                    c => c.Container.CashDeposit.Containers.Select(
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
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> SeperateDeposits(IEnumerable<CashDeposit> cashDeposits)
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
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> GenerateSettlementFileList(IEnumerable<CashDeposit> simpleDeposits,
            IEnumerable<CashDeposit> complexDeposits)
        {
            var list = new List<SettlementTransaction>();
            IEnumerable<Site> sites = _repository.All<Site>().ToList();

            IEnumerable<SettlementTransaction> settlementFiles = GenerateFromSimpleFiles(simpleDeposits, sites);
            IEnumerable<SettlementTransaction> settlementFiles1 = GenerateFromComplexDeposit(complexDeposits, sites);
            list.AddRange(settlementFiles);
            list.AddRange(settlementFiles1);
            return list;
        }



        /// <summary>
        ///     Generate settlement transaction file collection for multi deposits
        /// </summary>
        /// <param name="complexDeposits"></param>
        /// <param name="sites"></param>
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> GenerateFromComplexDeposit(IEnumerable<CashDeposit> complexDeposits,
            IEnumerable<Site> sites)
        {
            try
            {
                var settlementFileList = new List<SettlementTransaction>();
                int resubmittedStatusId = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED").StatusId;
                int statusId = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED").StatusId;

                foreach (CashDeposit complexDeposit in complexDeposits)
                {
                    Site siteInfo = sites.FirstOrDefault(a => a.SiteId == complexDeposit.SiteId);

                    Account bankInfo = _repository.Query<Account>(a => a.AccountId == complexDeposit.AccountId, 
                        b => b.Bank, t => t.AccountType, 
                        tt => tt.TransactionType).FirstOrDefault();

                    foreach (Container container in complexDeposit.Containers)
                    {
                        foreach (ContainerDrop drop in container.ContainerDrops)
                        {
                            if (drop.StatusId == statusId || drop.StatusId == resubmittedStatusId)
                            {
                                settlementFileList.Add(new SettlementTransaction
                                {
                                    SettlementIdentifier = drop.SettlementIdentifier,
                                    Payee = siteInfo.Name,
                                    Bank = bankInfo.Bank.Name,
                                    BankAccountNumber = bankInfo.AccountNumber,
                                    AccountType = bankInfo.AccountType.Name,
                                    BranchCode = bankInfo.BranchCode,
                                    TransactionType = bankInfo.TransactionType.Name,
                                    SbvDepositReference = drop.ReferenceNumber,
                                    TransactionAmount = drop.ActualAmount.ToString(CultureInfo.InvariantCulture),
                                    ClientReference = siteInfo.DepositReferenceIsEditable
                                        ? drop.Narrative
                                        : siteInfo.DepositReference
                                });
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
        /// <param name="sites"></param>
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> GenerateFromSimpleFiles(IEnumerable<CashDeposit> simpleDeposits,
            IEnumerable<Site> sites)
        {
            try
            {
                return (from cashDeposit in simpleDeposits
                    let siteInfo = sites.FirstOrDefault(a => a.SiteId == cashDeposit.SiteId)
                    join siteSettlementAccount in
                        _repository.Query<Account>(a => a.IsNotDeleted, b => b.Bank, t => t.TransactionType,
                            tt => tt.AccountType) on
                        cashDeposit.AccountId equals siteSettlementAccount.AccountId
                    where siteInfo != null
                    select new SettlementTransaction
                    {
                        SettlementIdentifier = cashDeposit.SettlementIdentifier,
                        Payee = siteInfo.Name,
                        Bank = siteSettlementAccount.Bank.Name,
                        BankAccountNumber = siteSettlementAccount.AccountNumber,
                        AccountType = siteSettlementAccount.AccountType.Name,
                        BranchCode = siteSettlementAccount.BranchCode,
                        TransactionType = siteSettlementAccount.TransactionType.Name,
                        SbvDepositReference = cashDeposit.TransactionReference,
                        TransactionAmount = cashDeposit.ActualAmount.ToString(),
                        ClientReference =
                            siteInfo.DepositReferenceIsEditable
                                ? cashDeposit.Narrative
                                : siteInfo.DepositReference
                    }).ToList();
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GENERATE FROM SIMPLE DEPOSITS]", ex);
                throw;
            }
        }

        #endregion

        #region MySBV Vault Helpers

        /// <summary>
        ///     Get All unsettled vault cash deposits
        /// </summary>
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> VaultSettlement()
        {
            try
            {
                var settlements = new List<SettlementTransaction>();

                //Vault Payments Deposits 
                IEnumerable<SettlementTransaction> vaultPaymentsDeposits = VaultPaymentsDeposits();
                if (vaultPaymentsDeposits != null)
                {
                    settlements.AddRange(vaultPaymentsDeposits);
                }

                // Vault Cit Payment Deposit
                IEnumerable<SettlementTransaction> vaultCitPaymentDeposit = VaultCitPaymentDeposit();
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
        private IEnumerable<SettlementTransaction> VaultCitPaymentDeposit()
        {
            try
            {
                this.Log().Info(string.Format("Vault Cit Deposits method..."));

                var settlementTransactions = new List<SettlementTransaction>();
                Status payUnConfirmedStatus = _statuses.FirstOrDefault(a => a.LookUpKey == "PAY_UNCONFIRMED");

                // Non NEDBANK Accounts
                var accountIds = GetNonNedbankAccountIds(); 

                // Get all MYSBV_VAULT Deposits that have not been PROCESSED, SETTLED and NOT DELETED
                List<CashDeposit> payments =
                    _repository.Query<CashDeposit>(a => a.StatusId == payUnConfirmedStatus.StatusId &&
                        accountIds.Contains(a.AccountId.Value),//accountIds.Contains(a.AccountId.Value),
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
                var processedVaultDeposits = (
                    from deposit in
                        _repository.Query<CashDeposit>(a =>
                            a.IsProcessed.HasValue && a.IsProcessed.Value &&
                            a.StatusId == confirmedStatus.StatusId && a.ProductTypeId == productType.ProductTypeId &&
                            // accountIds.Contains(a.AccountId.Value) &&
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
                        _repository.Query<VaultBeneficiary>(a => accountIds.Contains(a.AccountId.Value))
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
                            SettlementTransaction gptResult = HandleGptDepositSettlement(a);
                            if (gptResult != null)
                                settlementTransactions.Add(gptResult);
                            break;
                        case "GREYSTONE":
                            SettlementTransaction greystoneResult = HandleGreystoneDepositSettlement(a);
                            if (greystoneResult != null)
                                settlementTransactions.Add(greystoneResult);
                            break;
                        case "WEBFLO":
                            IEnumerable<SettlementTransaction> webFlowResult = HandleWebFloDepositSettlement(a);
                            if (webFlowResult != null)
                                settlementTransactions.AddRange(webFlowResult);
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

                return settlementTransactions;
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
        private IEnumerable<SettlementTransaction> VaultPaymentsDeposits()
        {
            try
            {
                this.Log().Info(string.Format("Vault Payments Deposits method..."));

                var settlementTransactions = new List<SettlementTransaction>();
                Status status = _repository.Query<Status>(a => a.LookUpKey == "PAY_UNCONFIRMED").FirstOrDefault();

                // Non NEDBANK Accounts
                var nedbankBeneficiaryCodes = _repository.Query<Account>(a => a.Bank.LookUpKey != "NEDBANK",
                                           b => b.Bank,
                                           t => t.AccountType,
                                           tt => tt.TransactionType
                                       ).Select(e => e.BeneficiaryCode); 

                // Get all payments that have not been paid and not deleted
                var payments = _repository.Query<VaultPartialPayment>(a =>
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
                            Container container = _repository.Query<Container>(c => c.SealNumber == a.BagSerialNumber,
                                    c => c.CashDeposit).FirstOrDefault();

                            if (container != null)
                            {
                                // Create a new Settlement Transaction and add
                                // it to the list of settlement transactions.
                                settlementTransactions.Add(new SettlementTransaction
                                {
                                    SettlementIdentifier = a.SettlementIdentifier,
                                    AccountType = account.AccountType.Name,
                                    Bank = account.Bank.Name,
                                    BankAccountNumber = account.AccountNumber,
                                    BranchCode = account.BranchCode,
                                    ClientReference = a.PaymentReference,
                                    Payee = account.AccountHolderName,
                                    SbvDepositReference = container.CashDeposit.TransactionReference,
                                    TransactionAmount = a.TotalToBePaid.ToString(CultureInfo.InvariantCulture),
                                    TransactionType = account.TransactionType.Name
                                });
                            }
                            else
                            {
                                // Remove vault partial payment because it is not going
                                // to be settled
                                _unsettledVaultPartialPayments.Remove(a);

                                this.Log()
                                    .Error(
                                        string.Format(
                                            "IEnumerable<SettlementTransaction> VaultPaymentsDeposits() -----> Container with serial number [{0}] not found.",
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
                                        "IEnumerable<SettlementTransaction> VaultPaymentsDeposits() -----> Account with Beneficiary Code [{0}] not found.",
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
                return settlementTransactions;
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
        private SettlementTransaction HandleGreystoneDepositSettlement(CashDeposit cashDeposit)
        {
            // NOTE : We always pay amount that has been sent by the client
            //        be it counted and confirmed or not. If there is a discrepancy
            //        normal discrepancy resolution process will be followed.
            //
            // NOTE : For GREYSTONE, We take the Vault Amount, 
            //        this amount will be the final amount to pay
            //        after all partial payments have been made. 
            try
            {
                this.Log().Debug(cashDeposit);
                var settlementTransaction = new SettlementTransaction
                {
                    SettlementIdentifier = cashDeposit.SettlementIdentifier,
                    AccountType = cashDeposit.Account.AccountType.Name,
                    Bank = cashDeposit.Account.Bank.Name,
                    BankAccountNumber = cashDeposit.Account.AccountNumber,
                    BranchCode = cashDeposit.Account.BranchCode,
                    ClientReference = cashDeposit.Narrative,
                    Payee = cashDeposit.Account.AccountHolderName,
                    SbvDepositReference = cashDeposit.TransactionReference,
                    TransactionAmount = cashDeposit.VaultAmount.ToString(),
                    TransactionType = cashDeposit.Account.TransactionType.Name
                };

                return settlementTransaction;
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
        /// <returns></returns>
        private SettlementTransaction HandleGptDepositSettlement(CashDeposit cashDeposit)
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
                var settlementTransaction = new SettlementTransaction
                {
                    SettlementIdentifier = cashDeposit.SettlementIdentifier,
                    AccountType = cashDeposit.Account.AccountType.Name,
                    Bank = cashDeposit.Account.Bank.Name,
                    BankAccountNumber = cashDeposit.Account.AccountNumber,
                    BranchCode = cashDeposit.Account.BranchCode,
                    ClientReference = cashDeposit.Narrative,
                    Payee = cashDeposit.Account.AccountHolderName,
                    SbvDepositReference = cashDeposit.TransactionReference,
                    TransactionAmount = cashDeposit.VaultAmount.ToString(),
                    TransactionType = cashDeposit.Account.TransactionType.Name
                };

                return settlementTransaction;
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
        /// <returns></returns>
        private IEnumerable<SettlementTransaction> HandleWebFloDepositSettlement(CashDeposit cashDeposit)
        {
            var settlementTransactions = new List<SettlementTransaction>();
            var items = new Dictionary<int, SettlementTransaction>();

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
                        items[a.AccountId.Value].TransactionAmount =
                            (Convert.ToDecimal(items[a.AccountId.Value].TransactionAmount) +
                             Convert.ToDecimal(a.ContainerDrop.Amount)).ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        // NOTE : We always pay amount that has been sent by the client
                        //        be it counted and confirmed or not. If there is a discrepancy
                        //        normal discrepancy resolution process will be followed.

                        // Create a settlement transaction for an accountID that
                        // does not exist in our items dictionary
                        var settlementTransaction = new SettlementTransaction
                        {
                            SettlementIdentifier = cashDeposit.SettlementIdentifier,
                            AccountType = a.Account.AccountType.Name,
                            Bank = a.Account.Bank.Name,
                            BankAccountNumber = a.Account.AccountNumber,
                            BranchCode = a.Account.BranchCode,
                            ClientReference = cashDeposit.Narrative,
                            Payee = a.Account.AccountHolderName,
                            SbvDepositReference = a.ContainerDrop.ReferenceNumber,
                            TransactionAmount = a.ContainerDrop.Amount.ToString(CultureInfo.InvariantCulture),
                            TransactionType = a.Account.TransactionType.Name
                        };

                        // Add the settlement transaction to the dictionary
                        // and it will be referenced by ID next time.
                        items.Add(a.AccountId.Value, settlementTransaction);
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
            items.ToList().ForEach(a => { settlementTransactions.Add(a.Value); });

            return settlementTransactions;
        }

        #endregion
    }
}