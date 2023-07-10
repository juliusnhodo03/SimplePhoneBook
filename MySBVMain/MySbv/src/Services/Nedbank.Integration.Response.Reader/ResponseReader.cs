using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Nedbank.Integration.FileManager;
using Nedbank.Integration.FileUtilities;
using Nedbank.Integration.SettlementHub;
using Utility.Core;
using Vault.Integration.Msmq.Connector;

namespace Nedbank.Integration.Response.Reader
{
    [Export(typeof (IResponseReader))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class ResponseReader : IResponseReader
    {
        #region Fields

        private static IFileManager _fileManager;
        private IRepository _repository { get; set; }
        private ILookup _lookup { get; set; }
        private IFileUtility _fileUtility { get; set; }
        private ISettlement _settlement { get; set; }
        private IMsmqConnector _msmqConnector { get; set; }

        private bool HasError { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public ResponseReader(IRepository repository, ILookup lookup, IMsmqConnector msmqConnector,
            IFileUtility fileUtility, ISettlement settlement, IFileManager fileManager)
        {
            _repository = repository;
            _lookup = lookup;
            _msmqConnector = msmqConnector;
            _fileUtility = fileUtility;
            _settlement = settlement;
            _fileManager = fileManager;
        }

        #endregion

        #region IResponse Reader

        /// <summary>
        ///     Reads Response Files from Nedbank.
        /// </summary>
        /// <param name="path"></param>
        public void ReadFiles(string path)
        {
            HasError = false;

            this.Log().Info(string.Format("Reading from path => {0} ...", path));

            this.Log().Debug(() =>
            {
                try
                {
                    IEnumerable<FileCategory> items = GetFiles(path);

                    foreach (FileCategory item in items)
                    {
                        foreach (string file in item.Files)
                        {
                            string extension = Path.GetExtension(file);

                            if (IsFileWellFormed(extension))
                            {
                                byte[] bytes = File.ReadAllBytes(file);
                                var memoryStream = new MemoryStream(bytes);
                                DownloadResponseFile(memoryStream, item.FileType);

                                // NOTE:     Only move files from directory when there is no errors.
                                if (HasError == false)
                                {
                                    _fileManager.ArchiveFile(file);
                                }
                            }
                            HasError = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.Log().Fatal("Exception On Method : [READ RESPONSE FILE]", ex);
                    throw;
                }
            },
            "Reading RESPONSE FILES from Nedbank...");
        }


        /// <summary>
        ///     Download all response files.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileType"></param>
        private void DownloadResponseFile(MemoryStream bytes, FileType fileType)
        {
            switch (fileType)
            {
                case FileType.ACK:
                case FileType.SECOND_ACK:
                   var details = DecomposeFile(bytes, fileType);
                    SaveResponseDetailChanges(details);
                    RunRecon(details);
                    break;

                case FileType.NACK:
                    var nackDetails = DecomposeNack(bytes);
                    SaveResponseDetailChanges(nackDetails);
                    RunRecon(nackDetails);
                    break;

                case FileType.UNPAIDS:
                case FileType.NAEDOS:
                    var unpaids = DecomposeUnpaidsFile(bytes, fileType);
                    SaveUnpaidsChanges(unpaids);
                    var unpaidsDetails = PrepareOtherRejectionsForRecon(bytes, FileType.UNPAIDS);
                    RunRecon(unpaidsDetails);
                    break;

                case FileType.DUPLICATE:
                    var file = DecomposeDuplicateFile(bytes, fileType);
                    SaveDuplicatesChanges(file);
                    var duplicatesDetails = PrepareOtherRejectionsForRecon(bytes, FileType.DUPLICATE);
                    RunRecon(duplicatesDetails);
                    break;
            }
        }


        /// <summary>
        /// Prepare Unpaids/Duplicates For Recon
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileType"></param>
        private IEnumerable<NedbankResponseDetail> PrepareOtherRejectionsForRecon(MemoryStream bytes, FileType fileType)
        {
            var responseDetails = new List<NedbankResponseDetail>();
            try
            {
                string headerIdentifier = Convert.ToInt32(RecordIdentifier.HEADER).ToString().PadLeft(2, '0');
                string trailerIdentifier = Convert.ToInt32(RecordIdentifier.TRAILER).ToString().PadLeft(2, '0');

                string[] fileDetails = _fileUtility.ConvertToArray(bytes);

                var fileHeader = fileDetails.FirstOrDefault(e => e.Substring(0, 2) == headerIdentifier);
                var fileTrailer = fileDetails.FirstOrDefault(e => e.Substring(0, 2) == trailerIdentifier);

                if (fileHeader != null)
                {
                    var fileSequenceNumber = fileHeader.Substring(12, 24);

                    var records = from item in _repository.All<NedbankFileItem>()
                                  join
                                      batch in _repository.All<NedbankBatchFile>() on item.NedbankBatchFileId equals
                                      batch.NedbankBatchFileId
                                  join
                                      header in _repository.All<NedbankHeaderRecord>() on batch.NedbankHeaderRecordId equals
                                      header.NedbankHeaderRecordId
                                  where header.FileSequenceNumber == fileSequenceNumber
                                  select item;

                    // Enumerate records
                    foreach (var record in records)
                    {
                        DateTime date = DateTime.Now;

                        var lineDetail = new NedbankResponseDetail
                        {
                            RecordIdentifier = record.RecordIdentifier,
                            NominatedAccountNumber = record.NominatedAccountNumber,
                            PaymentReferenceNumber = record.PaymentReferenceNumber,
                            DestinationBranchCode = record.DestinationBranchCode,
                            DestinationAccountNumber = record.DestinationAccountNumber,
                            Amount = record.Amount,
                            ActionDate = record.ActionDate,
                            Reference = record.Reference,
                            DestinationAccountHoldersName = record.DestinationAccountHoldersName,
                            TransactionType = record.TransactionType,
                            NedbankClientTypeId = record.NedbankClientTypeId,
                            ChargesAccountNumber = record.ChargesAccountNumber,
                            ServiceType = record.ServiceType,
                            OriginalPaymentReferenceNumber = record.OriginalPaymentReferenceNumber,
                            TransactionStatus = FileStatus.REJECTED.Name(),
                            Reason = fileTrailer != null ? fileTrailer.Substring(88, 30).Trim() : null,
                            ResponseFilename = fileType.Name(),
                            CreateDate = date,
                            LastChangedDate = date,
                            CreatedById = 1,
                            LastChangedById = 1,
                            IsNotDeleted = true,
                            EntityState = State.Added
                        };
                        responseDetails.Add(lineDetail);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        this.Log().Fatal(validationError, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [Prepare Unpaids For Recon] => [RESPONSE READER]", ex);
            }
            return responseDetails;
        }



        /// <summary>
        /// Save duplicate file entry.
        /// </summary>
        /// <param name="duplicateFile"></param>
        private void SaveDuplicatesChanges(NedbankDuplicate duplicateFile)
        {
            try
            {
                _repository.Add(duplicateFile);

                if (HasError == false)
                {
                    _settlement.ApplyDuplicateRejections(duplicateFile);
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityValidationError.ValidationErrors)
                    {
                        this.Log()
                            .Fatal(
                                string.Format(
                                    "DbEntityValidationException On Method : [ADD DUPLICATE DETAIL]\n[{0}]\nStacktrace\n",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [ADD DUPLICATE DETAIL] => [RESPONSE READER]", ex);
            }
        }


        /// <summary>
        /// Decompose duplicate details
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileType"></param>
        private NedbankDuplicate DecomposeDuplicateFile(MemoryStream bytes, FileType fileType)
        {
            try
            {
                string[] duplicateFileDetails = _fileUtility.ConvertToArray(bytes);

                string headerIdentifier = Convert.ToInt32(RecordIdentifier.HEADER).ToString().PadLeft(2, '0');
                string trailerIdentifier = Convert.ToInt32(RecordIdentifier.TRAILER).ToString().PadLeft(2, '0');
                var securityRecordIdentifier = Convert.ToInt32(RecordIdentifier.SECURITY_RECORD).ToString().PadLeft(2, '0');

                var header = duplicateFileDetails.FirstOrDefault(e => e.StartsWith(headerIdentifier));
                var trailer = duplicateFileDetails.FirstOrDefault(e => e.StartsWith(trailerIdentifier));
                var securityRecord = duplicateFileDetails.FirstOrDefault(e => e.StartsWith(securityRecordIdentifier));

                var duplicatesFileDetails = new NedbankDuplicate
                {
                    ClientProfileNumber = header.Substring(2, 10).Trim(),
                    FileSequenceNumber = header.Substring(12, 24).Trim(),
                    FileType = header.Substring(36, 2).Trim(),
                    NominatedAccountNumber = header.Substring(38, 16).Trim(),
                    ChargesAccountNumber = header.Substring(54, 16).Trim(),
                    TotalNumberOfTransactions = trailer.Substring(2, 8).Trim(),
                    TotalValue = trailer.Substring(10, 18).Trim(),
                    FileStatus = trailer.Substring(80, 8).Trim().ToUpper(),
                    Reason = trailer.Substring(88, 30).Trim(),
                    HashTotal = securityRecord.Substring(2, 256).Trim()
                };
                return duplicatesFileDetails;
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [DECOMPOSE DUPLICATE FILE] => [RESPONSE READER]", ex);
                return null;
            }
        }

        #endregion

        #region Decompose Files

        /// <summary>
        ///     Files are decomposed and property values are extracted before they are
        ///     saved to the database.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileType"></param>
        private IEnumerable<NedbankResponseDetail> DecomposeFile(MemoryStream bytes, FileType fileType)
        {
            var responseDetails = new List<NedbankResponseDetail>();
            try
            {
                string detailIdentifier = Convert.ToInt32(RecordIdentifier.DETAIL).ToString().PadLeft(2, '0');

                string[] fileDetails = _fileUtility.ConvertToArray(bytes);

                foreach (string line in fileDetails)
                {
                    string recordIdentifier = line.Substring(0, 2);

                    if (recordIdentifier == detailIdentifier)
                    {
                        var lineDetail = ExtractResponseDetails(line, recordIdentifier);

                        var isNack = (fileType == FileType.NACK);

                        lineDetail.TransactionStatus = isNack
                            ? FileStatus.REJECTED.Name()
                            : line.Substring(212, 8).Trim();

                        lineDetail.Reason = line.Substring(220, 98).Trim();
                        lineDetail.ResponseFilename = fileType.Name();
                        responseDetails.Add(lineDetail);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (var errors in ex.EntityValidationErrors) 
                {
                    foreach (var validationError in errors.ValidationErrors) 
                    {
                        this.Log().Fatal(validationError, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [DECOMPOSE FILE] => [RESPONSE READER]", ex);
            }
            return responseDetails;
        }


        /// <summary>
        ///     NACK Files are decomposed and property values are extracted before they are
        ///     saved to the database.
        /// </summary>
        /// <param name="bytes"></param>
        private IEnumerable<NedbankResponseDetail> DecomposeNack(MemoryStream bytes) 
        {
            var responseDetails = new List<NedbankResponseDetail>();
            try
            {
                string headerIdentifier = Convert.ToInt32(RecordIdentifier.HEADER).ToString().PadLeft(2, '0');
                string trailerIdentifier = Convert.ToInt32(RecordIdentifier.TRAILER).ToString().PadLeft(2, '0');

                string[] fileDetails = _fileUtility.ConvertToArray(bytes);

                var fileHeader = fileDetails.FirstOrDefault(e => e.Substring(0, 2) == headerIdentifier);
                var fileTrailer = fileDetails.FirstOrDefault(e => e.Substring(0, 2) == trailerIdentifier); 

                if (fileHeader != null)
                {
                    var fileSequenceNumber = fileHeader.Substring(12, 24);

                    var records = from item in _repository.All<NedbankFileItem>()
                        join
                            batch in _repository.All<NedbankBatchFile>() on item.NedbankBatchFileId equals
                            batch.NedbankBatchFileId
                        join
                            header in _repository.All<NedbankHeaderRecord>() on batch.NedbankHeaderRecordId equals
                            header.NedbankHeaderRecordId
                        where header.FileSequenceNumber == fileSequenceNumber
                        select item;

                    // Enumerate records
                    foreach (var record in records)
                    {
                        DateTime date = DateTime.Now;

                        var lineDetail = new NedbankResponseDetail
                        {
                            RecordIdentifier = record.RecordIdentifier,
                            NominatedAccountNumber = record.NominatedAccountNumber,
                            PaymentReferenceNumber = record.PaymentReferenceNumber,
                            DestinationBranchCode = record.DestinationBranchCode,
                            DestinationAccountNumber = record.DestinationAccountNumber,
                            Amount = record.Amount,
                            ActionDate = record.ActionDate,
                            Reference = record.Reference,
                            DestinationAccountHoldersName = record.DestinationAccountHoldersName,
                            TransactionType = record.TransactionType,
                            NedbankClientTypeId = record.NedbankClientTypeId,
                            ChargesAccountNumber = record.ChargesAccountNumber,
                            ServiceType = record.ServiceType,
                            OriginalPaymentReferenceNumber = record.OriginalPaymentReferenceNumber,
                            TransactionStatus = FileStatus.REJECTED.Name(),
                            Reason = fileTrailer != null ?  fileTrailer.Substring(88, 30).Trim(): null,
                            ResponseFilename = FileType.NACK.Name(),
                            CreateDate = date,
                            LastChangedDate = date,
                            CreatedById = 1,
                            LastChangedById = 1,
                            IsNotDeleted = true,
                            EntityState = State.Added
                        };
                        responseDetails.Add(lineDetail);
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (var errors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in errors.ValidationErrors)
                    {
                        this.Log().Fatal(validationError, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [DECOMPOSE FILE] => [RESPONSE READER]", ex);
            }
            return responseDetails;
        }


        /// <summary>
        ///     Files are decomposed and property values are extracted before they are
        ///     saved to the database.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="fileType"></param>
        private IEnumerable<NedbankUnpaidOrNaedo> DecomposeUnpaidsFile(MemoryStream bytes, FileType fileType)
        {
            var unpaidsDetails = new List<NedbankUnpaidOrNaedo>();
            try
            {
                string detailRecord = Convert.ToInt32(RecordIdentifier.DETAIL).ToString().PadLeft(2, '0');

                string[] fileDetails = _fileUtility.ConvertToArray(bytes);

                foreach (string line in fileDetails)
                {
                    string recordIdentifier = line.Substring(0, 2);

                    if (recordIdentifier == detailRecord)
                    {
                        var date = DateTime.Now;

                        var lineDetail = new NedbankUnpaidOrNaedo
                        {
                            RecordIdentifier = line.Substring(0, 2),
                            RecordType = line.Substring(2, 2),
                            PaymentReferenceNumber = line.Substring(4, 34),
                            NedbankReferenceNumber = line.Substring(38, 8),
                            RejectingBankCode = line.Substring(46, 3),
                            RejectingBankBranchCode = line.Substring(49, 6),
                            NewDestinationBranchCode = line.Substring(55, 6),
                            NewDestinationAccountNumber = line.Substring(61, 16),
                            NewDestinationAccountType = line.Substring(77, 1),
                            Status = line.Substring(78, 8),
                            Reason = line.Substring(86, 100),
                            UnpaidsUserReference = FileType.UNPAIDS == fileType ? line.Substring(186, 30) : string.Empty,
                            NaedosUserReference = FileType.NAEDOS == fileType ? line.Substring(186, 30) : string.Empty,
                            OriginalHomingAccountNumber = line.Substring(217, 11),
                            OriginalAccountType = line.Substring(228, 1),
                            Amount = line.Substring(229, 12),
                            OriginalActionDate = line.Substring(241, 6),
                            Class = line.Substring(247, 2),
                            TaxCode = line.Substring(249, 1),
                            ReasonCode = line.Substring(250, 2),
                            OriginalHomingAccountName = line.Substring(252, 30),
                            NewSequenceNumber = line.Substring(282, 6),
                            NumberOfTimesRedirected = line.Substring(288, 2),
                            NewActionDate = line.Substring(290, 6),
                            ResponseFilename = fileType.Name(),
                            LastChangedById = 1,
                            CreatedById = 1,
                            CreateDate = date,
                            LastChangedDate = date
                        };
                        lineDetail.SettlementIdentifier = GetSettlementIdentifier(lineDetail.PaymentReferenceNumber);
                        unpaidsDetails.Add(lineDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Info(ex);
            }
            return unpaidsDetails;
        }


        /// <summary>
        ///     Builds Nedbank Response Details from a file detail record line.
        /// </summary>
        /// <param name="fileLine"></param>
        /// <param name="recordIdentifier"></param>
        /// <returns></returns>
        private NedbankResponseDetail ExtractResponseDetails(string fileLine, string recordIdentifier)
        {
            DateTime date = DateTime.Now;

            var lineDetail = new NedbankResponseDetail
            {
                RecordIdentifier = recordIdentifier,
                NominatedAccountNumber = fileLine.Substring(2, 16).Trim(),
                PaymentReferenceNumber = fileLine.Substring(18, 34).Trim(),
                DestinationBranchCode = fileLine.Substring(52, 6).Trim(),
                DestinationAccountNumber = fileLine.Substring(58, 16).Trim(),
                Amount = fileLine.Substring(74, 12).Trim(),
                ActionDate = fileLine.Substring(86, 8).Trim(),
                Reference = fileLine.Substring(94, 30).Trim(),
                DestinationAccountHoldersName = fileLine.Substring(124, 30).Trim(),
                TransactionType = fileLine.Substring(154, 4).Trim(),
                NedbankClientTypeId = fileLine.Substring(158, 2).Trim(),
                ChargesAccountNumber = fileLine.Substring(160, 16).Trim(),
                ServiceType = fileLine.Substring(176, 2).Trim(),
                OriginalPaymentReferenceNumber = fileLine.Substring(178, 34).Trim(),
                CreateDate = date,
                LastChangedDate = date,
                CreatedById = 1,
                LastChangedById = 1,
                IsNotDeleted = true,
                EntityState = State.Added
            };
            return lineDetail;
        }

        #endregion

        #region Add Response File Detail Records

        /// <summary>
        ///     Add Unpaids details
        /// </summary>
        /// <param name="details"></param>
        private void AddUnpaidsDetail(IEnumerable<NedbankUnpaidOrNaedo> details)
        {
            try
            {
                foreach (NedbankUnpaidOrNaedo detail in details)
                {
                    _repository.Add(detail);
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityValidationError.ValidationErrors)
                    {
                        this.Log()
                            .Fatal(
                                string.Format(
                                    "DbEntityValidationException On Method : [ADD UPAIDS DETAIL] => RESPONSE READER\n[{0}]\nStacktrace\n",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Error saving Response Data from Nedbank. => [ADD UPAIDS DETAIL] => RESPONSE READER", ex);
            }
        }



        /// <summary>
        ///     Add Response file line-item to database.
        /// </summary>
        /// <param name="details"></param>
        private void AddFileDetail(IEnumerable<NedbankResponseDetail> details)
        {
            try
            {
                foreach (NedbankResponseDetail detail in details)
                {
                    _repository.Add(detail);
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityValidationError.ValidationErrors)
                    {
                        this.Log()
                            .Fatal(
                                string.Format(
                                    "DbEntityValidationException On Method : [ADD FILE DETAIL] => RESPONSE READER\n[{0}]\nStacktrace\n",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Error saving Response Data from Nedbank. => [ADD FILE DETAIL] => RESPONSE READER", ex);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Get all response files, which are:
        ///     1.  ACK
        ///     2.  NACK
        ///     3.  SECOND ACK,
        ///     4.  DUPLICATE
        ///     5.  UNPAIDS
        ///     6.  NAEDOS => Are removed as response files, Nedbank doesnt send these to MySBV.
        /// </summary>
        /// <param name="path"></param>
        private IEnumerable<FileCategory> GetFiles(string path)
        {
            var collection = new List<FileCategory>
            {
                new FileCategory
                {
                    FileType = FileType.ACK,
                    Files = Directory.GetFiles(path, GetStartCharacters(FileType.ACK))
                },
                new FileCategory
                {
                    FileType = FileType.NACK,
                    Files = Directory.GetFiles(path, GetStartCharacters(FileType.NACK))
                },
                new FileCategory
                {
                    FileType = FileType.SECOND_ACK,
                    Files = Directory.GetFiles(path, GetStartCharacters(FileType.SECOND_ACK))
                },
                new FileCategory
                {
                    FileType = FileType.DUPLICATE,
                    Files = Directory.GetFiles(path, GetStartCharacters(FileType.DUPLICATE))
                },
                new FileCategory
                {
                    FileType = FileType.UNPAIDS,
                    Files = Directory.GetFiles(path, GetStartCharacters(FileType.UNPAIDS))
                }
            };
            return collection;
        }


        /// <summary>
        ///     Check if the file is a correct and expected one from
        ///     Nedbank.
        /// </summary>
        /// <param name="extension"></param>
        private bool IsFileWellFormed(string extension)
        {
            return extension == ".SQ320";
        }


        /// <summary>
        ///     Get Nedbank settlement identifier
        /// </summary>
        /// <param name="paymentReferenceNumber"></param>
        private string GetSettlementIdentifier(string paymentReferenceNumber)
        {
            try
            {
                NedbankFileItem fileItem =
                    _repository.Query<NedbankFileItem>(a => a.PaymentReferenceNumber == paymentReferenceNumber)
                        .FirstOrDefault();
                return fileItem.SettlementIdentifier;
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [GET SETTLEMENT IDENTIFIER] => [RESPONSE READER]", ex);
            }
            return string.Empty;
        }
         

        /// <summary>
        ///     Save changes and apply settlements to affected Transactions.
        /// </summary>
        /// <param name="details"></param>
        private void SaveResponseDetailChanges(IEnumerable<NedbankResponseDetail> details)
        {
            try
            {
                NedbankResponseDetail[] items = details as NedbankResponseDetail[] ?? details.ToArray();
                AddFileDetail(items);

                if (HasError == false)
                {
                    _settlement.ApplySettlements(items);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [SAVE RESPONSE DETAIL CHANGES] => [RESPONSE READER]", ex);
            }
        }


        /// <summary>
        ///     Save changes and apply settlements to affected Transactions.
        /// </summary>
        /// <param name="details"></param>
        private void SaveUnpaidsChanges(IEnumerable<NedbankUnpaidOrNaedo> details)
        {
            try
            {
                NedbankUnpaidOrNaedo[] items = details as NedbankUnpaidOrNaedo[] ?? details.ToArray();

                AddUnpaidsDetail(items);

                if (HasError == false)
                {
                    _settlement.ApplyUnPaidRejections(items);
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal("Exception On Method : [SAVE UNPAIDS CHANGES] => [RESPONSE READER]", ex);
            }
        }



        /// <summary>
        /// Apply settlements to Cash deposits from ACK, NACK, DUPLICATE, SECOND-SCK Files.
        /// </summary>
        /// <param name="details"></param>
        private void RunRecon(IEnumerable<NedbankResponseDetail> details) 
        {
            try
            {
                this.Log().Info("RUNNING NEDBANK RECON DATA...");

                var date = DateTime.Now;

                foreach (var detail in details)
                {
                    var fileItem = _repository.Query<NedbankFileItem>(e =>
                        e.PaymentReferenceNumber == detail.PaymentReferenceNumber)
                        .FirstOrDefault();

                    if (fileItem != null)
                    {
                        var settlementIdentifier = fileItem.SettlementIdentifier;
                        var batch = GetBatch(fileItem.NedbankBatchFileId);

                        var accountTypeId = GetAccountTypeId(detail.PaymentReferenceNumber);

                        var transactionReference = (fileItem.NominatedAccountReference.Trim()).Substring(0, 14);

                        var wasLoaded = WasLoaded(transactionReference);

                        // Run Recon data
                        if (!wasLoaded)
                        {
                            var amount = !string.IsNullOrWhiteSpace(detail.Amount) ?
                                string.Format("{0}", Convert.ToDecimal(detail.Amount.Trim()) / 100) : "0";

                            var recon = new Recon
                            {
                                BankType = GetBankType(detail.PaymentReferenceNumber),
                                ClientReference = detail.Reference,
                                Amount = amount,
                                DateActioned = FormatDate(detail.ActionDate),
                                ClientSite = GetSiteName(settlementIdentifier),
                                MySbvReference = transactionReference,
                                StatusCode = GetSettlementStatusCode(detail.TransactionStatus),
                                BatchNumber = batch.BatchNumber,
                                DateSent = FormatDate(batch.BatchDate),
                                AccountNumber = detail.DestinationAccountNumber,
                                BranchCode = detail.DestinationBranchCode,
                                AccountTypeId = accountTypeId,    
                                //UniqueUserCode = "0000", // No Unique User Code: Please replace with one business suggests
                                CreateDate = date,
                                LastChangedDate = date,
                                CreatedById = 1,
                                LastChangedById = 1,
                                IsNotDeleted = true,
                                EntityState = State.Added
                            };
                            _repository.Add(recon);
                        }
                        else
                        {
                            var recon = _repository.Query<Recon>(e => e.MySbvReference == transactionReference)
                                .FirstOrDefault();

                            if (recon != null)
                            {
                                var amount = !string.IsNullOrWhiteSpace(detail.Amount) ?
                                    string.Format("{0}", Convert.ToDecimal(detail.Amount.Trim()) / 100) : "0";

                                recon.BankType = GetBankType(detail.PaymentReferenceNumber);
                                recon.ClientReference = detail.Reference;
                                recon.Amount = amount;
                                recon.DateActioned = FormatDate(detail.ActionDate);
                                recon.ClientSite = GetSiteName(settlementIdentifier);
                                recon.MySbvReference = transactionReference;
                                recon.StatusCode = GetSettlementStatusCode(detail.TransactionStatus.Trim());
                                recon.BatchNumber = batch.BatchNumber;
                                recon.DateSent = FormatDate(batch.BatchDate);
                                recon.AccountNumber = detail.DestinationAccountNumber;
                                recon.BranchCode = detail.DestinationBranchCode;
                                recon.AccountTypeId = accountTypeId;
                                //recon.UniqueUserCode = "0000"; // No Unique User Code: Please replace with one business suggests
                                recon.LastChangedDate = date;
                                recon.LastChangedById = 1;
                                recon.EntityState = State.Modified;
                                _repository.Update(recon);
                            }
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;
                foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                {
                    foreach (DbValidationError error in entityValidationError.ValidationErrors)
                    {
                        this.Log()
                            .Fatal(
                                string.Format(
                                    "DbEntityValidationException On Method : [RUN RECON] => RESPONSE READER\n[{0}]\nStacktrace\n",
                                    error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [RUN RECON] => [RESPONSE READER]", ex);
            }
        }


        /// <summary>
        /// Check if reference is loaded in database already.
        /// </summary>
        /// <param name="transactionReference"></param>
        private bool WasLoaded(string transactionReference)
        {
            var loaded = _repository.Any<Recon>(e => e.MySbvReference == transactionReference);
            return loaded;
        }


        /// <summary>
        /// Get site name
        /// </summary>
        /// <param name="settlementIdentifier"></param>
        private string GetSiteName(string settlementIdentifier)
        {
            var sql = string.Format(@"
                SELECT S.* FROM [CashDeposit] C INNER JOIN [Site] S ON S.SiteId = C.SiteId 
                WHERE C.SettlementIdentifier = '{0}'

                UNION

                SELECT S.* FROM [ContainerDrop] D 
	                INNER JOIN [Container] C ON C.ContainerId=D.ContainerId 
	                INNER JOIN [CashDeposit] CD ON CD.CashDepositId = C.CashDepositId
	                INNER JOIN [Site] S ON S.SiteId=CD.SiteId
                WHERE D.SettlementIdentifier = '{0}'

                UNION

                SELECT S.* FROM [VaultPartialPayment] P INNER JOIN [Site] S ON P.CitCode=S.CitCode 
                WHERE P.SettlementIdentifier = '{0}'", settlementIdentifier);

            var site = _repository.ExecuteQueryCommand<Site>(sql).FirstOrDefault();

            return (site != null) ? site.Name : string.Empty;
        }
        

        /// <summary>
        /// Get bank type
        /// </summary>
        /// <param name="paymentReferenceNumber"></param>
        private string GetBankType(string paymentReferenceNumber)
        {
            var sql = string.Format(@"SELECT BankType.* FROM [dbo].[Account] A 
                                INNER JOIN [dbo].[TransactionType] BankType ON A.TransactionTypeId=BankType.TransactionTypeId
                                INNER JOIN Nedbank.FileItem F ON F.AccountId = A.AccountId
                                WHERE F.PaymentReferenceNumber='{0}'", paymentReferenceNumber);

            var bank = _repository.ExecuteQueryCommand<TransactionType>(sql).FirstOrDefault();

            return (bank != null) ? bank.Name : string.Empty;
        }

        
        /// <summary>
        /// Get batch number
        /// </summary>
        /// <param name="batchId"></param>
        private NedbankBatchFile GetBatch(int batchId)
        {
            var sql = string.Format("SELECT * FROM [Nedbank].[BatchFeed] WHERE NedbankBatchFileId = {0}", batchId);
            var batch = _repository.ExecuteQueryCommand<NedbankBatchFile>(sql).FirstOrDefault();

            return batch;
        }


        /// <summary>
        /// Get account type
        /// </summary>
        /// <param name="paymentReferenceNumber"></param>
        private int GetAccountTypeId(string paymentReferenceNumber)
        {
            var sql = string.Format(@"SELECT A.* FROM Nedbank.FileItem F INNER JOIN 
	                                         Account A ON F.AccountId = A.AccountId
                                        WHERE F.PaymentReferenceNumber = '{0}'", paymentReferenceNumber);
            var account = _repository.ExecuteQueryCommand<Account>(sql).FirstOrDefault();

            return (account != null) ? account.AccountTypeId : 0;
        }


        /// <summary>
        /// Settlement Status Code
        /// </summary>
        /// <param name="transactionStatus"></param>
        private string GetSettlementStatusCode(string transactionStatus)
        {
            var transactionStatusCode = (transactionStatus.ToUpper() == "ACCEPTED") ? "SETTLED" : transactionStatus.ToUpper();

            var status = _repository.Query<SettlementStatus>(e => e.Status.ToUpper() == transactionStatusCode.ToUpper()).FirstOrDefault();

            return status.StatusCode;
        }
        

        /// <summary>
        /// Format date 
        /// </summary>
        /// <param name="dateString"></param>
        /// <returns></returns>
        private DateTime FormatDate(string dateString)
        {
            //20150609
            var day = Convert.ToInt16(dateString.Substring(6, 2));
            var month = Convert.ToInt16(dateString.Substring(4, 2));
            var year = Convert.ToInt16(dateString.Substring(0, 4));

            return new DateTime(year, month, day);
        }

        
        /// <summary>
        ///     get file path start characters.
        ///     Appending a "*" denotes search for files starting with the given characters.
        /// </summary>
        /// <param name="fileType"></param>
        private string GetStartCharacters(FileType fileType)
        {
            string path = string.Format("{0}*", _fileUtility.GetFilePrefix(fileType));
            return path;
        }

        #endregion
    }
}