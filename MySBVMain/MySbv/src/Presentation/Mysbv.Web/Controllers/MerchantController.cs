using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.Merchant;
using Application.Modules.Common;
using Application.Modules.Maintanance.Merchant;
using Domain.Data.Model;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
    public class MerchantController : BaseController
    {
        private readonly ILookup _lookup;
        private readonly IMerchantValidation _merchantValidation;

        public MerchantController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _merchantValidation = LocalUnityResolver.Retrieve<IMerchantValidation>();
        }



        #region Controller Actions

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVRecon, SBVFinanceReviewer")]
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<ListMerchantDto> merchants = _merchantValidation.All();
                return View(merchants);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Create(MerchantDto merchant, string command)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(merchant.Name))
                {
                    ModelState.AddModelError("Merchant Name", "Please enter a Merchant Name");
                    return View(merchant);
                }

                ModelState.Remove("ActiveStatus");

                if (!ModelState.IsValid)
                {
                    ShowMessage("Error Saving Merchant. Please call helpdesk for support.", MessageType.error,
                        "Error Saving Merchant");
                    PrepareDropDowns();
                    return View(merchant);
                }


                if (_merchantValidation.IsContractNumberUsed(merchant.ContractNumber))
                {
                    ModelState.AddModelError("Contract Number", "Contract Number already exists on the system");
                    PrepareDropDowns();
                    return View(merchant);
                }

                if (_merchantValidation.IsRegistrationNumberUsed(merchant.RegistrationNumber))
                {
                    ModelState.AddModelError("Registration Number", "Registration Number already exists on the system");
                    PrepareDropDowns();
                    return View(merchant);
                }

                if (_merchantValidation.IsNameUsed(merchant.Name))
                {
                    ModelState.AddModelError("Merchant Name", "Merchant Name already exists on the system");
                    PrepareDropDowns();
                    return View(merchant);
                }

                switch (command)
                {
                    case "Save":

                        if (_merchantValidation.Add(merchant, User.Identity.Name).Status == MethodStatus.Successful)
                        {
                            ShowMessage("Merchant saved successfully", MessageType.success, "Save Merchant");
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Create", merchant);


                    case "Continue":
                        MethodResult<int> result = _merchantValidation.Add(merchant, User.Identity.Name);
                        if (result.Status == MethodStatus.Successful)
                        {
                            ShowMessage("Merchant saved successfully", MessageType.success, "Save Merchant");
                            return RedirectToAction("Create", "Site", new {@merchantId = result.EntityResult});
                        }
                        PrepareDropDowns();
                        return View(merchant);
                    default:
                        ShowMessage("Error Saving Merchant. Please call helpdesk for support.", MessageType.error,
                            "Error Saving Merchant");
                        PrepareDropDowns();
                        return View(merchant);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Update(int id = 0)
        {
            if (VerifyAuthentication())
            {
                MerchantDto model = _merchantValidation.Find(id).EntityResult;

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Update(MerchantDto merchant)
        {
            if (VerifyAuthentication())
            {
                if (ModelState.IsValid)
                {
                    if (_merchantValidation.IsContractNumberUsedByAnother(merchant.ContractNumber, merchant.MerchantId))
                    {
                        ModelState.AddModelError("", "");
                        ShowMessage("You cannot give a contract Number of another existing Merchant!", MessageType.error,
                            "Update Failed");
                        PrepareDropDowns();
                        return View(merchant);
                    }

                    if (_merchantValidation.IsRegistrationNumberUsedByAnother(merchant.RegistrationNumber,
                        merchant.MerchantId))
                    {
                        ModelState.AddModelError("", "");
                        ShowMessage("You cannot give a registration Number of another existing Merchant!",
                            MessageType.error, "Update Failed");
                        PrepareDropDowns();
                        return View(merchant);
                    }

                    if (_merchantValidation.IsNameUsedByAnother(merchant.Name, merchant.MerchantId))
                    {
                        ModelState.AddModelError("", "");
                        ShowMessage("You cannot give a merchant name of another existing Merchant!", MessageType.error,
                            "Update Failed");
                        PrepareDropDowns();
                        return View(merchant);
                    }

                    MethodResult<bool> result = _merchantValidation.Edit(merchant, User.Identity.Name);
                    if (result.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Merchant updated successfully. ", //"An email will be sent for Approval.",
                            MessageType.success, "Merchant data");
                        //User user = _lookup.GetUser(User.Identity.Name);
                        //string taskRefNumber = result.EntityResult.ReferenceNumber;
                        //SendForApproval(merchant, taskRefNumber);
                        //SendConfirmation(user.EmailAddress, taskRefNumber);
                    }
                    else ShowMessage(result.Message, MessageType.error, "Update Merchant.");
                    return RedirectToAction("Index");
                }
                ShowMessage("Error updating Merchant", MessageType.error, "Merchant Error");
                PrepareDropDowns();
                return View(merchant);
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover, SBVRecon, SBVFinanceReviewer")]
        public ActionResult View(int id = 0)
        {
            if (VerifyAuthentication())
            {
                MerchantDto model = _merchantValidation.Find(id).EntityResult;

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                if (id > 0)
                {
                    MethodResult<MerchantDto> merchant = _merchantValidation.Find(id);
                    if (merchant != null)
                    {
                        MethodResult<Task> result = _merchantValidation.Delete(id, User.Identity.Name);
                        if (result.Status == MethodStatus.Successful)
                        {
                            ShowMessage(result.Message, MessageType.success, "Deleted Merchant");
                        }
                        else
                        {
                            MessageType messageType = (result.Status == MethodStatus.Warning)
                                ? MessageType.warning
                                : MessageType.error;
                            ShowMessage(result.Message, messageType, "Delete Merchant");
                        }
                    }
                }
                return Json(new {url = Url.Action("Index")});
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult MerchantPopupCreate([DataSourceRequest] DataSourceRequest request, MerchantDto merchant)
        {
            if (merchant != null && ModelState.IsValid)
            {
                _merchantValidation.Add(merchant, User.Identity.Name);
            }

            return Json(new[] {merchant}.ToDataSourceResult(request, ModelState));
        }

        private void PrepareDropDowns()
        {
            if (!ViewData.ContainsKey("MerchantDescriptions"))
                ViewData.Add("MerchantDescriptions",
                    new SelectList(_lookup.GetMerchantDescriptions().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("GetCompanyTypes"))
                ViewData.Add("GetCompanyTypes",
                    new SelectList(_lookup.GetCompanyTypes().ToDropDownModel(), "Id", "Name"));
        }

        private void DisableModelState()
        {
            ModelState.Remove("Name");
            ModelState.Remove("MerchantId");
            ModelState.Remove("CitCarrierId");
            ModelState.Remove("CityId");
            ModelState.Remove("CashCenterId");
            ModelState.Remove("SettlementTypeId");
            ModelState.Remove("CitCode");
            ModelState.Remove("Description");
            ModelState.Remove("SysproNumber");
            ModelState.Remove("ContactPersonEmailAddress1");
            ModelState.Remove("ContactPersonNumber1");
            ModelState.Remove("ContactPersonDesignation1");
            ModelState.Remove("ContactPersonEmailAddress2");
            ModelState.Remove("ContactPersonNumber2");
            ModelState.Remove("ContactPersonName1");
        }

        #endregion

        #region Listview Action

        public ActionResult MerchantReadToolbarTemplate([DataSourceRequest] DataSourceRequest request)
        {
            IEnumerable<ListMerchantDto> merchants = _merchantValidation.All();
            return Json(merchants.ToDataSourceResult(request));
        }

        public ActionResult MerchantsColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Contract Number", Tag = "ContractNumber"},
                new DropDownModel {Id = 3, Name = "Registered Name", Tag = "RegisteredName"},
                new DropDownModel {Id = 5, Name = "Company Group Name", Tag = "CompanyGroupName"},
                new DropDownModel {Id = 6, Name = "Franchise Name", Tag = "FranchiseName"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteMerchantsByColumn(string columName, string searchData)
        {
            List<ListMerchantDto> merchants = _merchantValidation.All().ToList();

            //return Json(merchants, JsonRequestBehavior.AllowGet);

            var items = new ArrayList();

            switch (columName)
            {
                case "ContractNumber":
                {
                    foreach (
                        ListMerchantDto merchant in
                            merchants.Where(e => string.IsNullOrEmpty(e.ContractNumber) == false))
                    {
                        if (merchant.ContractNumber.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(merchant.ContractNumber);
                        }
                    }
                    break;
                }
                case "RegisteredName":
                {
                    foreach (
                        ListMerchantDto merchant in
                            merchants.Where(e => string.IsNullOrEmpty(e.RegisteredName) == false))
                    {
                        if (merchant.RegisteredName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(merchant.RegisteredName);
                        }
                    }
                    break;
                }
                case "CompanyGroupName":
                {
                    foreach (
                        ListMerchantDto merchant in
                            merchants.Where(e => string.IsNullOrEmpty(e.CompanyGroupName) == false))
                    {
                        if (merchant.CompanyGroupName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(merchant.CompanyGroupName);
                        }
                    }
                    break;
                }
                case "FranchiseName":
                {
                    foreach (
                        ListMerchantDto merchant in merchants.Where(e => string.IsNullOrEmpty(e.FranchiseName) == false)
                        )
                    {
                        if (merchant.FranchiseName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(merchant.FranchiseName);
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