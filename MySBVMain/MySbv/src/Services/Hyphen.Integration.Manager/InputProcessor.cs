using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Domain.Data.Model;
using Domain.Notifications;
using Domain.Repository;
using Hyphen.Integration.Facs;
using Hyphen.Integration.FileManager;
using Hyphen.Integration.Manager.Infrastructure;
using Hyphen.Integration.Response.Data;
using Infrastructure.Logging;
using Ninject;
using Quartz;

namespace Hyphen.Integration.Manager
{
    [Export(typeof (IJob))]
    [ProcessorType(ProcessorType = ProcessorType.Input)]
    public class InputProcessor : IJob
    {
        #region Fields

        private static IFacs _facs;
        private static IFileManager _fileManager;
        private static INotification _notification;
        private static IRepository _repository;
        private static IResponseData _responseData;

        #endregion

        #region Constructor

        //[ImportingConstructor]
        //public InputProcessor(
        //    [Import(AllowDefault = true)] IRepository repository,
        //    [Import(AllowDefault = true)] IFacs facs,
        //    [Import(AllowDefault = true)] IResponseData responseData,
        //    [Import(AllowDefault = true)] IFileManager fileManager,
        //    [Import(AllowDefault = true)] INotification notification)
        //{
        //    _repository = repository;
        //    _facs = facs;
        //    _responseData = responseData;
        //    _fileManager = fileManager;
        //    _notification = notification;
        //}

        [ImportingConstructor]
        public InputProcessor()
        {
            IKernel kernel = new StandardKernel(new Bindings());
            _repository = kernel.Get<IRepository>();
            _facs = kernel.Get<IFacs>();
            _responseData = kernel.Get<IResponseData>();
            _fileManager = kernel.Get<IFileManager>();
            _notification = kernel.Get<INotification>();
        }

        #endregion

        #region IJob

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.Log().Debug(() => { ReadResponseFile(); }, "Execute Read Response File.");
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [Hyphen.Integration.Manager.InputProcessor.[IJob].Execute]", ex);
            }
        }

        #endregion

        #region Internal

        private void ReadResponseFile()
        {
            this.Log().Debug(() =>
            {
                List<MemoryStream> files = DownloadResponseFile();

                if (files != null && files.Count > 0)
                {
                    UpdateDatabase(files);

                    if (_responseData.HasRejectedDeposits)
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
                            this.Log().Fatal("Exception encountered on sending an email for rejected deposits", ex);
                        }
                    }
                }
            }, "Executing ReadFacsFile.");
        }

        private List<MemoryStream> DownloadResponseFile()
        {
            try
            {
                string filePathName = GetReturnFilePathName();
                return _fileManager.ReadFile(filePathName);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [DOWNLOAD RESPONSE FILE]", ex);
                throw;
            }
        }

        private string GetReturnFilePathName()
        {
            SystemConfiguration firstOrDefault =
                _repository.Query<SystemConfiguration>(a => a.LookUpKey == "PICKUP_PATH").FirstOrDefault();
            if (firstOrDefault != null && !string.IsNullOrEmpty(firstOrDefault.Value))
            {
                return firstOrDefault.Value;
            }
            throw new ArgumentNullException(string.Format("LookUpKey Not Found [{0}]", "PICKUP_PATH"));
        }

        private void UpdateDatabase(List<MemoryStream> files)
        {
            this.Log()
                .Debug(() => { _facs.ReadResponseFile(files); },
                    string.Format("Executing Update Database with total number of files [{0}]", files.Count));
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