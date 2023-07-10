using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Logging;
using Domain.Repository;
using Infrastructure.Logging;
using Microsoft.Reporting.WebForms;
using Mysbv.Web.CustomAttributes;
using Task = System.Threading.Tasks.Task;

namespace Mysbv.Web.Controllers
{
    /// <summary>
    ///     Handles Base related functionality
    /// </summary>
    [OutputCacheAttribute(VaryByParam = "*", Duration = 0, NoStore = true)]
    public class BaseController : Controller
    {
        #region Enums

        /// <summary>
        ///     Represent Messagetypes
        /// </summary>
        public enum MessageType
        {
            error,
            success,
            info,
            warning
        }

        #endregion


        private readonly ILogger _logger;
        private readonly IUserAccountValidation _userAccountValidation;
        private readonly IRepository _repository;

        public BaseController()
        {
            _logger = LocalUnityResolver.Retrieve<ILogger>();
            _userAccountValidation = LocalUnityResolver.Retrieve<IUserAccountValidation>();
            _repository = LocalUnityResolver.Retrieve<IRepository>();

            HasNotification();
            GenerateReceipts();
        }



        #region Overrides

        /// <summary>
        ///     Exception
        /// </summary>
        /// <param name="filterContext">Description of ExceptionStackTrace</param>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;

