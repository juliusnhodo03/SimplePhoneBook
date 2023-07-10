using System;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Address;
using Application.Dto.CashDeposit;
using Application.Dto.Merchant;

namespace Application.Dto.Site
{
    public class ListSiteDto
    {
        [ScaffoldColumn(false)]
        public int SiteSettlementAccountId { get; set; }

        [ScaffoldColumn(false)]
        public int SiteId { get; set; }

        public int AddressId { get; set; }

        [Display(Name = "Merchant Name")]
        public int MerchantId { get; set; }

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

        [Display(Name = "Cash Centre Name")]
        public string CashCentreName { get; set; }

        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [Display(Name = "City")]
        public int CityId { get; set; }

        [Display(Name = "CIT Carrier Name")]
        public int CitCarrierId { get; set; }

        [Display(Name = "Cash Center Name")]
        public int CashCenterId { get; set; }


        [Display(Name = "CIT Code")]
        public string CitCode { get; set; }

        [Display(Name = "Site Name")]
        public string Name { get; set; }

        [Display(Name = "Site Description")]
        public string SiteDescription { get; set; }

        [Display(Name = "Syspro Number")]
        public string SysproNumber { get; set; }

        [Display(Name = "Stree Number")]
        public string StreetNumber { get; set; }

        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Display(Name = "Suburb")]
        public string Suburb { get; set; }

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Display(Name = "Postal Address")]
        public string PostalAddress { get; set; }

        [Display(Name = "Contact Person 1")]
        public string ContactPersonName1 { get; set; }

        [Display(Name = "Contact Person Email 1")]
        public string ContactPersonEmailAddress1 { get; set; }

        [Display(Name = "Contact Person Ph 1")]
        public string ContactPersonNumber1 { get; set; }

        [Display(Name = "Designation 1")]
        public string Designation1 { get; set; }

        [Display(Name = "Contact Person 1")]
        public string ContactPersonName2 { get; set; }

        [Display(Name = "Contact Person Email 2")]
        public string ContactPersonEmailAddress2 { get; set; }

        [Display(Name = "Contact Person Ph 2")]
        public string ContactPersonNumber2 { get; set; }

        [Display(Name = "Designation 2")]
        public string Designation2 { get; set; }

        [Display(Name = "Implementation Date")]
        public DateTime ImplementationDate { get; set; }

        [Display(Name = "Termination Date")]
        public DateTime TerminationDate { get; set; }

        [Display(Name = "Allow Custom Deposit Reference")]
        public bool DepositReferenceIsEditable { get; set; }

        [Display(Name = "Deposit Reference")]
        public string SiteDepositReference { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }
        
        public bool ApprovalRequiredFlag { get; set; }
        public bool ReviewRequiredFlag { get; set; }
        public StatusDto Status { get; set; }
        public AddressDto Address { get; set; }
    }
}