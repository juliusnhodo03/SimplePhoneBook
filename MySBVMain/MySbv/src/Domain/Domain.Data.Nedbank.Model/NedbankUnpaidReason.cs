using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("UnpaidReason", Schema = "Nedbank")]
    public class NedbankUnpaidReason : EntityBase
    {
        [Key]
        [Required]
        [StringLength(2)]
        public string NedbankUnpaidReasonId { get; set; }   

        [Required]
        [StringLength(150)]
        public string Reason { get; set; } 

        public override int Key
        {
            get { return 0; }
        }
    }
}