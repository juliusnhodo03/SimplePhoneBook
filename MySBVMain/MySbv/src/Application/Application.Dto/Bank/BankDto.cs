using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Bank
{
    public class BankDto
    {
        [ScaffoldColumn(false)]
        public int BankId { get; set; }

        [Required(ErrorMessage = "Bank Name is missing")]
        [Display(Name = "Bank Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Branch Code is missing")]
        [RegularExpression(@"(^|\D)(\d{6})($|\D)", ErrorMessage = "Branch Code must be exactly Six Digits Long")]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        
    }
}