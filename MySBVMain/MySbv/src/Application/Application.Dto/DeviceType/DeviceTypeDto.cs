using System;
using System.ComponentModel.DataAnnotations;


namespace Application.Dto.DeviceType
{
    public class DeviceTypeDto
    {
        public int DeviceTypeId { get; set; }
        
        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select Manufacturer")]
        [Required(ErrorMessage = "Please Select Manufacturer")]
        [Display(Name = "Manufacturer")]
        public int ManufacturerId { get; set; }

        [RegularExpression("^[1-9][0-9]*$", ErrorMessage = "Please select Supplier")]
        [Required(ErrorMessage = "Please Select Supplier")]
        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }
        
        [Required(ErrorMessage = "Device Name is missing")]
        [Display(Name = "Device Name")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string LookUpKey { get; set; }

        [Required(ErrorMessage = "Device Model is missing")]
        [Display(Name = "Device Model")]
        public string Model { get; set; }
    }
}
