using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("EntryCode", Schema = "Nedbank")]
    public class NedbankEntryCode : EntityBase
    {
        public NedbankEntryCode()
        {
            NedbankFileItems = new HashSet<NedbankFileItem>();
        }

        [Key]
        [StringLength(2)]
        public string EntryCode { get; set; }

        [Required]
        [StringLength(50)]
        public string LookupKey { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public virtual ICollection<NedbankFileItem> NedbankFileItems { get; set; }

        public override int Key
        {
            get { return 0; }
        }
    }
}