using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("Scheduler", Schema = "Nedbank")]
    public class NedbankScheduler : EntityBase
    {
        public int NedbankSchedulerId { get; set; }

        public int NedbankBatchFileId { get; set; }

        [Required]
        [StringLength(8)]
        public string NumberOfDepositsSent { get; set; }

        [Required]
        [StringLength(50)]
        public string LastRan { get; set; }

        public virtual NedbankBatchFile NedbankBatchFile { get; set; }

        public override int Key
        {
            get { return NedbankSchedulerId; }
        }
    }
}