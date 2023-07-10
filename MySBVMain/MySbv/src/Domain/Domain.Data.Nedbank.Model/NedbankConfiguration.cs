using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("Configurations", Schema = "Nedbank")]
    public class NedbankConfiguration : EntityBase
    {
        public int NedbankConfigurationId { get; set; }

        [Required]
        [StringLength(50)]
        public string ConfigName { get; set; }

        [Required]
        [StringLength(50)]
        public string DocumentType { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; }

        [Required]
        [StringLength(50)]
        public string DailyCutoffTime { get; set; }

        public override int Key
        {
            get { return NedbankConfigurationId; }
        }
    }
}