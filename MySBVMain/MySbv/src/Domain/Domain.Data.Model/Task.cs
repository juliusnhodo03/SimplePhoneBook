using System;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Task : EntityBase
    {
        #region Mapped

        public int TaskId { get; set; }
        public string Title { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime Date { get; set; }
        public int? ApprovalObjectsId { get; set; }
        public int MerchantId { get; set; }
        public int SiteId { get; set; }
        public int AccountId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public string Module { get; set; }
        public string Link { get; set; }
        public bool IsExecuted { get; set; }
        public virtual Account Account { get; set; }
        public virtual Merchant Merchant { get; set; }
        public virtual Site Site { get; set; }
        public virtual User User { get; set; }
        public virtual Status Status { get; set; }
        public virtual ApprovalObjects ApprovalObjects { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return TaskId; }
        }

        public string SiteName
        {
            get { return Site != null ? Site.Name : string.Empty; }
        }

        public string CitCode
        {
            get { return Site != null ? Site.CitCode : string.Empty; }
        }

        public string MerchantName
        {
            get { return Merchant != null ? Merchant.Name : string.Empty; }
        }

        public string UserName
        {
            get { return User != null ? User.FirstName : string.Empty; }
        }

        public string StatusName
        {
            get { return Status != null ? Status.Name : string.Empty; }
        }

        #endregion
    }
}
