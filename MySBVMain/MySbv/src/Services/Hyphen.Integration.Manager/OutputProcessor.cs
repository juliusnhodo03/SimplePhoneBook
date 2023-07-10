using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using Domain.Data.Hyphen.Model;
using Domain.Repository;
using Hyphen.Integration.Facs;
using Hyphen.Integration.Manager.Infrastructure;
using Hyphen.Integration.Request.Data;
using Infrastructure.Logging;
using Ninject;
using Quartz;

namespace Hyphen.Integration.Manager
{
    [Export(typeof (IJob))]
    [ProcessorType(ProcessorType = ProcessorType.Output)]
    public class OutputProcessor : IJob
    {
        #region Fields

        public static IFacs _facs { get; set; }
        public static IRepository _repository { get; set; }
        public static IRequestData _requestData { get; set; }
        private int _batchFileId;
        private int _numberOfTransactions;

        #endregion

        #region Constructor

        //[ImportingConstructor]
        //public OutputProcessor(
        //     [Import(AllowDefault = true)] IRepository repository,
        //     [Import(AllowDefault = true)] IFacs facs,
        //     [Import(AllowDefault = true)] IRequestData requestData)
        //{
        //    _facs = facs;
        //    _requestData = requestData;
        //    _repository = repository;
        //    this.Log().Debug("Manager Initialized");
        //}

        [ImportingConstructor]
        public OutputProcessor()
        {
            IKernel kernel = new StandardKernel(new Bindings());
            _repository = kernel.Get<IRepository>();
            _facs = kernel.Get<IFacs>();
            _requestData = kernel.Get<IRequestData>();
            this.Log().Debug("Manager Initialized");
        }

        #endregion

