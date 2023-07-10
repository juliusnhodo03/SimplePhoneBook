using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using Application.Dto.Users;
using Application.Modules.Common;
using Application.Modules.Maintanance.Users;
using Application.Modules.Maintanance.Users.CashCenter;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Repository;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// /// <summary>
    /// Handles Cash Center User related functionality
    /// </summary>
    /// </summary>
    ///
    
    [Authorize]
    public class CashCenterUserController : BaseController
    {
        private readonly ICashCenterUserValidation _cashCenterUserValidation;
        private readonly ILookup _lookup;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _userAccountValidation;

        public CashCenterUserController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _cashCenterUserValidation = LocalUnityResolver.Retrieve<ICashCenterUserValidation>();
            _repository = LocalUnityResolver.Retrieve<IRepository>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
        }



        #region CashCenter
        /// <summary>
        /// CashcenterUser Home screen
        /// </summary>
        /// <returns>cashCenterUsers </returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVTellerSupervisor")]
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<CashCenterUserDto> cashCenterUsers = _cashCenterUserValidation.All().OrderByDescending(e => e.UserDto.CreateDate);
                return View(cashCenterUsers);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Add cashCenterUsers 
        /// </summary>
        /// <returns>cashCenterUsers </returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVTellerSupervisor")]
        public ActionResult Add()
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();
                CashCenterUserDto cashCenterUserDto = new CashCenterUserDto();
                UserDto userDto = new UserDto();

                cashCenterUserDto.UserDto = userDto;
                return View(cashCenterUserDto);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary> 
        /// Validate Post and Save cashCenter Users 
        /// </summary>
        /// <param name="cashCenterUser">represent cashCenterUsersId</param>
        /// <returns>cashCenterUsers</returns>
        [HttpPost]
        public ActionResult Add(CashCenterUserDto cashCenterUser)
        {
            if (VerifyAuthentication())
            {
                if (!Regex.Match(cashCenterUser.UserDto.UserName, @"^[a-zA-Z0-9._-]*$").Success)
                    ModelState.AddModelError("UserName", "Invalid Username");

                if (_cashCenterUserValidation.UserNameExist(cashCenterUser.UserDto.UserName))
                    ModelState.AddModelError("Username", "The user already exist");

                if (!cashCenterUser.UserDto.IsRecon && !cashCenterUser.UserDto.IsTeller &&
                    !cashCenterUser.UserDto.IsTellerSupervisor)
                    ModelState.AddModelError("Roles", "User must have at least one role");

                if (string.IsNullOrEmpty(cashCenterUser.UserDto.IdNumber) &&
                    string.IsNullOrEmpty(cashCenterUser.UserDto.PassportNumber))
                    ModelState.AddModelError("Identity", "User must supply at least an Id number OR PassportNumber");

                if (!string.IsNullOrEmpty(cashCenterUser.UserDto.IdNumber) &&
                    _cashCenterUserValidation.UserIdNumberExist(cashCenterUser.UserDto.IdNumber, cashCenterUser.UserDto.UserName))
                    ModelState.AddModelError("ID Number", "The id number belongs to another user on the system.");

                if (!string.IsNullOrEmpty(cashCenterUser.UserDto.PassportNumber) &&
                    _cashCenterUserValidation.UserIdNumberExist(cashCenterUser.UserDto.PassportNumber, cashCenterUser.UserDto.UserName))
                    ModelState.AddModelError("Passport Number", "The passport number belongs to another user on the system.");

                if (ModelState.IsValid)
                {
                    var result = _cashCenterUserValidation.Add(cashCenterUser, User.Identity.Name);

                    if (result.Status == MethodStatus.Successful)
                    {
                        ShowMessage("User was successfully created.", MessageType.success, "Create User");
                        GenerateAndSendEmail(cashCenterUser.UserDto.FirstName + " " + cashCenterUser.UserDto.LastName,
                            cashCenterUser.UserDto.Password,
                            cashCenterUser.UserDto.UserName, cashCenterUser.UserDto.EmailAddress);
                        return RedirectToAction("Index");
                    }
                    ShowMessage("User Not Created", MessageType.error, "Create User");
                }

                PrepareDropDowns();
                return View(cashCenterUser);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Edit cashCenterUsers by cashCenterUsersId
        /// </summary>
        /// <param name="id">Represent cashCenterUsersId</param>
        /// <returns>cashCenterUsers</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVTellerSupervisor")]
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _cashCenterUserValidation.Find(id).EntityResult;

                PrepareDropDowns();
                return View(result);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Validate,Post,Save cashCenterUsers  
        /// </summary>
        /// <param name="cashCenterUser"> Represent cashCenterUsers </param>
        /// <returns>cashCenterUsers </returns>
        [HttpPost]
        public ActionResult Edit(CashCenterUserDto cashCenterUser)
        {
            if (VerifyAuthentication())
            {
                ModelState.Remove("UserDto.UserName");
                ModelState.Remove("UserDto.Password");
                ModelState.Remove("UserDto.ConfirmPassword");

                if (!cashCenterUser.UserDto.IsRecon && !cashCenterUser.UserDto.IsTeller &&
                    !cashCenterUser.UserDto.IsTellerSupervisor)
                    ModelState.AddModelError("Roles", "User must have at least one role");

                if (string.IsNullOrEmpty(cashCenterUser.UserDto.IdNumber) &&
                    string.IsNullOrEmpty(cashCenterUser.UserDto.PassportNumber))
                    ModelState.AddModelError("Identity", "User must supply at least an Id number OR PassportNumber");

                if (!string.IsNullOrEmpty(cashCenterUser.UserDto.IdNumber) &&
                    _cashCenterUserValidation.UserNameExist(cashCenterUser.UserDto.IdNumber))
                    ModelState.AddModelError("ID Number", "The id number belongs to another user on the system.");

                if (!string.IsNullOrEmpty(cashCenterUser.UserDto.PassportNumber) &&
                    _cashCenterUserValidation.UserIdNumberExist(cashCenterUser.UserDto.PassportNumber, cashCenterUser.UserDto.UserName, Function.Update))
                    ModelState.AddModelError("Passport Number", "The passport number belongs to another user on the system.");

                if (ModelState.IsValid)
                {
                    if (_cashCenterUserValidation.Edit(cashCenterUser, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Edit Successful.", MessageType.success, "Edit User");
                        GenerateAndSendEmail(cashCenterUser.UserDto.FirstName + " " + cashCenterUser.UserDto.LastName,
                            cashCenterUser.UserDto.EmailAddress);
                        return RedirectToAction("Index");
                    }
                    ShowMessage("User Not Updated.", MessageType.error, "Edit User");
                }
                PrepareDropDowns();
                return View(cashCenterUser);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// View cashCenterUsers by cashCenterUsersId 
        /// </summary>
        /// <param name="id">Represent cashCenterUsersId</param>
        /// <returns>cashCenterUsers </returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVTellerSupervisor")]
       
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _cashCenterUserValidation.Find(id).EntityResult;

                PrepareDropDowns();
                return View(result);
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Delete cashCenterUsers by cashCenterUsersId
        /// </summary>
        /// <param name="id">Represent cashCenterUsersId</param>
        /// <returns>delete cashCenterUsers Redirect to cashCenterUsers Home Screen</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVTellerSupervisor")]
        public ActionResult Delete(int id)
        {
            if (VerifyAuthentication())
            {
                if (_cashCenterUserValidation.Delete(id, User.Identity.Name).Status == MethodStatus.Successful)
                {
                    ShowMessage("Delete Successful.", MessageType.success, "Delete User");
                }
                else
                {
                    ShowMessage("User Not Deleted.", MessageType.error, "Delete User");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// Get cashCenterUsers UserName
        /// </summary>
        /// <param name="firstname">Represent cashCenterUsersFirstname</param>
        /// <param name="lastname">Reprent cashCenterUsersLastName </param>
        /// <returns></returns>
        public ActionResult GetUsername(string firstname, string lastname)
        {
            string result = UserHelper.GenerateUserName(firstname, lastname.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// ReserPassword
        /// </summary>
        /// <param name="id">Represent CashCenterUserID</param>
        /// <param name="username">RepresentCashCenterUserName </param>
        /// <param name="actionName">Represent New CashCenterUserPassword</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult ResetPassword(int id, string username, string actionName)
        {
            UserHelper.ResetPasswordResult resetModel = UserHelper.ResetPassword(_repository, _userAccountValidation,
                username);
            if (resetModel.Result)
            {
                ShowMessage(
                    "The password has been reset. The user will receive an email with the new password information.",
                    MessageType.success, "Reset Password");
                GenerateAndSendEmail(resetModel.FullNames, resetModel.Password, resetModel.EmailAddress);
            }
            else
            {
                ShowMessage("A reset password error has occurred. Password reset has failed.", MessageType.error,
                    "Reset Password");
            }

            return RedirectToAction(actionName, new { id });
        }
        /// <summary>
        /// CashCenterUser
        /// </summary>
        /// <returns>CashCenterUserId,CashCenterUserName</returns>
        public ActionResult CashCenterUsersColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "First name", Tag = "FirstName"},
                new DropDownModel {Id = 2, Name = "Last name", Tag = "LastName"},
                new DropDownModel {Id = 3, Name = "Email Address", Tag = "EmailAddress"},
                new DropDownModel {Id = 4, Name = "Office Number", Tag = "OfficeNumber"},
                new DropDownModel {Id = 5, Name = "Cell Number", Tag = "CellNumber"},
                new DropDownModel {Id = 6, Name = "Cash Center Name", Tag = "CashCenterName"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// CashCenterUser AutoComplete
        /// </summary>
        /// <param name="columName">Represent CashCenterUser text as Typing....</param>
        /// <param name="searchData">Represent CashCenterUser Text as Typing...</param>
        /// <param name="userType">Represent CashCenterUserType</param>
        /// <returns>CashCenterUserResult</returns>
        public JsonResult AutoCompleteUsersByColumn(string columName, string searchData, int userType)
        {
            List<User> users = _lookup.GetAllUsers(userType).ToList();

            var items = new ArrayList();

            switch (columName)
            {
                case "FirstName":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.FirstName) == false))
                        {
                            if (user.FirstName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.FirstName);
                            }
                        }
                        break;
                    }
                case "LastName":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.LastName) == false))
                        {
                            if (user.LastName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.LastName);
                            }
                        }
                        break;
                    }
                case "EmailAddress":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.EmailAddress) == false))
                        {
                            if (user.EmailAddress.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.EmailAddress);
                            }
                        }
                        break;
                    }
                case "OfficeNumber":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.OfficeNumber) == false))
                        {
                            if (user.OfficeNumber.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.OfficeNumber);
                            }
                        }
                        break;
                    }
                case "CellNumber":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.CellNumber) == false))
                        {
                            if (user.CellNumber.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.CellNumber);
                            }
                        }
                        break;
                    }
                case "CashCenterName":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.CashCenterName) == false))
                        {
                            if (user.CashCenterName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.CashCenterName);
                            }
                        }
                        break;
                    }
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Helper
        /// <summary>
        /// Tittle Dropdown
        /// 
        /// </summary>
        private void PrepareDropDowns()
        {
            ViewData.Add("Titles", new SelectList(_lookup.Titles().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("CashCenters", new SelectList(_lookup.CashCenters().ToDropDownModel(), "Id", "Name"));
        }
        /// <summary>
        /// Send Email to CashCenterUser with ResetPasswordInformation
        /// </summary>
        /// <param name="fullnames">represent CashCenterUserFullnames</param>
        /// <param name="password">Represent CashCenterUser password reset information</param>
        /// <param name="username">Represent CashCenterUser username  information</param>
        /// <param name="emailAddress">Represent CashCenterUser emailAddress information</param>
        private void GenerateAndSendEmail(string fullnames, string password, string username, string emailAddress)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv User Account Details");
            html.RenderEndTag();
            html.WriteEncodedText(String.Format("Dear {0}", fullnames));
            html.WriteBreak();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("Your account has been created and your login details are below.");
            html.WriteBreak();
            html.WriteEncodedText(string.Format("Username : {0}\npassword : {1}", username, password));
            html.WriteBreak();
            html.WriteEncodedText("To change this password, login to the system and navigate to User Profile");
            html.WriteBreak();
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();

            SendEmailNotification(emailAddress, new MailMessage(), "New User Account Information", htmlString);
        }
        /// <summary>
        /// Send Email to CashCenterUser with ResetPasswordInformation
        /// </summary>
        /// <param name="fullnames">represent CashCenterUserFullnames</param>
        /// <param name="password">Represent CashCenterUser password reset information</param>
        /// <param name="username">Represent CashCenterUser username  information</param>
        /// <param name="emailAddress">Represent CashCenterUser emailAddress information</param>
        private void GenerateAndSendEmail(string fullnames, string password, string emailAddress)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv User Password Reset");
            html.RenderEndTag();
            html.WriteEncodedText(String.Format("Dear {0}", fullnames));
            html.WriteBreak();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("Your account password has been reset by SBV.");
            html.WriteBreak();
            html.WriteEncodedText(string.Format("The new password is {0}", password));
            html.WriteBreak();
            html.WriteEncodedText("To change this password, login to the system and navigate to User Profile");
            html.WriteBreak();
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();

            SendEmailNotification(emailAddress, new MailMessage(), "Reset Account Password", htmlString);
        }
        /// <summary>
        /// Send Email to CashCenterUser with ResetPasswordInformation
        /// </summary>
        /// <param name="fullnames">represent CashCenterUserFullnames</param>
       
        /// <param name="username">Represent CashCenterUser username  information</param>
        /// <param name="emailAddress">Represent CashCenterUser emailAddress information</param>
        public void GenerateAndSendEmail(string fullname, string emailaddress)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv User Account Details Update");
            html.RenderEndTag();
            html.WriteEncodedText(String.Format("Dear {0}", fullname));
            html.WriteBreak();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("Your account has been updated by SBV ");
            html.WriteEncodedText("To view the update, login to the system and navigate to User Profile");
            html.WriteBreak();
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();

            SendEmailNotification(emailaddress, new MailMessage(), "User Account Information Update", htmlString);
        }

        #endregion
    }
}