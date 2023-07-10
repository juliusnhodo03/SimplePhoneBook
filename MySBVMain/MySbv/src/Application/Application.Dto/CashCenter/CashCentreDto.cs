using System.ComponentModel.DataAnnotations;
using Application.Dto.Address;
using Application.Dto.ValidationAttributes;

namespace Application.Dto.CashCenter
{
    public class CashCentreDto
    {
        public int CashCenterId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select the City")]
        [Required(ErrorMessage = "City is missing")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        public int AddressId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select the Region")]
        [DropDownValidation(ErrorMessage = "Please select the Region")]
        [Display(Name = "Cluster")]
        public int ClusterId { get; set; }

        public int AddressTypeId { get; set; }


        [Display(Name = "Centre Name")]
        [Required(ErrorMessage = "Cash Centre Name is missing")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string LookUpKey { get; set; }

        [Display(Name = "Centre Number")]
        [Required(ErrorMessage = "Centre Number is missing")]
        [MinLength(3, ErrorMessage = "Centre Number is not correct")]
        [MaxLength(6, ErrorMessage = "Centre Number is not correct")]
        public string Number { get; set; }

        [Required(ErrorMessage = "Telephone Number is missing")]
        [Display(Name = "Telephone Number")]
        [MinLength(10, ErrorMessage = "Invalid Telephone Number")]
		[DataType(DataType.PhoneNumber)]
        public string TelephoneNumber { get; set; }

        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }

        [Required(ErrorMessage = "Email Address 1 is missing")]
        [Display(Name = "Email Address 1")]
        [EmailAddress]
        public string EmailAddress1 { get; set; }

        [Display(Name = "Email Address 2")]
        [EmailAddress]
        public string EmailAddress2 { get; set; }

        [Display(Name = "Email Address 3")]
        [EmailAddress]
        public string EmailAddress3 { get; set; }

        public virtual AddressDto Address { get; set; }
    }
}