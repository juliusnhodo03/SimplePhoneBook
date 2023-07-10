using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Merchant
{
    public class ListMerchantDto
    {
        [ScaffoldColumn(false)]
        public int MerchantId { get; set; }

        [Display(Name = "Company Type")]
        [Range(1, 50, ErrorMessage = "Company Type field is required.")]
        public int CompanyTypeId { get; set; }

        [Display(Name = "Merchant Name")]
        public string Name { get; set; }

        [Display(Name = "Merchant Number")]
        public string MerchantNumber { get; set; }

        [Display(Name = "Merchant Description")]
        public string MerchantDescription { get; set; }

        [Display(Name = "Trading Name")]
        public string TradingName { get; set; }

        [Display(Name = "Company Group Name")]
        public string CompanyGroupName { get; set; }

        [Display(Name = "Franchise Name")]
        public string FranchiseName { get; set; }

        [Display(Name = "Registered Name")]
        public string RegisteredName { get; set; }

        [Display(Name = "Registration Number")]
        public string RegistrationNumber { get; set; }

        [Display(Name = "VAT Number")]
        public string VATNumber { get; set; }

        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [Display(Name = "Implementation Date")]
        public DateTime ImplementationDate { get; set; }

        [Display(Name = "Termination Date")]
        public DateTime? TerminationDate { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Contact Person 1")]
        public string ContactPerson1 { get; set; }

        [Display(Name = "Contact Person 2")]
        public string ContactPerson2 { get; set; }

        [Display(Name = "Contact Person 1 Designation")]
        public string ContactPerson1Designation { get; set; }

        [Display(Name = "Contact Person 2 Designation")]
        public string ContactPerson2Designation { get; set; }

        [Display(Name = "Contract Document Url")]
        public string ContractDocumentUrl { get; set; }

        [Display(Name = "WebSite Url")]
        public string WebSiteUrl { get; set; }

        [Display(Name = "Financial Accountant")]
        public string FinancialAccountant { get; set; }

        [Display(Name = "Financial Accountant EmailAddress")]
        public string FinancialAccountantEmailAddress { get; set; }

        [Display(Name = "Status")]
        public string ApprovalStatus { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        public bool DepositSlipEmailIndicator { get; set; }
        public DateTime CapturedTimestamp { get; set; }
    }
}