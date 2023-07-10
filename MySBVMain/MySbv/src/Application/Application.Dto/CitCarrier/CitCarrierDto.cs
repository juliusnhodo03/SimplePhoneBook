using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Carrier
{
    public class CitCarrierDto
    {
        [ScaffoldColumn(false)]
        public int CitCarrierId { get; set; }

        [ScaffoldColumn(false)]
        public int LastChangedById { get; set; }

        [Required(ErrorMessage = "CIT Carrier Name is missing")]
        [Display(Name = "CIT Name")]
        public string Name { get; set; }

        [Display(Name = "CIT Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Cit Initial Serial# digit is missing")]
        [Display(Name = "CIT Initial Serial# Digit")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "CIT Initial Serial must be numeric")]
        public int SerialStartNumber { get; set; }

        [Display(Name = "Created On")]
        public DateTime LastChangedDate { get; set; }

        [Display(Name = "Created By")]
        public string LastChangedUser { get; set; }
        
    }
}