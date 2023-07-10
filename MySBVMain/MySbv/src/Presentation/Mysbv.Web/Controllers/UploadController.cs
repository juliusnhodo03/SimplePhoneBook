using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Application.Modules.Common;

namespace Mysbv.Web.Controllers
{
    public class UploadController : BaseController
    {
        private readonly ILookup _lookup;

        public UploadController(ILookup lookup)
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="cashOrderId"></param>
        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, int cashOrderId)
        {
            // The Name of the Upload component is "files"
            
            if (files != null)
            {
                foreach (var file in files)
                {
                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);

                    // { cashOrderId == 0 } means a new Order
                    // Attachments have been sent before the Order is Saved or Submitted

                    // So Create Temporary Folder with name as UserName or cashOrderId
                    var baseUrl = _lookup.CreateTemporaryFolder(cashOrderId, User.Identity.Name);

                    var physicalPath = Path.Combine(baseUrl, fileName); 

                    // The files are saved to the physical path
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="cashOrderId"></param>
        public ActionResult Remove(string[] fileNames, int cashOrderId)
        {
            // The parameter of the Remove action must be called "fileNames"

            var rootLocationUrl = _lookup.GetConfigurationValue("CASH_ORDER_EFT_ATTACHMENTS_URL");

            // So Create Temporary Folder with name as UserName or cashOrderId

            var physicalPath = _lookup.CreateTemporaryFolder(cashOrderId, User.Identity.Name);

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    physicalPath = Path.Combine(physicalPath, fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are removed from disk location
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }
    }
}