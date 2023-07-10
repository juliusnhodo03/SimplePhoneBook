using System.Collections.ObjectModel;
using System.ComponentModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class User : EntityBase
    {
        #region Constructor

        public User()
        {
            UserNotifications = new Collection<UserNotification>();
        }

        #endregion

        #region Mapped

        public int UserId { get; set; }
        public int? TitleId { get; set; }
        public int? CashCenterId { get; set; }
        public int? UserTypeId { get; set; }
        public int? MerchantId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdNumber { get; set; }
        public string PassportNumber { get; set; }
        public string EmailAddress { get; set; }
        public string CellNumber { get; set; }
        public string OfficeNumber { get; set; }
        public string FaxNumber { get; set; }
        public bool LockedStatus { get; set; }

        [DefaultValue(false)]
        public bool CanMakeVaultPayment { get; set; }

        public virtual UserType UserType { get; set; }
        public virtual CashCenter CashCenter { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual Title Title { get; set; }
        public virtual Collection<UserSite> UserSites { get; set; }
        public virtual Collection<UserNotification> UserNotifications { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return UserId; }
        }

        public string UserTypeDescription
        {
            get { return UserType != null ? UserType.Description : string.Empty; }
        }

        public string CashCenterName
        {
            get { return CashCenter != null ? CashCenter.Name : string.Empty; }
        }

        //public string Name
        //{
        //    get { return Merchant != null ? Merchant.Name : string.Empty; }
        //}

        public string TitleDescrition
        {
            get { return Title != null ? Title.Name : string.Empty; }
        }

        #endregion
    }
}