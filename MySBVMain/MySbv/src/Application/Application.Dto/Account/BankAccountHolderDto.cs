using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Account
{
    public class BankAccountHolderDto
    {
        public BankAccountHolderDto()
        {
            Accounts = new List<AccountDto>();    
        }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select the Site Name")]
        [Display(Name = "Site Name")]
        public int SiteId { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [Display(Name = "Process Type")]
        public string ProcessType { get; set; }

        public List<AccountDto> Accounts { get; set; }
    }
}