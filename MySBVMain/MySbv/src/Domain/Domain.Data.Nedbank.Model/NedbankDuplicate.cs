using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("Duplicate", Schema = "Nedbank")]
    public class NedbankDuplicate : EntityBase
    {
        public int NedbankDuplicateId { get; set; }

        [StringLength(10)]
        public string ClientProfileNumber { get; set; }

        [StringLength(24)]
        public string FileSequenceNumber { get; set; }

        [StringLength(2)]
        public string FileType { get; set; }

        [StringLength(16)]
        public string NominatedAccountNumber { get; set; }

        [StringLength(16)]
        public string ChargesAccountNumber { get; set; }

        [StringLength(8)]
        public string TotalNumberOfTransactions { get; set; }

        [StringLength(18)]
        public string TotalValue { get; set; }

        [StringLength(8)]
        public string FileStatus { get; set; }

        [StringLength(30)]
        public string Reason { get; set; } 

        [StringLength(256)]
        public string HashTotal { get; set; }

        public override int Key
        {
            get { return NedbankDuplicateId; }
        }
    }
}