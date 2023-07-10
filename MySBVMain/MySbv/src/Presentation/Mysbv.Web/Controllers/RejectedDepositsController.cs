using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Modules.FinancialManagement;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Security;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [CustomAuthorize(Roles = "SBVAdmin, SBVApprover, SBVTellerSupervisor")]
    public class RejectedDepositsController : BaseController
    {
        private readonly ISecurity _security;
        private readonly IRejectedDepositsValidation _rejectedDeposits;
        private readonly IUserAccountValidation _userAccountValidation;

        public RejectedDepositsController()
        {
            _security = LocalUnityResolver.Retrieve<ISecurity>();
            _rejectedDeposits = LocalUnityResolver.Retrieve<IRejectedDepositsValidation>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
        }


        #region Financial Management Controller Actions

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                User user = _userAccountValidation.UserByName(User.Identity.Name);
                ViewBag.IsValidUser = (user.UserTypeId != null);

                var result = _rejectedDeposits.All().EntityResult;
                return View(result);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dropId"></param>
        /// <param name="paymentId"></param>
        public ActionResult Details(int id, int dropId, int paymentId)
        {
            if (VerifyAuthentication())
            {
                var result = _rejectedDeposits.FindRejectedRecord(id, dropId, paymentId);
                return View(result.EntityResult);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public ActionResult PaymentDetails(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _rejectedDeposits.FindRejectedRecord(0, 0, id);
                return View("Details", result.EntityResult);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public ActionResult DropDetails(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _rejectedDeposits.FindRejectedRecord(0, id, 0);
                return View("Details", result.EntityResult);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public ActionResult DepositDetails(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _rejectedDeposits.FindRejectedRecord(id, 0, 0);
                return View("Details", result.EntityResult);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dropId"></param>
        /// <param name="paymentId"></param>
        [CustomAuthorize(Roles = "SBVAdmin, SBVTellerSupervisor")]
        public ActionResult Submit(int id, int dropId, int paymentId)
        {
            if (VerifyAuthentication())
            {
                var userId = _security.GetUserId(User.Identity.Name);
                var result = _rejectedDeposits.Submit(id, dropId, paymentId, userId);

                if (result.Status == MethodStatus.Successful)
                {
                    ShowMessage(result.Message, MessageType.success, "Submit Rejected Transaction");
                    return RedirectToAction("Index");
                }
                ShowMessage(result.Message, MessageType.error, "Submit Rejected Transaction");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult RejectedDepositColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Deposit Date", Tag = "DepositDateTime"},
                new DropDownModel {Id = 2, Name = "Rejection Date", Tag = "RejectionDateTime"},
                new DropDownModel {Id = 3, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 4, Name = "CIT Code", Tag = "CitCode"},
                new DropDownModel {Id = 5, Name = "Transaction Reference", Tag = "DepositReference"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="columName"></param>
        /// <param name="searchData"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        public JsonResult AutoCompleteRejectedDepositByColumn(string columName, string searchData, int userType)
        {
            var _rejectedTransactionDtos = _rejectedDeposits.All().EntityResult;
            var items = new ArrayList();

            switch (columName)
            {
                case "DepositDateTime":
                {
                    foreach (var deposit in
                            _rejectedTransactionDtos.Where(e => string.IsNullOrEmpty(e.DepositDateTime) == false))
                    {
                        if (deposit.DepositDateTime.StartsWith(searchData.ToLower()))
                        {
                            items.Add(deposit.DepositDateTime);
                        }
                    }
                    break;
                }
                case "RejectionDateTime":
                {
                    foreach (var deposit in
                            _rejectedTransactionDtos.Where(e => string.IsNullOrEmpty(e.RejectionDateTime) == false))
                    {
                        if (deposit.RejectionDateTime.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(deposit.RejectionDateTime);
                        }
                    }
                    break;
                }
                case "SiteName":
                {
                    foreach (var deposit in
                            _rejectedTransactionDtos.Where(e => string.IsNullOrEmpty(e.SiteName) == false))
                    {
                        if (deposit.SiteName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(deposit.SiteName);
                        }
                    }
                    break;
                }
                case "CitCode":
                {
                    foreach (var deposit in
                            _rejectedTransactionDtos.Where(e => string.IsNullOrEmpty(e.CitCode) == false))
                    {
                        if (deposit.CitCode.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(deposit.CitCode);
                        }
                    }
                    break;
                }
                case "DepositReference":
                {
                    foreach (var deposit in
                            _rejectedTransactionDtos.Where(e => string.IsNullOrEmpty(e.DepositReference) == false))
                    {
                        if (deposit.DepositReference.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(deposit.DepositReference);
                        }
                    }
                    break;
                }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion
	}
}