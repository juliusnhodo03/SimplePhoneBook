using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;

namespace Hyphen.Integration.DataAccess
{
    [Export(typeof (IDataAccess))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class DataAccess : IDataAccess
    {
        #region Fields

        private readonly IDictionary<string, List<DepositSettlementDetails>> _depositPayments;
        private readonly ILookup _lookup;
        private readonly IRepository _repository;
        private readonly IEnumerable<Status> _statuses;
        private List<CashDeposit> _sentDeposits;
        private bool _hasRejectedDeposits;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public DataAccess(IRepository repository, ILookup lookup)
        {
            _repository = repository;
            _lookup = lookup;
            _statuses = _repository.All<Status>();
            _depositPayments = new Dictionary<string, List<DepositSettlementDetails>>();
        }

        #endregion

        #region IDataAccess Implementation

        public bool HasRejectedDeposits
        {
            get { return _hasRejectedDeposits; }
        }

        public void UpdateDeposit()
        {
            try
            {
                if (_depositPayments.Count > 0) _hasRejectedDeposits = true;

                foreach (var depositPayment in _depositPayments)
                {
                    int id = Convert.ToInt32(depositPayment.Key);
                    CashDeposit cashDeposit = _repository.Query<CashDeposit>(a => a.CashDepositId == id,
                            T => T.DepositType).FirstOrDefault();

                    if (cashDeposit != null)
                    {
                        if (cashDeposit.DepositType.LookUpKey != "MULTI_DEPOSIT")
                        {
                            // For Single and Multi Drops, the will always be 1 deposit Settlement Detail record
                            DepositSettlementDetails depositDetails = depositPayment.Value[0];

                            int errorCodeID = _lookup.GetErrorCodeId(depositDetails.ErrorCode);
                            cashDeposit.StatusId = depositDetails.IsSettled
                                ? _lookup.GetStatusId("SETTLED")
                                : _lookup.GetStatusId("SETTLEMENT_REJECTED");
                            cashDeposit.Status = null;
                            cashDeposit.IsSettled = depositDetails.IsSettled;
                            cashDeposit.SettledDateTime = depositDetails.ResponseProcessDateTime;
                            cashDeposit.ErrorCodeId = (errorCodeID != -1) ? errorCodeID : new int?();
                            cashDeposit.LastChangedDate = DateTime.Now;
                            cashDeposit.EntityState = State.Modified;
                            _repository.Update(cashDeposit);
                        }
                        else
                        {
                            // For Multi Drops we want to update the actual drop details with the correct status
                            foreach (DepositSettlementDetails depositSettlementDetail in depositPayment.Value)
                            {
                                ContainerDrop drop =
                                    _repository.Query<ContainerDrop>(
                                        a => a.ReferenceNumber == depositSettlementDetail.ReferenceNumber)
                                        .FirstOrDefault();

                                if (drop != null)
                                {
                                    int errorCodeID = _lookup.GetErrorCodeId(depositSettlementDetail.ErrorCode);
                                    drop.StatusId = depositSettlementDetail.IsSettled
                                        ? _lookup.GetStatusId("SETTLED")
                                        : _lookup.GetStatusId("SETTLEMENT_REJECTED");
                                    drop.Status = null;
                                    drop.SettlementDateTime = depositSettlementDetail.ResponseProcessDateTime;
                                    drop.ErrorCodeId = (errorCodeID != -1) ? errorCodeID : new int?();
                                    drop.LastChangedDate = DateTime.Now;
                                    drop.EntityState = State.Modified;
                                    _repository.Update(drop);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE DEPOSIT]", ex);
            }
        }

        public void UpdateSettlementStatus()
        {
            try
            {
                if (_sentDeposits.Count <= 0) return;

                string query =
                    "UPDATE CashDeposit SET StatusId = (SELECT StatusId FROM Status WHERE LookUpKey = 'PENDING_SETTLEMENT'), SendDateTime = '" +
                    DateTime.Now + "' WHERE CashDepositId IN ";
                string dropQuery =
                    "UPDATE ContainerDrop SET StatusId = (SELECT StatusId FROM Status WHERE LookUpKey = 'PENDING_SETTLEMENT'), SendDateTime = '" +
                    DateTime.Now + "' WHERE ContainerId IN (SELECT ContainerId FROM CONTAINER WHERE CashDepositId IN ";

                string whereClause = "(";

                for (int i = 0; i < _sentDeposits.Count; i++)
                {
                    if (i == 0)
                        whereClause += _sentDeposits[i].CashDepositId;
                    whereClause += ", " + _sentDeposits[i].CashDepositId;
                }
                whereClause += ")";

                string fullQuery = query + whereClause;
                string fullDropQuery = dropQuery + whereClause + ")";

                _repository.ExecuteQuery(fullQuery);
                _repository.ExecuteQuery(fullDropQuery);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE SETTLEMENT STATUS]", ex);
            }
        }

        public IEnumerable<SettlementFile> GetProcessedDeposites()
        {
            try
            {
                int statusId = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED").StatusId;

                // Get a list of all deposits that are processed
                List<CashDeposit> cashDeposits =
                    _repository.Query<CashDeposit>(
                        a => (a.IsProcessed.HasValue && a.IsProcessed.Value) && (a.StatusId == statusId),
                        container => container.Containers,
                        containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops),
                        containerDropItems =>
                            containerDropItems.Containers.Select(c => c.ContainerDrops.Select(b => b.ContainerDropItems)))
                        .ToList();

                _sentDeposits = cashDeposits;
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

                // Retrieve all single and multi-drop deposits that have been marked 
                // for resubmission.
                int resubmittedStatusId = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED").StatusId;
                List<CashDeposit> reSubcashDeposits =
                    _repository.Query<CashDeposit>(a => (a.SettledDateTime.HasValue) &&
                                                        (a.StatusId == resubmittedStatusId),
                        container => container.Containers,
                        containerDrops => containerDrops.Containers.Select(c => c.ContainerDrops),
                        containerDropItems =>
                            containerDropItems.Containers.Select(c => c.ContainerDrops.Select(b => b.ContainerDropItems)))
                        .ToList();

                _sentDeposits.AddRange(reSubcashDeposits);
                simpleDeposits.AddRange(reSubcashDeposits);

                // All required information for multi deposit is sitting on container drop table. 
                // retrieve all deposits that have been marked with resubmission but exclude all the other
                // multi deposits that have not been market with resubmit but belonging to the same cash deposit.
                List<ContainerDrop> resubCont =
                    _repository.Query<ContainerDrop>(
                        a => a.SettlementDateTime.HasValue && a.StatusId == resubmittedStatusId,
                        c => c.Container.CashDeposit,
                        c => c.Container.CashDeposit.Containers, 
                        c => c.Container.CashDeposit.Containers.Select(b => b.ContainerDrops),
                        c => c.Container.CashDeposit.Containers.Select(b => b.ContainerDrops.Select(v => v.ContainerDropItems))).ToList();

                foreach (var containerDrop in resubCont)
                {
                    if (!_sentDeposits.Contains(containerDrop.Container.CashDeposit))
                        _sentDeposits.Add(containerDrop.Container.CashDeposit);
                }

                simpleDeposits = simpleDeposits.Select(a => a.RemoveDeleted()).ToList();
                complexDeposits = complexDeposits.Select(a => a.RemoveDeleted()).ToList();

                complexDeposits.AddRange(resubCont.Select(containerDrop => containerDrop.Container.CashDeposit));

                return GenerateSettlementFileList(simpleDeposits, complexDeposits);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET PROCESSED DEPOSITS]", ex);
                throw;
            }
        }

        public void AddDepositId(string depositId, DepositSettlementDetails settlementDetails)
        {
            if (!_depositPayments.ContainsKey(depositId))
            {
                _depositPayments.Add(new KeyValuePair<string, List<DepositSettlementDetails>>(depositId,
                    new List<DepositSettlementDetails> {settlementDetails}));
            }
            else
            {
                _depositPayments[depositId].Add(settlementDetails);
            }
        }

        private IEnumerable<SettlementFile> GenerateSettlementFileList(IEnumerable<CashDeposit> simpleDeposits,
            IEnumerable<CashDeposit> complexDeposits)
        {
            var list = new List<SettlementFile>();
            IEnumerable<Site> sites = _repository.All<Site>().ToList();

            IEnumerable<SettlementFile> settlementFiles = GenerateFromSimpleFiles(simpleDeposits, sites);
            IEnumerable<SettlementFile> settlementFiles1 = GenerateFromComplexDeposit(complexDeposits, sites);
            list.AddRange(settlementFiles);
            list.AddRange(settlementFiles1);
            return list;
        }

        private IEnumerable<SettlementFile> GenerateFromComplexDeposit(IEnumerable<CashDeposit> complexDeposits,
            IEnumerable<Site> sites)
        {
            try
            {
                var settlementFileList = new List<SettlementFile>();
                int resubmittedStatusId = _statuses.FirstOrDefault(a => a.LookUpKey == "RESUBMITTED").StatusId;
                int statusId = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED").StatusId;

                foreach (CashDeposit complexDeposit in complexDeposits)
                {
                    Site siteInfo = sites.FirstOrDefault(a => a.SiteId == complexDeposit.SiteId);
                    Account bankInfo =
                        _repository.Query<Account>(
                            a => a.AccountId == complexDeposit.AccountId, b => b.Bank, t => t.AccountType,
                            tt => tt.TransactionType).FirstOrDefault();

                    foreach (Container container in complexDeposit.Containers)
                    {
                        foreach (ContainerDrop drop in container.ContainerDrops)
                        {
                            if (drop.StatusId == statusId || drop.StatusId == resubmittedStatusId)
                            {
                                settlementFileList.Add(new SettlementFile
                                {
                                    TransactionId = complexDeposit.CashDepositId,
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

        private IEnumerable<SettlementFile> GenerateFromSimpleFiles(IEnumerable<CashDeposit> simpleDeposits,
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
                        select new SettlementFile
                        {
                            TransactionId = cashDeposit.CashDepositId,
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

    }
}