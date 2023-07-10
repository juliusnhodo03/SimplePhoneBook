using System.Collections.Generic;
using System.Net.Mail;

namespace Vault.Integration.WebService
{
    public interface IEmailManager
    {
        /// <summary>
        ///     Used to fire and forget the sending of email on a separate thread.
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachments to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        void SendEmail(List<string> recipients, MailMessage attachments, string subject, string body);
    }
}