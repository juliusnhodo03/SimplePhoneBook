using System.Collections.Specialized;

namespace Domain.Notifications
{
    public enum EmailTemplate
    {
        RejectedDeposits
    }

    public interface INotification
    {
        void SendEmail(string subject, EmailTemplate template, NameValueCollection parameters, string path,
            params string[] recipients);

        void SendFax();
        void SendSms();
    }
}