        #region IJob

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.Log().Debug(() => { CreateFacsFile(); }, "Execute Create File On Timer.Elapsed.");
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal("Exception On Method : [Hyphen.Integration.Manager.OutputProcessor.[IJob].Execute]", ex);
            }
        }

        #endregion

        #region Internal

        private void CreateFacsFile()
        {
            this.Log().Debug(() => { SettleTransactions(); }, "Executing Settle Transactions.");
            this.Log().Debug(() => { WriteSchedulerLog(); }, "Executing Write Scheduler Log.");
        }

        private void SettleTransactions()
        {
            try
            {
                IEnumerable<SettlementTransaction> transactions = _requestData.GetSettlementTransactions();
                SettlementTransaction[] settlementFiles = transactions as SettlementTransaction[] ??
                                                          transactions.ToArray();

                if (settlementFiles.Any())
                {
                    _numberOfTransactions = settlementFiles.Count();
                    this.Log().Debug(() =>
                    {
                        BatchFile batchFile = CreateHyphenBatchFile(settlementFiles);
                        _facs.CreateBatchFile(ref batchFile);
                        _batchFileId = batchFile.BatchCount;
                        _requestData.UpdateSettlementStatus();
                    }, string.Format("Number of deposits to be settled {0}", _numberOfTransactions));
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [SETTLE TRANSACTIONS]", ex);
            }
        }

        private void WriteSchedulerLog()
        {
            try
            {
                string query = "INSERT INTO HyphenScheduler ([LastRan],[batchNumber],[NumberOfDepositsSent],[IsNotDeleted],[LastChangedById],[CreatedById],[CreateDate],[LastChangedDate]) VALUES "
                               + "( '" + DateTime.Now + "', " + _batchFileId + ", " + _numberOfTransactions + ", " + 1 +
                               ", " + 1 + ", " + 1 + ", '" + DateTime.Now + "', '" + DateTime.Now + "')";
                _repository.ExecuteQuery(query);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [WRITE SCHEDULER LOG]", ex);
            }
        }

        private BatchFile CreateHyphenBatchFile(IEnumerable<SettlementTransaction> transactions)
        {
            var transactionDetailRecords = new Collection<TransactionDetailRecord>();
            int count = 0;

            this.Log().Debug(string.Format("Total number of records [{0}]", transactions.Count()));

            foreach (SettlementTransaction settlementFile in transactions)
            {
                try
                {
                    if (IsValid(settlementFile))
                    {
                        count++;
                        this.Log()
                            .Debug(
                                string.Format(
                                    "[{0}]. Settlement File [PAYEE - {1}] [BANK_ACC - {2}] [BRANCH_CODE - {3}] [ACC_TYPE - {4}] [TRANS_TYPE - {5}] [TRANS_AMT - {6}] [VALUE - {7}] [CODE2 - {8}] [USR_REF2 - {9}]\n",
                                    count, settlementFile.Payee, settlementFile.BankAccountNumber, settlementFile.BranchCode,
                                    settlementFile.AccountType, settlementFile.TransactionType,
                                    settlementFile.TransactionAmount, settlementFile.SettlementIdentifier,
                                    settlementFile.SbvDepositReference, settlementFile.ClientReference));

                        transactionDetailRecords.Add(new TransactionDetailRecord
                        {
                            Payee = settlementFile.Payee,
                            BankAccountNumber = settlementFile.BankAccountNumber,
                            BranchCode = settlementFile.BranchCode,
                            AccountType = settlementFile.AccountType,
                            TransactionType = settlementFile.TransactionType,
                            TransactionAmount = settlementFile.TransactionAmount,
                            Value = settlementFile.SettlementIdentifier.ToString(CultureInfo.InvariantCulture),
                            Code2 = settlementFile.SbvDepositReference,
                            UserReference2 = settlementFile.ClientReference,
                            CreateDate = DateTime.Now,
                            LastChangedDate = DateTime.Now,
                            LastChangedById = 1,
                            CreatedById = 1,
                            IsNotDeleted = true
                        });
                    }
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("Exception On Method : [CREATE HYPHEN BATCH FILE]", ex);
                }
            }

            return new BatchFile
            {
                TransactionDetailRecords = transactionDetailRecords,
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1
            };
        }

        private bool IsValid(SettlementTransaction settlementFile)
        {
            try
            {
                if (string.IsNullOrEmpty(settlementFile.SettlementIdentifier.ToString(CultureInfo.InvariantCulture)) ||
                    string.IsNullOrWhiteSpace(settlementFile.SettlementIdentifier.ToString(CultureInfo.InvariantCulture)))
                    throw new ArgumentNullException(string.Format(
                        "Settlement File PropertyName [TransactionId] Error [{0}]", settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.Payee) || string.IsNullOrWhiteSpace(settlementFile.Payee))
                    throw new ArgumentNullException(
                        string.Format("Settlement File PropertyName [Payee] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.Payee, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.BankAccountNumber) ||
                    string.IsNullOrWhiteSpace(settlementFile.BankAccountNumber))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Bank Account Number] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.BankAccountNumber, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.BranchCode) || string.IsNullOrWhiteSpace(settlementFile.BranchCode))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Branch Code] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.BranchCode, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.AccountType) ||
                    string.IsNullOrWhiteSpace(settlementFile.AccountType))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Account Type] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.AccountType, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.TransactionType) ||
                    string.IsNullOrWhiteSpace(settlementFile.TransactionType))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Transaction Type] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.TransactionType, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.TransactionAmount) ||
                    string.IsNullOrWhiteSpace(settlementFile.TransactionAmount))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Transaction Amount] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.TransactionAmount, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.SbvDepositReference) ||
                    string.IsNullOrWhiteSpace(settlementFile.SbvDepositReference))
                    throw new ArgumentNullException(
                        string.Format(
                            "Settlement File PropertyName [Transaction Reference] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.SbvDepositReference, settlementFile.SettlementIdentifier));

                if (string.IsNullOrEmpty(settlementFile.ClientReference) ||
                    string.IsNullOrWhiteSpace(settlementFile.ClientReference))
                    throw new ArgumentNullException(
                        string.Format("Settlement File PropertyName [Narrative] Error [{0}] for Cash Deposit with id {1}",
                            settlementFile.ClientReference, settlementFile.SettlementIdentifier));
                return true;
            }
            catch(Exception ex)
            {
                this.Log().Info("FATAL EXCEPTION on Method: [IS VALID] => HYPHEN", ex);
                return false;
            }
        }

        #endregion
    }
}