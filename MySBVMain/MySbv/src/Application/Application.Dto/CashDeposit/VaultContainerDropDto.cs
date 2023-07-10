using System;

namespace Application.Dto.CashDeposit
{
    public class VaultContainerDropDto
    {
        public int VaultContainerDropId { get; set; }
        public int VaultContainerId { get; set; }
        public int DenominationId { get; set; }
        public int ValueInCents { get; set; }
        public int? Count { get; set; }
        public decimal Value { get; set; }
        public int? DiscrepancyCount { get; set; }
        public decimal? DiscrepancyValue { get; set; }
        public bool HasDiscrepancy { get; set; }
        public int? ActualCount { get; set; }
        public decimal? ActualValue { get; set; }
        public string DenominationType { get; set; }
    }
}