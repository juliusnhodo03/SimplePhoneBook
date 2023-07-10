using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.UI;
using Application.Dto.Account;
using Application.Dto.Profile;
using Application.Modules.Common;
using Application.Modules.Profile;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Repository;
using Ninject;
using Utility.Core;
using Web.Common;
using UserType = Application.Modules.UserAccountValidation.UserType;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles user account related functionality
    /// </summary>
    /// 
    public class AccountController : BaseController
    {
        private readonly ILookup _lookup;
        private readonly IProfileValidation _profileValidation;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _userAccountValidation;
        private UserProfileDto _userProfile;

        public AccountController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _repository = LocalUnityResolver.Retrieve<IRepository>();
            _profileValidation = LocalUnityResolver.Retrieve<IProfileValidation>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
            _userProfile = LocalUnityResolver.Retrieve<UserProfileDto>();
        }




        #region Account Controller

        /// <summary>
        /// Redirects to the Home Page
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Login Function
        /// </summary>
        /// <param name="returnUrl">A URL to redirect to once a user is successfully logged in</param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        /// <summary>
        /// Login Function
        /// </summary>
        /// <param name="model">Model holding the user credentials</param>
        /// <param name="returnUrl">A URL to redirect to once a user is successfully logged in</param>
        /// <returns>Redirect to Homepage  after a user succesFully logged in</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                MethodResult<bool> result = _userAccountValidation.LogIn(model.UserName, model.Password,
                    model.RememberMe);

                if (result.Status == MethodStatus.Successful)
                    return RedirectToLocal(returnUrl);

                if (result.Status == MethodStatus.Warning)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(model);
                }

                if (result.Status == MethodStatus.NotFound)
                {
                    ModelState.AddModelError("", result.Message);
                    return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
            return View(model);
        }

        /// <summary>
        /// User Admin Login Function
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult AdminLogin()
        {
            return View(new LoginModel());
        }

        /// <summary>
        /// User Admin Login Function
        /// </summary>
        /// <param name="model">Model holding the user credentials</param>
        /// <returns>If Login Credentials is correct redirect to AdminPage</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult AdminLogin(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                MethodResult<bool> result = _userAccountValidation.LogIn(model.UserName, model.Password, false,
                    UserType.Admin);

                if (result.Status == MethodStatus.Successful)
                    return RedirectToAction("AdminResetPassword");
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Invalid Administrator Account.");
            return View(model);
        }
        /// <summary>
        /// It reset AdminPassword
        ///  /// <param name="localPasswordModel">Model holding the Admin credentials</param>
        /// <returns>Redirect to Login Screen</returns>
        /// </summary>
        /// 
        public ActionResult AdminResetPassword()
        {
            return View(new LocalPasswordModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AdminResetPassword(LocalPasswordModel localPasswordModel)
        {
            if (VerifyAuthentication())
            {
                if (ModelState.IsValid)
                {
                    MethodResult<bool> result = _userAccountValidation.ChangePassword(User.Identity.Name,
                        localPasswordModel.OldPassword, localPasswordModel.NewPassword, UserType.Admin);

                    if (result.Status == MethodStatus.Successful)
                    {
                        ShowMessage(result.Message, MessageType.success, "Change System Administrator Password");
                        return RedirectToAction("Login");
                    }
                }
                ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                return RedirectToAction("AdminResetPassword");
            }
            return RedirectToAction("Login", "Account");
        }
        /// <summary>
        /// It logoff and return to Login screen 
        /// ///  /// <param name="localPasswordModel">Model holding the Admin credentials</param>
        /// <returns>return to login Screen</returns>
        /// </summary>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _userAccountValidation.LogOf();
            return RedirectToAction("Login");
        }

        /// <summary>
        /// It Manages UserProfile
        /// <param name="message">It Describe PasswordStatus where is Changed or Set and Return UserProfile Screen</param>
        /// <returns></returns>
        /// </summary>
        /// 
        public ActionResult Manage()
        {
            MethodResult<UserProfileDto> result = _profileValidation.ProfileDetails(User.Identity.Name);
            if (result.Status == MethodStatus.Successful)
            {
                UserProfileDto profile = _userProfile ?? (_userProfile = result.EntityResult);

                PrepareDropDowns();
                return View(profile);
            }
            ShowMessage(result.Message, MessageType.error, "View User Profile");
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// It Manages UserProfile and Return to Home Page Screen
        /// <param name="model">It Represent UserProfile Attributes</param>
        /// <returns>Redirect to Home Page if Profile Information Updated successFully</returns>
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult Manage(UserProfileDto model)
        {
            ViewBag.ReturnUrl = Url.Action("Manage");

            if (ModelState.IsValid)
            {
                var loggedInUser = User.Identity.Name;

                MethodResult<bool> result = _profileValidation.Update(model, loggedInUser);

                if (result.Status == MethodStatus.Successful)
                {
                    ShowMessage("Profile information was updated successfully.", MessageType.success,
                        "Update User Information");
                    GenerateAndSendEmail(model.FirstName + " " + model.LastName, model.EmailAddress);
                    return RedirectToAction("Index", "Home");
                }

                if (result.Status == MethodStatus.Error)
                {
                    ShowMessage(result.Message, MessageType.error, "Update User Profile");
                }
            }

            // If we got this far, something failed, redisplay form
            PrepareDropDowns();
            return View(model);
        }
        /// <summary>
        /// It ChangeUserPassword
        /// <param name="localPasswordModel">It Determine if a OldPassword is Changed to a NewPassword </param>
        /// <returns>redisplay form if Password not Succesfully Changed </returns>
        /// </summary>
        /// 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(LocalPasswordModel localPasswordModel)
        {
            // ChangePassword will throw an exception rather than return false in certain failure scenarios.
            bool changePasswordSucceeded;
            try
            {
                changePasswordSucceeded =
                    _userAccountValidation.ChangePassword(User.Identity.Name, localPasswordModel.OldPassword,
                        localPasswordModel.NewPassword).EntityResult;
            }
            catch (Exception)
            {
                changePasswordSucceeded = false;
            }

            if (changePasswordSucceeded)
            {
                return RedirectToAction("Manage", new {Message = ManageMessageId.ChangePasswordSuccess});
            }
            ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");

            // If we got this far, something failed, redisplay form
            return RedirectToAction("Manage", new {Message = "Change Password Error"});
        }
        /// <summary>
        /// This Redirect to Home Page
        /// <param name="returnUrl">This will Redirect to LocalUrl</param>
        /// <returns>Redirect to Home page</returns>
        /// </summary>
        /// 
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// It Help user with Retrieve ForgotPassword
        /// 
        /// <returns></returns>
        /// </summary>
        /// 
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View(new ForgotPassworModel());
        }
      /// <summary>
      /// 
      /// </summary>
        /// <param name="forgotPassworModel">Identify UserEmail and UserName in order to ResetPassword</param>
        /// <returns>Email password to User  with resetpassword information</returns>
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ForgotPassword(ForgotPassworModel forgotPassworModel)
        {
            if (string.IsNullOrEmpty(forgotPassworModel.EmailAddress) ||
                string.IsNullOrEmpty(forgotPassworModel.UserName))
                ModelState.AddModelError("EmailUsername",
                    "Please supply email address and a username for us to reset your password.");

            if (ModelState.IsValid)
            {
                MethodResult<ForgotPasswordResult> result =
                    _userAccountValidation.ForgotPassword(forgotPassworModel.UserName, forgotPassworModel.EmailAddress);

                if (result.Status == MethodStatus.Successful)
                {
                    GenerateEmail(result.EntityResult.PasswordResetToken, result.EntityResult.FullName,
                        result.EntityResult.EmaillAddress);
                    ShowMessage("Check your email inbox for password reset information", MessageType.info,
                        "Reset Password");
                    return RedirectToAction("Index", "Home");
                }

                if (result.Status == MethodStatus.Error)
                    ShowMessage(result.Message, MessageType.error, "Forgot Password");
            }
            return View(forgotPassworModel);
        }
    /// <summary>
    /// Reset Passoword
    /// </summary>
    /// <param name="id">Represent by id to ResetPassoword</param>
    /// <returns>return to Home Page  if PasswordReseted </returns>
        [AllowAnonymous]
        public ActionResult ResetPassword(string id)
        {
            ViewBag.PasswordResetToken = id;
            return View(new ResetPasswordModel());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password">Get UserPassword</param>
        /// <param name="passwordConfirm">Confirm UserPassword if they Match</param>
        /// <returns>Return to HomePage after Password has been Reseted</returns>
        [AllowAnonymous]
        public ActionResult ResetUserPassword(ResetPasswordModel model)
        {
            if (model.Password.ToLower().Equals(model.ConfirmPassword.ToLower()))
            {
                if (_userAccountValidation.ResetPassword(User.Identity.Name, model.Password))
                {
                    ShowMessage("Password was reset successfully.", MessageType.success, "Reset Password");
                    return Json(model);
                }
                ShowMessage("Error resetting your password. Contact SBV for support.", MessageType.error,
                    "Reset Password");
            }

            return Json(model);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resetPasswordModel">Represent Password Reset</param>
        /// <param name="resetToken">Password reset validation status</param>
        /// <returns>Redirect to Home Page After Password Has successfully Reset</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel resetPasswordModel, string resetToken)
        {
            if (ModelState.IsValid)
            {
                if (_userAccountValidation.ResetPasswordFromToken(resetToken, resetPasswordModel.Password))
                {
                    ShowMessage("Password was reset successfully.", MessageType.success, "Reset Password");
                    return RedirectToAction("Index", "Home");
                }
                ShowMessage("Error resetting your password. Contact SBV for support.", MessageType.error,
                    "Reset Password");
                return RedirectToAction("Index", "Home");
            }
            return View(new ResetPasswordModel());
        }

        #endregion

        #region Helpers
        /// <summary>
        /// Manage message Status
        /// </summary>
        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resertPasswordToken">ResetPassword information</param>
        /// <param name="fullNames">FullName of a User wish to reset Password</param>
        /// <param name="emailAddress">Emailaddress to send  ResetPassword Information</param>
        private void GenerateEmail(string resertPasswordToken, string fullNames, string emailAddress)
        {
            string serverAddress =
                _repository.Query<SystemConfiguration>(a => a.LookUpKey == "SERVER_ADDRESS")
                    .FirstOrDefault()
                    .Value;
            string url = serverAddress + "/Account/ResetPassword/" + "?id=" + resertPasswordToken;

            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv User Password Reset");
            html.RenderEndTag();
            html.WriteEncodedText(String.Format("Dear {0}", fullNames));
            html.WriteBreak();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText(
                "We have received a request to reset you password. If you did not send this request, please ignore this email.");
            html.WriteBreak();
            html.WriteEncodedText(
                "However if you did send this request then follow the link below to reset you password.");
            html.WriteBreak();
            html.AddAttribute(HtmlTextWriterAttribute.Href, url);
            html.RenderBeginTag(HtmlTextWriterTag.A);
            html.WriteEncodedText("Reset Password");
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.H3);
            html.WriteEncodedText(
                "Note : This link will expire in 24 hours and you will have to provide the system with your details again for it to generate another link.");
            html.RenderEndTag();
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();

            SendEmailNotification(emailAddress, new MailMessage(), "Reset Account Password", htmlString);
        }
        /// <summary>
        /// This send Email to a user with ResetPassword Information
        /// </summary>
        /// <param name="fullnames">Represent UserFullName</param>
        /// <param name="password">Represent UserPassword</param>
        /// <param name="emailAddress">Represent  UserEmailAddress</param>
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
            html.WriteEncodedText("Your account password has been reset.");
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
            html.WriteEncodedText("Your have successfully updated you profile information.");
            html.WriteBreak();
            html.RenderEndTag();
            html.Flush();

            string htmlString = writer.ToString();

            SendEmailNotification(emailaddress, new MailMessage(), "User Account Information Update", htmlString);
        }
        /// <summary>
        /// Dropdwown for Tittles,CashCenters,Merchants,Sites
        /// </summary>
        private void PrepareDropDowns()
        {
            ViewData.Add("Titles", new SelectList(_lookup.Titles().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("CashCenters", new SelectList(_lookup.CashCenters().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Merchants", new SelectList(_lookup.Merchants().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("Sites", new MultiSelectList(_lookup.Sites().ToDropDownModel(), "Id", "Name"));
        }

        #endregion
    }
}