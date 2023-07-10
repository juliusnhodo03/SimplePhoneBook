using System;
using System.ComponentModel.Composition;
using System.Linq;
using Domain.Data.Model;
using Domain.Repository;
using Hyphen.Integration.Reconciliation.Controller.MetaData;
using Hyphen.Integration.Reconciliation.FileProcessor;
using Infrastructure.Logging;
using Ninject;
using Quartz;

namespace Hyphen.Integration.Reconciliation.Controller
{
    [Export(typeof (IJob))]
    [ProcessorType(ProcessorType = ProcessorType.Output)]
    public class RequestHub : IJob
    {
        #region Fields

        public static IFileManager _fileManager { get; set; }
        public static IRepository _repository { get; set; }
        private string _path { get; set; }

        #endregion

        #region Constructor

        [ImportingConstructor]
        public RequestHub()
        {
            var kernel = new StandardKernel(new Bindings());
            _repository = kernel.Get<IRepository>();
            _fileManager = kernel.Get<IFileManager>();
            this.Log().Debug("Hyphen.Integration.Reconciliation.Controller Initialized...");
        }

        #endregion

        #region IJob

        /// <summary>
        ///     Run scheduled Job on Request Processor.
        /// </summary>
        /// <param name="context"></param>
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.Log().Debug(() =>
                {
                    _path = GetFileUrl();

                    _fileManager.ReadFiles(_path);
                },
                "Reading Batch files On Scheduled Time...");
            }
            catch (Exception ex)
            {
                const string exception =
                    "Exception On Method : [Hyphen.Integration.Reconciliation.Controller.[IJob].Execute]";
                this.Log().Fatal(exception, ex);
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Get Hyphen pick up Url:
        ///     This will pick all the TxtReports in then folder.
        /// </summary>
        /// <returns></returns>
        private string GetFileUrl()
        {
            if (_path != null)
            {
                return _path;
            }
            SystemConfiguration configuration =
                _repository.Query<SystemConfiguration>(e => e.LookUpKey == "PICKUP_PATH")
                    .FirstOrDefault();

            return configuration.Value;
        }

        #endregion
    }
}