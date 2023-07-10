
using Domain.Data.Core;

namespace Domain.Data.Hyphen.Model
{
    public class TransactionDetailRecordResponse : EntityBase
    {
        public int TransactionDetailRecordResponseId { get; set; }
        public string SettlementIdentifier { get; set; }
        public string AccountType { get; set; }
        public string AgencyNumber { get; set; }
        public string AgencyPrefix { get; set; }
        public string BankAccountNumber { get; set; }
        public string BatchNumber { get; set; }
        public string Blank2 { get; set; }
        public string BranchCode { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string ErrorCode { get; set; }
        public string MessageType { get; set; }
        public string Payee { get; set; }
        public string ProcessingOption1 { get; set; }
        public string ProcessingOption2 { get; set; }
        public string RequisitionNumber { get; set; }
        public string TransactionAmount { get; set; }
        public string TransactionType { get; set; }
        public string UserReference1 { get; set; }
        public string UserReference2 { get; set; }
        public string ActionDate { get; set; }
        public string CashBookBankAccountNumber { get; set; }
        public string ChequeClearanceCode { get; set; }
        public string ClientChequeNumber { get; set; }
        public string Code1 { get; set; }
        public string Code2 { get; set; }
        public string HashTotal { get; set; }
        public string INDF { get; set; }
        public string ProgramNameCreated { get; set; }
        public string ThirdParty { get; set; }
        public string UniqueUserCode { get; set; }

        public override int Key
        {
            get { return TransactionDetailRecordResponseId; }
        }
    }
}
