using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("HeaderItem", Schema = "Nedbank")]
    public class NedbankHeaderRecord : EntityBase
    {
        public NedbankHeaderRecord()
        {
            NedbankBatchFiles = new HashSet<NedbankBatchFile>();
        }

        public int NedbankHeaderRecordId { get; set; }

        [Required]
        [StringLength(2)]
        public string RecordIdentifier { get; set; }

        [Required]
        [StringLength(10)]
        public string ClientProfileNumber { get; set; }

        [Required]
        [StringLength(24)]
        public string FileSequenceNumber { get; set; }

        [Required]
        [StringLength(2)]
        public string FileType { get; set; }

        [StringLength(16)]
        public string NominatedAccountNumber { get; set; }

        [StringLength(16)]
        public string ChargesAccountNumber { get; set; }
        
        [NotMapped]
        [StringLength(220)]
        public string Filler { get; set; }

        [StringLength(30)]
        public string StatementNarrative { get; set; }

        public virtual ICollection<NedbankBatchFile> NedbankBatchFiles { get; set; }

        public virtual NedbankInstructionFileType NedbankFileType { get; set; }

        public override int Key
        {
            get { return NedbankHeaderRecordId; }
        }
    }
}