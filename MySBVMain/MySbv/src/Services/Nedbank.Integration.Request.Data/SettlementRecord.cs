namespace Nedbank.Integration.Request.Data
{
    public class SettlementRecord
    {
        public string RecordIdentifier { get; set; }

        public string NominatedAccountNumber { get; set; }

        public string PaymentReferenceNumber { get; set; }

        public string DestinationBranchCode { get; set; }

        public string DestinationAccountNumber { get; set; }

        public string Amount { get; set; }

        public string ActionDate { get; set; }

        public string Reference { get; set; }

        public string DestinationAccountHoldersName { get; set; }

        public string TransactionType { get; set; }

        public string ClientType { get; set; }

        public string ChargesAccountNumber { get; set; }

        public string ServiceType { get; set; }

        public string OriginalPaymentReferenceNumber { get; set; }

        public string EntryClass { get; set; }

        public string NominatedAccountReference { get; set; }

        public string BDFIndicator { get; set; }

        public string Filler { get; set; }
    }
}