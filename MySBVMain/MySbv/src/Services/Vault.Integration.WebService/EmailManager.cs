using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.UI;
using Domain.Repository;
using Infrastructure.Logging;
using Ninject;

namespace Vault.Integration.WebService
{
    public class EmailManager : IEmailManager
    {
        /// <summary>
        /// 
        /// </summary>
        private IRepository _repository;

        /// <summary>
        /// 
        /// </summary>
        public EmailManager()
        {
            this.Log().Debug(() =>
            {
                IKernel kernel = new StandardKernel(new Bindings());
                _repository = kernel.Get<IRepository>();
            }, "");
        }

        /// <summary>
        ///     Used to fire and forget the sending of email on a separate thread.
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachments to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        public void SendEmail(List<string> recipients, MailMessage attachments, string subject, string body) 
        {
            Task.Factory.StartNew(() => SendEmails(recipients, attachments, subject, body));
        }



        /// <summary>
        ///     Used to send email notifications
        /// </summary>
        /// <param name="recipients">String representing the recipients separated by ';'.</param>
        /// <param name="attachments">A byte array representing the attachment to be sent.</param>
        /// <param name="subject">Subject for the email.</param>
        /// <param name="body">HTML representing the body of the email.</param>
        private void SendEmails(IEnumerable<string> recipients, MailMessage attachments, string subject, string body)
        {
            try
            {
                var message = new MailMessage();

                foreach (string recipient in recipients.Where(recipient => recipient.Trim() != ""))
                {
                    message.To.Add(recipient);
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
            }
        }
        
    }
}
