using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.SalesArea;
using Application.Modules.Common;
using Application.Modules.Maintanance.SalesArea;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class SalesAreaController : BaseController
    {
        private readonly ISalesAreaValidation _salesAreaValidation;
        private readonly ILookup _staticTypes;

        public SalesAreaController()
        {
            _salesAreaValidation = LocalUnityResolver.Retrieve<ISalesAreaValidation>();
            _staticTypes = LocalUnityResolver.Retrieve<ILookup>();
        }


        #region Controller Actions

        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                var salesareas = _salesAreaValidation.All();
                return View(salesareas);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Create(SalesAreaDto salesAreaDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(salesAreaDto.Name))
                {
                    ModelState.AddModelError("Area Name", "Please enter a Container Type Name");
                    return View(salesAreaDto);
                }

                if (_salesAreaValidation.IsAreaNameInUse(salesAreaDto.Name))
                {
                    ModelState.AddModelError("Area Name", "Area Name already exists on the system");
                }

                if (ModelState.IsValid)
                {
                    if (_salesAreaValidation.Add(salesAreaDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Sales Area added successfully", MessageType.success, "Save");

                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Sales Area not saved successfully", MessageType.error, "Error");
                return View(salesAreaDto);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _salesAreaValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Sales Area");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(SalesAreaDto salesAreaDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(salesAreaDto.Name))
                {
                    ModelState.AddModelError("Area Name", "Please enter a Container Type Name");
                    return View(salesAreaDto);
                }

                if (_salesAreaValidation.NameUsedByAnotherArea(salesAreaDto.Name, salesAreaDto.SalesAreaId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a name of another existing Sales Area!", MessageType.error, "Update Failed");
                }

                if (ModelState.IsValid)
                {
                    if (_salesAreaValidation.Edit(salesAreaDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Sales Area Details updated successfully", MessageType.success, "Saved");

                        return RedirectToAction("Index");
                    }
                }
                return View(salesAreaDto);
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _salesAreaValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Sales Area");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                if (_salesAreaValidation.Delete(id, User.Identity.Name).Status == MethodStatus.Successful)
                    ShowMessage("Sales Area deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage("Sales Area not deleted.", MessageType.error, "Delete Sales Area");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        public ActionResult SalesAreaColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Area Name", Tag = "Name"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AutoCompleteSalesAreaByColumn(string columName, string searchData)
        {
            var salesAreas = _salesAreaValidation.All().ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                    {
                        foreach (SalesAreaDto salesArea in salesAreas.Where(e => string.IsNullOrEmpty(e.Name) == false))
                        {
                            if (salesArea.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(salesArea.Name);
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