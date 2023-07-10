using System;

namespace Hyphen.Integration.Response.Data
{
    public class DepositSettlement
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
        public string SettlementIdentifier { get; set; } 
        
    }
}