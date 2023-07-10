using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Task = System.Threading.Tasks.Task;

namespace Mysbv.Web.Hubs
{
    public class TriggeredEmailNotification
    {
        //private readonly IRepository _repository;

        //public TriggeredEmailNotification(IRepository repository)
        //{
        //    _repository = repository;
        //}

        ///// <summary>
        /////     Check if there is Notifications from Cit messages from devices.
        ///// </summary>
        //public bool HasNotification()
        //{
        //    using (var connection = new SqlConnection(MvcApplication.ConnectionString))
        //    {
        //        connection.Open();

        //        const string query = "SELECT * FROM [dbo].[CitRequestDetail] WHERE IsReceiptPrinted = 0";

        //        using (var command = new SqlCommand(query, connection))
        //        {
        //            command.Notification = null;

        //            var dependency = new SqlDependency(command);
        //            dependency.OnChange += OnChangeNotifier;

        //            if (connection.State == ConnectionState.Closed)
        //            {
        //                connection.Open();
        //            }

        //            var reader = command.ExecuteReader();

        //            if (reader.Read())
        //            {
        //                return true;
        //            }
        //        }
        //    }
        //    return false;
        //}


        ///// <summary>
        /////     SQL Notification handler
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void OnChangeNotifier(object sender, SqlNotificationEventArgs e)
        //{
        //    if (e.Type == SqlNotificationType.Change)
        //    {

        //    }
        //}


        //#region Cit Email Runner

        ///// <summary>
        ///// 
        ///// </summary>
        //private void GenerateReceipts()
        //{
        //    var isSendingEmails = Convert.ToBoolean(System.Web.HttpContext.Current.Application["IsSendingEmails"]);

        //    if (isSendingEmails == false)
        //    {
        //        var requests = _repository.Query<CitRequestDetail>(e => e.IsReceiptPrinted == false);

        //        foreach (CitRequestDetail request in requests)
        //        {
        //            try
        //            {
        //                byte[] attachmentBytes = GeneratePdfDocument(request.CashDepositId);

        //                var ms = new MemoryStream(attachmentBytes);
        //                var attachement = new Attachment(ms, "CIT Settlement Receipt.pdf");

        //                SendEmailAttachment(request, attachement);
        //                UpdateRequest(request.CashDepositId);
        //            }
        //            catch (Exception ex)
        //            {
        //                ;
        //            }
        //        }
        //        System.Web.HttpContext.Current.Application["IsSendingEmails"] = false;
        //    }
        //}


        ///// <summary>
        /////     update cit  to printed
        ///// </summary>
        ///// <param name="cashDepositId"></param>
        //private void UpdateRequest(long cashDepositId)
        //{
        //    CitRequestDetail request =
        //        _repository.Query<CitRequestDetail>(e => e.CashDepositId == cashDepositId).FirstOrDefault();
        //    if (request != null)
        //    {
        //        request.IsReceiptPrinted = true;
        //        request.EntityState = State.Modified;
        //        request.LastChangedDate = DateTime.Now;
        //        _repository.Update(request);
        //    }
        //}




        ///// <summary>
        ///// </summary>
        ///// <param name="cashDepositId"></param>
        //private byte[] GeneratePdfDocument(long cashDepositId)
        //{
        //    const string reportPath = "/Payments/VaultPaymentSlip";

        //    var paramz = new Dictionary<string, string>();
        //    paramz.Add("VaultPartialPaymentId", cashDepositId.ToString());
        //    paramz.Add("IsPayment", "false");

        //    byte[] result = GenerateReport(reportPath, paramz);
        //    return result;
        //}




        ///// <summary>
        ///// </summary>
        ///// <param name="request"></param>
        ///// <param name="attachement"></param>
        //private void SendEmailAttachment(CitRequestDetail request, Attachment attachement)
        //{
        //    var message = new MailMessage();
        //    message.Attachments.Add(attachement);

        //    string recipient = GetSiteContactPersonEmail(request.CitCode);

        //    //NOTE:     Email is sent to site contact.
        //    if (!string.IsNullOrWhiteSpace(recipient))
        //    {
        //        this.Log().Info(string.Format("Sending Email to => {0}", recipient));

        //        string beneficiaryEmailBody = CreateReceiptBody(request);

        //        Task.Factory.StartNew(
        //            () =>
        //                SendEmailNotification(recipient, message, "Vault CIT Settlement Request",
        //                    beneficiaryEmailBody));
        //    }
        //}


        ///// <summary>
        /////     Create email body
        ///// </summary>
        ///// <param name="request"></param>
        //private string CreateReceiptBody(CitRequestDetail request)
        //{
        //    var writer = new StringWriter();
        //    var html = new HtmlTextWriter(writer);

        //    string siteName = GetSiteName(request.CitCode);
        //    string salutation = string.Format("Dear {0}", siteName);

        //    string beneficiary = GetBeneficiary(request.BeneficiaryCode);
        //    string pleaseText = string.Format("Please find attached the CIT Settlement Receipt you have just made to {0}",
        //        beneficiary);

        //    this.Log().Info(string.Format("Beneficiary => {0}", beneficiary));

        //    html.RenderBeginTag(HtmlTextWriterTag.H2);
        //    html.WriteEncodedText(salutation);
        //    html.RenderEndTag();
        //    html.RenderBeginTag(HtmlTextWriterTag.Hr);
        //    html.RenderEndTag();
        //    html.RenderBeginTag(HtmlTextWriterTag.P);
        //    html.WriteEncodedText(pleaseText);

        //    html.WriteBreak();
        //    html.WriteEncodedText("Thank you for using mySBV.vault");

        //    html.WriteBreak();
        //    html.WriteEncodedText("Have a nice day further");
        //    html.WriteBreak();
        //    html.WriteEncodedText("Kind Regards,");
        //    html.WriteBreak();
        //    html.WriteEncodedText("SBV");
        //    html.RenderEndTag();

        //    html.Flush();
        //    return writer.ToString();
        //}


        ///// <summary>
        /////     Gets Site name.
        ///// </summary>
        ///// <param name="citCode"></param>
        //public string GetSiteName(string citCode)
        //{
        //    Site site =
        //        _repository.Query<Site>(
        //            e => e.CitCode == citCode).FirstOrDefault();
        //    return site != null ? site.Name : string.Empty;
        //}


        ///// <summary>
        /////     Get Site Contact Person Email Address
        ///// </summary>
        ///// <param name="citCode"></param>
        //public string GetSiteContactPersonEmail(string citCode)
        //{
        //    IEnumerable<Site> contactPersonEmail =
        //        _repository.Query<Site>(a => a.CitCode == citCode);

        //    Site email = contactPersonEmail.FirstOrDefault();
        //    return email != null ? email.ContactPersonEmailAddress1 : string.Empty;
        //}


        ///// <summary>
        /////     Gets a Beneficiary name
        ///// </summary>
        ///// <param name="beneficiaryCode"></param>
        //public string GetBeneficiary(string beneficiaryCode)
        //{
        //    Account account =
        //        _repository.Query<Account>(
        //            e => e.BeneficiaryCode == beneficiaryCode).FirstOrDefault();

        //    return account != null ? account.AccountHolderName : string.Empty;
        //}

        //#endregion
    }
}