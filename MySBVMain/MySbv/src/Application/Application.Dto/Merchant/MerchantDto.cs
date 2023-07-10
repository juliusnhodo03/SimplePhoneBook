using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Site;
using Domain.Data.Model;

namespace Application.Dto.Merchant
{
    public class MerchantDto
    {
        
        [ScaffoldColumn(false)]
        public int MerchantId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Merchant Description is required.")]
        [Display(Name = "Merchant Description")]
        public int? MerchantDescriptionId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Merchant Type field is required.")]
        [Display(Name = "Merchant Type")]
        public int CompanyTypeId { get; set; }

        [Required(ErrorMessage = "Merchant Name is required.")]
        [Display(Name = "Merchant Name")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Contract Number is required")]
        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [Display(Name = "Merchant Number")]
        public string MerchantNumber { get; set; }

        [Required(ErrorMessage = "Trading Name is required.")]
        [Display(Name = "Trading Name")]
        public string TradingName { get; set; }


        [Display(Name = "Company Group Name")]
        public string CompanyGroupName { get; set; }

        [Display(Name = "Franchise Name")]
        public string FranchiseName { get; set; }

        [Required(ErrorMessage = "Registration Name is required.")]
        [Display(Name = "Registered Name")]
        public string RegisteredName { get; set; }

        [Required(ErrorMessage = "Registration Number is required.")]
        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "VAT Number must be numeric")]
        [Display(Name = "VAT Number")]
        public string VATNumber { get; set; }

        [Display(Name = "Implementation Date")]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}(?:\s\d{1,2}:\d{2}:\d{2})?",
            ErrorMessage = "Please enter Implementation Date in correct format yyyy-mm-dd")]
        public DateTime? ImplementationDate { get; set; }

        [Display(Name = "Termination Date")]
        [RegularExpression(@"\d{4}-\d{2}-\d{2}(?:\s\d{1,2}:\d{2}:\d{2})?",
            ErrorMessage = "Please enter Termination Date in correct format yyyy-mm-dd")]
        public DateTime? TerminationDate { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Invalid Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must be [0000000000]")]
        [MaxLength(10, ErrorMessage = "Phone Number must be 10 characters long")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Contact Person 1 is required.")]
        [Display(Name = "Contact Person 1")]
        public string ContactPerson1 { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Contact Person 1 Email Address is required.")]
        [Display(Name = "Contact Person E-Address")]
        [RegularExpression(
            @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
            ErrorMessage = "Invalid Email Address")]
        public string ContactPerson1EmailAddress { get; set; }

        [Display(Name = "Contact Person 2")]
        public string ContactPerson2 { get; set; }

        [EmailAddress]
        [Display(Name = "Contact Person E-Address")]
        public string ContactPerson2EmailAddress { get; set; }

        [Display(Name = "Designation")]
        [Required(ErrorMessage = "Contact Person 1 Designation is required.")]
        public string ContactPerson1Designation { get; set; }

        [Display(Name = "Designation")]
        public string ContactPerson2Designation { get; set; }

        [Required(ErrorMessage = "Contact Person 1 Phone number is required.")]
        [Display(Name = "Contact Person Phone")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Phone number must be [0000000000]")]
        [MaxLength(10, ErrorMessage = "Phone Number must be 10 characters long")]
        public string ContactPerson1Phone { get; set; }

        [Display(Name = "Contact Person Phone")]
        public string ContactPerson2Phone { get; set; }

        [Display(Name = "Contract Document")]
        public string ContractDocumentUrl { get; set; }

        [Display(Name = "WebSite Url")]
        public string WebSiteUrl { get; set; }


        [Display(Name = "Financial Accountant")]
        public string FinancialAccountant { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        public string FinancialAccountantEmailAddress { get; set; }

        [Display(Name = "Deposit Slip E-mail Indicator")]
        public bool DepositSlipEmailIndicator { get; set; }

        [Display(Name = "Comments")]
        public string Comments { get; set; }

        [Display(Name = "Active Status")]
        public bool ActiveStatus { get; set; }

        [Display(Name = "Is Approved")]
        public bool IsApproved { get; set; }

        [Display(Name = "Status")]
        public string ApprovalStatus { get; set; }

        public int ApprovalStatusId { get; set; }

        public DateTime CapturedTimestamp { get; set; }

        public bool IsNotDeleted { get; set; }

        public virtual CompanyType CompanyType { get; set; }
        public virtual MerchantDescription MerchantDescription { get; set; }
        public virtual List<SiteDto> Sites { get; set; }
    }
}