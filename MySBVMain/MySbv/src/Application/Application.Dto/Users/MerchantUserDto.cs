using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Dto.ValidationAttributes;

namespace Application.Dto.Users
{
    public class MerchantUserDto
    {
        [Display(Name = "Merchant")]
        [DropDownValidation(ErrorMessage = "User must belong to a Merchant")]
        public int MerchantId { get; set; }

        [Display(Name = "Merchant Name")]
        public string Name { get; set; }

        [Display(Name = "Merchant Number")]
        public string MerchantNumber { get; set; }

        [Display(Name = "Email")]
        public bool IsEmailNotificationType { get; set; }

        [Display(Name = "SMS")]
        public bool IsSmsNotificationType { get; set; }

        [Display(Name = "FAX")]
        public bool IsFaxNotificationType { get; set; }

        [Display(Name = "Make Vault Payment")]
        public bool CanMakeVaultPayment { get; set; }

        public UserDto UserDto { get; set; }

        [Display(Name = "Site")]
        [MultiSelectValidation(ErrorMessage = "User must belong to a Site")]
        public List<int> SiteIds { get; set; }
        
        public List<UserSiteDto> UserSites { get; set; }
    }
}