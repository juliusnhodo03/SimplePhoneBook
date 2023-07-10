using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("BatchFeed", Schema = "Nedbank")]
    public class NedbankBatchFile : EntityBase
    {
        public NedbankBatchFile()
        {
            NedbankFileItems = new HashSet<NedbankFileItem>();
            NedbankSchedulers = new HashSet<NedbankScheduler>();
        }

        public int NedbankBatchFileId { get; set; }

        public int NedbankHeaderRecordId { get; set; }

        public int NedbankTrailerRecordId { get; set; }

        [Required]
        [StringLength(15)]
        public string BatchNumber { get; set; }

        [Required]
        [StringLength(18)]
        public string BatchTotal { get; set; }

        [Required]
        [StringLength(3)]
        public string BatchCount { get; set; }

        [Required]
        [StringLength(8)]
        public string BatchDate { get; set; }

        public bool IsSent { get; set; }

        [Required]
        [StringLength(50)]
        public string FileName { get; set; }

        public virtual NedbankHeaderRecord NedbankHeaderRecord { get; set; }

        public virtual NedbankTrailerRecord NedbankTrailerRecord { get; set; }

        public virtual ICollection<NedbankFileItem> NedbankFileItems { get; set; }

        public virtual ICollection<NedbankScheduler> NedbankSchedulers { get; set; }

        public override int Key
        {
            get { return NedbankBatchFileId; }
        }
    }
}