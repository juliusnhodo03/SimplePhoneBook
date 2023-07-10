namespace Hyphen.Integration.DataAccess
{
    public class SettlementFile
    {
        public int TransactionId { get; set; }

        public string Payee { get; set; }

        public string Bank { get; set; }

        public string BankAccountNumber { get; set; }

        public string AccountType { get; set; }

        public string BranchCode { get; set; }

        public string TransactionType { get; set; }

        public string TransactionAmount { get; set; }

        public string ClientReference { get; set; }

        public string SbvDepositReference { get; set; }
    }
}