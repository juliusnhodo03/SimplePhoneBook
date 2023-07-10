using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;

namespace Hyphen.Integration.FileManager
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

        public List<MemoryStream> ReadFile(string path)
        {
            try
            {
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                var readFiles = new List<MemoryStream>();
                const string fileName = @"MSBVIPAY*";
                string[] fileList = Directory.GetFiles(path, fileName);
                foreach (string file in fileList)
                {
                    byte[] bytes = File.ReadAllBytes(file);
                    MoveToArchive(file);
                    readFiles.Add(new MemoryStream(bytes));
                }

                return readFiles;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [READ FILE]", ex);
                throw;
            }
        }

        #endregion

        #region Helpers

        private void MoveToArchive(string file)
        {
            try
            {
                SystemConfiguration firstOrDefault = _repository.Query<SystemConfiguration>(a => a.LookUpKey == "ARCHIVE_PATH").FirstOrDefault();
                if (firstOrDefault != null)
                {
                    string archivePath = firstOrDefault.Value;

                    string fileName = Path.GetFileName(file);

                    if (!Directory.Exists(archivePath)) Directory.CreateDirectory(archivePath);

                    string filePath = Path.Combine(archivePath,
                        string.Concat(DateTime.Now.Day, " ", DateTime.Now.ToString("MMMM"), " ", DateTime.Now.Year));
                    if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

                    string newFileName = string.Concat(fileName, "_",
                        string.Concat(DateTime.Now.Year, DateTime.Now.Month.ToString("00"),
                            DateTime.Now.Day.ToString("00"),
                            DateTime.Now.Hour.ToString("00"), 
                            DateTime.Now.Minute.ToString("00"),
                            DateTime.Now.Second.ToString("00")));
                    string newFilePath = Path.Combine(filePath, newFileName);

                    File.Move(file, newFilePath);
                }
                else
                {
                    throw new ArgumentNullException(string.Format("LookUpKey Not Found [{0}]", "ARCHIVE_PATH"));
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [MOVE TO ARCHIVE]", ex);
                throw;
            }
        }

        #endregion
    }
}