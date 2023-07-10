using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Account
{
    public class ViewAccountDto
    {
        [ScaffoldColumn(false)]
        public int AccountId { get; set; }

        [Display(Name = "Site Name")]
        public int SiteId { get; set; }

        [ScaffoldColumn(false)]
        public int AccountTypeId { get; set; }

        [Display(Name = "Bank Name")]
        public int BankId { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [ScaffoldColumn(false)]
        public int TransactionTypeId { get; set; }

        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Beneficiary Code")]
        public string BeneficiaryCode { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Ifsc Code")]
        public string IfscCode { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        [Display(Name = "Default Account")]
        public string DefaultAccount { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
    }
}