            if (filterContext.Exception != null)
            {
                _logger.LogException(filterContext.Exception);

                if (filterContext.Exception is NotauthorizedHttpException)
                {
                    View("NotAuthorized", filterContext).ExecuteResult(filterContext);
                }
                else
                    View("Error").ExecuteResult(filterContext);
            }
        }

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);

            if (User.Identity.IsAuthenticated)
            {
                User user = _userAccountValidation.LoggedOnUser(User.Identity.Name);
                ViewBag.IsValidUser = (user.UserTypeId != null);
            }
        }

        #endregion

        #region Toasts

        /// <summary>
        ///     Used to display a toast message on the screen
        /// </summary>
        /// <param name="message">The text to display.</param>
        /// <param name="type">Enum for the message type</param>
        /// <param name="title">Title for the toast</param>
        public void ShowMessage(string message, MessageType type, string title)
        {
            // Cleans the message to allow single quotation marks 
            string cleamessage = message.Replace("'", "\'");
            string script = "ShowAlert('" + Enum.GetName(typeof (MessageType), type) + "', '" + cleamessage + "', '" +
                            title + "');";
            if (!TempData.ContainsKey("Script"))
            {
                TempData.Add("Script", script);
            }
            TempData["Script"] = script;
        }

        #endregion

        #region Reporting

        /// <summary>
        ///     Used to generate a report from report server
        /// </summary>
        /// <param name="reportPath">Path to the report</param>
        /// <param name="format">The format in which to render the report.Default is PDF</param>
        /// <param name="parameters">A dictionary of the report's parameters</param>
        /// <returns>Returns byte array result of the report if found,else array length will be 0. </returns>
        public byte[] GenerateReport(string reportPath, Dictionary<string, string> parameters, string format = "PDF")
        {
            byte[] reportResult;

            using (var reportViewer = new ReportViewer())
            {
                reportViewer.ProcessingMode = ProcessingMode.Remote;
                //reportViewer.KeepSessionAlive = true;
                reportViewer.ServerReport.ReportServerUrl = new Uri(ConfigurationManager.AppSettings["ReportServer"]);

                reportViewer.ServerReport.ReportPath = reportPath;

                if (parameters != null && parameters.Count > 0)
                {
                    List<ReportParameter> reportParameters =
                        parameters.Select(b => new ReportParameter(b.Key, b.Value)).ToList();
                    reportViewer.ServerReport.SetParameters(reportParameters);
                }

                reportResult = reportViewer.ServerReport.Render(format);
            }
            return reportResult;
        }

        #endregion

        #region Email

        /// <summary>
        ///     Used to fire and forget the sending of email on a separate thread.
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachments to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        public void SendEmailNotification(string recipients, MailMessage attachments, string subject, string body)
        {
            Task.Factory.StartNew(() => SendEmail(recipients, attachments, subject, body));
        }

        /// <summary>
        ///     Used to fire and forget the sending of email on a separate thread.
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachments to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        public void SendEmailsToMany(List<string> recipients, MailMessage attachments, string subject, string body)
        {
            Task.Factory.StartNew(() => SendMoreEmails(recipients, attachments, subject, body));
        }

        /// <summary>
        ///     Used to send email notifications
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachment to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        private void SendEmail(string recipients, MailMessage attachments, string subject, string body)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(recipients);
                message.IsBodyHtml = true;
                message.Body = body;
                message.Subject = subject;
                var smtpClient = new SmtpClient();

                if (attachments.Attachments.Any())
                {
                    foreach (Attachment attachment in attachments.Attachments.Select(e => e))
                    {
                        message.Attachments.Add(attachment);
                    }
                }
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        /// <summary>
        ///     Used to send email notifications
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachment to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        private void SendMoreEmails(List<string> recipients, MailMessage attachments, string subject, string body)
        {
            try
            {
                var message = new MailMessage();

                int noOfRecepients = 1;
                foreach (string recipient in recipients.Where(recipient => recipient.Trim() != ""))
                {
                    if (noOfRecepients == 1)
                    {
                        message.To.Add(recipient);
                    }
                    else
                    {
                        message.CC.Add(recipient);
                    }
                    noOfRecepients++;
                }

                message.IsBodyHtml = true;
                message.Body = body;
                message.Subject = subject;
                var smtpClient = new SmtpClient();

                if (attachments.Attachments.Any())

                    foreach (Attachment attachment in attachments.Attachments.Select(e => e))
                    {
                        message.Attachments.Add(attachment);
                    }
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogException(ex);
            }
        }

        #endregion

        #region Helpers

        internal bool VerifyAuthentication()
        {
            if (User == null)
            {
                DependencyResolver.Current.GetService<IUserAccountValidation>().LogOf();
                return false;
            }
            return true;
        }

        #endregion

        #region Cit Email Runner



        /// <summary>
        ///     Check if there is Notifications from Cit messages from devices.
        /// </summary>
        public bool HasNotification()
        {
            using (var connection = new SqlConnection(MvcApplication.ConnectionString))
            {
                connection.Open();

                const string query = @"
                    SELECT [CitRequestDetailId]
                          ,[CashDepositId]
                          ,[BeneficiaryCode]
                          ,[CitCode]
                          ,[DeviceSerialNumber]
                          ,[BagSerialNumber]
                          ,[TransactionDate]
                          ,[ItramsReference]
                          ,[UserReferance]
                          ,[IsReceiptPrinted]
                          ,[IsNotDeleted]
                      FROM [dbo].[CitRequestDetail] WHERE IsReceiptPrinted = 0 AND IsNotDeleted = 1";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Notification = null;

                    var dependency = new SqlDependency(command);
                    dependency.OnChange += OnChangeNotifier;

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    var reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// <summary>
        ///     SQL Notification handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnChangeNotifier(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                GenerateReceipts();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void GenerateReceipts()
        {
            var isSendingEmails = Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsSendingEmails"]);

            if (isSendingEmails == false)
            {
                var requests = _repository.Query<CitRequestDetail>(e => e.IsReceiptPrinted == false);

                foreach (CitRequestDetail request in requests)
                {
                    try
                    {
                        byte[] attachmentBytes = GeneratePdfDocument(request.CashDepositId);

                        var ms = new MemoryStream(attachmentBytes);
                        var attachement = new Attachment(ms, "CIT Settlement Receipt.pdf");

                        SendEmailAttachment(request, attachement);
                        UpdateRequest(request.CashDepositId);
                    }
                    catch (Exception ex)
                    {
                        ;
                    }
                }
                System.Web.HttpContext.Current.Application["IsSendingEmails"] = false;
            }
        }


        /// <summary>
        ///     update cit  to printed
        /// </summary>
        /// <param name="cashDepositId"></param>
        private void UpdateRequest(long cashDepositId)
        {
            CitRequestDetail request =
                _repository.Query<CitRequestDetail>(e => e.CashDepositId == cashDepositId).FirstOrDefault();
            if (request != null)
            {
                request.IsReceiptPrinted = true;
                request.EntityState = State.Modified;
                request.LastChangedDate = DateTime.Now;
                _repository.Update(request);
            }
        }




        /// <summary>
        /// </summary>
        /// <param name="cashDepositId"></param>
        private byte[] GeneratePdfDocument(long cashDepositId)
        {
            const string reportPath = "/Payments/VaultPaymentSlip";

            var paramz = new Dictionary<string, string>();
            paramz.Add("VaultPartialPaymentId", cashDepositId.ToString());
            paramz.Add("IsPayment", "false");

            byte[] result = GenerateReport(reportPath, paramz);
            return result;
        }




        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <param name="attachement"></param>
        private void SendEmailAttachment(CitRequestDetail request, Attachment attachement)
        {
            var message = new MailMessage();
            message.Attachments.Add(attachement);

            string recipient = GetSiteContactPersonEmail(request.CitCode);

            //NOTE:     Email is sent to site contact.
            if (!string.IsNullOrWhiteSpace(recipient))
            {
                this.Log().Info(string.Format("Sending Email to => {0}", recipient));

                string beneficiaryEmailBody = CreateReceiptBody(request);

                Task.Factory.StartNew(
                    () =>
                        SendEmailNotification(recipient, message, "Vault CIT Settlement Request",
                            beneficiaryEmailBody));
            }
        }


        /// <summary>
        ///     Create email body
        /// </summary>
        /// <param name="request"></param>
        private string CreateReceiptBody(CitRequestDetail request)
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            string siteName = GetSiteName(request.CitCode);
            
            html.RenderBeginTag(HtmlTextWriterTag.H3);
            html.WriteEncodedText(string.Format("Dear {0}", siteName));
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.Hr);
            html.RenderEndTag();
            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("Please find attached the CIT settlement receipt.");

            html.WriteBreak();
            html.WriteEncodedText("A CIT service was performed and there was a balance on your mySBV.vault device cash wallet");

            html.WriteBreak();
            html.WriteEncodedText("that will be settled to your default bank account.");

            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText("Best Regards,");
            html.WriteBreak();
            html.WriteEncodedText("SBV Services (Pty) Ltd");
            html.RenderEndTag();

            html.Flush();
            return writer.ToString();
        }


        /// <summary>
        ///     Gets Site name.
        /// </summary>
        /// <param name="citCode"></param>
        public string GetSiteName(string citCode)
        {
            Site site =
                _repository.Query<Site>(
                    e => e.CitCode == citCode).FirstOrDefault();
            return site != null ? site.Name : string.Empty;
        }


        /// <summary>
        ///     Get Site Contact Person Email Address
        /// </summary>
        /// <param name="citCode"></param>
        public string GetSiteContactPersonEmail(string citCode)
        {
            IEnumerable<Site> contactPersonEmail =
                _repository.Query<Site>(a => a.CitCode == citCode);

            Site email = contactPersonEmail.FirstOrDefault();
            return email != null ? email.ContactPersonEmailAddress1 : string.Empty;
        }


        /// <summary>
        ///     Gets a Beneficiary name
        /// </summary>
        /// <param name="beneficiaryCode"></param>
        public string GetBeneficiary(string beneficiaryCode)
        {
            Account account =
                _repository.Query<Account>(
                    e => e.BeneficiaryCode == beneficiaryCode).FirstOrDefault();

            return account != null ? account.AccountHolderName : string.Empty;
        }

        #endregion
    }
}