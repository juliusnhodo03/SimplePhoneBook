using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.UI;
using Application.Dto.Account;
using Application.Modules.Common;
using Application.Modules.Maintanance.Site;
using Application.Modules.Maintanance.BankAccount;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Bank Account related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
    public class BankAccountController : BaseController
    {

        private readonly ILookup _lookup;
        private readonly IBankAccountValidation _bankAccountValidation;
        private readonly ISiteValidation _siteValidation;
        private readonly IUserAccountValidation _accountValidation;

        public BankAccountController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _bankAccountValidation = LocalUnityResolver.Retrieve<IBankAccountValidation>();
            _siteValidation = LocalUnityResolver.Retrieve<ISiteValidation>();
            _accountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
        }


        #region Controller Actions

        /// <summary>
        /// 
        /// </summary>
        /// <param name="load">Represent if Bank is Saved SuccesFully</param>
        /// <returns>Redirect to Bank Home Page</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Index(bool load = false)
        {
            if (VerifyAuthentication())
            {
                if (load)
                {
                    ShowMessage("Bank Account saved successfully", MessageType.success, "Save Bank Account");
                    return RedirectToAction("Index");
                }

                var siteSettlementAccounts = _bankAccountValidation.All();
                return View(siteSettlementAccounts);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Get Bank
        /// </summary>
        /// <param name="id">Get Bank by id</param>
        /// <returns>Display Bank if it Exist</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
        public ActionResult View(int id = 0)
        {
            if (VerifyAuthentication())
            {

                var result = _bankAccountValidation.Find(id);
                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "View Bank Account");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Droddown for Bank,Site,AccountTypes
        /// </summary>
        private void PrepareDropDowns()
        {
            ViewData.Add("Banks", new SelectList(_lookup.GetBanks().ToBanksDropDownModel(), "Id", "Name"));
            ViewData.Add("GetSites", new SelectList(_lookup.Sites().ToDropDownModel(), "Id", "Name"));
            ViewData.Add("GetAccountTypes", new SelectList(_lookup.GetAccountTypes().ToDropDownModel(), "Id", "Name"));
        }


        /// <summary>
        /// Create BankAccount and Validate it
        /// </summary>
        /// <param name="siteId">Represent BankId</param>
        /// <returns></returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Create(int siteId = 0)
        {
            if (VerifyAuthentication())
            {
                PrepareDropDowns();

                BankAccountHolderDto accountHolderDto;

                var accountDto = new AccountDto
                {
                    AccountTypeId = 0,
                    TransactionTypeId = 0,
                    BankId = 0,
                    AccountNumber = " ",
                    AccountHolderName = " ",
                    DefaultAccount = false,
                    BeneficiaryCode = _bankAccountValidation.GenerateBeneficiaryCode(),
                };

                if (siteId > 0)
                {
                    accountHolderDto = new BankAccountHolderDto
                    {
                        SiteId = siteId,
                        CitCode = _siteValidation.Find(siteId).EntityResult.CitCode
                    };
                    accountHolderDto.Accounts.Add(accountDto);
                }
                else
                {
                    accountHolderDto = new BankAccountHolderDto
                    {
                        SiteId = siteId,
                        CitCode = ""
                    };

                    accountHolderDto.Accounts.Add(accountDto);
                }

                return View(accountHolderDto);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Create BankAccount and Validate 
        /// </summary>
        /// <param name="bankAccounts">Represent a Bank</param>
        /// <returns>Save a bank if It doesnt Exist </returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Create(BankAccountHolderDto bankAccounts)
        {
            if (bankAccounts.ProcessType == "captured")
            {
                var result = _bankAccountValidation.Add(bankAccounts, User.Identity.Name);
                if (result.Status == MethodStatus.Successful)
                {
                    ShowMessage("Bank Account saved successfully", MessageType.success, "Save Bank Account");
                    return Json(new { saveStatus = "successfull" });
                }
                else 
                {
                    PrepareDropDowns();
                    ShowMessage(result.Message, MessageType.error, "Error");
                }
                
            }
            else
            {
                var result = _bankAccountValidation.AddOnContinue(bankAccounts, User.Identity.Name);
                if (result.Status == MethodStatus.Successful)
                {
                    //Send Email Notifications
                    foreach (var account in bankAccounts.Accounts)
                    {
                        var accountId = _bankAccountValidation.GetAccountByBeneficiaryCode(account.BeneficiaryCode);
                        if (accountId != 0)
                        {
                            account.AccountId = accountId;
                            User capturer = _lookup.SubmittedByUser(accountId);

                            string taskRefNumber = _bankAccountValidation.GetTaskRef(accountId);
                            SendForApproval(account, taskRefNumber);
                            SendConfirmation(capturer, taskRefNumber);
                        }
                    }

                    ShowMessage("Bank Account was successfully Submitted. An email will be sent for Approval.", MessageType.success, "Submit Bank Account");
                    return Json(new { saveStatus = "successfull" });
                }
                else
                {
                    PrepareDropDowns();
                    ShowMessage(result.Message, MessageType.error, "Error");
                }
            }

            ShowMessage("Bank Account Not Saved", MessageType.error, "Error");
            return Json(new { url = "" });
        }

        /// <summary>
        /// Get a bank by BankId and validate it
        /// </summary>
        /// <param name="id">Represent  BankId</param>
        /// <returns>Redirect to Bank Home Page After it Succesfully Saved</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture")]
        public ActionResult Edit(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _bankAccountValidation.Find(id);
	            ViewBag.IsDefaultAccount = result.EntityResult.DefaultAccount;

                if (result.EntityResult.StatusId == _lookup.GetStatusId("PENDING"))
                {
                    var accountId = id;
                    ShowMessage("Bank Account is already in a pending state and can not be updated.", MessageType.info, "Bank Account Update.");
                    return RedirectToAction("View", "BankAccount", new { @id = accountId });
                }

                if (result.Status == MethodStatus.Successful)
                {
                    PrepareDropDowns();
                    return View(result.EntityResult);
                }
                ShowMessage(result.Message, MessageType.error, "Edit Bank Account");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Post and Save bank after being Edited
        /// </summary>
        /// <param name="accountDto">Represent Current Bank </param>
        /// <param name="command"></param>
        /// <returns>Save bank After it is Edited and Redirect to Bank Home Page</returns>
        [HttpPost]
        public ActionResult Edit(AccountDto accountDto, string command)
        {
            if (VerifyAuthentication())
            {
                if (ModelState.IsValid)
                {
                    User approver = _lookup.GetApprover();
                    if (approver == null)
                    {
                        ShowMessage("The is no user with approval role on the system. Create an approver account to continue", MessageType.warning, "No Approver");
                        return RedirectToAction("Index");
                    }

                    if (!accountDto.DefaultAccount)
                    {
                        if (!_bankAccountValidation.AreThereDefaultAccount(accountDto.AccountId, accountDto.SiteId))
                        {
                            ShowMessage("There Must at least be one Default Account for this Site", MessageType.error, "Default Bank Account");
                            return View(accountDto);
                        }
                    }
                    else
                    {
                        if (_bankAccountValidation.DefaultAccountByStatus(accountDto.AccountId, accountDto.SiteId, _lookup.GetStatusId("PENDING")))
                        {
                            ShowMessage("There is already a Default Acccount that has been submitted for Approval", MessageType.error, "Default Bank Account");
                            return View(accountDto);
                        }
                    }


                    //Chevk if the Account Number is already in use by the current Site
                    if (_bankAccountValidation.AccountInUseByCurrentSite(accountDto.AccountId, accountDto.SiteId, accountDto.AccountNumber))
                    {
                        ShowMessage("Bank Account with Account Number: " + accountDto.AccountNumber + " is already in use by this site", MessageType.error, "Duplicate Bank Account");
                        return View(accountDto);
                    }


                    if (!IsNumeric(accountDto.AccountNumber))
                    {
                        ShowMessage("Account Number must be numeric", MessageType.error, "Save Bank Account");
                        return View(accountDto);
                    }

                    switch (command)
                    {
                        case "Save":

                            if (accountDto.StatusId == _lookup.GetStatusId("ACTIVE") ||
                                accountDto.StatusId == _lookup.GetStatusId("DECLINED"))
                            {
                                var saveResult = _bankAccountValidation.Submit(accountDto, User.Identity.Name);
                                if (saveResult.Status == MethodStatus.Successful)
                                {
                                    User capturer = _lookup.SubmittedByUser(accountDto.AccountId);

                                    string taskRefNumber = _bankAccountValidation.GetTaskRef(accountDto.AccountId);
                                    SendForApproval(accountDto, taskRefNumber);
                                    SendConfirmation(capturer, taskRefNumber);
                                    ShowMessage(saveResult.Message, MessageType.success, "Bank Account Submit");
                                    return RedirectToAction("Index");
                                }
                            }
                            else
                            {
                                if (_bankAccountValidation.DefaultAccountByStatus(accountDto.AccountId, accountDto.SiteId, _lookup.GetStatusId("ACTIVE")))
                                {
                                    ShowMessage("There is already an Active Default Account in the System, Please submit this Account to be approved as the new Default Account", MessageType.error, "Default Bank Account");
                                    return View(accountDto);
                                }

                                //Edit the Record and redirect to the Index Page once Successful
                                if (_bankAccountValidation.Edit(accountDto, User.Identity.Name).Status == MethodStatus.Successful)
                                {
                                    ShowMessage("Bank Account Details updated successfully", MessageType.success, "Saved");
                                    return RedirectToAction("Index");
                                }
                            }
                            
                            PrepareDropDowns();
                            return View(accountDto);

                        case "Submit":
                            var sumbitResult = _bankAccountValidation.Submit(accountDto, User.Identity.Name);
                            if (sumbitResult.Status == MethodStatus.Successful)
                            {
                                User capturer = _lookup.SubmittedByUser(accountDto.AccountId);
                                
                                string taskRefNumber = _bankAccountValidation.GetTaskRef(accountDto.AccountId);
                                SendForApproval(accountDto, taskRefNumber);
                                SendConfirmation(capturer, taskRefNumber);
                                ShowMessage(sumbitResult.Message, MessageType.success, "Bank Account Submit");
                                return RedirectToAction("Index");
                            }
                            PrepareDropDowns();
                            return View(accountDto);

                        default:
                            ShowMessage("Error Saving Site. Please call helpdesk for support.", MessageType.error, "Error Saving Site");
                            PrepareDropDowns();
                            return View(accountDto);
                    }
                }
                return View(accountDto);
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Get a bank by BankId and validate it
        /// </summary>
        /// <param name="id">Represent  BankId</param>
        /// <returns>Redirect to Bank Home Page After it Succesfully Saved</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVApprover, SBVDataCapture")]
        public ActionResult Approve(int id)
        {
            if (VerifyAuthentication())
            {
                var result = _bankAccountValidation.ApprovalFind(id);
                if (result.Status == MethodStatus.Successful)
                {
                    var actualAccount = _bankAccountValidation.Find(result.EntityResult.AccountId);

                    if (actualAccount.EntityResult.StatusId == _lookup.GetStatusId("SAVED") || actualAccount.EntityResult.StatusId == _lookup.GetStatusId("ACTIVE"))
                    {
                        var accountId = actualAccount.EntityResult.AccountId;
                        ShowMessage("Bank Account is not in a state that requires approval", MessageType.info, "Bank Account Approval.");
                        return RedirectToAction("View", "BankAccount", new { @id = accountId });
                    }

                    if (result.Status == MethodStatus.Successful)
                    {
                        PrepareDropDowns();
                        return View(result.EntityResult);
                    }
                }
                ShowMessage(result.Message, MessageType.error, "Bank Account Approval");
                return RedirectToAction("Index", "Approval");
            }
            return RedirectToAction("Login", "Account");
        }

        
        [HttpPost]
        public ActionResult Approve(AccountDto account)//, string username)
        {
            if (ModelState.IsValid)
            {
                if (!_bankAccountValidation.IsBankAccountInApprovalState(account.AccountId))
                {
                    ShowMessage("Bank Account has already been Approved or Declined", MessageType.error, "Approve");
                    return RedirectToAction("Index", "Approval");
                }

                int userId = _accountValidation.UserByName(User.Identity.Name).UserId;
                User capturer = _lookup.SubmittedByUser(account.AccountId);
                User currentUser = _lookup.GetUser(User.Identity.Name);

                string taskRefNumber = _bankAccountValidation.GetTaskRef(account.AccountId);

                if (_bankAccountValidation.UserApprovingOwnTask(userId, account.AccountId))
                {
                    ShowMessage("You cannot approve a Bank Account that was submitted by you for approval", MessageType.error, "Approve");
                    return RedirectToAction("Index", "Approval");
                }

                var result = _bankAccountValidation.Edit(account, User.Identity.Name);
                if (result.Status == MethodStatus.Successful)
                {
                    string capturerEmailbody = GenerateEmailBody_NoLink("Your mySBV task submitted to the approver for approval has been approved", capturer, taskRefNumber);
                    SendEmailNotification(capturer.EmailAddress, new MailMessage(), "Bank Account Approved", capturerEmailbody);

                    string approverEmailbody = GenerateEmailBody_NoLink("You have recently approved the following mySBV task", capturer, taskRefNumber);
                    SendEmailNotification(currentUser.EmailAddress, new MailMessage(), "Bank Account Approved", approverEmailbody);

                    ShowMessage("Bank Account approved successfully", MessageType.success, "Bank Account Approval");
                    return RedirectToAction("Index", "Approval");
                }
            }
            else
            {
                PopulateDropdownLists();
                ShowMessage("Error approving Bank Account", MessageType.error, "Bank Account Error");
            }
            PopulateDropdownLists();
            return View(account);
        }


        // Reject
        [CustomAuthorize(Roles = "SBVAdmin, SBVApprover")]
        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult Reject(RejectAccountArgumentsDto rejectAccountDto)
        {
            var bankAccount = _bankAccountValidation.Find(rejectAccountDto.Id);
            User currentUser = _lookup.GetUser(User.Identity.Name);

            if (!_bankAccountValidation.IsBankAccountInApprovalState(rejectAccountDto.Id))
            {
                ShowMessage("Bank Account has already been Approved or Declined", MessageType.error, "Approve");
                return Json
                    (
                        new
                        {
                            url = Url.Action("Index", "Approval"),
                            Message = "Bank Account has already been Approved or Declined",
                        }, JsonRequestBehavior.AllowGet
                    );
            }

            if (_bankAccountValidation.UserApprovingOwnTask(currentUser.UserId, rejectAccountDto.Id))
            {
                ShowMessage("You cannot decline a Bank Account that was submitted by you for approval", MessageType.error, "Decline");
                return Json
                    (
                        new
                        {
                            url = Url.Action("Index", "Approval"),
                            Message = "Bank Account has already been Approved or Declined",
                        }, JsonRequestBehavior.AllowGet
                    );
            }
            
            AccountDto account = new AccountDto
            {
                AccountId = bankAccount.EntityResult.AccountId,
                SiteId = bankAccount.EntityResult.SiteId,
                AccountTypeId = bankAccount.EntityResult.AccountTypeId,
                BankId = bankAccount.EntityResult.BankId,
                TransactionTypeId = bankAccount.EntityResult.TransactionTypeId,
                AccountNumber = bankAccount.EntityResult.AccountNumber,
                AccountHolderName = bankAccount.EntityResult.AccountHolderName,
                BeneficiaryCode = bankAccount.EntityResult.BeneficiaryCode,
                IsApproved = false
            };
            
            var results = _bankAccountValidation.IsBankAccountRejected(rejectAccountDto, account, User.Identity.Name);
            if (results)
            {
                User capturer = _lookup.SubmittedByUser(bankAccount.EntityResult.AccountId);
                string taskRefNumber = _bankAccountValidation.GetTaskRef(account.AccountId);

                //Send email Notification to the Capturer that their Task has been Rejected
                string capturerEmailbody = GenerateEmailBody_NoLink("Your mySBV task submitted to the Approver for approval has been declined, Please review the data  " +
                                                       "that has been captured for this client and resubmit for approval", capturer, taskRefNumber);
                SendEmailNotification(capturer.EmailAddress, new MailMessage(), "Bank Account Approval Rejection", capturerEmailbody);
                
                //Send email Notification to the Approver that they have just declined a Task
                string approverbody = GenerateEmailBody_NoLink("You have just declined the following Task", capturer, taskRefNumber);
                SendEmailNotification(currentUser.EmailAddress, new MailMessage(), "Bank Account Approval Rejection", approverbody);

                ShowMessage("Bank Account was successfully rejected. A notification email was sent to the capturer.", MessageType.success, "Reject Bank Account");
                return Json
                    (
                        new
                        {
                            url = Url.Action("Index", "Approval"),
                        }, JsonRequestBehavior.AllowGet
                    );
            }
            else
            {
                return Json
                    (
                        new
                        {
                            url = Url.Action("Index", "Approval"),
                            Message = "Failed to reject Bank Account!",
                        }, JsonRequestBehavior.AllowGet
                    );
            }
        }


        /// <summary>
        /// Delete Bank by Id
        /// </summary>
        /// <param name="id">Represent Bank Id</param>
        /// <returns>Delete a bank and Return to Bank Home Page</returns>
        [CustomAuthorize(Roles = "SBVAdmin, SBVDataCapture, SBVApprover")]
        public ActionResult Delete(int id = 0)
        {
            if (VerifyAuthentication())
            {
                //Find the record to do some later checks
                var account = _bankAccountValidation.Find(id);

                if (_bankAccountValidation.IsAccountDefault(id, account.EntityResult.SiteId))
                {
                    ShowMessage("Cannot Delete a Default Account, Please Set your new Default Account and Try Deleting this Bank Account", MessageType.error, "Delete Bank Account.");
                    return Json(new { url = Url.Action("Index") });
                }


                //Perform the Deletion
                var result = _bankAccountValidation.Delete(id, User.Identity.Name);
                
                if (result.Status == MethodStatus.Successful)
                {
                    //If the deletion was successful and the Status was SAVED or DECLINED, Then provide user with message only  + Redirect
                    if (account.EntityResult.StatusId == _lookup.GetStatusId("SAVED") ||
                        account.EntityResult.StatusId == _lookup.GetStatusId("DECLINED"))
                    {
                        ShowMessage(result.Message, MessageType.success, "Deleted");
                    }
                    else
                    {
                        //If the deletion was successful and the Status was not SAVED or DECLINED, Then go through the Approval Process
                        User capturer = _lookup.SubmittedByUser(id);
                        User currentUser = _lookup.GetUser(User.Identity.Name);

                        string taskRefNumber = _bankAccountValidation.GetTaskRef(id);
                        //Send email Notification to the Capturer that their Task has been Rejected
                        string capturerEmailbody = GenerateEmailBody_NoLink(
                                "Your MySBV task submitted to the Approver for approval.", capturer, taskRefNumber);
                        SendEmailNotification(capturer.EmailAddress, new MailMessage(), "Bank Account Deletion", capturerEmailbody);

                        int approvalObjectsId = _bankAccountValidation.GetApprovalObjectIdByRefNumber(taskRefNumber);

                        //Send email Notification to the Approver that they have just declined a Task
                        string approverbody = GenerateAccountApprovalEmailBody("A MySBV task submitted to you for approval.", approvalObjectsId, taskRefNumber);
                        SendEmailNotification(currentUser.EmailAddress, new MailMessage(), "Bank Account Deletion", approverbody);

                        ShowMessage(result.Message, MessageType.success, "Deleted");
                    }
                }
                else
                {
                    ShowMessage(result.Message, MessageType.error, "Delete Bank Account");
                }
                return Json(new { url = Url.Action("Index") });
            }
            return RedirectToAction("Login", "Account");
        }

        #endregion

        #region Helpers

        /// <summary>
        /// GetBank DropDown
        /// </summary>
        /// <returns></returns>
        public JsonResult GetBanksKendo()
        {
            var banks = _lookup.GetBanks().ToDropDownModel();
            return Json(banks, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Bank Account Validation
        /// </summary>
        /// <param name="text">Bank validation</param>
        /// <returns></returns>
        bool IsNumeric(string text)
        {
            Regex regex = new Regex(@"^[-+]?[0-9]*\.?[0-9]+$");
            return regex.IsMatch(text);
        }

        /// <summary>
        /// GetAccountType
        /// </summary>
        /// <returns>AccountTypes</returns>
        public JsonResult GetAccountTypes()
        {
            var accountTypes = _lookup.GetAccountTypes().ToDropDownModel();
            return Json(accountTypes, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetSites
        /// </summary>
        /// <returns>returnSites</returns>
        public JsonResult GetSites()
        {
            IEnumerable<DropDownModel> sites = _lookup.Sites().ToDropDownModel();
            return Json(sites, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Validate BankCode
        /// </summary>
        /// <param name="bankId">Bank Code</param>
        /// <returns>Save bankCode if it doesnt Exist</returns>
        public JsonResult GetBankCode(int bankId)
        {
            if (bankId > 0 && bankId != 1000500)
            {
                var bank = _lookup.GetBankCode(bankId);
                return Json(new { BranchCode = bank.BranchCode }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { BranchCode = "Undefined" }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Validate Citcode
        /// </summary>
        /// <param name="siteId">Represent CitCode</param>
        /// <returns>Save Cit Code if it doesn't Exist</returns>
        public JsonResult GetCitCode(int siteId)
        {
            if (siteId > 0)
            {
                Site site = _lookup.GetCitCode(siteId);
                return Json(new { site.CitCode }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { CitCode = "Undefined" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// /Check if Account has Default Accounts
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public JsonResult HasDefaultAccount(int siteId)
        {
            var defaultAccount = _lookup.GetDefaultAccount(siteId);
            return Json(new {doesNotExist = defaultAccount == null } , JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Check if a deafult account exist for the current Site
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public JsonResult DefaultAccountExists(int siteId)
        {
            if (siteId > 0)
            {
                var defaultAccount = _bankAccountValidation.DefaultAccountAlreadyExist(siteId);
                return Json(new { DefaultAccount = defaultAccount }, JsonRequestBehavior.AllowGet);
            }
            return Json(new {DefaultAccount = false}, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Check for duplicate Accounts from the Database
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="accountNumber"></param>
        /// <param name="bankId"></param>
        /// <returns></returns>
        public JsonResult DuplicateBankAccounts(int siteId, string accountNumber, int bankId)
        {
            if (siteId > 0 && accountNumber != "" && bankId > 0)
            {
                var hasDuplicateAccounts = _bankAccountValidation.DuplicateBankAccounts(siteId, accountNumber, bankId);
                return Json(new {HasDuplicates = hasDuplicateAccounts}, JsonRequestBehavior.AllowGet);
            }
            return Json(new {HasDefaultAccount = false}, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// Get Site Bank Accounts
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public JsonResult GetSiteBankAccouns(int siteId)
        {
            if (siteId > 0)
            {
                var bankAccounts = _bankAccountValidation.SiteBankAccounts(siteId).ToList();
                return Json(new {Accounts = bankAccounts}, JsonRequestBehavior.AllowGet);
            }
            return Json(new { Accounts = "" }, JsonRequestBehavior.AllowGet);
        }
        

        /// <summary>
        /// Generate Account Approval Email Body
        /// </summary>
        /// <param name="messageHeader"></param>
        /// <param name="approvalObjectsId"></param>
        /// <param name="taskRefNumber"></param>
        /// <returns></returns>
        private string GenerateAccountApprovalEmailBody(string messageHeader, int approvalObjectsId, string taskRefNumber)
        {
            User user = _lookup.GetUser(User.Identity.Name);

            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.Table);
            html.WriteEncodedText(messageHeader);
            html.WriteBreak();
            html.WriteBreak();

            html.RenderBeginTag(HtmlTextWriterTag.Strong);
            html.WriteEncodedText("Details of request");
            html.RenderEndTag();
            html.WriteBreak();

            string hostUrl = _lookup.GetServerAddress();

            html.WriteEncodedText("Task Reference Number : " + taskRefNumber);

            html.WriteBreak();
            html.WriteBreak();
            hostUrl += "/BankAccount/Approve/?id=" + approvalObjectsId;

            html.WriteEncodedText("Submitted by: " + user.FirstName + " " + user.LastName);
            html.WriteBreak();
            html.WriteEncodedText("Submitted date: " + DateTime.Now.ToShortDateString());

            html.WriteBreak();
            html.WriteBreak();

            html.RenderBeginTag(HtmlTextWriterTag.A);
            html.WriteEncodedText(hostUrl);
            html.RenderEndTag();

            html.RenderEndTag(); // End Table

            string htmlString = writer.ToString();
            return htmlString;
        }
        

        /// <summary>
        /// Generate Email Body without a Link
        /// </summary>
        /// <param name="messageHeader"></param>
        /// <param name="user"></param>
        /// <param name="taskRefNumber"></param>
        /// <returns></returns>
        private string GenerateEmailBody_NoLink(string messageHeader, User user, string taskRefNumber)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.Table);
            html.WriteEncodedText(messageHeader);
            html.WriteBreak();
            html.WriteBreak();

            html.RenderBeginTag(HtmlTextWriterTag.Strong);
            html.WriteEncodedText("Details of request");
            html.RenderEndTag();
            html.WriteBreak();
            
            html.WriteEncodedText("Task Reference Number : " + taskRefNumber);

            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText("Submitted by: " + user.FirstName + " " + user.LastName);
            html.WriteBreak();
            html.WriteEncodedText("Submitted date: " + DateTime.Now.ToShortDateString());

            html.RenderEndTag(); // End Table

            string htmlString = writer.ToString();
            return htmlString;
        }
        
        /// <summary>
        /// Create Confirmation Message Body
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taskReferenceNumber"></param>
        /// <returns></returns>
        private string createConfirmationMessageBody(User user, string taskReferenceNumber)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.Table);
            html.WriteEncodedText("Your MySBV task submitted to the Approver for approval.");
            html.WriteBreak();
            html.WriteBreak();

            html.RenderBeginTag(HtmlTextWriterTag.Strong);
            html.WriteEncodedText("Details of request");
            html.RenderEndTag();
            html.WriteBreak();
            
            html.WriteEncodedText("Task Reference Number : " + taskReferenceNumber);

            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText("Submitted by: " + user.FirstName + " " + user.LastName);
            html.WriteBreak();
            html.WriteEncodedText("Submitted date: " + DateTime.Now.ToShortDateString());

            html.RenderEndTag(); // End Table

            string htmlString = writer.ToString();
            return htmlString;
        }
        
        /// <summary>
        /// Send Confirmation 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="taskReferenceNumber"></param>
        public void SendConfirmation(User user, string taskReferenceNumber)
        {
            string body = createConfirmationMessageBody(user, taskReferenceNumber);
            const string subject = "Bank Account Maintenance";
            SendEmailNotification(user.EmailAddress, new MailMessage(), subject, body);
        }
        

        /// <summary>
        /// Send for Approval
        /// </summary>
        /// <param name="account"></param>
        /// <param name="taskRefNumber"></param>
        public void SendForApproval(AccountDto account, string taskRefNumber)
        {
            if (account != null)
            {
                User emails = _lookup.GetApprover();

                int activeStatusId = _lookup.GetStatusId("ACTIVE");
                string body = String.Empty;
                int approvalObjectsId = _bankAccountValidation.GetApprovalObjectIdByRefNumber(taskRefNumber);
                
                //Check if the current Record is Default Account and if there is another Active Default in the system
                if (account.DefaultAccount && _bankAccountValidation.DefaultAccountByStatus(account.AccountId, account.SiteId, activeStatusId))
                {
                    body = GenerateAccountApprovalEmailBody("A MySBV task submitted to you for approval. Please note that this Bank Account will be the new Default Account for this Site", approvalObjectsId, taskRefNumber);
                }
                else
                {
                    body = GenerateAccountApprovalEmailBody("A MySBV task submitted to you for approval.", approvalObjectsId, taskRefNumber);
                }

                const string subject = "Bank Account Maintenance";
                SendEmailNotification(emails.EmailAddress, new MailMessage(), subject, body);

                ShowMessage("Email for Bank Account approval request has been escalated to the Approver.",
                    MessageType.success, "Approval request");
            }
            else
            {
                ShowMessage("Error sending email!", MessageType.error, "Bank Account Approval");
            }
        }
        
        #endregion

        #region ListView Actions
        /// <summary>
        /// Site Settlement Account Columns Listing Toolbar Template
        /// </summary>
        /// <returns></returns>
        public ActionResult SiteSettlementAccountColumnsListingToolbarTemplate()
        {
            var categories = new List<DropDownModel>
            {
                new DropDownModel {Id = 1, Name = "Bank Name", Tag = "BankName"},
                new DropDownModel {Id = 2, Name = "Site Name", Tag = "SiteName"},
                new DropDownModel {Id = 3, Name = "Branch Code", Tag = "BranchCode"},
                new DropDownModel {Id = 4, Name = "Account Number", Tag = "AccountNumber"},
                new DropDownModel {Id = 5, Name = "BeneficiaryCode", Tag = "BeneficiaryCode"},
            };
            return Json(categories, JsonRequestBehavior.AllowGet);
        }
        
        /// <summary>
        /// AutoComplete as User is typing......
        /// </summary>
        /// <param name="columName">User Text as is Typing....</param>
        /// <param name="searchData">Display suggestion Based on what is Typing....</param>
        /// <returns>Return user Result Information</returns>
        public JsonResult AutoCompleteSiteSettlementAccountByColumn(string columName, string searchData)
        {
            var siteSettlementAccounts = _bankAccountValidation.All().ToList();

            //return Json(merchants, JsonRequestBehavior.AllowGet);

            var items = new List<string>();

            switch (columName)
            {
                case "BankName":
                    {
                        foreach (
                            var siteSettlementAccount in siteSettlementAccounts.Where(e => string.IsNullOrEmpty(e.BankName) == false))
                        {
                            if (siteSettlementAccount.BankName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(siteSettlementAccount.BankName);
                            }
                        }
                        break;
                    }
                case "SiteName":
                    {
                        foreach (
                            var siteSettlementAccount in siteSettlementAccounts.Where(e => string.IsNullOrEmpty(e.SiteName) == false))
                        {
                            if (siteSettlementAccount.SiteName.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(siteSettlementAccount.SiteName);
                            }
                        }
                        break;
                    }
                case "BranchCode":
                    {
                        foreach (
                            var siteSettlementAccount in siteSettlementAccounts.Where(e => string.IsNullOrEmpty(e.BranchCode) == false))
                        {
                            if (siteSettlementAccount.BranchCode.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(siteSettlementAccount.BranchCode);
                            }
                        }
                        break;
                    }
                case "AccountNumber":
                    {
                        foreach (
                            var siteSettlementAccount in siteSettlementAccounts.Where(e => string.IsNullOrEmpty(e.AccountNumber) == false))
                        {
                            if (siteSettlementAccount.AccountNumber.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(siteSettlementAccount.AccountNumber);
                            }
                        }
                        break;
                    }
                case "BeneficiaryCode":
                    {
                        foreach (
                            var siteSettlementAccount in siteSettlementAccounts.Where(e => string.IsNullOrEmpty(e.BeneficiaryCode) == false))
                        {
                            if (siteSettlementAccount.BeneficiaryCode.ToLower().StartsWith(searchData.ToLower()))
                            {
                                items.Add(siteSettlementAccount.BeneficiaryCode);
                            }
                        }
                        break;
                    }
            }

            return Json(items.Distinct(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// GetBeneficiaryCode
        /// </summary>
        /// <returns>Beneficiary Code</returns>
        public string GetBeneficiaryCode()
        {
            return _bankAccountValidation.GenerateBeneficiaryCode();
        }
        
        /// <summary>
        /// Populate Dropdown Lists
        /// </summary>
        /// <returns>DropdownLists</returns>
        public JsonResult PopulateDropdownLists()
        {
            var banks = _lookup.GetBanks().ToDropDownModel();
            var accountTypes = _lookup.GetAccountTypes().ToDropDownModel();
            var beneficiaryCode = _bankAccountValidation.GenerateBeneficiaryCode();

            return
                Json(
                    new
                    {
                        Bank = banks,
                        AccountType = accountTypes,
                        BeneficiaryCode = beneficiaryCode
                    }, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}