namespace Application.Dto.VaultPayment
{
    public class VaultPaymentDto
    {
        // Beneficiary Details
        public string AccountId { get; set; }
        public int DeviceId { get; set; }
        public int MerchantId { get; set; }
        public int SiteId { get; set; }
        public string SiteName { get; set; }

        public string PaymentReference { get; set; }
        public string BeneficiaryEmailAddress { get; set; }
        public string BeneficiaryName { get; set; }
        public string BankName { get; set; }
        public string BeneficiaryCode { get; set; }
        public bool IsDefaultReference { get; set; }

        public decimal AvailableFunds { get; set; }

        public string DeviceSerialNumber { get; set; }
        public string BagSerialNumber { get; set; }
        public string CitCode { get; set; }
        public decimal AmountToBePaid { get; set; }

        public string TransactionDate { get; set; }
    }
}