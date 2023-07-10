using System;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CashOrderTask : EntityBase
    {
        #region Mapped

        public int CashOrderTaskId { get; set; }
        public int CashOrderId { get; set; }
        public string ReferenceNumber { get; set; }
        public DateTime Date { get; set; }
        public int SiteId { get; set; }
        public int UserId { get; set; }
        public int StatusId { get; set; }
        public string RequestUrl { get; set; }
        public virtual Site Site { get; set; }
        public virtual User User { get; set; }
        public virtual Status Status { get; set; }

        public override int Key
        {
            get { return CashOrderTaskId; }
        }

        #endregion

    }
}
