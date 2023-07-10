using System;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class ContainerDropItem : EntityBase
    {
        #region Mapped

        public int ContainerDropItemId { get; set; }
        public int ContainerDropId { get; set; }
        public int DenominationId { get; set; }
        public int ValueInCents { get; set; }
        public int Count { get; set; }
        public decimal Value { get; set; }
        public int? DiscrepancyCount { get; set; }
        public decimal? DiscrepancyValue { get; set; }
        public int? ActualCount { get; set; }
		public decimal? ActualValue { get; set; }
		public string DenominationType { get; set; }
		public string DenominationName { get; set; }
        public virtual ContainerDrop ContainerDrop { get; set; }
        public virtual Denomination Denomination { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContainerDropItemId; }
        }

        #endregion
    }
}