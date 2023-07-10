using System;
using System.IO;
using System.Web.Mvc;
using Application.Modules.Common;

namespace Mysbv.Web.Controllers
{
    public class DownloadController : BaseController
    {
        private readonly ILookup _lookup;

        public DownloadController(ILookup lookup)
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
        }




        #region Downloads

        /// <summary>
        ///     Download EFT Attachment
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <param name="fileName"></param>
        [AllowAnonymous]
        public FileResult EftAttachments(int cashOrderId, string fileName)
        {
            try
            {
                byte[] bytes = GetFileBytes(cashOrderId, fileName);

                string extension = Path.GetExtension(fileName);

                if (!string.IsNullOrWhiteSpace(extension))
                {
                    extension = extension.Replace(".", "");
                }

                // send the file to browser
                FileResult fileResult = new FileContentResult(bytes, string.Format("application/{0}", extension));
                fileResult.FileDownloadName = fileName;

                return fileResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion


        
        #region Helpers

        /// <summary>
        ///     Download cash order EFT attachment
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <param name="fileName"></param>
        private byte[] GetFileBytes(int cashOrderId, string fileName)
        {
            string baseUrl = _lookup.GetConfigurationValue("CASH_ORDER_EFT_ATTACHMENTS_URL");

            string physicalPath = Path.Combine(baseUrl, cashOrderId.ToString());

            string file = Path.Combine(physicalPath, fileName);

            byte[] bytes = System.IO.File.ReadAllBytes(file);

            return bytes;
        }

        #endregion
    }
}