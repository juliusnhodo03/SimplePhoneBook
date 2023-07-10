
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Application.Dto.FailedVaultTransaction;
using Application.Modules.Common;
using Application.Modules.FailedVaultRequest;
using Application.Modules.UserAccountValidation;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
	/// <summary>
	/// Failed VAult Transactions Controller
	/// </summary>
	[Authorize]
	public class VaultTransactionsController : BaseController
	{
		private readonly IVaultRequestValidation _vaultRequestValidation;
		private readonly ILookup _lookup;
		private readonly IUserAccountValidation _userAccountValidation;

        public VaultTransactionsController()
        {
			_vaultRequestValidation = LocalUnityResolver.Retrieve<IVaultRequestValidation>();
			_userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
			_lookup = LocalUnityResolver.Retrieve<ILookup>();
		}


        /// <summary>
        /// List all failed transactions
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
		public ActionResult Index()
		{
			if (VerifyAuthentication())
			{
				// get logged in user
				var user = _userAccountValidation.UserByName(User.Identity.Name);

				// set valid user status
				ViewBag.IsValidUser = (user.UserTypeId != null);

				// get all failed requests
				var failedRequests = _vaultRequestValidation.GetFailedHeaders();

				// send Requests to UI
				return View(failedRequests);
			}
			// if not logged in; send to the login page
			return RedirectToAction("Login", "Account");
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public ActionResult FailedRequests([DataSourceRequest] DataSourceRequest request) 
		{
			var failedRequests = _vaultRequestValidation.GetFailedHeaders();
			return Json(failedRequests.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
		}


		/// <summary>
		/// Displays a Failed Request in non editable mode
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		[CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
		public ActionResult Edit(int id)
		{
			if (VerifyAuthentication())
			{
				// get logged user
				var user = _userAccountValidation.UserByName(User.Identity.Name);

				// Find Request by Id
				var result = _vaultRequestValidation.Find(id);

				if (result.Status != MethodStatus.Successful)
				{
					ShowMessage(result.Message, MessageType.error, "Vault Deposit");
					return RedirectToAction("Index", "VaultTransactions");
				}

				var requestItem = result.EntityResult;
				requestItem.IndexUrl = Url.Action("Index");
				requestItem.PostUrl = Url.Action("Edit");
				requestItem.ApproveUrl = Url.Action("Approve");
				requestItem.DeclineUrl = Url.Action("Decline");
				requestItem.IsApprover = Roles.IsUserInRole("SBVApprover");
				requestItem.SameAsCapturer = user.UserId == requestItem.UserId;

				var request = JavaScriptConvert.SerializeObject(requestItem);
				ViewBag.Model = request;

				return View();

				//return request;
			}
			return RedirectToAction("Login", "Account");
		}
		

		/// <summary>
		/// Displays a Failed Request in non editable mode
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Edit(FailedRequestDto request)
		{
			// validate request
			var validRequest = _vaultRequestValidation.ValidationMessage(request.RequestMessage);

			// check if validation failed
			if (validRequest.Status == MethodStatus.Error)
			{
				return Json(new
				{
					ResponseCode = false,
					Message = validRequest.EntityResult
				}, JsonRequestBehavior.AllowGet);
			}

			// get logged user
			var user = _userAccountValidation.UserByName(User.Identity.Name);

			// try to update request changes from UI.
			var saveResult = _vaultRequestValidation.Edit(request, user);

			var message = new List<MessageError> { new MessageError { Error = saveResult.Message } };

			// return update status		
			return Json(new
			{
				ResponseCode = saveResult.EntityResult,
				Message = message
			}, JsonRequestBehavior.AllowGet);
		}
		

		/// <summary>
		/// Approves a Failed Request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Approve(FailedRequestDto request)
		{
			// get logged user
			var user = _userAccountValidation.UserByName(User.Identity.Name);

			// Approve request.
			var saveResult = _vaultRequestValidation.Approve(request, user);

			var message = new List<MessageError> { new MessageError { Error = saveResult.Message } };

			// return approval status		
			return Json(new
			{
				ResponseCode = saveResult.EntityResult,
				Message = message
			}, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// Approves a Failed Request
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public ActionResult Decline(FailedRequestDto request) 
		{
			// get logged user
			var user = _userAccountValidation.UserByName(User.Identity.Name);

			// Decline request.
			var saveResult = _vaultRequestValidation.Decline(request, user);

			var message = new List<MessageError> { new MessageError { Error = saveResult.Message } };

			// return approval status		
			return Json(new
			{
				ResponseCode = saveResult.EntityResult,
				Message = message
			}, JsonRequestBehavior.AllowGet);
		}
		

		#region LIST GRID Methods
		/// <summary>
		/// Failed requests dropdwon
		/// </summary>
		/// <returns>Id,Name,Tag</returns>
		public ActionResult FailedRequestsColumnsListingToolbarTemplate()
		{
			var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Cit Code", Tag = "CitCode"},
                new DropDownModel {Id = 2, Name = "Beneficiary Code", Tag = "BeneficiaryCode"},
                new DropDownModel {Id = 3, Name = "Serial Number", Tag = "SerialNumber"},
                new DropDownModel {Id = 4, Name = "Supplier", Tag = "Supplier"},
                new DropDownModel {Id = 5, Name = "Cit Status", Tag = "CitReceivedStatus"},
            };
			return Json(categories, JsonRequestBehavior.AllowGet);
		}


		/// <summary>
		/// AutoCompleteCashDeposits as User is Typing...
		/// </summary>
		/// <param name="columName">Is User text as Typing...</param>
		/// <param name="searchData">Is user result as Suggestion or result</param>
		/// <returns>Return user Results</returns>
		public JsonResult AutoCompleteByColumn(string columName, string searchData)
		{
			var result = _vaultRequestValidation.GetFailedHeaders();

			var items = new List<string>();

			switch (columName)
			{
				case "CitCode":
					{
						foreach (var item in result.Where(e => string.IsNullOrEmpty(e.CitCode) == false))
						{
							if (item.CitCode.ToLower().StartsWith(searchData.ToLower()))
							{
								items.Add(item.CitCode);
							}
						}
						break;
					}
				case "BeneficiaryCode":
					{
						foreach (var item in result.Where(e => string.IsNullOrEmpty(e.BeneficiaryCode) == false))
						{
							if (item.BeneficiaryCode.ToLower().StartsWith(searchData.ToLower()))
							{
								items.Add(item.BeneficiaryCode);
							}
						}
						break;
					}
				case "SerialNumber":
					{
						foreach (var item in result.Where(e => string.IsNullOrEmpty(e.SerialNumber) == false))
						{
							if (item.SerialNumber.ToLower().StartsWith(searchData.ToLower()))
							{
								items.Add(item.SerialNumber);
							}
						}
						break;
					}
				case "Supplier":
					{
						foreach (var item in result.Where(e => string.IsNullOrEmpty(e.Supplier) == false))
						{
							if (item.Supplier.ToLower().StartsWith(searchData.ToLower()))
							{
								items.Add(item.Supplier);
							}
						}
						break;
					}
				case "CitReceivedStatus":
					{
						foreach (var item in result.Where(e => string.IsNullOrEmpty(e.CitReceivedStatus) == false))
						{
							if (item.CitReceivedStatus.ToLower().StartsWith(searchData.ToLower()))
							{
								items.Add(item.CitReceivedStatus);
							}
						}
						break;
					}
			}
			return Json(items.Distinct(), JsonRequestBehavior.AllowGet);
		}


		/// <summary>
		/// 
		/// </summary>
		/// <param name="serialNumber"></param>
		/// <param name="request"></param>
		/// <returns></returns>
		public ActionResult FailedNonCitMessages(string serialNumber, [DataSourceRequest] DataSourceRequest request) 
		{
			var result = _vaultRequestValidation.GetFailedRequests(serialNumber);

			return Json(result.ToDataSourceResult(request));
		}

		#endregion
	}
}