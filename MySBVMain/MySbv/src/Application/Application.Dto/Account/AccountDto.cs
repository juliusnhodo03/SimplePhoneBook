using System.ComponentModel.DataAnnotations;
using Application.Dto.Bank;
using Application.Dto.Site;

namespace Application.Dto.Account
{
    public class AccountDto
    {
        [ScaffoldColumn(false)]
        public int AccountId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select the Site Name")]
        [Display(Name = "Site Name")]
        public int SiteId { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select the Account Type")]
        [Display(Name = "Account Type")]
        public int AccountTypeId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select the Bank Name")]
        [Required(ErrorMessage = "Bank Name Required")]
        [Display(Name = "Bank Name")]
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

        [Required(ErrorMessage = "Beneficiary Code is missing")]
        [Display(Name = "Beneficiary Code")]
        public string BeneficiaryCode { get; set; }

        public bool ToBeDeleted { get; set; }

        [Display(Name = "Ifsc Code")]
        public string IfscCode { get; set; }

        [Display(Name = "Swift Code")]
        public string SwiftCode { get; set; }

        [Display(Name = "Default Account")]
        public bool DefaultAccount { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Current Comments")]
        public string CurrentComments { get; set; }

        [Display(Name = "Previous Comments")]
        public string PreviousComments { get; set; }

        public bool IsDataCapture { get; set; }

        public int StatusId { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        //public string BeneficiaryEmailAddress { get; set; } 

        public virtual SiteDto Site { get; set; }
        public virtual BankDto Bank { get; set; }
    }
}