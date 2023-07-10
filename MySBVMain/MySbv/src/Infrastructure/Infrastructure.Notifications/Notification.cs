using System;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.Net.Mail;
using System.Reflection;
using Domain.Notifications;

namespace Infrastructure.Notifications
{
    [Export(typeof (INotification))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Notification : INotification
    {
        #region INotification

        public void SendEmail(string subject, EmailTemplate template, NameValueCollection parameters, string path,
            params string[] recipients)
        {
            string _message = "";
            string _subject = "";

            switch (template)
            {
                case EmailTemplate.RejectedDeposits:
                    _message = GetEmailBody(path);
                    _subject = subject;
                    break;
            }

            if (parameters != null && parameters.Count > 0)
            {
                for (int i = 0; i < parameters.Count; i++)
                {
                    _message = _message.Replace("##" + parameters.GetKey(i) + "##", parameters.Get(i));
                }
            }

            string _recepients = string.Empty;

            foreach (string recipient in recipients)
            {
                if (string.IsNullOrEmpty(_recepients)) _recepients += recipient;
                else
                    _recepients += string.Concat(", ", recipient);
            }
            SendEmail(_recepients, _message, _subject);
        }

        public void SendFax()
        {
            throw new NotImplementedException();
        }

        public void SendSms()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Helpers

        private static string GetEmailBody(string filename)
        {
            string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string fullPath = Path.Combine(assemblyPath, filename);
            string fileContent = File.ReadAllText(fullPath);
            return fileContent;
        }

        private static void SendEmail(string recipients, string body, string subject)
        {
            try
            {
                var message = new MailMessage();
                message.To.Add(recipients);
                message.IsBodyHtml = true;
                message.Body = body;
                message.Subject = subject;
                var smtpClient = new SmtpClient();
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}