using System.ComponentModel.DataAnnotations;
using Application.Dto.ValidationAttributes;

namespace Application.Dto.Address
{
    public class AddressDto
    {
        public int AddressId { get; set; }

        [Display(Name = "Address Type")]
        [Required(ErrorMessage = "Address Type is missing")]
        [DropDownValidation(ErrorMessage = "Address Type is missing")]
        public int AddressTypeId { get; set; }


		[Required(ErrorMessage = "AddressLine1 is required")]
        public string AddressLine1 { get; set; }

		[Required(ErrorMessage = "AddressLine2 is required")]
        public string AddressLine2 { get; set; }

		[Required(ErrorMessage = "AddressLine3 is required")]
        public string AddressLine3 { get; set; }
        
        [Display(Name = "Postal Code")]
        [MinLength(4, ErrorMessage = "Postal Code is not correct")]
        [MaxLength(6, ErrorMessage = "Postal Code is not correct")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Postal Code is not correct")]
        [Required(ErrorMessage = "Postal Code is missing")]
        public string PostalCode { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

    }
}