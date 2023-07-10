using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("ClientProfile", Schema = "Nedbank")]
    public class NedbankClientProfile : EntityBase
    {
        [Key]
        [Required]
        [StringLength(10)]
        public string ProfileNumber { get; set; }

        [Required]
        [StringLength(2)]
        public string NedbankClientTypeId { get; set; } 

        [Required]
        [StringLength(50)]
        public string ClientName { get; set; }

        [Required]
        [StringLength(2)]
        public string Prefix { get; set; } 

        [Required]
        [StringLength(50)]
        public string LookupKey { get; set; }

        [Required]
        [StringLength(16)]
        public string ChargesAccountNumber { get; set; }

        [Required]
        [StringLength(16)]
        public string NominatedAccountNumber { get; set; }

        [StringLength(30)]
        public string StatementNarrative { get; set; }

        public NedbankClientType NedbankClientType { get; set; }

        public override int Key
        {
            get { return 0; }
        }
    }
}