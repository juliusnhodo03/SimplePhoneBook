using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CashOrderContainerDropItem : EntityBase
    {
        #region Mapped

        public int CashOrderContainerDropItemId { get; set; }
        public int CashOrderContainerDropId { get; set; }
        public int DenominationId { get; set; }
        public int ValueInCents { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
        public int? VerifiedCount { get; set; }
        public decimal? VerifiedValue { get; set; }
        public int? PackedCount { get; set; }
        public decimal? PackedValue { get; set; }
        public string DenominationType { get; set; }
        public virtual CashOrderContainerDrop CashOrderContainerDrop { get; set; }
        public virtual Denomination Denomination { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CashOrderContainerDropItemId; }
        }

        #endregion
        
    }
}