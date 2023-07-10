using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Notifications;
using Domain.Repository;
using Domain.Security;
using Infrastructure.Logging;
using Nedbank.Integration.FileUtilities;
using Utility.Core;

namespace Nedbank.Integration.SettlementHub
{
    [Export(typeof (ISettlement))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Settlement : ISettlement
    {
        #region Fields

        public IRepository _repository { get; set; }
        public ILookup _lookup { get; set; }
        public IFileUtility _fileUtility { get; set; }

        private static INotification _notification;

        private static ISecurity _security;

        #endregion


        #region Constructor

        [ImportingConstructor]
        public Settlement(IRepository repository, ILookup lookup, IFileUtility fileUtility, INotification notification, ISecurity security)
        {
            _repository = repository;
            _lookup = lookup;
            _fileUtility = fileUtility;
            _notification = notification;
            _security = security;
        }

        #endregion
        

        #region ISettlement

        /// <summary>
        /// Apply settlements to Cash deposits from ACK, NACK, DUPLICATE, SECOND-SCK Files.
        /// </summary>
        /// <param name="details"></param>
        public void ApplySettlements(IEnumerable<NedbankResponseDetail> details) 
        {
            try
            {
                foreach (var detail in details)
                {
                    var fileItem = _repository.Query<NedbankFileItem>(e =>
                        e.PaymentReferenceNumber == detail.PaymentReferenceNumber)
                        .FirstOrDefault();
                    
                    if (fileItem != null)
                    {
                        var settlementIdentifier = fileItem.SettlementIdentifier;
                        Save(detail.TransactionStatus, settlementIdentifier, detail.ResponseFilename);
                    }
                }
                // send email on rejections
                var hasRejections = details.Any(e => e.TransactionStatus.ToUpper() == FileStatus.REJECTED.Name());

                SendEmailOnRejections(hasRejections);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [APPLY ACKNOWLEDGED SETTLEMENTS] => [SETTLEMENT HUB]", ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionStatus"></param>
        /// <param name="settlementIdentifier"></param>
        /// <param name="responseFilename"></param>
        private void Save(string transactionStatus, string settlementIdentifier, string responseFilename)
        {
            string startString = settlementIdentifier.Substring(0, 2);

            switch (startString)
            {
                case "CD":
                    SettleCashDeposits(transactionStatus, settlementIdentifier, responseFilename);
                    break;
                case "MD":
                    SettleMultiDeposits(transactionStatus, settlementIdentifier, responseFilename);
                    break;
                case "VP":
                    SettlePartialPayments(transactionStatus, settlementIdentifier, responseFilename);
                    break;
                default:
                    this.Log().Fatal(String.Format("Invalid table identifier supplied [{0}]", startString), new ArgumentException("Invalid table identifier."));
                    break;
            }
        }



        /// <summary>
        /// All unpaid transactions are set to SETTLEMENT_REJECTED.
        /// The UnPaid Reason is also supplied.
        /// </summary>
        /// <param name="unpaids"></param>
        public void ApplyUnPaidRejections(IEnumerable<NedbankUnpaidOrNaedo> unpaids)
        {
            try
            {
                foreach (var detail in unpaids)
                {
                    var fileItem =
                        _repository.Query<NedbankFileItem>(e => e.PaymentReferenceNumber == detail.PaymentReferenceNumber)
                            .FirstOrDefault();

                    if (fileItem != null)
                    {
                        var settlementIdentifier = fileItem.SettlementIdentifier;
                        Save(detail.Status, settlementIdentifier, detail.ResponseFilename);
                    }
                }

                // send email on rejections
                var hasRejections = unpaids.Any(e => e.Status.ToUpper() == FileStatus.REJECTED.Name());
                //var unpaid = unpaids.Any(e => e.ResponseFilename.ToUpper() == FileType.UNPAIDS.Name());

                //var subject = string.Format("Rejected Deposits: '{0}' File Notification", unpaid ? "UNPAIDs" : "NAEDOs");

                SendEmailOnRejections(hasRejections);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [APPLY UNPAIDS REJECTIONS] => [SETTLEMENT HUB]", ex);
            }            
        }



        /// <summary>
        /// Apply duplicate rejections.
        /// First see see if another file ran updates before.
        /// if applied just log a duplicate entry without updating deposits, otherwise update.
        /// </summary>
        /// <param name="duplicateFile"></param>
        public void ApplyDuplicateRejections(NedbankDuplicate duplicateFile)
        {
            try
            {
                var fileDetails = from i in _repository.All<NedbankFileItem>()
                                  join b in _repository.All<NedbankBatchFile>() on i.NedbankBatchFileId equals b.NedbankBatchFileId
                                  join h in _repository.All<NedbankHeaderRecord>() on b.NedbankHeaderRecordId equals
                                      h.NedbankHeaderRecordId
                                  where h.FileSequenceNumber == duplicateFile.FileSequenceNumber
                                  select i;

                // get settled status
                var status = _repository.Query<Status>(e => e.LookUpKey == "SETTLED").FirstOrDefault();

                foreach (var fileItem in fileDetails)
                {
                    var identifier = fileItem.SettlementIdentifier;

                    string startString = identifier.Substring(0, 2);

                    switch (startString)
                    {
                        case "CD":
                            var cashDeposit = _repository.Query<CashDeposit>(e => e.SettlementIdentifier == identifier).FirstOrDefault();

                            if (status.StatusId != cashDeposit.StatusId)
                            {
                                SettleCashDeposits(FileStatus.REJECTED.Name(), identifier, FileType.DUPLICATE.Name());
                            }
                            break;
                        case "MD":
                            var multiDeposit = _repository.Query<ContainerDrop>(e => e.SettlementIdentifier == identifier).FirstOrDefault();

                            if (status.StatusId != multiDeposit.StatusId)
                            {
                                SettleMultiDeposits(FileStatus.REJECTED.Name(), identifier, FileType.DUPLICATE.Name());
                            }
                            break;
                        case "VP":
                            var vaultDeposit = _repository.Query<VaultPartialPayment>(e => e.SettlementIdentifier == identifier).FirstOrDefault();

                            if (status.StatusId != vaultDeposit.StatusId)
                            {
                                SettlePartialPayments(FileStatus.REJECTED.Name(), identifier, FileType.DUPLICATE.Name());
                            }
                            break;
                        default:
                            this.Log().Fatal(String.Format("Invalid table identifier supplied [{0}]", startString), new ArgumentException("Invalid table identifier."));
                            break;
                    }
                }

                if (duplicateFile != null)
                {
                    //const string subject = "Rejected Deposits: 'DUPLICATE' File Notification";
                    SendEmailOnRejections(true);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [APPLY DUPLICATE REJECTIONS] => [SETTLEMENT HUB]", ex);
            }
        }

        #endregion


        #region Helpers

        /// <summary>
        /// Send email on rejections found.
        /// </summary>
        /// <param name="hasRejections"></param>
        private void SendEmailOnRejections(bool hasRejections)
        {
            if (hasRejections)
            {
                try
                {
                    string path = string.Concat(@"EmailTemplates\", "RejectedDeposits.html");

                    var recepients = GetEmailRecepients().ToArray();

                    _notification.SendEmail("Rejected MYSBV Payments", EmailTemplate.RejectedDeposits,
                        null, path,
                        recepients);
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("Exception On Method : [SEND EMAIL ON REJECTIONS] => [SETTLEMENT HUB]", ex);
                }
            }
        }

        /// <summary>
        /// Settle cash deposits.
        /// </summary>
        /// <param name="transactionStatus"></param>
        /// <param name="settlementIdentifier"></param>
        /// <param name="responseFilename"></param>
        private void SettleCashDeposits(string transactionStatus, string settlementIdentifier, string responseFilename)
        {
            try
            {
                var cashDeposit = _repository.Query<CashDeposit>(a => a.SettlementIdentifier == settlementIdentifier,
                            T => T.DepositType,
                            T => T.Device).FirstOrDefault();

                if (cashDeposit != null)
                {
                    string status = GetSettlementStatus(transactionStatus, responseFilename);

                    cashDeposit.StatusId = _lookup.GetStatusId(status);

                    cashDeposit.Status = null;
                    cashDeposit.IsSettled = transactionStatus.ToUpper() == FileStatus.ACCEPTED.Name();
                    cashDeposit.SettledDateTime = DateTime.Now;
                    cashDeposit.LastChangedDate = DateTime.Now;
                    cashDeposit.EntityState = State.Modified;
                    _repository.Update(cashDeposit);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [SETTLE CASH DEPOSITS] => [SETTLEMENT HUB]", ex);
            }
        }


        /// <summary>
        /// get settlement status
        /// </summary>
        /// <param name="transactionStatus"></param>
        /// <param name="responseFilename"></param>
        private string GetSettlementStatus(string transactionStatus, string responseFilename)
        {
            string status;

            if (responseFilename.ToUpper() == FileType.NACK.Name() ||
                responseFilename.ToUpper() == FileType.DUPLICATE.Name() ||
                responseFilename.ToUpper() == FileType.UNPAIDS.Name())
            {
                status = "SETTLEMENT_REJECTED";
            }
            else
            {
                status = transactionStatus.ToUpper() == FileStatus.ACCEPTED.Name()
                                        ? "SETTLED"
                                        : "SETTLEMENT_REJECTED";
            }
            return status;
        }


        /// <summary>
        /// Settle vault partial payments
        /// </summary>
        /// <param name="transactionStatus"></param>
        /// <param name="settlementIdentifier"></param>
        /// <param name="responseFilename"></param>
        private void SettlePartialPayments(string transactionStatus, string settlementIdentifier, string responseFilename)
        {
            try
            {
                var vaultPartialPayment = _repository.Query<VaultPartialPayment>(a => a.SettlementIdentifier == settlementIdentifier).FirstOrDefault();

                if (vaultPartialPayment != null)
                {
                    string recordStatus = GetSettlementStatus(transactionStatus, responseFilename);

                    vaultPartialPayment.StatusId = _lookup.GetStatusId(recordStatus);
                    vaultPartialPayment.Status = null;
                    vaultPartialPayment.SettlementDate = DateTime.Now;
                    vaultPartialPayment.LastChangedDate = DateTime.Now;
                    vaultPartialPayment.EntityState = State.Modified;
                    _repository.Update(vaultPartialPayment);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE VAULT PARTIAL PAYMENT] => [SETTLEMENT HUB]", ex);
            }
        }


        /// <summary>
        /// Settle Multi deposits.
        /// </summary>
        /// <param name="transactionStatus"></param>
        /// <param name="settlementIdentifier"></param>
        /// <param name="responseFilename"></param>
        private void SettleMultiDeposits(string transactionStatus, string settlementIdentifier, string responseFilename)
        {
            try
            {
                var drop = _repository.Query<ContainerDrop>(a => a.SettlementIdentifier == settlementIdentifier)
                            .FirstOrDefault();

                    if (drop != null)
                    {
                        string recordStatus = GetSettlementStatus(transactionStatus, responseFilename);

                        drop.StatusId = _lookup.GetStatusId(recordStatus);
                        drop.Status = null;
                        drop.SettlementDateTime = DateTime.Now;
                        drop.LastChangedDate = DateTime.Now;
                        drop.EntityState = State.Modified;
                        _repository.Update(drop);
                    }
                
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [UPDATE CONTAINER DROP] => [SETTLEMENT HUB]", ex);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        private List<string> GetEmailRecepients()
        {
            const string query = @"SELECT DISTINCT U.EmailAddress
                    FROM [User] U INNER JOIN webpages_UsersInRoles UIR ON U.UserId = UIR.UserId
	                     INNER JOIN webpages_Roles R ON R.RoleId=UIR.RoleId
                    WHERE R.RoleName IN ('SBVAdmin', 'SBVDataCapture', 'SBVApprover') AND (U.EmailAddress IS NOT NULL) AND (u.IsNotDeleted = 1)";

            var recepients = _repository.ExecuteQueryCommand<string>(query);
            return recepients;
        }
        #endregion
    }
}