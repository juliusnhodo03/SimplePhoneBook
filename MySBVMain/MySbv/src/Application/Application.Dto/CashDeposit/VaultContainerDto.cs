using System;
using System.Collections.Generic;

namespace Application.Dto.CashDeposit
{
    public class VaultContainerDto
    {
        public int VaultContainerId { get; set; }
        public int ContainerId { get; set; }
        public int CashDepositId { get; set; }
        public int DeviceId { get; set; }
        public int SiteId { get; set; }
        public int SupervisorId { get; set; }
        public int ProductTypeId { get; set; }
        public string ProductTypeName { get; set; }
        public string ContractNumber { get; set; }
        public string CitCode { get; set; }
        public string TransactionNumber { get; set; }
        public string MerchantName { get; set; }
        public string DepositReference { get; set; }
        public int? UserTypeId { get; set; }
        public int? DiscrepancyReasonId { get; set; }
        public string Comment { get; set; }
        public decimal Amount { get; set; }
        public decimal? ActualAmount { get; set; }
        public decimal? DiscrepancyAmount { get; set; }
        public bool HasDiscrepancy { get; set; }
        public string SerialNumber { get; set; }
        public string DeviceName { get; set; }
        public int? ProcessedById { get; set; }
        public DateTime? ProcessedDateTime { get; set; }

        public IEnumerable<VaultContainerDropDto> VaultContainerDrops { get; set; }
    }
}