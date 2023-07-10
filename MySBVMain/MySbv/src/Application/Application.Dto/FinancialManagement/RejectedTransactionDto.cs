
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.FinancialManagement
{
    public class RejectedTransactionDto
    {
        public int? ContainerDropId { get; set; }
        public int? CashDepositId { get; set; }
        public int? PaymentId { get; set; } 

        [Display(Name = "Transaction Date")]
        public string TransactionDate { get; set; }

        [Display(Name = "Rejection Date")]
        public string RejectionDate { get; set; } 

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "CIT Code")]
        public string CitCode { get; set; }

        [Display(Name = "Bank Acc No")]
        public string AccountNumber { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Reference")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Transaction Amount")]
        public decimal TransactionAmount { get; set; }

        [Display(Name = "Rejected By")]
        public string RejectedBy { get; set; }

        [Display(Name = "Rejection Reason")]
        public string RejectionReason { get; set; }

        [Display(Name = "Transaction Type")]
        public string TransactionTypeName { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Client Reference")]
        public string Narrative { get; set; }

        [Display(Name = "Deposit Type")]
        public string DepositType { get; set; }

        [Display(Name = "Transaction Reference")]
        public string SbvReference { get; set; }
    }
}