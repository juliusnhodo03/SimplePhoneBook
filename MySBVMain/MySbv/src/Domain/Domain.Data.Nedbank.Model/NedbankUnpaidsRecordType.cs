using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("UnpaidsRecordType", Schema = "Nedbank")]
    public class NedbankUnpaidsRecordType : EntityBase
    {
        [Required]
        [StringLength(2)]
        public string NedbankUnpaidsRecordTypeId { get; set; }

        [Required]
        [StringLength(50)]
        public string LookupKey { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }

        public override int Key
        {
            get { return 0; }
        }
    }
}