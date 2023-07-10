using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashDeposit
{
    public class ContainerTypeDto
    {
        public int ContainerTypeId { get; set; }

		[Required(ErrorMessage = "Container Type is required!")]
		[MaxLength(50, ErrorMessage = "Container Type must be equal to 100 characters!")]
        [Display(Name = "Container Type")]
        public string Name { get; set; }

        [Display(Name = "Container Type Description")]
        public string Description { get; set; }
		public int CreatedById { get; set; }

		[Required(ErrorMessage = "Serial Number is required")]
		[Range(14, 14, ErrorMessage = "Serial number must be 14 characters in length!")]
		[Display(Name = "Serial Number")]
		public string SerialNumber { get; set; }

		[Required(ErrorMessage = "Seal Number is required")]
		[Range(14, 14, ErrorMessage = "Seal number must be 14 characters in length!")]
		[Display(Name = "Seal Number")]
		public string SealNumber { get; set; }

        public DateTime CreateDate { get; set; }
		public List<ContainerTypeAttributeDto> ContainerTypeAttributes { get; set; } 
    }
}