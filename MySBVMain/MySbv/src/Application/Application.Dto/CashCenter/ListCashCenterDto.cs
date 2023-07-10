using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashCenter
{
    public class ListCashCenterDto
    {
        [ScaffoldColumn(false)]
        public int CashCenterId { get; set; }

        [Required(ErrorMessage = "City is missing")]
        public int CityId { get; set; }

        public int AddressId { get; set; }

        [Display(Name = "SBV Region")]
        public int ClusterId { get; set; }

        [Display(Name = "Centre Name")]
        [Required(ErrorMessage = "Cash Centre Name is missing")]
        public string Name { get; set; }

        [Display(Name = "Centre Number")]
        [Required(ErrorMessage = "Centre Number is missing")]
        [MinLength(3, ErrorMessage = "Postal Code is not correct")]
        [MaxLength(6, ErrorMessage = "Postal Code is not correct")]
        public string Number { get; set; }

        [Display(Name = "Telephone Number")]
        [MinLength(10, ErrorMessage = "Minimum is 10 characters")]
        public string TelephoneNumber { get; set; }

        [Display(Name = "Contact Person")]
        [Required(ErrorMessage = "Contact Person Missing")]
        public string ContactPerson { get; set; }

        [Display(Name = "Email Address 1")]
        [EmailAddress]
        public string EmailAddress1 { get; set; }

        [Display(Name = "Email Address 2")]
        [EmailAddress]
        public string EmailAddress2 { get; set; }

        [Display(Name = "Email Address 3")]
        [EmailAddress]
        public string EmailAddress3 { get; set; }


        [ScaffoldColumn(false)]
        public int LastChangedById { get; set; }


        [ScaffoldColumn(false)]
        public string RegionName
        {
            get { return ""; }
            private set { }
        }

        public bool IsNotDeleted { get; set; }


        //Address information
        [Display(Name = "Address Type")]
        [Required(ErrorMessage = "Address Type is missing")]
        public int AddressTypeId { get; set; }

        [Display(Name = "Address Line 1")]
        [Required(ErrorMessage = "AddressLine1 is missing")]
        public string AddressLine1 { get; set; }

        [Display(Name = "Address Line 2")]
        public string AddressLine2 { get; set; }

        [Display(Name = "Address Line 3")]
        public string AddressLine3 { get; set; }

        [Display(Name = "Postal Code")]
        [MinLength(4, ErrorMessage = "Postal Code is not correct")]
        [MaxLength(6, ErrorMessage = "Postal Code is not correct")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Postal Code is not correct")]
        [Required(ErrorMessage = "Postal Code is missing")]
        public string PostalCode { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }
        public DateTime LastChangedDate { get; set; }
    }
}