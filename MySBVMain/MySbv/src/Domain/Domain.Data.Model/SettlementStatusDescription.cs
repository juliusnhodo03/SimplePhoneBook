using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class SettlementStatusDescription : EntityBase
    {
        [Key]
        public int SettlementStatusDescriptionId { get; set; }

        [StringLength(50)]
        public string Description { get; set; }

        [StringLength(1)]
        public string StatusCode { get; set; }

        [StringLength(50)]
        public string LookupKey { get; set; } 

        public override int Key
        {
            get { return SettlementStatusDescriptionId; }
        }

        public SettlementStatus SettlementStatus { get; set; }
    }
}