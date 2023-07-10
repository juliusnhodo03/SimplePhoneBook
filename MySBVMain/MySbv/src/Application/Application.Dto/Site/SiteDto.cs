using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Account;
using Application.Dto.Address;

namespace Application.Dto.Site
{
    public class SiteDto
    {
        public int SiteId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Merchant Name is required.")]
        [Display(Name = "Merchant Name")]
        public int MerchantId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "CIT Carrier is required.")]
        [Required(ErrorMessage = "CIT Carrier is required")]
        [Display(Name = "CIT Carrier")]
        public int CitCarrierId { get; set; }

        public int StatusId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "City is required.")]
        [Required(ErrorMessage = "City is required")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Cash Center Name is required.")]
        [Required(ErrorMessage = "Cash Center Name is required")]
        [Display(Name = "Cash Center Name")]
        public int CashCenterId { get; set; }

        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        public int AddressId { get; set; }

        [MinLength(5, ErrorMessage = "Cit Code cannot be less than 5 characters")]
        [MaxLength(8, ErrorMessage = "Cit Code cannot be more than 8 characters")]
        [Required(ErrorMessage = "CIT Code is required")]
        [Display(Name = "CIT Code")]
        public string CitCode { get; set; }

        [Required(ErrorMessage = "Site Name is required.")]
        [Display(Name = "Site Name")]
        public string Name { get; set; }


        [Display(Name = "Site Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Syspro Number is required.")]
        [Display(Name = "Syspro Number")]
        public string SysproNumber { get; set; }

        public string PostalCode { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [Required(ErrorMessage = "Deposit Reference is required.")]
        [Display(Name = "Deposit Reference")]
        [RegularExpression(pattern: "^[a-zA-Z0-9 ]{1,20}$", ErrorMessage = "Deposit Reference must not contain special characters")]
        public string DepositReference { get; set; }

        [Display(Name = "Reference Is Editable")]
        public bool DepositReferenceIsEditable { get; set; }

        [Required(ErrorMessage = "Contact Person 1 Name is required.")]
        [Display(Name = "Contact Person 1")]
        public string ContactPersonName1 { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Contact Person 1 Email Address is required.")]
        [Display(Name = "Contact Person Email")]
        [RegularExpression(
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Invalid Email Address")]
        public string ContactPersonEmailAddress1 { get; set; }

        [Required(ErrorMessage = "Contact Person 1 Phone is required.")]
        [Display(Name = "Contact Person Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Contact Person 1 Phone Number must be [0000000000]")]
        [MaxLength(10, ErrorMessage = "Phone Number must be 10 characters long")]
        public string ContactPersonNumber1 { get; set; }

        [Required(ErrorMessage = "Contact Person 1 Designation is required.")]
        [Display(Name = "Designation 1")]
        public string ContactPersonDesignation1 { get; set; }

        [Display(Name = "Contact Person 2")]
        public string ContactPersonName2 { get; set; }

        [EmailAddress]
        [Display(Name = "Contact Person Email 2")]
        [RegularExpression(
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Invalid Email Address")]
        public string ContactPersonEmailAddress2 { get; set; }

        [Display(Name = "Contact Person Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Contact Person 2 Phone Number must be [0000000000]")]
        [MaxLength(10, ErrorMessage = "Phone Number must be 10 characters long")]
        public string ContactPersonNumber2 { get; set; }

        [Display(Name = "Designation 2")]
        public string ContactPersonDesignation2 { get; set; }
        public bool ApprovalRequiredFlag { get; set; }


        [Display(Name = "Centre Capturing Deposit")]
		public bool IsCashCentreAllowedDepositCapturing { get; set; } 

        public string Comments { get; set; }
        public DateTime? ImplementationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public string LookUpKey { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        public bool IsNotDeleted { get; set; }
        public bool IsDefaultReference { get; set; }

        public AddressDto Address { get; set; }

        [Display(Name = "Site Containers")]
        public List<int> ContainerTypeIds { get; set; }
        public List<SiteContainerDto> SiteContainers { get; set; }
        public List<AccountDto> Accounts { get; set; }
    }
}