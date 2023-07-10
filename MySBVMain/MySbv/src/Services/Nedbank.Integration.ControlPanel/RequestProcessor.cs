using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.Globalization;
using System.Linq;
using System.Transactions;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Nedbank.Integration.ControlPanel.MetaData;
using Nedbank.Integration.FileCreator;
using Nedbank.Integration.Request.Generator;
using Ninject;
using Quartz;

namespace Nedbank.Integration.ControlPanel
{
    [Export(typeof (IJob))]
    [ProcessorType(ProcessorType = ProcessorType.Input)]
    public class RequestProcessor : IJob
    {
        #region Fields

        private int _batchFileId;
        private int _numberOfTransactions;
        public static IFileCreator _fileCreator { get; set; }
        public static IRepository _repository { get; set; }
        public static IRequestWriter _request { get; set; }

        #endregion
        
        #region Constructor

        [ImportingConstructor]
        public RequestProcessor()
        {
            IKernel kernel = new StandardKernel(new Bindings());
            _repository = kernel.Get<IRepository>();
            _fileCreator = kernel.Get<IFileCreator>();
            _request = kernel.Get<IRequestWriter>();
            this.Log().Debug("Nedbank Control Panel Initialized...");
        }

        #endregion
        
        #region IJob

        /// <summary>
        /// Run scheduled Job on Request Processor.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.Log().Debug(() =>
                {
                    var fileType = _repository.Query<NedbankInstructionFileType>(e => e.LookupKey == "TRANSACTION_INSTRUCTION").FirstOrDefault();

                    GetInstructionDetails(fileType.FileType);
                }, 
                "Generating Instruction File On Scheduled Time...");
            }
            catch (Exception ex)
            {
                const string exception = "Exception On Method : [Nedbank.Integration.ControlPanel.RequestProcessor.[IJob].Execute]";
                this.Log().Fatal(exception, ex);
            }
        }

        #endregion
        
        #region Request Methods

        /// <summary>
        /// Get transactions to send to Nedbank
        /// for Settlement.
        /// Details are also written to the Scheduler Log.
        /// </summary>
        private void GetInstructionDetails(string fileType)
        {
            this.Log().Debug(() =>
            {
                GetSettlementRecords(fileType);
            }, "Getting Transactions to settle...");

            this.Log().Debug(() =>
            {
                WriteSchedulerLog();
            }, "Writing Scheduler Log...");
        }



        /// <summary>
        /// Only available Nedbank transactions ready for settlement
        /// are picked.
        /// </summary>
        private void GetSettlementRecords(string fileType)
        {
            try
            {
                var batch = _fileCreator.BatchInitialiser(fileType);
                _request.CollectRecordsToSettle(ref batch);

                var records = batch.NedbankFileItems;

                if (records.Any())
                {
                    this.Log().Debug(() =>
                    {
                        _numberOfTransactions = records.Count();
                        LogTransactions(records, batch);
                        _fileCreator.GenerateBatchFile(ref batch);
                        _batchFileId = batch.NedbankBatchFileId;
                        _request.UpdateSettlementStatus();
                    },
                        string.Format("Number of deposits to be settled {0}", _numberOfTransactions));
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GET TRANSACTIONS READY FOR SETTLEMENT]", ex);
            }
        }


        /// <summary>
        /// Write to the Scheduler Log.
        /// </summary>
        private void WriteSchedulerLog()
        {
            try
            {
                if (_batchFileId > 0)
                {
                    var date = DateTime.Now;

                    string query = @"INSERT INTO [Nedbank].[Scheduler] ([LastRan],[NedbankBatchFileId],[NumberOfDepositsSent],[IsNotDeleted],[LastChangedById],[CreatedById],[CreateDate],[LastChangedDate]) 
                                VALUES('" + date + "', " + _batchFileId + ", " + _numberOfTransactions + ", " + 1 +
                                   ", " + 1 + ", " + 1 + ", '" + date + "', '" + date + "')";

                    _repository.ExecuteQuery(query);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [WRITE SCHEDULER LOG]", ex);
            }
        }


        /// <summary>
        /// Log validated Transactions for settlement.
        /// Ones that did not pass validation are left out.
        /// </summary>
        /// <param name="records"></param>
        /// <param name="batch"></param>
        /// <returns></returns>
        private void LogTransactions(IEnumerable<NedbankFileItem> records, NedbankBatchFile batch)
        {
            int count = 0;
            var totalNumberOfTransactions = records.Count();

            this.Log().Debug(string.Format("Total number of records [{0}]", totalNumberOfTransactions));

            foreach (var record in records)
            {
                try
                {
                    if (IsValidSettlement(record, batch))
                    {
                        count++;

                        this.Log()
                            .Debug(
                                string.Format("[{0}]. Settlement File [RECORD_IDENTIFIER - {1}] [NOMINATED_ACC_NO - {2}] [PAYMENT_REFERENCE - {3}] [DESTINATION_BRANCH_CODE - {4}] [DESTINATION_ACC_NO - {5}] [TRANS_AMT - {6:N}] [VALUE - {7}] [REFERENCE - {8}] [ACCOUNT_HOLDER - {9}] [BANK - {10}]\n",
                                    count,
                                    record.RecordIdentifier.Trim(),
                                    record.NominatedAccountNumber.Trim(),
                                    record.PaymentReferenceNumber.Trim(),
                                    record.DestinationBranchCode.Trim(),
                                    record.DestinationAccountNumber.Trim(),
                                    Convert.ToDecimal(record.Amount) / 100,
                                    record.SettlementIdentifier.Trim(),
                                    record.Reference.Trim(),
                                    record.DestinationAccountHoldersName.Trim(),
                                    GetBank(record.AccountId)
                                    )); 
                    }
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("Exception On Method : [LOG TRANSACTIONS]", ex);
                }
            }
        }


        /// <summary>
        /// Get bank name given account Id
        /// </summary>
        /// <param name="accountId"></param>
        private string GetBank(int accountId)
        {
            try
            {
                var account = _repository.Query<Account>(e => e.AccountId == accountId, o => o.Bank).FirstOrDefault();
                return account.Bank.Name;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var error in errors.ValidationErrors)
                    {
                        this.Log().Fatal(error);                        
                    }
                }
                throw new DbEntityValidationException("Account Entity Validation Exception:", ex);
            }
            catch (Exception ex)
            {
                this.Log().Fatal(ex);
                throw new Exception("Bank Not found:",ex);
            }
        }


        /// <summary>
        /// Validate Settlement
        /// </summary>
        /// <param name="settlementRecord"></param>
        /// <param name="batch"></param>
        private bool IsValidSettlement(NedbankFileItem settlementRecord, NedbankBatchFile batch)
        {
            try
            {
                if (string.IsNullOrEmpty(settlementRecord.SettlementIdentifier.ToString(CultureInfo.InvariantCulture)) ||
                    string.IsNullOrWhiteSpace(settlementRecord.SettlementIdentifier.ToString(CultureInfo.InvariantCulture)))
                    throw new ArgumentNullException(string.Format(
                        "Settlement File PropertyName [TransactionId] Error [{0}]", settlementRecord.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementRecord.RecordIdentifier.ToString(CultureInfo.InvariantCulture)) ||
                    string.IsNullOrWhiteSpace(settlementRecord.RecordIdentifier.ToString(CultureInfo.InvariantCulture)))
                    throw new ArgumentNullException(string.Format(
                        "Settlement File PropertyName [Record Identifier] Error [{0}]", settlementRecord.RecordIdentifier));

                if (string.IsNullOrEmpty(batch.NedbankHeaderRecord.ClientProfileNumber.ToString(CultureInfo.InvariantCulture)) ||
                    string.IsNullOrWhiteSpace(batch.NedbankHeaderRecord.ClientProfileNumber.ToString(CultureInfo.InvariantCulture)))
                    throw new ArgumentNullException(string.Format(
                        "Settlement File PropertyName [Client Profile Number] Error [{0}]", batch.NedbankHeaderRecord.ClientProfileNumber));

                if (string.IsNullOrEmpty(settlementRecord.DestinationAccountHoldersName) || string.IsNullOrWhiteSpace(settlementRecord.DestinationAccountHoldersName))
                    throw new ArgumentNullException(
                        string.Format("Settlement File PropertyName [Destination Account Holder's Name] Error [{0}] for Cash Deposit with id {1}",
                            settlementRecord.DestinationAccountHoldersName, settlementRecord.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementRecord.DestinationAccountNumber) ||
                    string.IsNullOrWhiteSpace(settlementRecord.DestinationAccountNumber))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Destination Account Number] Error [{0}] for Cash Deposit with id {1}",
                            settlementRecord.DestinationAccountNumber, settlementRecord.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementRecord.DestinationBranchCode) || string.IsNullOrWhiteSpace(settlementRecord.DestinationBranchCode))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Branch Code] Error [{0}] for Cash Deposit with id {1}",
                            settlementRecord.DestinationBranchCode, settlementRecord.SettlementIdentifier));

                if (string.IsNullOrEmpty(batch.NedbankHeaderRecord.FileType) ||
                    string.IsNullOrWhiteSpace(batch.NedbankHeaderRecord.FileType))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [File Type] Error [{0}] for Cash Deposit with id {1}",
                            batch.NedbankHeaderRecord.FileType, settlementRecord.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementRecord.TransactionType) ||
                    string.IsNullOrWhiteSpace(settlementRecord.TransactionType))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Transaction Type] Error [{0}] for Cash Deposit with id {1}",
                            settlementRecord.TransactionType, settlementRecord.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementRecord.Amount) ||
                    string.IsNullOrWhiteSpace(settlementRecord.Amount))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Amount] Error [{0}] for Cash Deposit with id {1}",
                            settlementRecord.Amount, settlementRecord.SettlementIdentifier));
                return true;
            }
            catch (Exception ex)
            {
                this.Log().Info("FATAL EXCEPTION on Method: [IS VALID SETTLEMENT] => NEDBANK.Request.Processor", ex);
                return false;
            }
        }

        #endregion
    }
}