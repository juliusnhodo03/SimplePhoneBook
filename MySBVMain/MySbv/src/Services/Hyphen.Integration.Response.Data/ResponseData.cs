using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;

namespace Hyphen.Integration.Response.Data
{
    /// <summary>
    /// Handles the response file from Hyphen after all data has be read
    /// </summary>
    [Export(typeof (IResponseData))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ResponseData : IResponseData
    {
        #region Fields

        ///// <summary>
        ///// A dictionary of settled deposits
        ///// </summary>
        //private readonly IDictionary<string, List<DepositSettlement>> _depositPayments;

        /// <summary>
        /// A dictionary of settled deposits
        /// </summary>
        private List<DepositSettlement> _depositPayments;

        /// <summary>
        /// Rejected deposits indicator
        /// </summary>
        private bool _hasRejectedDeposits;

        /// <summary>
        /// An IRepository instance
        /// </summary>
        public IRepository _repository { get; set; }

        /// <summary>
        /// An ILookup Instance
        /// </summary>
        public ILookup _lookup { get; set; }

        /// <summary>
        /// IMsmqConnector Instance
        /// </summary>
        public IMsmqConnector _msmqConnector { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="lookup"></param>
        /// <param name="msmqConnector"></param>
        [ImportingConstructor]
        public ResponseData(
             IRepository repository,
             ILookup lookup,
             IMsmqConnector msmqConnector)
        {
            _repository = repository;
            _lookup = lookup;
            _msmqConnector = msmqConnector;
            //_depositPayments = new Dictionary<string, List<DepositSettlement>>();
            _depositPayments = new List<DepositSettlement>();
        }

        #endregion

        #region IResponse Data

        ///// <summary>
        ///// Add a deposit settlement object to settlement object list
        ///// </summary>
        ///// <param name="settlementIdentifier"></param>
        ///// <param name="settlementDetails"></param>
        //public void AddDeposit(string settlementIdentifier, DepositSettlement settlementDetails)
        //{
        //    if (!_depositPayments.ContainsKey(settlementIdentifier))
        //    {
        //        _depositPayments.Add(new KeyValuePair<string, List<DepositSettlement>>(settlementIdentifier,
        //            new List<DepositSettlement> { settlementDetails }));
        //    }
        //    else
        //    {
        //        _depositPayments[settlementIdentifier].Add(settlementDetails);
        //    }
        //}


        /// <summary>
        /// Add a deposit settlement object to settlement object list
        /// </summary>
        /// <param name="settlementDetails"></param>
        public void AddDeposit(List<DepositSettlement> settlementDetails)
        {
            _depositPayments = settlementDetails;
        }

        /// <summary>
        /// Update All Transaction Tables
        /// </summary>
        public void UpdateDeposit()
        {
            try
            {
                if (_depositPayments.Count > 0 && !_hasRejectedDeposits) {_hasRejectedDeposits = true;}

                foreach (var depositPayment in _depositPayments)
                {
                    string settlementIdentifier = depositPayment.SettlementIdentifier;
                    string startString = settlementIdentifier.Substring(0, 2);

                    switch (startString)
                    {
                        case "CD":
                            UpdateCashDeposit(depositPayment, settlementIdentifier);
                            break;
                        case "MD":
                            UpdateContainerDrop(depositPayment);
                            break;
                        case "VP":
                            UpdateVaultPartialPayments(depositPayment, settlementIdentifier);
                            break;
                        default:
                            this.Log().Fatal(string.Format("Invalid table identifier supplied [{0}]", startString), new ArgumentException("Invalid table identifier."));
                            break;
                    }
                }
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
                                    "DbEntityValidationException On Method : [UPDATE DEPOSIT] {0}",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE DEPOSIT]", ex);
            }
        }

        /// <summary>
        /// Update Cash Deposit for Single and Multi Drop Deposit
        /// </summary>
        /// <param name="depositPayment"></param>
        /// <param name="settlementIdentifier"></param>
        private void UpdateCashDeposit(DepositSettlement depositPayment, string settlementIdentifier)
        {
            try
            {
                CashDeposit cashDeposit =
                    _repository.Query<CashDeposit>(a => a.SettlementIdentifier == settlementIdentifier,
                        T => T.DepositType,
                        T => T.Device).FirstOrDefault();

                if (cashDeposit != null)
                {
                    // For Single and Multi Drops, the will always be 1 deposit Settlement Detail record
                    DepositSettlement depositDetails = depositPayment;

                    if (cashDeposit.VaultSource != "MYSBV")
                        depositDetails.DeviceSerial = cashDeposit.Device.SerialNumber;

                    int errorCodeId = _lookup.GetErrorCodeId(depositDetails.ErrorCode);

                    cashDeposit.StatusId = depositDetails.IsSettled
                        ? _lookup.GetStatusId("SETTLED")
                        : _lookup.GetStatusId("SETTLEMENT_REJECTED");

                    cashDeposit.Status = null;
                    cashDeposit.IsSettled = depositDetails.IsSettled;
                    cashDeposit.SettledDateTime = depositDetails.ResponseProcessDateTime;
                    cashDeposit.ErrorCodeId = (errorCodeId != -1) ? errorCodeId : new int?();
                    cashDeposit.LastChangedDate = DateTime.Now;
                    cashDeposit.EntityState = State.Modified;
                    _repository.Update(cashDeposit);

                    MappAndAddToQueue(depositDetails);
                }
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
                                    "DbEntityValidationException On Method : [UPDATE CASH DEPOSIT] {0}",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE CASH DEPOSIT]", ex);
                throw;
            }
            
        }

        /// <summary>
        /// Update Container Drop for Multi Deposits
        /// </summary>
        /// <param name="detail"></param>
        private void UpdateContainerDrop(DepositSettlement detail)
        {
            try
            {
                ContainerDrop drop = _repository.Query<ContainerDrop>(a => a.ReferenceNumber == detail.ReferenceNumber)
                    .FirstOrDefault();

                if (drop != null)
                {
                    int errorCodeId = _lookup.GetErrorCodeId(detail.ErrorCode);

                    drop.StatusId = detail.IsSettled
                        ? _lookup.GetStatusId("SETTLED")
                        : _lookup.GetStatusId("SETTLEMENT_REJECTED");

                    drop.Status = null;
                    drop.SettlementDateTime = detail.ResponseProcessDateTime;
                    drop.ErrorCodeId = (errorCodeId != -1) ? errorCodeId : new int?();
                    drop.LastChangedDate = DateTime.Now;
                    drop.EntityState = State.Modified;
                    _repository.Update(drop);

                }
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
                                    "DbEntityValidationException On Method : [UPDATE CONTAINER DROP] {0}",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE CONTAINER DROP]", ex);
                throw;
            }
        }

