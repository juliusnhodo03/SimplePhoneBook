using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("InstructionFileType", Schema = "Nedbank")]
    public class NedbankInstructionFileType : EntityBase
    {
        public NedbankInstructionFileType()
        {
            NedbankHeaderRecords = new HashSet<NedbankHeaderRecord>();
        }

        [Key]
        [StringLength(2)]
        public string FileType { get; set; }

        [Required]
        [StringLength(50)]
        public string FileTypeName { get; set; }

        [Required]
        [StringLength(50)]
        public string LookupKey { get; set; }

        public virtual ICollection<NedbankHeaderRecord> NedbankHeaderRecords { get; set; }

        public override int Key
        {
            get { return 0; }
        }
    }
}