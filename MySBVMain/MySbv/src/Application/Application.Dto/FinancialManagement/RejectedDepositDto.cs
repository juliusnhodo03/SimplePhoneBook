using System.ComponentModel.DataAnnotations;

namespace Application.Dto.FinancialManagement
{
    public class RejectedDepositDto
    {
        [Display(Name = "Deposit DateTime")]
        public string DepositDateTime { get; set; }

        [Display(Name = "Rejection DateTime")]
        public string RejectionDateTime { get; set; }

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "CIT Code")]
        public string CitCode { get; set; }

        [Display(Name = "Deposit Type")]
        public string DepositType { get; set; }

        [Display(Name = "Bank Acc No")]
        public string AccountNumber { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Reference")]
        public string DepositReference { get; set; }

        [Display(Name = "Deposit Amount")]
        public string DepositAmount { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Client Reference")]
        public string Narrative { get; set; }

        [Display(Name = "Transaction Reference")]
        public string SbvReference { get; set; }

        [Display(Name = "Error Code")]
        public string ErrorCode { get; set; }
        public int PaymentId { get; set; }
        public int CashDepositId { get; set; }
        public int ContainerDropId { get; set; }
        public bool IsPayment { get; set; } 
        public bool IsCashDeposit { get; set; }
        public bool IsContainerDrop { get; set; }

        [Display(Name = "Rejection Reason")]
        public string Reason { get; set; }

        [Display(Name = "Rejection Reason")]
        public string RejectionReason { get; set; }
    }
}