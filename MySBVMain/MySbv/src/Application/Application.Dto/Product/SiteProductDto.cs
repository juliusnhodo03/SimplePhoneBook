using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Product
{
    public class SiteProductDto
    {
        public SiteProductDto()
        {
            Products = new List<ProductDto>();   
        }

        [Required]
        [Display(Name = "Site Name")]
        public int SiteId { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        public List<ProductDto> Products { get; set; }

        
    }
}