using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Product
{
    public class ProductFeeDto
    {
        public int ProductId { get; set; }
        public int FeeId { get; set; }
        public bool IsActive { get; set; }

    }
}
