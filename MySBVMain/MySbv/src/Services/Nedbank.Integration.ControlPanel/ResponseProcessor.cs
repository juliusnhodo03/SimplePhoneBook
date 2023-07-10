using System;
using System.ComponentModel.Composition;
using Infrastructure.Logging;
using Nedbank.Integration.ControlPanel.MetaData;
using Nedbank.Integration.FileUtilities;
using Nedbank.Integration.Response.Reader;
using Ninject;
using Quartz;

namespace Nedbank.Integration.ControlPanel
{
    [Export(typeof (IJob))]
    [ProcessorType(ProcessorType = ProcessorType.Output)]
    public class ResponseProcessor : IJob
    {
        #region Fields

        private static IFileUtility _fileUtility;
        private static IResponseReader _responseReader;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public ResponseProcessor()
        {
            var kernel = new StandardKernel(new Bindings());
            _fileUtility = kernel.Get<IFileUtility>();
            _responseReader = kernel.Get<IResponseReader>();
        }

        #endregion

        #region IJob

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                this.Log().Debug(() => { ReadResponseFiles(); }, "Execute Read Response Files...");
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal("Exception On Method : [Nedbank.Integration.ControlPanel.ResponseProcessor.[IJob].Execute]",
                        ex);
            }
        }

        #endregion

        #region Internal

        /// <summary>
        /// Reads All Files send by Nedbank.
        /// </summary>
        private void ReadResponseFiles()
        {
            string path = _fileUtility.GetResponseFilePath();
            _responseReader.ReadFiles(path);
        }

        #endregion
    }
}