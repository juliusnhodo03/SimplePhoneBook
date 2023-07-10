using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.CashDeposit;
using Application.Modules.Common;
using Application.Modules.Maintanance.ContainerType;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Container Type related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class ContainerTypeController : BaseController
    {

        private readonly IContainerTypeValidation _containerTypeValidation;
        private readonly ILookup _staticTypes;

        public ContainerTypeController()
        {
            _staticTypes = LocalUnityResolver.Retrieve<ILookup>();
            _containerTypeValidation = LocalUnityResolver.Retrieve<IContainerTypeValidation>();
        }



        #region Controller Actions

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                var containerTypes = _containerTypeValidation.All();
                return View(containerTypes);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="containerTypeDto">Database entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(ContainerTypeDto containerTypeDto)
        {
            if (VerifyAuthentication())
            {
                //Reomve Serial Number and Seal Number Validation
                ModelState.Remove("SerialNumber");
                ModelState.Remove("SealNumber");

                if (string.IsNullOrWhiteSpace(containerTypeDto.Name))
                {
                    ModelState.AddModelError("Container Type Name", "Please enter a Container Type Name");
                    return View(containerTypeDto);
                }

                if (_containerTypeValidation.ContainerTypeNameExists(containerTypeDto.Name))
                {
                    ModelState.AddModelError("Container Type Name", "Container Type Name already exists on the system");
                    return View(containerTypeDto);
                }

                if (ModelState.IsValid)
                {
                    if (_containerTypeValidation.Add(containerTypeDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("container Type added successfully", MessageType.success, "Save");

                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Container Type not saved successfully", MessageType.error, "Error");
                return View(containerTypeDto);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _containerTypeValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Container Type");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="containerTypeDto">Database Entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(ContainerTypeDto containerTypeDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(containerTypeDto.Name))
                {
                    ModelState.AddModelError("Container Type Name", "Please enter a Container Type Name");
                    return View(containerTypeDto);
                }

                if (_containerTypeValidation.IsContainerTypeNameInUse(containerTypeDto.Name, containerTypeDto.ContainerTypeId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a name of another existing Container Type!", MessageType.error,
                        "Update Failed");
                }

                if (ModelState.IsValid)
                {
                    if (_containerTypeValidation.Edit(containerTypeDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Container Type Details updated successfully", MessageType.success, "Saved");

                        return RedirectToAction("Index");
                    }
                }
                return View(containerTypeDto);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _containerTypeValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Container Type");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                if (_containerTypeValidation.Delete(id, User.Identity.Name).Status == MethodStatus.Successful)
                    ShowMessage("Container Type deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage("Container Type not deleted.", MessageType.error, "Delete Container Type");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Listview Action

        /// <summary>
        /// Container type Column Listing
        /// </summary>
        /// <returns></returns>
        public ActionResult ContainerTypeColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Container Type Name", Tag = "Name"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Auto Complte container Type Column
        /// </summary>
        /// <param name="columName"> Column Name</param>
        /// <param name="searchData"> Search String</param>
        /// <returns></returns>
        public JsonResult AutoCompleteContainerTypeByColumn(string columName, string searchData)
        {
            var containerTypes = _containerTypeValidation.All().ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                    {
                        foreach (
                            var containerType in containerTypes.Where(e => string.IsNullOrEmpty(e.Name) == false))
                        {
                            if (containerType.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(containerType.Name);
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