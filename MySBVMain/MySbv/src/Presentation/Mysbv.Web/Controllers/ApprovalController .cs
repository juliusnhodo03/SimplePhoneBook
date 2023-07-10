using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.Task;
using Application.Modules.Common;
using Application.Modules.Maintanance.Approval;
using Mysbv.Web.CustomAttributes;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Approval Account related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
    public class ApprovalController : BaseController
    {

        private readonly IApprovalValidation _approvalValidation;
        private readonly ILookup _lookup;

        public ApprovalController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _approvalValidation = LocalUnityResolver.Retrieve<IApprovalValidation>();
        }





        #region Controller Actions

        /// <summary>
        /// </summary>
        /// <returns>Redirect to Approval Home Page</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVApprover, SBVDataCapture")]
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<ListTaskDto> approvals = _approvalValidation.All();
                return View(approvals);
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region ListView Actions

        /// <summary>
        ///     DropDowns for Approval
        /// </summary>
        /// <returns></returns>
        public ActionResult ApprovalColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Reference Number", Tag = "ReferenceNumber"},
                new DropDownModel {Id = 2, Name = "Merchant Name", Tag = "MerchantName"},
                new DropDownModel {Id = 3, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 4, Name = "User Name", Tag = "UserName"},
                new DropDownModel {Id = 5, Name = "Module", Tag = "Module"},
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     AutoComplete as User is typing......
        /// </summary>
        /// <param name="columName">User Text as is Typing....</param>
        /// <param name="searchData">Display suggestion Based on what is Typing....</param>
        /// <returns>Return user Result Information</returns>
        public JsonResult AutoCompleteApprovalByColumn(string columName, string searchData)
        {
            List<ListTaskDto> approvals = _approvalValidation.All().ToList();

            var items = new List<string>();

            switch (columName)
            {
                case "ReferenceNumber":
                {
                    foreach (
                        ListTaskDto approval in approvals.Where(e => string.IsNullOrEmpty(e.ReferenceNumber) == false))
                    {
                        if (approval.ReferenceNumber.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(approval.ReferenceNumber);
                        }
                    }
                    break;
                }
                case "MerchantName":
                {
                    foreach (
                        ListTaskDto approval in approvals.Where(e => string.IsNullOrEmpty(e.MerchantName) == false))
                    {
                        if (approval.MerchantName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(approval.MerchantName);
                        }
                    }
                    break;
                }
                case "SiteName":
                {
                    foreach (
                        ListTaskDto approval in approvals.Where(e => string.IsNullOrEmpty(e.SiteName) == false))
                    {
                        if (approval.SiteName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(approval.SiteName);
                        }
                    }
                    break;
                }
                case "Module":
                {
                    foreach (
                        ListTaskDto approval in approvals.Where(e => string.IsNullOrEmpty(e.Module) == false))
                    {
                        if (approval.Module.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(approval.Module);
                        }
                    }
                    break;
                }
            }

            return Json(items.Distinct(), JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}