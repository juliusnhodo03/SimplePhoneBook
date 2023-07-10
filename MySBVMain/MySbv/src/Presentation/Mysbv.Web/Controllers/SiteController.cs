using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.Site;
using Application.Modules.Common;
using Application.Modules.Common.Helpers;
using Application.Modules.Maintanance.Merchant;
using Application.Modules.Maintanance.Site;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
    public class SiteController : BaseController
    {
        private readonly IUserAccountValidation _accountValidation;
        private readonly ILookup _lookup;
        private readonly IMerchantValidation _merchantValidation;
        private readonly ISiteValidation _siteValidation;

        public SiteController()
        {
            _accountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
            _merchantValidation = LocalUnityResolver.Retrieve<IMerchantValidation>();
            _siteValidation = LocalUnityResolver.Retrieve<ISiteValidation>();
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
        }


        #region Controller Action

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<ListSiteDto> sites = _siteValidation.All();
                return View(sites);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        ///     Site Onload - Create
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public ActionResult Create(int merchantId = 0)
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();
                SiteDto siteDto;

                if (merchantId > 0)
                {
                    siteDto = new SiteDto
                    {
                        MerchantId = merchantId,
                        ContractNumber = _merchantValidation.Find(merchantId).EntityResult.ContractNumber
                    };
                }
                else
                {
                    siteDto = new SiteDto
                    {
                        MerchantId = merchantId,
                        ContractNumber = ""
                    };
                }

                return View(siteDto);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Create(SiteDto siteDto, string command)
        {
            if (VerifyAuthentication())
            {
                ModelState.Remove("Address.AddressTypeId");
                ModelState.Remove("Address.CashCenterAddressLine1");

                //Check if the Site name has been filled in
                if (string.IsNullOrWhiteSpace(siteDto.Name))
                {
                    ModelState.AddModelError("Site Name", "Please enter a Site Name");
                    PrepareDropDowns();
                    return View(siteDto);
                }

                //Check if Site Containers are Null
                if (siteDto.ContainerTypeIds == null)
                {
                    ModelState.AddModelError("Site Containers", "Site must at least have one Site Container");
                    PrepareDropDowns();
                    return View(siteDto);
                }

                //Check that the Site Name already Exists before Inserting
                if (_siteValidation.SiteNameExists(siteDto.Name))
                {
                    ModelState.AddModelError("Site Name", "Site Name already exists on the system");
                    ShowMessage("Site Name", MessageType.error, "Site Name already exists on the system");
                    PrepareDropDowns();
                    return View(siteDto);
                }

                //Check that the Street Number already Exists before Inserting
                if ((string.IsNullOrWhiteSpace(siteDto.Address.AddressLine1)))
                {
                    ModelState.AddModelError("Street Number", "Street Number is required");
                    PrepareDropDowns();
                    return View(siteDto);
                }

                //Check that the Street Name already Exists before Inserting
                if ((string.IsNullOrWhiteSpace(siteDto.Address.AddressLine2)))
                {
                    ModelState.AddModelError("Street Name", "Street Name is required");
                    PrepareDropDowns();
                    return View(siteDto);
                }


                //Check that the CIT Code already Exists before Inserting
                if (_siteValidation.CitCodeExists(siteDto.CitCode))
                {
                    ModelState.AddModelError("CitCode", "CIT Code already exists on the system");
                    ShowMessage("CIT Code", MessageType.error, "CIT Code already exists on the system");
                    PrepareDropDowns();
                    return View(siteDto);
                }

                ModelState.Remove("Address.AddressTypeId");
                ModelState.Remove("Address.CashCenterAddressLine1");

                if (!ModelState.IsValid)
                {
                    PrepareDropDowns();
                    ShowMessage("Error Saving Site. Please call helpdesk for support.", MessageType.error,
                        "Error Saving Site");
                    return View(siteDto);
                }

                switch (command)
                {
                    case "Save":

                        if (_siteValidation.Add(siteDto, User.Identity.Name).Status == MethodStatus.Successful)
                        {
                            ShowMessage("Site saved successfully", MessageType.success, "Save Site");
                            return RedirectToAction("Index");
                        }
                        return RedirectToAction("Create", siteDto);

                    case "Continue":
                        MethodResult<int> result = _siteValidation.Submit(siteDto, User.Identity.Name);
                        if (result.Status == MethodStatus.Successful)
                        {
                            ShowMessage("Site saved successfully", MessageType.success, "Save Site");
                            return RedirectToAction("Create", "BankAccount", new {@siteId = result.EntityResult});
                        }
                        PrepareDropDowns();
                        return View(siteDto);

                    default:
                        ShowMessage("Error Saving Site. Please call helpdesk for support.", MessageType.error,
                            "Error Saving Site");
                        PrepareDropDowns();
                        return View(siteDto);
                }
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Edit(int id = 0)
        {
            if (VerifyAuthentication())
            {
                MethodResult<SiteDto> result = _siteValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Site");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
        public ActionResult View(int id = 0, SiteDto site = null)
        {
            if (VerifyAuthentication())
            {
                DisableModelState();

                MethodResult<SiteDto> result = _siteValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Site");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                if (id > 0)
                {
                    MethodResult<SiteDto> site = _siteValidation.Find(id);
                    if (site != null)
                    {
                        MethodResult<bool> isInUse = _siteValidation.IsInUse(id);
                        if (isInUse.Status == MethodStatus.Error)
                        {
                            ShowMessage(isInUse.Message, MessageType.error, "Delete Site");
                            return Json(new {url = Url.Action("Index")});
                        }


                        MethodResult<Task> result = _siteValidation.Delete(id, User.Identity.Name);
                        if (result.Status == MethodStatus.Successful)
                        {
                            ShowMessage(result.Message, MessageType.success, "Deleted Site");
                        }
                        else
                        {
                            MessageType messageType = (result.Status == MethodStatus.Warning)
                                ? MessageType.warning
                                : MessageType.error;
                            ShowMessage(result.Message, messageType, "Delete Site");
                        }
                    }
                }
                return Json(new {url = Url.Action("Index")});
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(SiteDto site)
        {
            if (VerifyAuthentication())
            {
                ModelState.Remove("Address.AddressTypeId");
                ModelState.Remove("Address.CashCenterAddressLine1");


                if (string.IsNullOrWhiteSpace(site.Name))
                {
                    ModelState.AddModelError("Site Name", "Please enter a Site Name");
                    PrepareDropDowns();
                    return View(site);
                }

                if (site.ContainerTypeIds == null)
                {
                    ModelState.AddModelError("Site Containers", "Site must at least have one Site Container");
                    PrepareDropDowns();
                    return View(site);
                }

                //Check that the Site Name already Exists before Inserting
                if (_siteValidation.NameUsedByAnotherSite(site.Name, site.SiteId))
                {
                    ModelState.AddModelError("Site Name", "You cannot give a name of another existing Site!");
                    PrepareDropDowns();
                    return View(site);
                }

                //Check that the Street Number already Exists before Inserting
                if ((string.IsNullOrWhiteSpace(site.Address.AddressLine1)))
                {
                    ModelState.AddModelError("Street Number", "Street Number is required");
                    PrepareDropDowns();
                    return View(site);
                }

                //Check that the Street Name already Exists before Inserting
                if ((string.IsNullOrWhiteSpace(site.Address.AddressLine2)))
                {
                    ModelState.AddModelError("Street Name", "Street Name is required");
                    PrepareDropDowns();
                    return View(site);
                }

                //Check that the CIT Code already Exists before Inserting
                if (_siteValidation.CitCodeUsedByAnotherSite(site.CitCode, site.SiteId))
                {
                    ModelState.AddModelError("CitCode", "CIT Code already exists on the system");
                    ShowMessage("CIT Code", MessageType.error, "CIT Code already exists on the system");
                    PrepareDropDowns();
                    return View(site);
                }


                if (ModelState.IsValid)
                {
                    MethodResult<bool> result = _siteValidation.Edit(site, User.Identity.Name);
                    if (result.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Site updated successfully.", MessageType.success,
                            "Site data");
                    }
                    else ShowMessage(result.Message, MessageType.error, "Update Site.");
                }
                else
                {
                    ShowMessage("Error updating Site", MessageType.error, "Site Error");
                    PrepareDropDowns();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        private void PrepareDropDowns()
        {
            if (!ViewData.ContainsKey("GetSettlementTypes"))
                ViewData.Add("GetSettlementTypes",
                    new SelectList(_lookup.GetSettlementTypes().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("GetMerchantsKendo"))
                ViewData.Add("GetMerchantsKendo", new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("Cities"))
                ViewData.Add("Cities", new SelectList(_lookup.GetCities().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("CashCenters"))
                ViewData.Add("CashCenters", new SelectList(_lookup.CashCenters().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("CitCarriers"))
                ViewData.Add("CitCarriers", new SelectList(_lookup.GetCitCarries().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("GetContainerTypes"))
                ViewData.Add("GetContainerTypes", new SelectList(_lookup.GetContainerTypes(), "Id", "Name"));
        }

        // GET: /Sites/GetMerchantsKendo
        public JsonResult GetMerchantsKendo()
        {
            IEnumerable<DropDownModel> merchants = _lookup.Merchants().ToDropDownModel();
            return Json(merchants, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetContainerTypes()
        {
            IEnumerable<DropDownModel> containerTypes = _lookup.GetContainerTypes().ToDropDownModel().RemoveFirst();
            return Json(containerTypes, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetMerchantContract(int merchantId)
        {
            if (merchantId > 0)
            {
                Merchant merchant = _lookup.GetMerchantById(merchantId);
                return Json(new {merchant.ContractNumber}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {ContractNumber = "Undefined Contract Number"}, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCoordinates(string address)
        {
            GeoGeometry geoResponse = GeoLocationHelper.GetCoordinates(address);

            return Json(geoResponse == null ? new GeoLocation {Lat = 0, Lng = 0} : geoResponse.Location,
                JsonRequestBehavior.AllowGet);
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
            ModelState.Remove("ContainerTypeIds");
            ModelState.Remove("DepositReference");
        }

        #endregion

        #region Listview Action

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public ActionResult SiteColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Contract Number", Tag = "ContractNumber"},
                new DropDownModel {Id = 2, Name = "Merchant Name", Tag = "MerchantName"},
                new DropDownModel {Id = 3, Name = "Site Name", Tag = "Name"},
                new DropDownModel {Id = 4, Name = "CIT Code", Tag = "CitCode"},
                new DropDownModel {Id = 5, Name = "Syspro Number", Tag = "SysproNumber"},
                new DropDownModel {Id = 6, Name = "Cash Centre", Tag = "CashCentreName"} //,
                //new DropDownModel {Id = 7, Name = "Status", Tag = "StatusName"}
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// </summary>
        /// <param name="columName"></param>
        /// <param name="searchData"></param>
        /// <returns></returns>
        public JsonResult AutoCompleteSitesByColumn(string columName, string searchData)
        {
            List<ListSiteDto> sites = _siteValidation.All().ToList();

            //return Json(merchants, JsonRequestBehavior.AllowGet);

            var items = new ArrayList();

            switch (columName)
            {
                case "ContractNumber":
                {
                    foreach (ListSiteDto site in sites.Where(e => string.IsNullOrEmpty(e.ContractNumber) == false))
                    {
                        if (site.ContractNumber.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(site.ContractNumber);
                        }
                    }
                    break;
                }
                case "MerchantName":
                {
                    foreach (ListSiteDto site in sites.Where(e => string.IsNullOrEmpty(e.MerchantName) == false))
                    {
                        if (site.MerchantName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(site.MerchantName);
                        }
                    }
                    break;
                }
                case "Name":
                {
                    foreach (ListSiteDto site in sites.Where(e => string.IsNullOrEmpty(e.Name) == false))
                    {
                        if (site.Name.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(site.Name);
                        }
                    }
                    break;
                }
                case "CitCode":
                {
                    foreach (ListSiteDto site in sites.Where(e => string.IsNullOrEmpty(e.CitCode) == false))
                    {
                        if (site.CitCode.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(site.CitCode);
                        }
                    }
                    break;
                }
                case "SysproNumber":
                {
                    foreach (ListSiteDto site in sites.Where(e => string.IsNullOrEmpty(e.SysproNumber) == false))
                    {
                        if (site.SysproNumber.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(site.SysproNumber);
                        }
                    }
                    break;
                }
                case "CashCentreName":
                {
                    foreach (ListSiteDto site in sites.Where(e => string.IsNullOrEmpty(e.CashCentreName) == false))
                    {
                        if (site.CashCentreName.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(site.CashCentreName);
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