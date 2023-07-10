using System.Collections.Generic;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CashOrderContainer : EntityBase
    {
        #region Mapped

        public int CashOrderContainerId { get; set; }
        public string SerialNumber { get; set; }
        public decimal Amount { get; set; }
        public decimal VerifiedAmount { get; set; }
        public decimal PackedAmount { get; set; }
        public virtual List<CashOrderContainerDrop> CashOrderContainerDrops { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CashOrderContainerId; }
        }

        #endregion
    }
}