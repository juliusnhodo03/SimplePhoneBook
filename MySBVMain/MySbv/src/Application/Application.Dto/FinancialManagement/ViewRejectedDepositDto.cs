using System.ComponentModel.DataAnnotations;

namespace Application.Dto.FinancialManagement
{
    public class ViewRejectedDepositDto
    {
        public int CashDepositId { get; set; }

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "CIT Code")]
        public string CitCode { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Account Type")]
        public string AccountType { get; set; }

        [Display(Name = "Client Reference")]
        public string Narrative { get; set; }

        [Display(Name = "Deposited Amount")]
        public string DepositAmount { get; set; }

        [Display(Name = "Deposit Type")]
        public string DepositType { get; set; }

        [Display(Name = "Transaction Reference")]
        public string SbvReference { get; set; }

        [Display(Name = "Rejection Date Time")]
        public string RejectionDateTime { get; set; }

        [Display(Name = "Error Code")]
        public string ErrorCode { get; set; }

        [Display(Name = "Error Description")]
        public string ErrorDescription { get; set; }
    }
}