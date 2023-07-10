using System.Collections.Generic;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CashOrderContainerDrop : EntityBase
    {
        #region Mapped

        public int CashOrderContainerDropId { get; set; }
        public int CashOrderContainerId { get; set; }
        public decimal Amount { get; set; }
        public decimal? VerifiedAmount { get; set; }
        public decimal? PackedAmount { get; set; }
        public bool IsCashRequiredInExchange { get; set; }
        public bool IsCashForwardedForExchange { get; set; }
        public virtual CashOrderContainer CashOrderContainer { get; set; }
        public virtual List<CashOrderContainerDropItem> CashOrderContainerDropItems { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CashOrderContainerDropId; }
        }

        #endregion
    }
}