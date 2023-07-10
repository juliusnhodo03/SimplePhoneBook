using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Account
{
    public class ListAccountDto
    {
        [ScaffoldColumn(false)]
        public int AccountId { get; set; }

        [ScaffoldColumn(false)]
        public int SiteId { get; set; }

        [ScaffoldColumn(false)]
        public int AccountTypeId { get; set; }

        [ScaffoldColumn(false)]
        public int BankId { get; set; }

        [ScaffoldColumn(false)]
        public int TransactionTypeId { get; set; }

        [Required(ErrorMessage = "Account Number is missing")]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Account Holder Name is missing")]
        [Display(Name = "Account Holder Name")]
        public string AccountHolderName { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Beneficiary Code")]
        public string BeneficiaryCode { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Ifsc Code")]
        public string IfscCode { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        [Display(Name = "Default Account")]
        public bool DefaultAccount { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public int StatusId { get; set; }
    }
}