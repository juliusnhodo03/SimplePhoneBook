using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.Carrier;
using Application.Modules.Maintanance.Carrier;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// /// <summary>
    /// Handles Carrier related functionality
    /// </summary>
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class CarrierController : BaseController
    {
        private readonly ICarrierValidation _iCarrierValidation;

        public CarrierController()
        {
            _iCarrierValidation = LocalUnityResolver.Retrieve<ICarrierValidation>();
        }


        #region Controller Actions
        /// <summary>
        /// Carrier Home page
        /// </summary>
        /// <returns>Display CIT Carrier</returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<CitCarrierDto> carriers = _iCarrierValidation.All();
                return View(carriers);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Create CIT Carrier
        /// </summary>
        /// <returns>Login account</returns>
        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Create CITCarrier
        /// </summary>
        /// <param name="carrierDto">Represent CITCarrier</param>
        /// <returns>Redirect to CIT Carrier Home Page</returns>
        [HttpPost]
        public ActionResult Create(CitCarrierDto carrierDto)
        {
            if (VerifyAuthentication())
            {
                //Ensure that the user has entered a Carrier Name
                if (string.IsNullOrWhiteSpace(carrierDto.Name))
                {
                    ModelState.AddModelError("Carrier Name", "Please enter a Carrier Name");
                    return View(carrierDto);
                }

                //Check that the Carrier Name already Exists before Inserting
                if (_iCarrierValidation.IsCarrierNameInUse(carrierDto.Name))
                {
                    ModelState.AddModelError("CarrierName", "Carrier Name already exists on the system");
                }

                //Insert the record and redirect to the Index Page once Successful
                if (ModelState.IsValid)
                {
                    var carrier = _iCarrierValidation.Add(carrierDto, User.Identity.Name);
                    if (carrier.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Carrier added successfully", BaseController.MessageType.success, "Saved");
                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Carrier not saved successfully", MessageType.error, "Error");
                return View(carrierDto);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Get carrier by ID and edit
        /// </summary>
        /// <param name="id">represent Carrier Id</param>
        /// <returns>Redirect to Carrier Home Page</returns>
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _iCarrierValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Cit Carrier");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Validate  Post and Save Edited Carrier
        /// </summary>
        /// <param name="carrierDto">Represent Carrier Instance</param>
        /// <returns>Save Carrier</returns>
        [HttpPost]
        public ActionResult Edit(CitCarrierDto carrierDto)
        {
            if (VerifyAuthentication())
            {
                //Ensure that the user has entered a Carrier Name
                if (string.IsNullOrWhiteSpace(carrierDto.Name))
                {
                    ModelState.AddModelError("Carrier Name", "Please enter a Carrier Name");
                    return View(carrierDto);
                }

                //Check that the Carrier Name already Exists before Inserting
                if (_iCarrierValidation.NameUsedByAnotherCarrier(carrierDto.Name, carrierDto.CitCarrierId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a name of another existing Carrier!", MessageType.error, "Update Failed");
                }


                if (ModelState.IsValid)
                {
                    //Edit the Record and redirect to the Index Page once Successful
                    if (_iCarrierValidation.Edit(carrierDto, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("CIT Carrier Details updated successfully", MessageType.success, "Saved");
                        return RedirectToAction("Index");
                    }
                }
                return View(carrierDto);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// View Carrier By ID
        /// </summary>
        /// <param name="id"> Represent Carrier ID</param>
        /// <returns>Redirect to Carrier Home page</returns>
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _iCarrierValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Cit Carrier");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Delete Carrier by CarrierId
        /// </summary>
        /// <param name="id">Represnt CarrierID</param>
        /// <returns>Redirect to Carrier Home Page</returns>
        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                var results = _iCarrierValidation.Delete(id, User.Identity.Name);
                if (results.Status == MethodStatus.Successful)
                    ShowMessage("Carrier deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage(results.Message, MessageType.error, "Delete Carrier");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region ListView Actions
        /// <summary>
        /// Carrier Dropdown
        /// </summary>
        /// <returns>Id,Name</returns>
        public ActionResult CarrierColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Carrier Name", Tag = "Name"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// AutoComplete as user is typing.....
        /// </summary>
        /// <param name="columName">Represend User text as Typing....</param>
        /// <param name="searchData">Represent User Suggestion as Typing....</param>
        /// <returns>Return user Result</returns>
        public JsonResult AutoCompleteCitCarriersByColumn(string columName, string searchData)
        {
            List<CitCarrierDto> carriers = _iCarrierValidation.All().ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                    {
                        foreach (CitCarrierDto carrier in carriers.Where(e => string.IsNullOrEmpty(e.Name) == false))
                        {
                            if (carrier.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(carrier.Name);
                            }
                        }
                        break;
                    }
                case "Description":
                    {
                        foreach (
                            CitCarrierDto carrier in carriers.Where(e => string.IsNullOrEmpty(e.Description) == false))
                        {
                            if (carrier.Description.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(carrier.Description);
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