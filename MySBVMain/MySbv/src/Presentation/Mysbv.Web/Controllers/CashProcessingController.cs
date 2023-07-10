using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI;
using Application.Dto.CashDeposit;
using Application.Dto.CashProcessing;
using Application.Modules.CashHandling.CashProcessing.VaultProcessor;
using Application.Modules.CashHandling.CashProcessing.WebProcessor;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Mysbv.Web.CustomAttributes;
using Utility.Core;
using Web.Common;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    /// Handles Cash Processing related functionality
    /// </summary>
    [Authorize]
    [CustomAuthorize(Roles = "SBVAdmin, SBVTeller, SBVTellerSupervisor")]
    public class CashProcessingController : BaseController
    {
        private readonly ICashDepositWebProcessingValidation _cashDepositProcessing;
        private readonly ICashDepositVaultProcessingValidation _vaultDepositProcessing; 
        private readonly ILookup _lookup;
        private readonly IUserAccountValidation _userAccountValidation;

        public CashProcessingController()
        {
            _lookup = LocalUnityResolver.Retrieve<ILookup>();
            _cashDepositProcessing = LocalUnityResolver.Retrieve<ICashDepositWebProcessingValidation>();
            _vaultDepositProcessing = LocalUnityResolver.Retrieve<ICashDepositVaultProcessingValidation>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
        }



        #region Processing

        /// <summary>
        /// Index Home page for Cash Processing
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            if (VerifyAuthentication())
            {
                ViewBag.IsLoaded = false;
                ViewBag.HeaderText = "Cash Processing";

				User user = _userAccountValidation.UserByName(User.Identity.Name);
				SelectedDropDowns();
                var model = new CashProccessingModel
                {   
                    UserTypeId = user.UserTypeId,
                    CashProcessing = new CashProcessingDto(),
                    VaultProcessing = new VaultContainerDto()
                };
                return View(model);
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Index
        /// </summary>
        /// <param name="model">Database Entity instance</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(CashProccessingModel model)
        {
            if (VerifyAuthentication())
            {
                ViewBag.IsLoaded = true;

                var deposit = _cashDepositProcessing.FindBySealSerialNumber(model.CashProcessing.SealSerialNumber, User.Identity.Name);
                
                if (deposit.Status == MethodStatus.Successful)
                {
                    SelectedDropDowns();

                    CashProcessingDto proccessing = null;
                    VaultContainerDto vault = null;
                    bool isVaultProccessingType;

                    var productTypeId = _lookup.GetProductTypeId("MYSBV_DEPOSIT");

                    if (deposit.EntityResult.ProductTypeId == productTypeId)
                    {
                        isVaultProccessingType = false;
                        ViewBag.HeaderText = "Process Cash Deposit";
                        proccessing = deposit.EntityResult;
                    }
                    else
                    {
                        // massage data and map to VaultContainer format.
                        isVaultProccessingType = true;
                        vault = _vaultDepositProcessing.FormatToVaultDeposit(deposit.EntityResult);
                        ViewBag.HeaderText = "Process Vault Deposit";
                    }

                    var proccessingModel = new CashProccessingModel
                    {
                        IsVaultDeposit = isVaultProccessingType,
                        CashProcessing = proccessing,
                        VaultProcessing = vault
                    };

                    return View(proccessingModel);
                }
                ShowMessage(string.Format("{0}", deposit.Message), MessageType.info, "Process Cash Deposit");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// Process
        /// </summary>
        /// <param name="cashDepositDto">Database Entity instance</param>
        /// <returns></returns>
        public ActionResult Process(CashProcessingDto cashDepositDto)
        {
            if (VerifyAuthentication())
            {
                var result = _cashDepositProcessing.Process(cashDepositDto, User.Identity.Name);

                if (result.Status == MethodStatus.Successful)
                {
                    var user = _userAccountValidation.UserByName(User.Identity.Name);
                    var emailAddresses = _lookup.GetEmailAddresses(cashDepositDto.SiteId, user);
                    MailProcessingResults(emailAddresses, cashDepositDto, user);

                    ShowMessage(string.Format("Cash Deposit with Reference : ({0}) was processed successfully",
                        cashDepositDto.TransactionReference), MessageType.success,
                        "Process Cash Deposit");

                    return Json(new
                    {
                        url = Url.Action("Index"),
                        message = string.Format("Cash Deposit with Reference : ({0}) was processed successfully", cashDepositDto.TransactionReference)
                    });
                }

                ViewBag.IsLoaded = true;
                cashDepositDto.Confirm = false;

                ShowMessage(string.Format("Failed to process Cash Deposit!"), MessageType.error, "Process Cash Deposit");

                return Json(new { url = Url.Action("Index", cashDepositDto), message = "Failed to process Cash Deposit!" });
            }
            return RedirectToAction("Login", "Account");
        }



        /// <summary>
        /// Process
        /// </summary>
        /// <param name="vaultDepositDto">Database Entity instance</param>
        /// <returns></returns>
        public ActionResult ProcessVault(VaultContainerDto vaultDepositDto)
        {
            if (VerifyAuthentication())
            {
                var result = _vaultDepositProcessing.ProcessVault(vaultDepositDto, User.Identity.Name);

                if (result.Status == MethodStatus.Successful)
                {
                    var user = _userAccountValidation.UserByName(User.Identity.Name);
                    var emailAddresses = _lookup.GetEmailAddresses(vaultDepositDto.SiteId, user);
                    MailProcessingResults(emailAddresses, result.EntityResult, user);

                    ShowMessage(string.Format("Cash Deposit with Reference : ({0}) was processed successfully",
                        result.EntityResult.TransactionReference), MessageType.success,
                        "Process Cash Deposit");

                    return Json(new
                    {
                        url = Url.Action("Index"),
                        message = string.Format("Cash Deposit with Reference : ({0}) was processed successfully",
                        result.EntityResult.TransactionReference)
                    });
                }

                //ViewBag.IsLoaded = true;
                //cashDepositDto.Confirm = false;

                ShowMessage(string.Format("Failed to process Cash Deposit!"), MessageType.error, "Process Cash Deposit");

                return Json(new { url = Url.Action("Index", vaultDepositDto), 
                    message = "Failed to process Cash Deposit!" });
            }
            return RedirectToAction("Login", "Account");
        }


        /// <summary>
        /// Verify User
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="password">password</param>
        /// <returns></returns>
	    public ActionResult VerifyUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return Json(new { result = "NO", message = "Username and Password are required fields" },
                    JsonRequestBehavior.AllowGet);

            if (Membership.ValidateUser(username, password))
            {
                if (Roles.IsUserInRole(username, "SBVTellerSupervisor") || Roles.IsUserInRole(username, "SBVAdmin"))
                {
                    if (_userAccountValidation.IsCurrentUser(username))
                        return
                            Json(
                                new
                                {
                                    result = "NO",
                                    message = "Supervisor cannot process and verify a discrepancy at the same time."
                                },
                                JsonRequestBehavior.AllowGet);

                    int supervisorId = _userAccountValidation.GetUserId(username);
                    return Json(new { result = "OK", verifiedByid = supervisorId, message = "" },
                        JsonRequestBehavior.AllowGet);
                }
                return Json(new { result = "NO", message = "specified user is not an SBVTeller Supervisor " },
                    JsonRequestBehavior.AllowGet);
            }
            return Json(new { result = "NO", message = "Password or Username is invalid" }, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Email

        /// <summary>
        /// Create Email
        /// </summary>
        /// <param name="cashDeposit"> id for Cash deposit</param>
        /// <param name="user">user</param>
        /// <returns></returns>
        private MailMessage CreateMailMessage(CashProcessingDto cashDeposit, User user)
        {
            var isVaultDeposit = _lookup.GetProductTypeId("MYSBV_VAULT") == cashDeposit.ProductTypeId ||
                cashDeposit.DeviceId.HasValue;

            var message = new MailMessage();
            string reportPath;
            string subject;

            if (isVaultDeposit)
            {
                subject = "Vault Verification Report.pdf";
                reportPath = "/CashProcessing/VaultDepositProcessingReport";
            }
            else
            {
                subject = "Cash Verification Report.pdf";
                reportPath = (user.CashCenterId == null) ? "/CashProcessing/CashVerificationReport_To_Client " : "/CashProcessing/Cash_Center_VerificationReport";
            }
			

            byte[] attachmentBytes = PdfGeneration(cashDeposit.CashDepositId, reportPath);
            var memoryStream = new MemoryStream(attachmentBytes);
            var attachment = new Attachment(memoryStream, subject);

            message.Attachments.Add(attachment);
            return message;
        }

        /// <summary>
        /// Generate email body
        /// </summary>
        /// <param name="processingDto">Database entity instance</param>
        /// <returns></returns>
        private string GenerateEmailBody(CashProcessingDto processingDto)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.H2);
            html.WriteEncodedText("MySbv Cash Processing");
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.Hr);
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("Your transaction was successfully processed by SBV.");
            html.WriteBreak();
            html.WriteEncodedText(
                string.Format(
                    "Transaction Reference Number : {0}\n\nDeposit Type : {1}\nDate captured : {2}",
                    processingDto.TransactionReference,
                    processingDto.DepositTypeName,
                    DateTime.Now));
            html.WriteBreak();
            html.WriteEncodedText("Use the Transaction Reference Number to track the Transaction.");
            html.RenderEndTag();
            html.Flush();

            return writer.ToString();
        }

        /// <summary>
        /// Generate Pdf
        /// </summary>
        /// <param name="cashDepositid"> Cash Deposit id</param>
        /// <param name="reportPath">The path of the report</param>
        /// <returns></returns>
        private byte[] PdfGeneration(int cashDepositid, string reportPath)
        {
            var reportParameters = new Dictionary<string, string> { { "CashDepositId", cashDepositid.ToString() } };
            return GenerateReport(reportPath, reportParameters);
        }

        /// <summary>
        /// Email Processing results
        /// </summary>
        /// <param name="emailAddresses">email Address</param>
        /// <param name="processingDto">Database Entity instance</param>
        /// <param name="user"></param>
        private void MailProcessingResults(List<string> emailAddresses , CashProcessingDto processingDto, User user)
        {
			MailMessage clientAttachment = CreateMailMessage(processingDto, user);

			string clientMessageBody = GenerateEmailBody(processingDto);
			SendEmailsToMany(emailAddresses, clientAttachment, "Cash Processing Report", clientMessageBody);
        }

        #endregion

		#region DropDowns Data

        /// <summary>
        /// Selected DropDown
        /// </summary>
		private void SelectedDropDowns()
		{
			ViewData.Add("dropDownListNote", new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name"));
			ViewData.Add("dropDownListCoin", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name"));

			ViewData.Add("Reasons", new SelectList(_lookup.DiscrepancyReasons().ToDropDownModel(), "Id", "Name"));

			ViewData.Add("20000", new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 20000));
			ViewData.Add("10000", new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 10000));
			ViewData.Add("5000", new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 5000));
			ViewData.Add("2000", new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 2000));
			ViewData.Add("1000", new SelectList(_lookup.GetDenominations("Notes").ToDenominationDropDownModel(), "Id", "Name", 1000));

			ViewData.Add("500", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 500));
			ViewData.Add("200", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 200));
			ViewData.Add("100", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 100));
			ViewData.Add("50", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 50));
			ViewData.Add("20", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 20));
			ViewData.Add("10", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 10));
			ViewData.Add("5", new SelectList(_lookup.GetDenominations("Coins").ToDenominationDropDownModel(), "Id", "Name", 5));

			ViewBag.NotesCount = _lookup.GetDenominations("Notes").Count();
			ViewBag.CoinsCount = _lookup.GetDenominations("Coins").Count();
		}

		#endregion
    }
}
