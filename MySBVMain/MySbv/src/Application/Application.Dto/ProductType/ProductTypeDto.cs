using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Product;

namespace Application.Dto.ProductType
{
    public class ProductTypeDto
    {
        [ScaffoldColumn(false)]
        public int ProductTypeId { get; set; }

        [Required(ErrorMessage = "Product Name is missing")]
        [Display(Name = "Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

    }
}