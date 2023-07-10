using System;

namespace Vault.Integration.DataContracts
{
    public class SettlementResponse : IMessageLabel
    {
        public string ReferenceNumber { get; set; }
        public string iTramsReference { get; set; }
        public string DeviceSerial { get; set; }
        public bool IsSettled { get; set; }
        public string ErrorCode { get; set; }
        public double DepositAmount { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolder { get; set; }
        public DateTime ResponseProcessDateTime { get; set; }
        public string Label { get; set; }
        public string MessageId { get; set; }
    }
}