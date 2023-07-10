using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Account : EntityBase
    {
        public int AccountId { get; set; }
        public int SiteId { get; set; }
        public int AccountTypeId { get; set; }
        public int BankId { get; set; }
        public int TransactionTypeId { get; set; }
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public string IfscCode { get; set; }
        public string SwiftCode { get; set; }
        public string BeneficiaryCode { get; set; }
        public bool DefaultAccount { get; set; }
        public bool ToBeDeleted { get; set; }
        public int StatusId { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
        public virtual Site Site { get; set; }
        public virtual Status Status { get; set; }
        public virtual AccountType AccountType { get; set; }
        public virtual Bank Bank { get; set; }
        public virtual TransactionType TransactionType { get; set; }

        public override int Key
        {
            get { return AccountId; }
        }

        public string BranchCode
        {
            get { return Bank != null ? Bank.BranchCode : string.Empty; }
        }

    }
}