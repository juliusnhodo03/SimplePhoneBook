using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Application.Dto.Bank;
using Application.Modules.Maintanance.Bank;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Bank related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin")]
    public class BankController : BaseController
    {

        private readonly IBankValidation _bankValidation;

        public BankController()
        {
            _bankValidation = LocalUnityResolver.Retrieve<IBankValidation>();
        }


        #region Controller Actions
        /// <summary>
        /// Bank Home Page
        /// </summary>
        /// <returns>All Banks</returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                var banks = _bankValidation.All();
                return View(banks);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Create bank
        /// </summary>
        /// <returns>Bank</returns>
        public ActionResult Create()
        {
            if (VerifyAuthentication())
            {
                return View();
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Create a bank and Validate it
        /// </summary>
        /// <param name="bankDto">Represent Bank to be created </param>
        /// <returns>To bank HomePage</returns>
        [HttpPost]
        public ActionResult Create(BankDto bankDto)
        {
            if (VerifyAuthentication())
            {
                //Ensure that the user has entered a Bank Name
                if (string.IsNullOrWhiteSpace(bankDto.Name))
                {
                    ModelState.AddModelError("Bank Name", "Please enter a Bank Name");
                    return View(bankDto);
                }

                //Check that the Bank Name already Exists before Inserting
                if (_bankValidation.IsBankNameInUse(bankDto.Name))
                {
                    ModelState.AddModelError("Bank Name", "Bank Name already exists on the system");
                }

                //Check that the Branch Code already Exists before Inserting
                if (_bankValidation.IsBranchCodeInUse(bankDto.BranchCode))
                {
                    ModelState.AddModelError("Bank Code", "Branch Code already exists on the system");
                }

                //Insert the record and redirect to the Index Page once Successful
                if (ModelState.IsValid)
                {
                    var bank = _bankValidation.Add(bankDto, User.Identity.Name);
                    if (bank.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Bank added successfully", MessageType.success, "Save");
                        return RedirectToAction("Index");
                    }
                }
                ShowMessage("Bank not saved successfully", MessageType.error, "Error");
                return View(bankDto);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        ///Get Bank  by Id and edit 
        /// </summary>
        /// <param name="id">Represent BankId</param>
        /// <returns>Bank</returns>
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _bankValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Bank");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Validate and Post bank after being edited
        /// </summary>
        /// <param name="bankDto">Represent bank to Edited</param>
        /// <returns>Save Bank and Redirect to Bank Home Page</returns>
        [HttpPost]
        public ActionResult Edit(BankDto bankDto)
        {
            if (VerifyAuthentication())
            {
                if (string.IsNullOrWhiteSpace(bankDto.Name))
                {
                    ModelState.AddModelError("Bank Name", "Please enter a Bank Name");
                    return View(bankDto);
                }

                //Check that the Bank Name already Exists before Inserting
                if (_bankValidation.NameUsedByAnotherBank(bankDto.Name, bankDto.BankId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a name of another existing bank!", MessageType.error, "Update Failed");
                }

                //Check that the Branch Code already Exists before Inserting
                if (_bankValidation.CodeUsedByAnotherBank(bankDto.BranchCode, bankDto.BankId))
                {
                    ModelState.AddModelError("", "");
                    ShowMessage("You cannot give a code of another existing bank!", MessageType.error, "Update Failed");
                }

                if (ModelState.IsValid)
                {
                    //Edit the Record and redirect to the Index Page once Successful
                    var bank = _bankValidation.Edit(bankDto, User.Identity.Name);
                    if (bank.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Bank Details updated successfully", MessageType.success, "Saved");

                        return RedirectToAction("Index");
                    }
                }
                return View(bankDto);
            }
            return RedirectToAction("Login", "Account");
        }
        
        /// <summary>
        /// View bank and Its Details
        /// </summary>
        /// <param name="id">Represent  Bank by bank Id</param>
        /// <returns>Return Bank Details</returns>
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _bankValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Bank");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Delete A bank by Bank Id
        /// </summary>
        /// <param name="id">Represent bankId </param>
        /// <returns>Delete bank and  Redirect to Bank Home Page</returns>
        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                var deleteBank = _bankValidation.Delete(id, User.Identity.Name);
                if (deleteBank.Status == MethodStatus.Successful)
                    ShowMessage("Bank deleted successfully", MessageType.success, "Deleted");
                else
                {
                    ShowMessage(deleteBank.Message, MessageType.error, "Delete Bank");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region ListView Actions
        /// <summary>
        /// Bank dropdwon
        /// </summary>
        /// <returns>bankid,Name</returns>
        public ActionResult BankColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Bank Name", Tag = "Name"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// AutoComplete as User is Typing.....
        /// </summary>
        /// <param name="columName">User text as Typing....</param>
        /// <param name="searchData">User Suggestion as Typing....</param>
        /// <returns>Return User Result</returns>
        public JsonResult AutoCompleteBankByColumn(string columName, string searchData)
        {
            var banks = _bankValidation.All().ToList();
            var items = new ArrayList();

            switch (columName)
            {
                case "Name":
                    {
                        foreach (BankDto bank in banks.Where(e => string.IsNullOrEmpty(e.Name) == false))
                        {
                            if (bank.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(bank.Name);
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