        /// <summary>
        /// Update VaultPartialPayment for partial payments.
        /// </summary>
        /// <param name="depositPayment"></param>
        /// <param name="settlementIdentifier"></param>
        private void UpdateVaultPartialPayments(DepositSettlement depositPayment, string settlementIdentifier)
        {
            try
            {
                VaultPartialPayment vaultPartialPayment = _repository.Query<VaultPartialPayment>(a => a.SettlementIdentifier == settlementIdentifier).FirstOrDefault();

                if (vaultPartialPayment != null)
                {
                    // For Single and Multi Drops, the will always be 1 deposit Settlement Detail record
                    depositPayment.DeviceSerial = vaultPartialPayment.DeviceSerialNumber;

                    int errorCodeId = _lookup.GetErrorCodeId(depositPayment.ErrorCode);

                    vaultPartialPayment.StatusId = depositPayment.IsSettled
                        ? _lookup.GetStatusId("SETTLED")
                        : _lookup.GetStatusId("SETTLEMENT_REJECTED");

                    vaultPartialPayment.Status = null;
                    vaultPartialPayment.SettlementDate = depositPayment.ResponseProcessDateTime;
                    vaultPartialPayment.ErrorCodeId = (errorCodeId != -1) ? errorCodeId : new int?();
                    vaultPartialPayment.LastChangedDate = DateTime.Now;
                    vaultPartialPayment.EntityState = State.Modified;
                    _repository.Update(vaultPartialPayment);

                    MappAndAddToQueue(depositPayment);
                }

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
                                    "DbEntityValidationException On Method : [UPDATE VAULT PARTIAL PAYMENT] {0}",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE VAULT PARTIAL PAYMENT]", ex);
                throw;
            }
        }

        /// <summary>
        /// Check whether there's any rejected deposits.
        /// </summary>
        public bool HasRejectedDeposits { get { return _hasRejectedDeposits; } }

        #endregion

        #region Helpers

        private void MappAndAddToQueue(DepositSettlement depositSettlement)
        {
            if( depositSettlement.DeviceSerial == null) return;
            
            var settlementResponse = new SettlementResponse
            {
                AccountHolder = depositSettlement.AccountHolder,
                AccountNumber = depositSettlement.AccountNumber,
                DepositAmount = depositSettlement.DepositAmount,
                DeviceSerial = depositSettlement.DeviceSerial,
                ErrorCode = depositSettlement.ErrorCode,
                iTramsReference = depositSettlement.iTramsReference,
                ReferenceNumber = depositSettlement.ReferenceNumber,
                IsSettled = depositSettlement.IsSettled,
                ResponseProcessDateTime = depositSettlement.ResponseProcessDateTime,
                Label = "Settlement_Message"
            };
            var result = _msmqConnector.AddMessage(QueueIdentifier.Response, settlementResponse);

            if (result.Status == MethodStatus.Error)
            {
                this.Log().Fatal(string.Format("Error adding message to queue - [{0}]",result.Message));
            }
        }
        #endregion

    }
}