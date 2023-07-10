using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Nedbank.Integration.FileUtilities;

namespace Nedbank.Integration.FileCreator
{
    [Export(typeof (IFileCreator))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FileCreator : IFileCreator
    {
        #region Fields

        private readonly NedbankClientProfile _clientProfile;
        
        private readonly IRepository _repository;
        private readonly IFileUtility _fileUtility; 

        #endregion

        #region Constructor

        [ImportingConstructor]
        public FileCreator(IRepository repository, IFileUtility fileUtility)
        {
            _repository = repository;
            _fileUtility = fileUtility;
            _clientProfile = _repository.Query<NedbankClientProfile>(e => e.LookupKey == "CLIENT_PROFILE_NUMBER").FirstOrDefault();
        }

        #endregion

        #region IFileCreator

        /// <summary>
        ///     Creates a collection of entry details for the Nedbank Batch File.
        ///     Each line holds a full transaction record of an individual Client, bearing all information needed for settlement.
        /// </summary>
        /// <param name="batchFile"></param>
        public void GenerateBatchFile(ref NedbankBatchFile batchFile)
        {
            this.Log().Info("Creating Batch file...");

            CreateModelTrailer(ref batchFile);

            // create File Header
            StringBuilder fileHeader = CreateFileHeader(batchFile.NedbankHeaderRecord);

            // create File Details
            StringBuilder fileDetails = CreateFileDetails(batchFile);

            // create File Trailer
            StringBuilder fileTrailer = CreateFileTrailer(batchFile);

            // build and generate a batch file
            FileBuilderHelper(batchFile, fileHeader, fileDetails, fileTrailer);
        }


        /// <summary>
        ///     Initializes the batch file properties.
        ///     File Types:
        ///     1.  01 – Transaction instructions.
        ///     2.  02 – Disallow instructions.
        /// </summary>
        /// <param name="fileType"></param>
        public NedbankBatchFile BatchInitialiser(string fileType)
        {
            var batch = new NedbankBatchFile
            {
                BatchDate = _fileUtility.GetDate(Format.YYYYMMDD),
                NedbankFileItems = new Collection<NedbankFileItem>(),
                IsNotDeleted = true,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                EntityState = State.Added,
                BatchNumber = _fileUtility.GetBatchNumber(Format.YYYYMMDD)
            };
            batch.BatchCount = batch.BatchNumber.Replace("NB-" + batch.BatchDate + "-", "");
            batch.FileName = _fileUtility.GetFilename(FileType.TRANSACTION, batch.BatchCount);
            batch.IsSent = false;
            // attach batch header
            CreateModelHeader(ref batch, fileType);
            return batch;
        }

        #endregion

        #region Nedbank Model Builder

        /// <summary>
        ///     A Header record for the Nedbank database model.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="fileType"></param>
        private void CreateModelHeader(ref NedbankBatchFile batch, string fileType)
        {
            string batchCount = batch.BatchCount.ToString(CultureInfo.InvariantCulture).PadLeft(6, '0');

            try
            {
                // get Client Profile Number
                string clientProfileNumber = _clientProfile.ProfileNumber.PadLeft(10, '0');

                // get Nominated Account Number
                string nominatedAccountNumber = _clientProfile.NominatedAccountNumber.PadLeft(16, '0');

                // get Charges Account Number
                string chargesAccountNumber = _clientProfile.ChargesAccountNumber.PadLeft(16, '0');

                //  This narrative will appear on the client’s (i.e. SBV) statement where
                //  accumulated entries have been chosen. Note: where no narrative
                //  is populated, the file number will appear on the statement instead
                //  of a client-specified narrative.
                string narrative = _clientProfile.StatementNarrative ?? string.Empty;
                string statementNarrative = narrative.PadLeft(30, ' ');

                // populate model header record 
                var header = new NedbankHeaderRecord
                {
                    RecordIdentifier = Convert.ToInt32(RecordIdentifier.HEADER).ToString().PadLeft(2, '0'),
                    ClientProfileNumber = clientProfileNumber,
                    FileSequenceNumber = clientProfileNumber + batch.BatchDate + batchCount,
                    FileType = fileType.PadLeft(2, '0'),
                    NominatedAccountNumber = nominatedAccountNumber,
                    ChargesAccountNumber = !string.IsNullOrWhiteSpace(chargesAccountNumber) ? chargesAccountNumber : null,
                    StatementNarrative = !string.IsNullOrWhiteSpace(statementNarrative) ? statementNarrative : null,
                    Filler = string.Empty.PadLeft(220, ' '),
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreatedById = 1,
                    EntityState = State.Added,
                    IsNotDeleted = true
                };

                // attatch header to batch
                batch.NedbankHeaderRecord = header;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception on Method: [FILE_CREATOR => CreateModelHeader]", ex);
                throw;
            }
        }


        /// <summary>
        ///     A Trailer record for the Nedbank database model.
        /// </summary>
        /// <param name="batch"></param>
        private void CreateModelTrailer(ref NedbankBatchFile batch)
        {
            int transactionsCount = batch.NedbankFileItems.Count;

            // populate model header record 
            var trailer = new NedbankTrailerRecord
            {
                RecordIdentifier = Convert.ToInt32(RecordIdentifier.TRAILER).ToString().PadLeft(2, '0'),
                TotalNumberOfTransactions = transactionsCount.ToString().PadLeft(8, '0'),
                TotalValue = batch.BatchTotal.PadLeft(18, '0'),
                Filler = string.Empty.PadLeft(292, ' '),
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                EntityState = State.Added,
                IsNotDeleted = true
            };
            // attach trailer to batch
            batch.NedbankTrailerRecord = trailer;
        }

        #endregion

        #region File Builders

        /// <summary>
        ///     This Method creates a Header record for the Nedbank batch file.
        /// </summary>
        /// <param name="header"></param>
        private StringBuilder CreateFileHeader(NedbankHeaderRecord header)
        {
            // use string builder to create a header record.
            var headerStringBuilder = new StringBuilder();
            headerStringBuilder.Append(header.RecordIdentifier);
            headerStringBuilder.Append(header.ClientProfileNumber);
            headerStringBuilder.Append(header.FileSequenceNumber);
            headerStringBuilder.Append(header.FileType);
            headerStringBuilder.Append(header.NominatedAccountNumber);
            headerStringBuilder.Append(header.ChargesAccountNumber);
            headerStringBuilder.Append(header.StatementNarrative);
            headerStringBuilder.Append(header.Filler);
            headerStringBuilder.Append(Environment.NewLine);
            return headerStringBuilder;
        }


        /// <summary>
        ///     File Transaction detail listing.
        /// </summary>
        /// <param name="batch"></param>
        private StringBuilder CreateFileDetails(NedbankBatchFile batch)
        {
            var detailBuilder = new StringBuilder();

            var entryClass = string.Empty.PadLeft(2, ' ');

            foreach (NedbankFileItem item in batch.NedbankFileItems)
            {
                detailBuilder.Append(item.RecordIdentifier);
                detailBuilder.Append(item.NominatedAccountNumber);
                detailBuilder.Append(item.PaymentReferenceNumber);
                detailBuilder.Append(item.DestinationBranchCode);
                detailBuilder.Append(item.DestinationAccountNumber);
                detailBuilder.Append(item.Amount);
                detailBuilder.Append(item.ActionDate);
                detailBuilder.Append(item.Reference);
                detailBuilder.Append(item.DestinationAccountHoldersName);
                detailBuilder.Append(item.TransactionType);
                detailBuilder.Append(item.NedbankClientTypeId);
                detailBuilder.Append(item.ChargesAccountNumber);
                detailBuilder.Append(item.ServiceType);
                detailBuilder.Append(item.OriginalPaymentReferenceNumber);
                detailBuilder.Append(entryClass);
                detailBuilder.Append(item.NominatedAccountReference);
                detailBuilder.Append(item.BDFIndicator);
                detailBuilder.Append(item.Filler);
                detailBuilder.Append(Environment.NewLine);
            }
            return detailBuilder;
        }


        /// <summary>
        ///     File Trailer details.
        /// </summary>
        /// <param name="batch"></param>
        private StringBuilder CreateFileTrailer(NedbankBatchFile batch)
        {
            var trailerBuilder = new StringBuilder();
            trailerBuilder.Append(batch.NedbankTrailerRecord.RecordIdentifier);
            trailerBuilder.Append(batch.NedbankTrailerRecord.TotalNumberOfTransactions);
            trailerBuilder.Append(batch.NedbankTrailerRecord.TotalValue);
            trailerBuilder.Append(batch.NedbankTrailerRecord.Filler);
            trailerBuilder.Append(Environment.NewLine);
            return trailerBuilder;
        }


        /// <summary>
        ///     Generates a Batch File
        ///     and
        ///     Update Nedbank Batch File.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="headerBuilder"></param>
        /// <param name="detailBuilder"></param>
        /// <param name="trailerBuilder"></param>
        private void FileBuilderHelper(NedbankBatchFile batch, StringBuilder headerBuilder, StringBuilder detailBuilder,
            StringBuilder trailerBuilder)
        {
            try
            {
                batch.IsSent = true;
                _repository.Add(batch);

                var file = new StringBuilder();
                file.Append(headerBuilder);
                file.Append(detailBuilder);
                file.Append(trailerBuilder);

                FileGeneratorHelper(batch.FileName, file);
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
                                    "DbEntityValidationException On Method : [FILE BUILDER]\n[{0}]\nStacktrace\n",
                                    error.ErrorMessage), ex);
                    }
                }
                // throw new exception
                throw new DbEntityValidationException(
                    "DbEntityValidationException On Method : [FILE BUILDER]\n[{0}]\nStacktrace\n",
                    ex.EntityValidationErrors);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [FILE BUILDER]", ex);
            }
        }


        /// <summary>
        ///     Generates a File to the specified Connect Direct path.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="file"></param>
        private void FileGeneratorHelper(string filename, StringBuilder file)
        {
            try
            {
                SystemConfiguration dropPath =
                    _repository.Query<SystemConfiguration>(s => s.LookUpKey == "NEDBANK_DROP_PATH")
                        .FirstOrDefault();

                if (dropPath != null)
                {
                    string path = dropPath.Value;

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    string fullPath = Path.Combine(path, filename);
                    File.WriteAllText(fullPath, file.ToString());
                }
                else
                {
                    string exception = string.Format("LookUpKey Not Found [{0}]", "NEDBANK_DROP_PATH");
                    throw new ArgumentNullException(exception);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [FILE GENERATOR]", ex);
                throw;
            }
        }

        #endregion
    }
}