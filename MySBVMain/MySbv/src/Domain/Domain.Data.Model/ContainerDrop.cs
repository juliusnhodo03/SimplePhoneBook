using System;
using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class ContainerDrop : EntityBase
    {
        #region Mapped

        public int ContainerDropId { get; set; }
        public int ContainerId { get; set; }
        public int? DiscrepancyReasonId { get; set; }
        public int StatusId { get; set; }
        public string Narrative { get; set; }
        public string ReferenceNumber { get; set; }
        public string BagSerialNumber { get; set; }
		public decimal Amount { get; set; }
		public bool HasDiscrepancy { get; set; }
        public decimal DiscrepancyAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public string Comment { get; set; }
        public int Number { get; set; }
        public int? ErrorCodeId { get; set; }
        public string SettlementIdentifier { get; set; }
        public string DuplicateChecksum { get; set; }
		public DateTime? SettlementDateTime { get; set; }
		public DateTime? SendDateTime { get; set; }
		public string TransactionDateTime { get; set; } 
        public virtual Container Container { get; set; }
        public virtual DiscrepancyReason DiscrepancyReason { get; set; }
        public virtual Status Status { get; set; }
        public virtual ErrorCode ErrorCode { get; set; }
        public virtual Collection<ContainerDropItem> ContainerDropItems { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContainerDropId; }
        }

        public string DiscrepancyReasonName
        {
            get { return DiscrepancyReason != null ? DiscrepancyReason.Name : string.Empty; }
        }

        public string Name
        {
            get { return Status != null ? Status.LookUpKey : string.Empty; }
        }

        public string ErrorCodeName
        {
            get { return ErrorCode != null ? ErrorCode.Description : string.Empty; }
        }

        #endregion
    }
}