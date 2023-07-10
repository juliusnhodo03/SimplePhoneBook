using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Container : EntityBase
    {
        #region Mapped

        public int ContainerId { get; set; }
        public int CashDepositId { get; set; }
        public int ContainerTypeId { get; set; } 
        public string ReferenceNumber { get; set; }
		public string SerialNumber { get; set; }
		public string SealNumber { get; set; } 
        public decimal Amount { get; set; }
        public decimal? DiscrepancyAmount { get; set; }
        public decimal? ActualAmount { get; set; }
        public bool IsPrimaryContainer { get; set; }
        public virtual CashDeposit CashDeposit { get; set; }
        public virtual ContainerType ContainerType { get; set; }
        public virtual Collection<ContainerDrop> ContainerDrops { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContainerId; }
        }

        public string ContainerTypeName
        {
            get { return ContainerType != null ? ContainerType.Name : string.Empty; }
        }

        #endregion
    }
}