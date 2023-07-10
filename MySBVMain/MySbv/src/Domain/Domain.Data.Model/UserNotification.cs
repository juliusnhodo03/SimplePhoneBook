using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class UserNotification : EntityBase
    {
        #region Mapped

        public int UserNotificationId { get; set; }
        public int UserId { get; set; }
        public int NotificationTypeId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }

        [ForeignKey("NotificationTypeId")]
        public NotificationType NotificationType { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return UserNotificationId; }
        }

        public string NotificationName
        {
            get { return (NotificationType != null) ? NotificationType.Name : string.Empty; }
        }

        public string UserDetails
        {
            get { return (User != null) ? User.FirstName + " " + User.LastName : string.Empty; }
        }

        #endregion
    }
}