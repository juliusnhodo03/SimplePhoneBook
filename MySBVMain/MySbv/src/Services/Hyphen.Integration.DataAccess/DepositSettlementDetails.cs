using System;

namespace Hyphen.Integration.DataAccess
{
    public class DepositSettlementDetails
    {
        public string ReferenceNumber { get; set; }
        public bool IsSettled { get; set; }
        public string ErrorCode { get; set; }
        public DateTime ResponseProcessDateTime { get; set; }
        public double DepositAmount { get; set; }
        public string AccountNumber { get; set; }
    }
}