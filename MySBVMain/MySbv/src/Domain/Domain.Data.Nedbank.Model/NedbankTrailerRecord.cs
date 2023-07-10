using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("TrailerItem", Schema = "Nedbank")]
    public class NedbankTrailerRecord : EntityBase
    {
        public NedbankTrailerRecord()
        {
            NedbankBatchFiles = new HashSet<NedbankBatchFile>();
        }

        public int NedbankTrailerRecordId { get; set; }

        [Required]
        [StringLength(2)]
        public string RecordIdentifier { get; set; }

        [Required]
        [StringLength(8)]
        public string TotalNumberOfTransactions { get; set; }

        [Required]
        [StringLength(18)]
        public string TotalValue { get; set; }

        [NotMapped]
        [StringLength(292)]
        public string Filler { get; set; }

        public virtual ICollection<NedbankBatchFile> NedbankBatchFiles { get; set; }

        public override int Key
        {
            get { return NedbankTrailerRecordId; }
        }
    }
}