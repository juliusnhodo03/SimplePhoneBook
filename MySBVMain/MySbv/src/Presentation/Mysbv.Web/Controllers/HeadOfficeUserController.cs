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
using Application.Modules.Maintanance.Users.HeadOffice;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Repository;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Head Office User related functionality
    /// </summary>
    [Authorize]
    public class HeadOfficeUserController : BaseController
    {
        private readonly IHeadOfficeUserValidation _headOfficeUser;
        private readonly ILookup _lookup;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _userAccountValidation;

        public HeadOfficeUserController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _headOfficeUser = LocalUnityResolver.Retrieve<IHeadOfficeUserValidation>();
            _repository = LocalUnityResolver.Retrieve<IRepository>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
        }


        #region Head Office User

        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<UserDto> headOfficeUser = _headOfficeUser.All().OrderByDescending(e => e.CreateDate);
                return View(headOfficeUser);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Add()
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();
                return View(new UserDto());
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Add
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Add(UserDto model)
        {
            if (VerifyAuthentication())
            {
                if (!Regex.Match(model.UserName, @"^[a-zA-Z0-9._-]*$").Success)
                    ModelState.AddModelError("UserName", "Invalid Username");

                if (!model.IsAdmin && !model.IsApprover && !model.IsDataCapture && !model.IsFinanceReviewer)
                    ModelState.AddModelError("Roles", "User must have at least one role");

                if (_headOfficeUser.UserExist(model.UserName))
                    ModelState.AddModelError("Username", "The user already exist");

                if (string.IsNullOrEmpty(model.IdNumber) &&
                    string.IsNullOrEmpty(model.PassportNumber))
                    ModelState.AddModelError("Identity", "User must supply at least an Id number OR PassportNumber");

                if (!string.IsNullOrEmpty(model.IdNumber) &&
                    _headOfficeUser.UserIdExist(model.IdNumber, model.UserName))
                    ModelState.AddModelError("ID Number", "The id number belongs to another user on the system.");

                if (!string.IsNullOrEmpty(model.PassportNumber) &&
                    _headOfficeUser.UserIdExist(model.PassportNumber, model.UserName))
                    ModelState.AddModelError("Passport Number", "The passport number belongs to another user on the system.");

                if (ModelState.IsValid)
                {
                    MethodResult<UserDto> result = _headOfficeUser.Add(model, User.Identity.Name);

                    if (result != null)
                    {
                        ShowMessage("User was successfully created.", MessageType.success, "Create User");
                        GenerateAndSendEmail(model.FirstName + " " + model.LastName, model.Password, model.UserName,
                            model.EmailAddress);
                        return RedirectToAction("Index");
                    }
                    ShowMessage("User Not Created", MessageType.error, "Create User");
                }

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                UserDto model = _headOfficeUser.Find(id).EntityResult;

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Edit
        /// </summary>
        /// <param name="model">model</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(UserDto model)
        {
            if (VerifyAuthentication())
            {
                ModelState.Remove("UserName");
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
                ModelState.Remove("UserSites");

                if (string.IsNullOrEmpty(model.IdNumber) &&
                    string.IsNullOrEmpty(model.PassportNumber))
                    ModelState.AddModelError("Identity", "User must supply at least an Id number OR PassportNumber");

                if (!string.IsNullOrEmpty(model.IdNumber) && _headOfficeUser.UserIdExist(model.IdNumber, model.UserName, Function.Update))
                    ModelState.AddModelError("ID Number", "The id number belongs to another user on the system.");

                if (!string.IsNullOrEmpty(model.PassportNumber) &&
                    _headOfficeUser.UserIdExist(model.PassportNumber, model.UserName, Function.Update))
                    ModelState.AddModelError("Passport Number", "The passport number belongs to another user on the system.");

                if (ModelState.IsValid)
                {
                    MethodResult<bool> result = _headOfficeUser.Edit(model, User.Identity.Name);
                    if (result.Status == MethodStatus.Successful)
                    {
                        ShowMessage("Edit Successful.", MessageType.success, "Edit User");
                        GenerateAndSendEmail(model.FirstName + " " + model.LastName,
                            model.EmailAddress);
                        return RedirectToAction("Index");
                    }
                    ShowMessage("User Not Updated.", MessageType.error, "Edit User");
                }
                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// View
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                UserDto model = _headOfficeUser.Find(id).EntityResult;

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="id">id</param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Delete(int id)
        {
            if (VerifyAuthentication())
            {
                if (_headOfficeUser.Delete(id, User.Identity.Name).Status == MethodStatus.Successful)
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
        /// Get User Name
        /// </summary>
        /// <param name="firstname">First Name</param>
        /// <param name="lastname">Last name</param>
        /// <returns></returns>
        public ActionResult GetUsername(string firstname, string lastname)
        {
            string result = UserHelper.GenerateUserName(firstname, lastname.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Reset Password
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="username"> User Name</param>
        /// <param name="actionName">Action Name</param>
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
        /// Head Office User Column listing
        /// </summary>
        /// <returns></returns>
        public ActionResult HeadOfficeUsersColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "First name", Tag = "FirstName"},
                new DropDownModel {Id = 2, Name = "Last name", Tag = "LastName"},
                new DropDownModel {Id = 3, Name = "Email Address", Tag = "EmailAddress"},
                new DropDownModel {Id = 4, Name = "Office Number", Tag = "OfficeNumber"},
                new DropDownModel {Id = 5, Name = "Cell Number", Tag = "CellNumber"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// AutoComplete user By Column
        /// </summary>
        /// <param name="columName">Column Name</param>
        /// <param name="searchData">Search String</param>
        /// <param name="userType">User Type</param>
        /// <returns></returns>
        public JsonResult AutoCompleteUsersByColumn(string columName, string searchData, int userType)
        {
            IEnumerable<User> users = _lookup.GetAllUsers(userType);

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
            }

            return Json(items, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Helper

        /// <summary>
        /// Prepare DropDowns
        /// </summary>
        private void PrepareDropDowns()
        {
            ViewData.Add("Titles", new SelectList(_lookup.Titles().ToDropDownModel(), "Id", "Name"));
        }

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
        /// Generate and Send Email
        /// </summary>
        /// <param name="fullnames"> Full names</param>
        /// <param name="password">Password</param>
        /// <param name="emailAddress">Email Address</param>
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
        /// Generate and Send Email
        /// </summary>
        /// <param name="fullname">Full names</param>
        /// <param name="emailaddress">Email Address</param>
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