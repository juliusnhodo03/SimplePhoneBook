using System;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class VaultPartialPayment : EntityBase
    {
        public int VaultPartialPaymentId { get; set; }
        public int StatusId { get; set; }
        public int? ErrorCodeId { get; set; }
        public string PaymentReference { get; set; }
        public string DeviceSerialNumber { get; set; }
        public string BagSerialNumber { get; set; }
        public string CitCode { get; set; }
        public string BeneficiaryCode { get; set; }
        public decimal TotalToBePaid { get; set; }
        public string DeviceUserName { get; set; }
        public string DeviceUserRole { get; set; }
        public string SettlementIdentifier { get; set; }
        public Status Status { get; set; }
        public ErrorCode ErrorCode { get; set; }
        public DateTime? SendDateTime { get; set; }
        public DateTime? SettlementDate { get; set; }
        public DateTime? TransactionDate { get; set; }

        public override int Key
        {
            get { return VaultPartialPaymentId; }
        }

    }
}