using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.DeviceType;
using Application.Modules.Common;
using Application.Modules.Maintanance.DeviceType;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Device related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class DeviceTypeController : BaseController
    {
        private readonly IDeviceTypeValidation _deviceValidation;
        private readonly ILookup _lookup;

        public DeviceTypeController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _deviceValidation = LocalUnityResolver.Retrieve<IDeviceTypeValidation>();
        }



        #region Controller Actions

        /// <summary>
        ///     Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<DeviceTypeDto> devices = _deviceValidation.All();
                return View(devices);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Create Device
        /// </summary>
        /// <returns></returns>
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
        ///     Create
        /// </summary>
        /// <param name="deviceTypeDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(DeviceTypeDto deviceTypeDto)
        {
            if (VerifyAuthentication())
            {
                //Check that the Device Name already Exists before Inserting
                if (_deviceValidation.IsDeviceNameInUse(deviceTypeDto.Name))
                {
                    ModelState.AddModelError("Device Name", "Device Name already exists on the system");
                }

                //Insert the record and redirect to the Index Page once Successful
                if (ModelState.IsValid)
                {
                    MethodResult<bool> deviceTypess = _deviceValidation.Add(deviceTypeDto, User.Identity.Name);
                    if (deviceTypess.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Device added successfully", MessageType.success, "Save");
                        return RedirectToAction("Index");
                    }
                }
                PrepareDropDowns();
                ShowMessage("Device not saved successfully", MessageType.error, "Error");
                return View(deviceTypeDto);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Edit Device
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                MethodResult<DeviceTypeDto> result = _deviceValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Device");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Edit
        /// </summary>
        /// <param name="deviceTypeDto"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(DeviceTypeDto deviceTypeDto)
        {
            if (VerifyAuthentication())
            {
                if (ModelState.IsValid)
                {
                    //Edit the Record and redirect to the Index Page once Successful
                    MethodResult<bool> device = _deviceValidation.Edit(deviceTypeDto, User.Identity.Name);
                    if (device.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Device Details updated successfully", MessageType.success, "Saved");

                        return RedirectToAction("Index");
                    }
                }
                PrepareDropDowns();
                ShowMessage("Device not updated successfully", MessageType.error, "Error");
                return View(deviceTypeDto);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     View
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                MethodResult<DeviceTypeDto> result = _deviceValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Device");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        ///     Delete Device
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                MethodResult<bool> deleteDeviceType = _deviceValidation.Delete(id, User.Identity.Name);
                if (deleteDeviceType.Status == MethodStatus.Successful)
                    ShowMessage("Device deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage(deleteDeviceType.Message, MessageType.error, "Delete Device");
                }
                return Json(new {url = Url.Action("Index")});
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Prepare DropDown
        /// </summary>
        private void PrepareDropDowns()
        {
            if (!ViewData.ContainsKey("GetSites"))
                ViewData.Add("GetSites", new SelectList(_lookup.Sites().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("GetManufactures"))
                ViewData.Add("GetManufactures", new SelectList(_lookup.Manufactures().ToDropDownModel(), "Id", "Name"));
            if (!ViewData.ContainsKey("GetSuppliers"))
                ViewData.Add("GetSuppliers", new SelectList(_lookup.Suppliers().ToDropDownModel(), "Id", "Name"));
        }

        #endregion

        /// <summary>
        ///     Device colunm listing
        /// </summary>
        /// Device Column Listing
        /// <returns></returns>

        #region ListView Actions
        public ActionResult DeviceColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Device Name", Tag = "Name"},
                new DropDownModel {Id = 2, Name = "Serial Number", Tag = "SerialNumber"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Auto Complete Device by Column
        /// </summary>
        /// <param name="columName"> Column Name</param>
        /// <param name="searchData">Search string</param>
        /// <returns></returns>
        public JsonResult AutoCompleteDeviceByColumn(string columName, string searchData)
        {
            List<DeviceTypeDto> devices = _deviceValidation.All().ToList();
            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                {
                    foreach (DeviceTypeDto device in devices.Where(e => string.IsNullOrEmpty(e.Name) == false))
                    {
                        if (device.Name.ToLower().StartsWith(searchData.ToLower()))
                        {
                            items.Add(device.Name);
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