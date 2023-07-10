using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.CashCenter;
using Application.Modules.Common;
using Application.Modules.Common.Helpers;
using Application.Modules.Maintanance.CashCenter;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Cashcenter related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class CashCenterController : BaseController
    {
        private readonly ICashCenterValidation _cashCenterValidation;
        private readonly ILookup _lookup;

        public CashCenterController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _cashCenterValidation = LocalUnityResolver.Retrieve<ICashCenterValidation>();
        }



        #region Controller Actions
        /// <summary>
        /// Cash Center home Page
        /// </summary>
        /// <returns>Return All Cash Centers</returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                var cashCenters = _cashCenterValidation.All();
                return View(cashCenters);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        ///Create Cash Center
        /// </summary>
        /// <returns>Create Cash Center</returns>
        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Validate and Create CashCenter
        /// </summary>
        /// <param name="cashCenterDto">Represent Cash Center</param>
        /// <returns>Save Cash Center  and Redirect t CashCenter Home Page</returns>
        [HttpPost]
        public ActionResult Create(CashCentreDto cashCenterDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(cashCenterDto.Address.AddressLine1))
                {
                    ModelState.AddModelError("Address Line 1", "Address Line 1 is required");
                    return View(cashCenterDto);
                }

                //Ensure that the user has entered a Cash Center Name
                if (string.IsNullOrWhiteSpace(cashCenterDto.Name))
                {
                    ModelState.AddModelError("Cash Center Name", "Please enter a Cash Center Name");
                    return View(cashCenterDto);
                }

                if (_cashCenterValidation.ExistsByName(cashCenterDto.Name))
                {
                    ModelState.AddModelError("Cash Center Name", "Cash Centre Name already exists on the system");
                    PrepareDropDowns();
                    return View(cashCenterDto);
                }

                if (string.IsNullOrWhiteSpace(cashCenterDto.Address.AddressLine1))
                {
                    ModelState.AddModelError("Address Line 1", "Address Line 1 is required");
                    PrepareDropDowns();
                    return View(cashCenterDto);
                }


                if (_cashCenterValidation.ExistsByNumber(cashCenterDto.Number))
                {
                    ModelState.AddModelError("Cash Center Number", "Cash Centre Number already exists on the system");
                    PrepareDropDowns();
                    return View(cashCenterDto);
                }

                if (ModelState.IsValid)
                {
                    if (_cashCenterValidation.Add(cashCenterDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Cash Centre added successfully", MessageType.success, "Save");
                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Cash center not saved successfully", MessageType.error, "Error");
                PrepareDropDowns();
                return View(cashCenterDto);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Edit Cash Center By CashCenterId
        /// </summary>
        /// <param name="id">Represent  Cash Center By CashCenterId</param>
        /// <returns>CashCenter Results</returns>
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _cashCenterValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Cash Center");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Validate ,Post and Edit CashCenter
        /// </summary>
        /// <param name="cashCenterDto">Represent CashCenter</param>
        /// <returns>Save Cash Center </returns>
        [HttpPost]
        public ActionResult Edit(CashCentreDto cashCenterDto)
        {
            if (VerifyAuthentication())
            {
                //Ensure that the user has entered a Cash Center Name
                if (string.IsNullOrWhiteSpace(cashCenterDto.Name))
                {
                    ModelState.AddModelError("Cash Center Name", "Please enter a Cash Center Name");
                    return View(cashCenterDto);
                }

                if (string.IsNullOrWhiteSpace(cashCenterDto.Address.AddressLine1))
                {
                    ModelState.AddModelError("Address Line 1", "Please enter Address Line 1");
                    return View(cashCenterDto);
                }

                if (string.IsNullOrWhiteSpace(cashCenterDto.Address.AddressLine1))
                {
                    ModelState.AddModelError("Address Line 1", "Address Line 1 is required");
                    PrepareDropDowns();
                    return View(cashCenterDto);
                }

                ModelState.Remove("AddressLine1");
                ModelState.Remove("PostalCode");


                if (_cashCenterValidation.NameUsedByAnotherCashCenter(cashCenterDto.Name, cashCenterDto.CashCenterId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a name of another existing Cash Center!", MessageType.error, "Update Failed");
                }

                if (ModelState.IsValid)
                {

                    if (_cashCenterValidation.Edit(cashCenterDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Cash center updated successfully", MessageType.success, "Saved");
                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Cash center not saved", MessageType.error, "Error");
                PrepareDropDowns();

                return View(cashCenterDto);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// View CashCenter CashCenterId
        /// </summary>
        /// <param name="id">Represent CashCenterId</param>
        /// <returns>CashCenter</returns>
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _cashCenterValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Cash Center");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Delete CashCenter by cashcenterId
        /// </summary>
        /// <param name="id">is CashCenterId</param>
        /// <returns>Delete and Redirect to cashCenter Home Page</returns>
        public ActionResult Delete(int id)
        {
            if (VerifyAuthentication())
            {
                if (id > 0)
                {
                    var center = _cashCenterValidation.Find(id);
                    if (center != null)
                    {
                        var deleteCashCenter = _cashCenterValidation.Delete(id, User.Identity.Name);
                        if (deleteCashCenter.Status == MethodStatus.Successful)
                            ShowMessage("Cash Center deleted successfully", MessageType.success, "Deleted");
                        else
                        {
                            ShowMessage(deleteCashCenter.Message, MessageType.error, "Delete Cash Center");
                        }
                        return Json(new { url = Url.Action("Index") });
                    }
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// GetGeolocation
        /// </summary>
        /// <param name="address">Geo addres</param>
        /// <returns>Redirect to cash Center home Page</returns>
        public ActionResult GetCoordinates(string address)
        {
            if (VerifyAuthentication())
            {
                GeoGeometry geoResponse = GeoLocationHelper.GetCoordinates(address);

                return Json(geoResponse == null ? new GeoLocation { Lat = 0, Lng = 0 } : geoResponse.Location,
                    JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Dropdwons Region,City
        /// </summary>
        private void PrepareDropDowns()
        {
            ViewData.Add("Cities", new SelectList(_lookup.GetCities().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("AddressTypes", new SelectList(_lookup.GetAddressTypes().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("GetRegions", new SelectList(_lookup.GetRegions().ToDropDownModel(), "Id", "Name"));
        }

        /// <summary>
        /// City Dropdown
        /// </summary>
        /// <returns>Cities</returns>
        public JsonResult Cities()
        {
            var cities = _lookup.GetCities().ToDropDownModel();
            if (cities == null) return Json("Cities Not Found", JsonRequestBehavior.AllowGet);
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Getregions
        /// </summary>
        /// <returns>Regions</returns>
        public JsonResult GetRegions()
        {
            var regions = _lookup.GetRegions().ToDropDownModel();
            if (regions == null) return Json("Regions Not Found", JsonRequestBehavior.AllowGet);
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// /GetCitiesPerInProvince
        /// </summary>
        /// <param name="provinceId">Represent ProvinceId </param>
        /// <returns>Province</returns>
        public JsonResult GetCitiesPerInProvince(int provinceId)
        {
            var cities = _lookup.GetCitiesInProvince(provinceId).ToDropDownModel();
            if (cities == null) return Json("Province Cities Not Found", JsonRequestBehavior.AllowGet);
            return Json(cities, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ListView Actions
        /// <summary>
        /// Cashcenter
        /// </summary>
        /// <returns>CashCenterID,CashCenterName</returns>
        public ActionResult CashCentersColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Cash Centre Number", Tag = "Number"},
                new DropDownModel {Id = 2, Name = "Cash Centre Name", Tag = "Name"},
                new DropDownModel {Id = 3, Name = "Region Name", Tag = "RegionName"},
                new DropDownModel {Id = 4, Name = "Email Address", Tag = "EmailAddress1"},
                new DropDownModel {Id = 5, Name = "Telephone Number", Tag = "TelephoneNumber"},
                new DropDownModel {Id = 6, Name = "Contact Person", Tag = "ContactPerson"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// AutoComplete as user is Typing..........
        /// </summary>
        /// <param name="columName">Represent User text as Typing..........</param>
        /// <param name="searchData">Represnt User Result as Suggestion result</param>
        /// <returns>Display Cash Center Result</returns>
        public JsonResult AutoCompleteCashCentersByColumn(string columName, string searchData)
        {
            var cashCenters = _cashCenterValidation.All().ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "Number":
                    {
                        foreach (
                            ListCashCenterDto cashCenter in
                                cashCenters.Where(e => string.IsNullOrEmpty(e.Number) == false))
                        {
                            if (cashCenter.Number.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(cashCenter.Number);
                            }
                        }
                        break;
                    }
                case "Name":
                    {
                        foreach (
                            ListCashCenterDto cashCenter in
                                cashCenters.Where(e => string.IsNullOrEmpty(e.Name) == false))
                        {
                            if (cashCenter.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(cashCenter.Name);
                            }
                        }
                        break;
                    }
                case "RegionName":
                    {
                        foreach (
                            ListCashCenterDto cashCenter in
                                cashCenters.Where(e => string.IsNullOrEmpty(e.RegionName) == false))
                        {
                            if (cashCenter.RegionName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(cashCenter.RegionName);
                            }
                        }
                        break;
                    }
                case "EmailAddress1":
                    {
                        foreach (
                            ListCashCenterDto cashCenter in
                                cashCenters.Where(e => string.IsNullOrEmpty(e.EmailAddress1) == false))
                        {
                            if (cashCenter.EmailAddress1.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(cashCenter.EmailAddress1);
                            }
                        }
                        break;
                    }
                case "TelephoneNumber":
                    {
                        foreach (
                            ListCashCenterDto cashCenter in
                                cashCenters.Where(e => string.IsNullOrEmpty(e.TelephoneNumber) == false))
                        {
                            if (cashCenter.TelephoneNumber.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(cashCenter.TelephoneNumber);
                            }
                        }
                        break;
                    }
                case "ContactPerson":
                    {
                        foreach (
                            ListCashCenterDto cashCenter in
                                cashCenters.Where(e => string.IsNullOrEmpty(e.ContactPerson) == false))
                        {
                            if (cashCenter.ContactPerson.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(cashCenter.ContactPerson);
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