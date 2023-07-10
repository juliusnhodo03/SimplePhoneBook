using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using Domain.Data.Core;
using Domain.Data.Hyphen.Model;
using Domain.Data.Model;
using Domain.Repository;
using Hyphen.Integration.Response.Data;
using Infrastructure.Logging;
using Utility.Core;

namespace Hyphen.Integration.Facs
{
    [Export(typeof (IFacs))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Facs : IFacs
    {
        #region Fields

        private readonly IResponseData _responseData;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public Facs(IRepository repository, IResponseData responseData)
        {
            _repository = repository;
            _responseData = responseData;
        }

        #endregion

        #region IFacs
        
        public void CreateBatchFile(ref BatchFile batchfileToCreate)
        {
            if (this.Log().IsDebugEnabled)
            {
                this.Log().Debug("Initializing Batch File");
            }

            DateTime now = DateTime.Now;
            string nextBatchNumber = GetNextBatchNumber();
            BatchFile initializedBatchFile = InitializeBatchFile(batchfileToCreate, nextBatchNumber, now);

            if (this.Log().IsDebugEnabled)
            {
                this.Log().Debug("Batch File Initialized");
            }

            this.Log().Debug(() =>
            {
                // use a string builder to construct the file,
                StringBuilder headerBuilder = CreateHeaderRecord(ref initializedBatchFile, nextBatchNumber, now);
                StringBuilder detailBuilder = CreateTransactionDetailRecord(ref initializedBatchFile);
                StringBuilder trailerBuilder = CreateTrailerRecord(ref initializedBatchFile);

                SaveChanges(initializedBatchFile, headerBuilder, detailBuilder, trailerBuilder);
            },"Creating Header, Detail and Trailer Records");
        }

        public void ReadResponseFile(List<MemoryStream> files)
        {
            try
            {
                foreach (MemoryStream file in files)
                {
                    string[] fileLines = ConvertToLineItemArray(file);
                    
                    var transactions = CreateTransactionDetailRecordResponses(fileLines);

                    var responses = transactions as IList<TransactionDetailRecordResponse> ?? transactions.ToList();

                    UpdateResponseData(responses);
                    
                    this.Log().Debug(() =>
                    {
                        foreach (var transactionDetailRecordResponse in responses)
                        {
                            try
                            {
                                _repository.Add(transactionDetailRecordResponse);
                            }
                            catch (Exception ex)
                            {
                                this.Log().Debug(string.Format("Failed Name - [{0}]", transactionDetailRecordResponse.Payee));
                                this.Log().Fatal("Exception On [ADD_TRANSACCTION_DETAIL_RESPONSE]", ex);
                                
                                transactionDetailRecordResponse.Payee = string.Empty;
                                _repository.Add(transactionDetailRecordResponse);
                            }
                        }
                    }, "Saving Response Data");
                        
                    this.Log().Debug(() =>
                    {
                        _responseData.UpdateDeposit();
                    }, "Executing Update Deposit.");
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [READ RESPONSE FILE]", ex);
                throw;
            }
        }

        private LoadReport UpdateLoadReport(BatchFile batchFile, List<TransactionDetailRecordResponse> transactions)
        {
// update the response data
            DateTime transmissionDateTime = GetDate(batchFile.HeaderRecord.TransmissionDate,
                batchFile.HeaderRecord.TransmissionTime);

            // try get existing report otherwise create a new one
            LoadReport loadReport = batchFile.LoadReports.SingleOrDefault(
                x => x.TransmissionDateTime == transmissionDateTime) ??
                                    new LoadReport
                                    {
                                        BatchNumber = batchFile.HeaderRecord.BatchNumber,
                                        CreatedById = 1,
                                        LastChangedById = 1,
                                        EntityState = State.Added,
                                        IsNotDeleted = true
                                    };

            this.Log().Debug(() =>
            {
                int totalReceivedCount =
                    loadReport.TotalReceivedCount = transactions.Count(x => x.ProcessingOption1 == "X"
                                                                            && x.ProcessingOption2 == "X");
                loadReport.TotalReceivedCount = totalReceivedCount;

                // successful transactions
                // Confirmation that transaction is reconciled from bank statement – Money value on bank account
                decimal totalReceivedAmount = transactions.Where(x => x.ProcessingOption1 == "X"
                                                                      && x.ProcessingOption2 == "X")
                    .Select(transtactionDetailRecord => decimal.Parse(transtactionDetailRecord.TransactionAmount))
                    .Aggregate<decimal, decimal>(0, (current, transactionAmount) => current + transactionAmount);

                //// Transaction is reversed by a user on the FACS system if not accompanied by a 2 digit code in ##ERRC
                //var transactionsReversed = transactions.Where(x => x.ProcessingOption1 == "R"
                //                                                   && x.ProcessingOption2 == "R");

                //// Transaction is sent for validation only and will not create on FACS. If the transaction fails the CDV the transaction will be returned accompanied by a 4 digit code in ##ERRC field
                //var transactionsValidationOnly = transactions.Where(x => x.ProcessingOption1 == "V"
                //                                                   && x.ProcessingOption2 == "V");


                // format the amount
                string amount0 = totalReceivedAmount.ToString(CultureInfo.InvariantCulture);

                if (amount0 != "0")
                {
                    string amount1 = amount0.Substring(amount0.Length - 2, 2);
                    string amount2 = amount0.Substring(0, amount0.Length - 2);
                    decimal amount3 = decimal.Parse(amount2 + "." + amount1);

                    loadReport.TotalReceivedAmount = amount3;
                }
                else
                {
                    loadReport.TotalReceivedAmount = 0;
                }

                loadReport.TransmissionDateTime = transmissionDateTime;
            }, "Calculating report values.");
            return loadReport;
        }

        private BatchFile CreateBatchFile(HeaderRecord header)
        {
            var nowDate = DateTime.Now;
            return new BatchFile()
            {
                TransactionDetailRecordResponses = new Collection<TransactionDetailRecordResponse>(),
                LoadReports = new Collection<LoadReport>(),
                HeaderRecord = header,
                DateTimeCreated = nowDate,
                CreateDate = nowDate,
                LastChangedDate = nowDate,
                CreatedById = 1,
                LastChangedById = 1,
                IsNotDeleted = true,
                EntityState = State.Added

            };
        }

        private IEnumerable<TransactionDetailRecordResponse> CreateTransactionDetailRecordResponses(string[] fileLines)
        {
            var transactions = new List<TransactionDetailRecordResponse>();

            for (int i = 1; i < fileLines.Length - 1; i++)
            {
                string line = fileLines[i];
                var nowDate = DateTime.Now;
                var transactionDetail = new TransactionDetailRecordResponse()
                {
                    CreateDate = nowDate,
                    LastChangedDate = nowDate,
                    CreatedById = 1,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    EntityState = State.Added
                };

                // update the batch file record in the DB

                transactionDetail.MessageType = line.Substring(0, 9).Trim();
                transactionDetail.BatchNumber = line.Substring(9, 8).Trim();

                string transactionType = line.Substring(17, 5).Trim();

                transactionDetail.TransactionType = transactionType;

                string documentType = line.Substring(22, 2).Trim();
                transactionDetail.DocumentType = documentType;

                transactionDetail.RequisitionNumber = line.Substring(24, 9).Trim();
                transactionDetail.DocumentNumber = line.Substring(33, 8).Trim();
                transactionDetail.UserReference1 = line.Substring(41, 2).Trim();
                transactionDetail.UserReference2 = line.Substring(43, 20).Trim();
                transactionDetail.ThirdParty = line.Substring(63, 30).Trim();
                transactionDetail.Code1 = line.Substring(93, 20).Trim();
                transactionDetail.Code2 = line.Substring(113, 20).Trim();

                // Read the cash deposit id
                string settlementIdentifier = transactionDetail.Code1.Substring(8).Trim();
                transactionDetail.SettlementIdentifier = settlementIdentifier;
                string amount = line.Substring(133, 11).ToString(CultureInfo.InvariantCulture).Trim();

                transactionDetail.TransactionAmount = amount;

                string nameOfClient = line.Substring(144, 80).Trim();
                transactionDetail.Payee = nameOfClient;
                transactionDetail.ErrorCode = line.Substring(224, 4).Trim();
                transactionDetail.ProcessingOption1 = line.Substring(228, 1).Trim();
                transactionDetail.ProcessingOption2 = line.Substring(229, 1).Trim();
                transactionDetail.ProgramNameCreated = line.Substring(230, 10).Trim();

                string accNr = line.Substring(240, 17).Trim();
                transactionDetail.BankAccountNumber = accNr;

                string branchCode = line.Substring(257, 6).Trim();
                transactionDetail.BranchCode = branchCode;
                transactionDetail.AccountType = line.Substring(263, 1).Trim();
                transactionDetail.AgencyPrefix = line.Substring(264, 1).Trim();
                transactionDetail.AgencyNumber = line.Substring(265, 6).Trim();
                transactionDetail.Blank2 = string.Empty.PadRight(2, ' ').Trim();
                transactionDetail.ChequeClearanceCode = line.Substring(273, 10).Trim();
                transactionDetail.ClientChequeNumber = line.Substring(283, 9).Trim();
                transactionDetail.CashBookBankAccountNumber = line.Substring(292, 17).Trim();
                transactionDetail.UniqueUserCode = line.Substring(309, 4).Trim();
                transactionDetail.ActionDate = line.Substring(313, 10).Trim();
                transactionDetail.INDF = "";
                transactions.Add(transactionDetail);

               // UpdateResponseData(settlementIdentifier, transactionDetail);
            }

            return transactions;
        }

        //private void UpdateResponseData(string settlementIdentifier, TransactionDetailRecordResponse transactionDetail1)
        //{
        //    this.Log().Debug(() =>
        //    {
        //        _responseData.AddDeposit(settlementIdentifier, new DepositSettlement()
        //        {
        //            IsSettled = string.IsNullOrWhiteSpace(transactionDetail1.ErrorCode),
        //            ReferenceNumber = transactionDetail1.Code2,
        //            ErrorCode = transactionDetail1.ErrorCode,
        //            ResponseProcessDateTime = Convert.ToDateTime(transactionDetail1.ActionDate),
        //            AccountNumber = transactionDetail1.BankAccountNumber,
        //            DepositAmount = Convert.ToDouble(transactionDetail1.TransactionAmount),
        //            iTramsReference = transactionDetail1.UserReference2,
        //            AccountHolder = transactionDetail1.Payee
        //        });
        //    }, string.Format("Adding deposit details. Cash Deposit ID [{0}]", settlementIdentifier));
        //}



        private void UpdateResponseData(IEnumerable<TransactionDetailRecordResponse> transactionDetails)
        {
            this.Log().Debug(() =>
            {
                var settlementRecords = new List<DepositSettlement>();

                var responseRecords = transactionDetails as IList<TransactionDetailRecordResponse> ?? transactionDetails.ToList();

                foreach (var response in responseRecords)
                {
                    var hasError = responseRecords.Any(e => e.Code2 == response.Code2 && !string.IsNullOrWhiteSpace(response.ErrorCode));

                    if (hasError)
                    {
                        // Rejected Transactions

                        // NOTE:    Get the error record
                        var errorRecord = responseRecords.FirstOrDefault( e => e.Code2 == response.Code2 && !string.IsNullOrWhiteSpace(response.ErrorCode));
                        
                        // Remove duplication of error record
                        if (settlementRecords.Any(e => e.ReferenceNumber == response.Code2) == false)
                        {
                            var deposit = GetDepositSettlement(errorRecord);
                            settlementRecords.Add(deposit);
                        }
                    }
                    else
                    {
                        // Accepted Transactions
                        var deposit = GetDepositSettlement(response);
                        settlementRecords.Add(deposit);
                    }
                }
                _responseData.AddDeposit(settlementRecords);
            }, "Creating Response Collection");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionDetail"></param>
        private DepositSettlement GetDepositSettlement(TransactionDetailRecordResponse transactionDetail)
        {
            var deposit = new DepositSettlement()
            {
                IsSettled = string.IsNullOrWhiteSpace(transactionDetail.ErrorCode),
                ReferenceNumber = transactionDetail.Code2,
                ErrorCode = transactionDetail.ErrorCode,
                ResponseProcessDateTime = Convert.ToDateTime(transactionDetail.ActionDate),
                AccountNumber = transactionDetail.BankAccountNumber,
                DepositAmount = Convert.ToDouble(transactionDetail.TransactionAmount),
                iTramsReference = transactionDetail.UserReference2,
                AccountHolder = transactionDetail.Payee,
                SettlementIdentifier = transactionDetail.SettlementIdentifier
            };

            return deposit;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="transmissionDate"></param>
        /// <param name="transmissionTime"></param>
        /// <returns></returns>
        private DateTime GetDate(string transmissionDate, string transmissionTime)
        {
            int year = int.Parse(transmissionDate.Substring(0, 4));
            int month = int.Parse(transmissionDate.Substring(5, 2));
            int day = int.Parse(transmissionDate.Substring(8, 2));
            int hour = int.Parse(transmissionTime.Substring(0, 2));
            int minute = int.Parse(transmissionTime.Substring(2, 2));
            int second = int.Parse(transmissionTime.Substring(4, 2));

            return new DateTime(year, month, day, hour, minute, second);
        }

        private string[] ConvertToLineItemArray(MemoryStream file)
        {
            string respons = Encoding.ASCII.GetString(file.ToArray());
            string[] lines = respons.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);

            // Remove the last line as it contains blank
            IEnumerable<string> finalLines = lines.Where(a => !string.IsNullOrEmpty(a));

            return finalLines.ToArray();
        }

        #endregion

        #region CreateHypheBatchFile Helpers

        private string GetNextBatchNumber()
        {
            // Get the next bacthNumber from the DB
            int nextbatchNr =
                _repository.All<BatchFile>()
                    .OrderByDescending(a => a.BatchCount)
                    .Select(b => b.BatchCount)
                    .FirstOrDefault();
            nextbatchNr++;
            return nextbatchNr.ToString(CultureInfo.InvariantCulture).PadLeft(8, '0');
        }

        private BatchFile InitializeBatchFile(BatchFile batchfileToCreate, string nextBatchNr, DateTime dateTime)
        {
            int batchCount = int.Parse(nextBatchNr);
            batchfileToCreate.IsSent = false;
            batchfileToCreate.BatchCount = batchCount;
            batchfileToCreate.DateTimeCreated = dateTime;
            batchfileToCreate.FileName = "MSBVIPAY" + nextBatchNr.Substring(6, 2);
            return batchfileToCreate;
        }

        private StringBuilder CreateHeaderRecord(ref BatchFile initializedBatchFile, string batchNumber, DateTime dateTime)
        {
            var headerBuilder = new StringBuilder();

            // setup the header
            var header = new HeaderRecord
            {
                MessageType = "000070000",
                BatchNumber = batchNumber,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                IsNotDeleted = true
            };

            // Transmission date	10	Transmission date	Numeric  (CCYY-MM-DD)
            string month = dateTime.Month.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            string day = dateTime.Day.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            string transmissionDate = string.Format("{0}-{1}-{2}", dateTime.Year, month, day);

            header.TransmissionDate = transmissionDate;

            // Transmission time	6	Transmission time	Numeric (HHMMSS)
            string hour = dateTime.Hour.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            string minute = dateTime.Minute.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');
            string second = dateTime.Second.ToString(CultureInfo.InvariantCulture).PadLeft(2, '0');

            string transmissionTime = string.Format("{0}{1}{2}", hour, minute, second);

            header.TransmissionTime = transmissionTime;

            // Filler Field	1	Blank	Blank
            header.Blank1 = " ";

            headerBuilder.Append(header.MessageType);

            headerBuilder.Append(header.BatchNumber);

            headerBuilder.Append(header.TransmissionDate);

            headerBuilder.Append(header.TransmissionTime);

            headerBuilder.Append(header.Blank1);

            initializedBatchFile.HeaderRecord = header;
            headerBuilder.Append(Environment.NewLine);
            return headerBuilder;
        }

        private StringBuilder CreateTransactionDetailRecord(ref BatchFile batchfileToCreate)
        {
            var detailBuilder = new StringBuilder();

            foreach (TransactionDetailRecord transactionDetail1 in batchfileToCreate.TransactionDetailRecords)
            {
                transactionDetail1.MessageType = "000080000";
                detailBuilder.Append(transactionDetail1.MessageType);
                transactionDetail1.BatchNumber = batchfileToCreate.HeaderRecord.BatchNumber;
                detailBuilder.Append(transactionDetail1.BatchNumber);
                string transactionType = transactionDetail1.TransactionType.PadRight(5, ' ');
                transactionDetail1.TransactionType = transactionType;
                detailBuilder.Append(transactionDetail1.TransactionType);
                
                // Document type for 1 Day Tracking
                const string documentType = "PT";
                transactionDetail1.DocumentType = documentType;
                detailBuilder.Append(transactionDetail1.DocumentType);
                
                // Requisition Number. 9 long blank
                transactionDetail1.RequisitionNumber = string.Empty.PadLeft(9, ' ');
                detailBuilder.Append(transactionDetail1.RequisitionNumber);
                
                // Document Number 8 long blank
                transactionDetail1.DocumentNumber = string.Empty.PadLeft(8, ' ');
                detailBuilder.Append(transactionDetail1.DocumentNumber);
                
                // User Reference 1 . Not compulsory 2 long
                transactionDetail1.UserReference1 = string.Empty.PadLeft(2, ' ');
                detailBuilder.Append(transactionDetail1.UserReference1);
                
                // User Reference 2 .. client reference
                string clientRefernce = transactionDetail1.UserReference2; // "SBV0001";
                clientRefernce = clientRefernce.Length > 20 ? clientRefernce.Substring(0, 20) : clientRefernce;
                transactionDetail1.UserReference2 = clientRefernce.PadRight(20, ' ');
                detailBuilder.Append(transactionDetail1.UserReference2);
                
                // Third Party .. 30 long Blank if not used
                transactionDetail1.ThirdParty = string.Empty.PadRight(30, ' ');
                detailBuilder.Append(transactionDetail1.ThirdParty);
                
                // Code 1.
                // First 8 characters used by FACS to carry the incoming batch number. 
                // Last 12 characters can be used by client for a unique reference number. 
                // FACS returns the same as input from client if message required.
                string code1 = transactionDetail1.BatchNumber + transactionDetail1.Value;
                transactionDetail1.Code1 = code1.PadRight(20, ' ');
                detailBuilder.Append(transactionDetail1.Code1);
                
                // Code 2
                transactionDetail1.Code2 = transactionDetail1.Code2.PadRight(20, ' ');
                detailBuilder.Append(transactionDetail1.Code2);
                
                // Transaction Amount . 11 long
                transactionDetail1.TransactionAmount = transactionDetail1.TransactionAmount.PadLeft(12, '0');
                string amount = transactionDetail1.TransactionAmount.Remove(transactionDetail1.TransactionAmount.IndexOf('.'), 1);
                //transactionDetail1.TransactionAmount = transactionDetail1.TransactionAmount.Remove(transactionDetail1.TransactionAmount.IndexOf('.'), 1);
                detailBuilder.Append(amount);
                
                // Payee = the name of the client
                string nameOfClient = transactionDetail1.Payee;
                transactionDetail1.Payee = nameOfClient.PadRight(80, ' ');
                detailBuilder.Append(transactionDetail1.Payee);
                
                // Error Code
                transactionDetail1.ErrorCode = string.Empty.PadLeft(4, ' ');
                detailBuilder.Append(transactionDetail1.ErrorCode);
                
                // Processing Option 1... creation of a single transaction
                transactionDetail1.ProcessingOption1 = "I";
                detailBuilder.Append(transactionDetail1.ProcessingOption1);
                
                // Processing Option 2.. create a transaction on FACS
                transactionDetail1.ProcessingOption2 = "S";
                detailBuilder.Append(transactionDetail1.ProcessingOption2);
                
                // Program Name Created . Not compulsory. 10 long
                transactionDetail1.ProgramNameCreated = string.Empty.PadLeft(10, ' ');
                detailBuilder.Append(transactionDetail1.ProgramNameCreated);
                
                // Bank Account Number 
                string accNr = transactionDetail1.BankAccountNumber.PadLeft(17, '0');
                transactionDetail1.BankAccountNumber = accNr.Substring(0, 17);
                detailBuilder.Append(transactionDetail1.BankAccountNumber);
                
                // Branch Code
                string branchCode = transactionDetail1.BranchCode.PadLeft(6, '0');
                transactionDetail1.BranchCode = branchCode;
                detailBuilder.Append(transactionDetail1.BranchCode);
                
                // Account Type . 1 = cheque
                transactionDetail1.AccountType = "1";
                detailBuilder.Append(transactionDetail1.AccountType);
                
                // Agency 
                transactionDetail1.AgencyPrefix = " ";
                detailBuilder.Append(transactionDetail1.AgencyPrefix);
                
                // Agency Number . 6 long
                transactionDetail1.AgencyNumber = string.Empty.PadLeft(6, ' ');
                detailBuilder.Append(transactionDetail1.AgencyNumber);
                
                // blank 2 long
                transactionDetail1.Blank2 = string.Empty.PadRight(2, ' ');
                detailBuilder.Append(transactionDetail1.Blank2);
                
                // Cheque Clearance code. 10 long not compulsory
                transactionDetail1.ChequeClearanceCode = string.Empty.PadRight(10, ' ');
                detailBuilder.Append(transactionDetail1.ChequeClearanceCode);
                
                // Client Cheque Number . 9 long
                transactionDetail1.ClientChequeNumber = string.Empty.PadRight(9, ' ');
                detailBuilder.Append(transactionDetail1.ClientChequeNumber);
                
                // Bank account number for the cash book. 17 long
                transactionDetail1.CashBookBankAccountNumber = string.Empty.PadLeft(17, ' ');
                detailBuilder.Append(transactionDetail1.CashBookBankAccountNumber);
                
                // Unique user code. Blank 4 long
                transactionDetail1.UniqueUserCode = string.Empty.PadLeft(4, ' ');
                detailBuilder.Append(transactionDetail1.UniqueUserCode);
                
                // Action date of transaction . 10 long
                transactionDetail1.ActionDate = batchfileToCreate.HeaderRecord.TransmissionDate.PadRight(10, ' ');
                detailBuilder.Append(transactionDetail1.ActionDate);
                
                // Blank
                transactionDetail1.INDF = " ";
                detailBuilder.Append(" ");
                
                // Blank Hash total.. not used 10 long
                transactionDetail1.HashTotal = string.Empty.PadRight(10, ' ');
                detailBuilder.Append(transactionDetail1.HashTotal);
                
                detailBuilder.Append(Environment.NewLine);
            }
            return detailBuilder;
        }

        private StringBuilder CreateTrailerRecord(ref BatchFile batchfileToCreate)
        {
            var trailerBuilder = new StringBuilder();
            var trailerRecord = new TrailerRecord
            {
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                IsNotDeleted = true,
                MessageType = "000090000"
            };

            // Message Type 9
            trailerBuilder.Append(trailerRecord.MessageType);
            // Batch Number 8
            trailerRecord.BatchNumber = batchfileToCreate.HeaderRecord.BatchNumber;
            trailerBuilder.Append(trailerRecord.BatchNumber);

            // Number of transactions15
            trailerRecord.NumberOfTransactions = batchfileToCreate.TransactionDetailRecords.Count().ToString(CultureInfo.InvariantCulture).PadLeft(8, '0');
            trailerBuilder.Append(trailerRecord.NumberOfTransactions);

            //Value 
            double val = 0;
            UInt64 hash1Total = 0;
            int count = 0;
            foreach (var detailRecord in batchfileToCreate.TransactionDetailRecords)
            {
                count++;
                try
                {
                    double amount2 = Convert.ToDouble(detailRecord.TransactionAmount);
                    val = val + amount2;

                // calculate the HASH

                /*
                Hash Total Calculation Steps

                1.	Take the account number (##ACNO) and multiply by the amount (##CAMT) for each record.

                Special Conditions: 

 	                Blanks are treated as zeros. If the account number field (##ACNO) contains zero’s then the amount field must be multiplied by zero and the result will be zero.
 	                If the account number in the account number field (##ACNO) contains a blank space then the space must be filled with a zero and treated as a numeric field. 
                    * Multiply the account number field that is filled with a zero with the amount field to get the result.
 	                If the account number field (##ACNO) contains Non-numeric characters then the value for ##ACNO changes to  “1” for this calculation. Multiply the amount field with “1”
 	                The batch total display as 1000050 – implied decimal point and no spaces

                2.	The result of the calculation is added to a summary total that holds 18 significant digits. Truncation is on the digits on the left of the field.
                 */

                
                    // step 1
                    string amount = detailRecord.TransactionAmount.Replace(".", "");

                    decimal longAccNr = decimal.Parse(detailRecord.BankAccountNumber);
                    decimal longAmount = decimal.Parse(Convert.ToDouble(amount).ToString(CultureInfo.InvariantCulture));
                    var calculatedTotal = longAccNr * longAmount;
                    decimal hash1 = 0;

                    decimal.TryParse(calculatedTotal.ToString(CultureInfo.InvariantCulture), NumberStyles.Any, CultureInfo.InvariantCulture, out hash1);
                    hash1 = decimal.Parse(hash1.ToString(CultureInfo.InvariantCulture).GetLast(18));
                    var total = hash1Total + hash1;
                    var lastNumber = total.ToString(CultureInfo.InvariantCulture).GetLast(18);
                    hash1Total = UInt64.Parse(lastNumber);
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("exception on create trailer record", ex);
                    throw;
                }
                
                
            }

            // step 2
            string checksum = hash1Total.ToString(CultureInfo.InvariantCulture).PadLeft(18, '0');
            string totalValue = String.Format("{0:0.00}", val).Replace(".", "");

            trailerRecord.TotalValue = totalValue.PadLeft(15, '0');
            trailerBuilder.Append(trailerRecord.TotalValue);

            //Checksum
            trailerRecord.Checksum = checksum;
            trailerBuilder.Append(trailerRecord.Checksum);

            batchfileToCreate.TrailerRecord = trailerRecord;
            return trailerBuilder;
        }

        private void SaveChanges(BatchFile batchFile, StringBuilder headerBuilder, StringBuilder detailBuilder, StringBuilder trailerBuilder)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    StringBuilder dataToWrite = headerBuilder.Append(detailBuilder).Append(trailerBuilder);
                    GenerateFile(batchFile.FileName, dataToWrite);
                    batchFile.IsSent = true;
                    _repository.Add(batchFile);
                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [SAVE CHANGES => CREATE FACS FILE]", ex);
            }
        }

        #endregion

        #region Helper

        private void GenerateFile(string filename, StringBuilder data)
        {
            try
            {
                var dropPath = _repository.Query<SystemConfiguration>(s => s.LookUpKey == "DROP_PATH").FirstOrDefault();

                if (dropPath != null)
                {
                    string path = dropPath.Value;

                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    string fullPath = Path.Combine(path, filename);
                    File.WriteAllText(fullPath, data.ToString());
                }
                else
                {
                    throw new ArgumentNullException(string.Format("LookUpKey Not Found [{0}]", "DROP_PATH"));
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [GENERATE FILE]", ex);
                throw;
            }
        }

        #endregion
    }

}