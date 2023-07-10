using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Text;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;

namespace Hyphen.Integration.Reconciliation.FileProcessor
{
    public class FileManager : IFileManager
    {
        #region Fields

        private static IFileManager _fileManager;
        private IRepository _repository { get; set; }
        private ILookup _lookup { get; set; }

        private bool HasError { get; set; }
        private string FileName { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public FileManager(IRepository repository, ILookup lookup)
        {
            _repository = repository;
            _lookup = lookup;
        }

        #endregion

        #region IFileManager

        /// <summary>
        ///     Reads Response Files from Nedbank.
        /// </summary>
        /// <param name="path"></param>
        public void ReadFiles(string path)
        {
            try
            {
                IEnumerable<string> files = GetFiles(path);

                foreach (string file in files)
                {
                    HasError = false;

                    FileName = file;
                    this.Log().Info(string.Format("READING HYPHEN RECON FILE => '{0}'....", FileName));

                    byte[] bytes = File.ReadAllBytes(file);
                    var memoryStream = new MemoryStream(bytes);
                    DecomposeFile(memoryStream);

                    // NOTE:    Only Archive processed files
                    //          Otherwise process the next file.
                    if (HasError == false)
                    {
                        ArchiveFile(file);
                    }
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [READ HYPHEN RECON FILE]", ex);
                throw;
            }
        }

        #endregion  
        
        #region Helpers

        /// <summary>
        /// Decompose file properties
        /// </summary>
        /// <param name="memoryStream"></param>
        private void DecomposeFile(MemoryStream memoryStream)
        {
            try
            {
                DateTime date = DateTime.Now;

                var fileLines = ConvertToArray(memoryStream).Where(e => e.Trim().Length > 0);

                var enumerable = fileLines as string[] ?? fileLines.ToArray();
                var numberOfTransactions = enumerable.Count(e => !string.IsNullOrWhiteSpace(e));

                this.Log().Info(string.Format("File '{0}' has '{1}' records", FileName, numberOfTransactions));

                foreach (var line in enumerable)
                {
                    var dateSentToHyphen = GetDate(line.Substring(157, 10));
                    var transactionReference = line.Substring(127, 20).Trim();

                    var wasLoaded = WasLoaded(transactionReference, dateSentToHyphen);

                    if (!wasLoaded)
                    {
                        var recon = new Recon
                        {
                            BankType = line.Substring(0, 5),
                            ClientReference = line.Substring(5, 20).Trim(),
                            Amount = line.Substring(25, 12).Trim(),
                            DateActioned = GetDate(line.Substring(37, 10)),
                            ClientSite = line.Substring(47, 80).Trim(),
                            MySbvReference = transactionReference,
                            StatusCode = line.Substring(147, 1).Trim(),
                            UniqueUserCode = line.Substring(148, 4).Trim(),
                            BatchNumber = line.Substring(152, 5).Trim(),
                            DateSent = dateSentToHyphen,
                            AccountNumber = line.Substring(167, 17).Trim(),
                            BranchCode = line.Substring(184, 6).Trim(),
                            AccountTypeId = Convert.ToInt16(line.Substring(190, 1).Trim()),
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
                        var recon = _repository.Query<Recon>(e => e.MySbvReference == transactionReference,
                            o => o.SettlementStatus,
                            o => o.AccountType,
                            o => o.AccountType.Accounts.Select(e => e))
                            .FirstOrDefault();

                        if (recon != null)
                        {
                            recon.BankType = line.Substring(0, 5);
                            recon.ClientReference = line.Substring(5, 20).Trim();
                            recon.Amount = line.Substring(25, 12).Trim();
                            recon.DateActioned = GetDate(line.Substring(37, 10));
                            recon.ClientSite = line.Substring(47, 80).Trim();
                            recon.MySbvReference = transactionReference;
                            recon.StatusCode = line.Substring(147, 1).Trim();
                            recon.UniqueUserCode = line.Substring(148, 4).Trim();
                            recon.BatchNumber = line.Substring(152, 5).Trim();
                            recon.DateSent = dateSentToHyphen;
                            recon.AccountNumber = line.Substring(167, 17).Trim();
                            recon.BranchCode = line.Substring(184, 6).Trim();
                            recon.AccountTypeId = Convert.ToInt16(line.Substring(190, 1).Trim());
                            recon.LastChangedDate = date;
                            recon.EntityState = State.Modified;
                            _repository.Update(recon);
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HasError = true;

                this.Log().Fatal(string.Format("ERROR READING HYPHEN RECON FILE => '{0}'....", FileName));

                foreach (var validationError in ex.EntityValidationErrors)
                {
                    foreach (var error in validationError.ValidationErrors)
                    {
                        this.Log().Fatal(string.Format("{0} => [DECOMPOSE FILE]", error.ErrorMessage), ex);
                    }
                }
            }
            catch (Exception ex)
            {
                HasError = true;
                this.Log().Fatal(string.Format("ERROR READING HYPHEN RECON FILE => '{0}'....", FileName));
                this.Log().Fatal("Hyphen.Integration.Reconciliation.FileProcessor => [DECOMPOSE FILE]", ex);
            }
        }


        /// <summary>
        /// get actioned date
        /// </summary>
        /// <param name="dateString"></param>
        private DateTime GetDate(string dateString)
        {
            var day = Convert.ToInt32(dateString.Substring(8, 2));
            var month = Convert.ToInt32(dateString.Substring(5, 2));
            var year = Convert.ToInt32(dateString.Substring(0, 4));
            return new DateTime(year, month, day);
        }


        /// <summary>
        /// Check if reference is loaded in database already.
        /// </summary>
        /// <param name="transactionReference"></param>
        /// <param name="dateSentToHyphen"></param>
        private bool WasLoaded(string transactionReference, DateTime dateSentToHyphen)
        {
            var loaded = _repository.Any<Recon>(
                    e => e.MySbvReference == transactionReference && e.DateSent == dateSentToHyphen);
            return loaded;
        }


        /// <summary>
        /// Gets all recon files from path
        /// </summary>
        /// <param name="path"></param>
        private IEnumerable<string> GetFiles(string path)
        {
            var collection = new List<string>();

            var files = Directory.GetFiles(path, "TRXREPORT*").ToList();
            collection.AddRange(files);
            return collection;
        }


        /// <summary>
        /// Archive file
        /// </summary>
        /// <param name="filePath"></param>
        private void ArchiveFile(string filePath)
        {
            try
            {
                var path = _repository.Query<SystemConfiguration>(a => a.LookUpKey == "ARCHIVE_PATH").FirstOrDefault();

                if (path != null)
                {
                    string archivePath = path.Value;

                    string fileName = Path.GetFileName(filePath);

                    if (!Directory.Exists(archivePath)) Directory.CreateDirectory(archivePath);

                    string folder = Path.Combine(archivePath,
                        string.Concat(DateTime.Now.Day, " ", DateTime.Now.ToString("MMMM"), " ", DateTime.Now.Year));

                    if (!Directory.Exists(folder))
                    {
                        Directory.CreateDirectory(folder);
                    }

                    string newFilePath = Path.Combine(folder, fileName);

                    File.Move(filePath, newFilePath);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [Hyphen.Integration.Reconciliation.FileProcessor => ARCHIVE FILE]", ex);
            }
        }



        /// <summary>
        /// Convert MemoryStream to string[] array
        /// </summary>
        /// <param name="file"></param>
        private IEnumerable<string> ConvertToArray(MemoryStream file)
        {
            string respons = Encoding.ASCII.GetString(file.ToArray());
            string[] lines = respons.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);

            // Remove the last line as it contains blank
            IEnumerable<string> finalLines = lines.Where(a => !string.IsNullOrEmpty(a));

            return finalLines.ToArray();
        }

        #endregion 
    }
}