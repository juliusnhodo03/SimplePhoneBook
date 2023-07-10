using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class SettlementStatus : EntityBase
    {
        [Key]
        [StringLength(1)]
        public string StatusCode { get; set; }


        [StringLength(50)]
        public string Status { get; set; }

        public override int Key
        {
            get { return 0; }
        }
    }
}