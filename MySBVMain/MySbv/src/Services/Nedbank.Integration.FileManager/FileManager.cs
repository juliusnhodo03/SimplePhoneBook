using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;

namespace Nedbank.Integration.FileManager
{
    [Export(typeof (IFileManager))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class FileManager : IFileManager
    {
        #region Fields

        private readonly IRepository _repository;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public FileManager(IRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region IFile Manager

        /// <summary>
        /// Archive file
        /// </summary>
        /// <param name="filePath"></param>
        public void ArchiveFile(string filePath) 
        {
            try
            {
                var path = _repository.Query<SystemConfiguration>(a => a.LookUpKey == "NEDBANK_ARCHIVE_PATH")
                            .FirstOrDefault();

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
                else
                {
                    throw new ArgumentNullException(string.Format("LookUpKey Not Found [{0}]", "NEDBANK_ARCHIVE_PATH"));
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [ARCHIVE FILE]", ex);
                throw;
            }
        }

        #endregion
    }
}