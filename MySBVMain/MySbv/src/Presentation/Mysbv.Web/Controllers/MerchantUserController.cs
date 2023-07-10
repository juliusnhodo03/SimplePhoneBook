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
using Application.Modules.Maintanance.Users.Merchant;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Repository;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    [Authorize]
    public class MerchantUserController : BaseController
    {
        private readonly ILookup _lookup;
        private readonly IMerchantUserValidation _merchantUserValidation;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _userAccountValidation;

        public MerchantUserController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _merchantUserValidation = LocalUnityResolver.Retrieve<IMerchantUserValidation>();
            _repository = LocalUnityResolver.Retrieve<IRepository>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
        }



        #region Merchant User 

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                IEnumerable<MerchantUserDto> merchantUsers = _merchantUserValidation.All().OrderByDescending(e => e.UserDto.CreateDate);
                return View(merchantUsers);
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Add()
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();

                MerchantUserDto merchantUserDto = new MerchantUserDto();
                UserDto userDto = new UserDto();

                merchantUserDto.UserDto = userDto;
                
                return View(merchantUserDto);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Add(MerchantUserDto model)
        {
            if (VerifyAuthentication())
            {
                if (model.SiteIds != null && (model.SiteIds.Contains(0)))
                {
                    model.SiteIds.Remove(0);
                }

                if (model.SiteIds != null && (model.SiteIds.Count == 0))
                {
                    ModelState.AddModelError("Sites", "User must belong to a Site");
                }
                

                if (!Regex.Match(model.UserDto.UserName, @"^[a-zA-Z0-9._-]*$").Success)
                    ModelState.AddModelError("UserName", "Invalid Username");

                if (_merchantUserValidation.UserExist(model.UserDto.UserName))
                    ModelState.AddModelError("Username", "The user already exist");

                if (!model.IsEmailNotificationType && !model.IsFaxNotificationType &&
                    !model.IsSmsNotificationType)
                    ModelState.AddModelError("Notification", "User must have at least 1 notification assigned");

                if (!model.UserDto.IsUser && !model.UserDto.IsSupervisor && !model.UserDto.IsViewer)
                    ModelState.AddModelError("Roles", "User must have at least one role");

                if (string.IsNullOrEmpty(model.UserDto.IdNumber) &&
                    string.IsNullOrEmpty(model.UserDto.PassportNumber))
                    ModelState.AddModelError("Identity", "User must supply at least an Id number OR PassportNumber");

                if (!string.IsNullOrEmpty(model.UserDto.IdNumber) &&
                    _merchantUserValidation.UserIdExist(model.UserDto.IdNumber, model.UserDto.UserName))
                    ModelState.AddModelError("ID Number", "The id number belongs to another user on the system.");

                if (!string.IsNullOrEmpty(model.UserDto.PassportNumber) &&
                    _merchantUserValidation.UserIdExist(model.UserDto.PassportNumber, model.UserDto.UserName))
                    ModelState.AddModelError("Passport Number", "The passport number belongs to another user on the system.");
                
                if (ModelState.IsValid)
                {
                    var result = _merchantUserValidation.Add(model, User.Identity.Name);

                    if (result != null)
                    {
                        ShowMessage("User was successfully created.", MessageType.success, "Create User");
                        GenerateAndSendEmail(model.UserDto.FirstName + " " + model.UserDto.LastName,
                            model.UserDto.Password,
                            model.UserDto.UserName, model.UserDto.EmailAddress);
                        return RedirectToAction("Index");
                    }
                    ShowMessage("User Not Created", MessageType.error, "Create User");
                }

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var model = _merchantUserValidation.Find(id).EntityResult;

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult Edit(MerchantUserDto model)
        {
            if (VerifyAuthentication())
            {
                ModelState.Remove("UserDto.UserName");
                ModelState.Remove("UserDto.Password");
                ModelState.Remove("UserDto.ConfirmPassword");
                
                if (model.SiteIds != null && (model.SiteIds.Contains(0)))
                {
                    model.SiteIds.Remove(0);
                }

                if (model.SiteIds != null && (model.SiteIds.Count == 0))
                {
                    ModelState.AddModelError("Sites", "User must belong to a Site");
                }

                if (!model.IsEmailNotificationType && !model.IsFaxNotificationType &&
                    !model.IsSmsNotificationType)
                    ModelState.AddModelError("Notification", "User must have at least 1 notification assigned");

                if (!model.UserDto.IsUser && !model.UserDto.IsSupervisor && !model.UserDto.IsViewer)
                    ModelState.AddModelError("Roles", "User must have at least one role");

                if (string.IsNullOrEmpty(model.UserDto.IdNumber) &&
                    string.IsNullOrEmpty(model.UserDto.PassportNumber))
                    ModelState.AddModelError("Identity", "User must supply at least an Id number OR PassportNumber");

                if (!string.IsNullOrEmpty(model.UserDto.IdNumber) &&
                    _merchantUserValidation.UserIdExist(model.UserDto.IdNumber, model.UserDto.UserName, Function.Update))
                    ModelState.AddModelError("ID Number", "The id number belongs to another user on the system.");

                if (!string.IsNullOrEmpty(model.UserDto.PassportNumber) &&
                    _merchantUserValidation.UserIdExist(model.UserDto.PassportNumber, model.UserDto.UserName, Function.Update))
                    ModelState.AddModelError("Passport Number", "The passport number belongs to another user on the system.");

                if (ModelState.IsValid)
                {
                    if (_merchantUserValidation.Edit(model, User.Identity.Name).Status == MethodStatus.Successful)
                    {
                        ShowMessage("Edit Successful.", MessageType.success, "Edit User");
                        GenerateAndSendEmail(model.UserDto.FirstName + " " + model.UserDto.LastName,
                            model.UserDto.EmailAddress);
                        return RedirectToAction("Index");
                    }
                    ShowMessage("User Not Updated.", MessageType.error, "Edit User");
                }
                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult View(int id)
        {
            if (VerifyAuthentication())
            {
                var model = _merchantUserValidation.Find(id).EntityResult;

                PrepareDropDowns();
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Delete(int id)
        {
            if (VerifyAuthentication())
            {
                if (_merchantUserValidation.Delete(id, User.Identity.Name).Status == MethodStatus.Successful)
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

        public ActionResult GetUsername(string firstname, string lastname)
        {
            string result = UserHelper.GenerateUserName(firstname, lastname.Trim());
            return Json(result, JsonRequestBehavior.AllowGet);
        }

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

        public ActionResult MerchantUsersColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "First name", Tag = "FirstName"},
                new DropDownModel {Id = 2, Name = "Last name", Tag = "LastName"},
                new DropDownModel {Id = 3, Name = "Email Address", Tag = "EmailAddress"},
                new DropDownModel {Id = 4, Name = "Merchant Name", Tag = "MerchantName"},
                new DropDownModel {Id = 5, Name = "Merchant Number", Tag = "MerchantNumber"}
            };

            return Json(categories, JsonRequestBehavior.AllowGet);
        }

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
                case "MerchantName":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.Merchant.Name) == false))
                        {
                            if (user.Merchant.Name.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.Merchant.Name);
                            }
                        }
                        break;
                    }
                case "MerchantNumber":
                    {
                        foreach (User user in users.Where(e => string.IsNullOrEmpty(e.Merchant.Number) == false))
                        {
                            if (user.Merchant.Number.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(user.Merchant.Number);
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

        private void PrepareDropDowns()
        {
            ViewData.Add("Titles", new SelectList(_lookup.Titles().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Merchants", new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Sites", new MultiSelectList(_lookup.Sites().ToDropDownModel(), "Id", "Name"));
        }

        public JsonResult GetSites(string merchant)
        {
            var merchantId = Convert.ToInt32(merchant);
            var sites = _lookup.Sites().Where(a => a.MerchantId == merchantId).ToDropDownModel();
            return Json(sites, JsonRequestBehavior.AllowGet);
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