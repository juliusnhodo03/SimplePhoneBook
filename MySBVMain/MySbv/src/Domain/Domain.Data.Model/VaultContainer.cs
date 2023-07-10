using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class VaultContainer : EntityBase
    {
        public int VaultContainerId { get; set; }
        public int ContainerId { get; set; }
        public int CashDepositId { get; set; }
        public string SettlementIdentifier { get; set; }    
        public int SiteId { get; set; }
        public int SupervisorId { get; set; }
        public int DeviceId { get; set; }
        public int? DiscrepancyReasonId { get; set; }
        public string CitCode { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public decimal? ActualAmount { get; set; }
        public decimal? DiscrepancyAmount { get; set; }
        public bool HasDiscrepancy { get; set; }
        public int? ProcessedById { get; set; }
        public DateTime? ProcessedDateTime { get; set; }

        [StringLength(18)]
        public string SerialNumber { get; set; }

        public Device Device { get; set; }

        public ICollection<VaultContainerDrop> VaultContainerDrops { get; set; }

        public override int Key
        {
            get { return VaultContainerId; }
        }
    }
}