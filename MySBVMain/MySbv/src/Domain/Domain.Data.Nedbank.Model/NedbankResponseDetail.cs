using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("ResponseDetail", Schema = "Nedbank")]
    public class NedbankResponseDetail : EntityBase
    {
        public int NedbankResponseDetailId { get; set; }

        [Required]
        [StringLength(2)]
        public string RecordIdentifier { get; set; }

        [StringLength(16)]
        public string NominatedAccountNumber { get; set; }

        [Required]
        [StringLength(34)]
        public string PaymentReferenceNumber { get; set; }

        [Required]
        [StringLength(6)]
        public string DestinationBranchCode { get; set; }

        [Required]
        [StringLength(16)]
        public string DestinationAccountNumber { get; set; }

        [Required]
        [StringLength(12)]
        public string Amount { get; set; }

        [Required]
        [StringLength(8)]
        public string ActionDate { get; set; }

        [Required]
        [StringLength(30)]
        public string Reference { get; set; }

        [Required]
        [StringLength(30)]
        public string DestinationAccountHoldersName { get; set; }

        [Required]
        [StringLength(4)]
        public string TransactionType { get; set; }

        [Required]
        [StringLength(2)]
        public string NedbankClientTypeId { get; set; }

        [StringLength(16)]
        public string ChargesAccountNumber { get; set; }

        [Required]
        [StringLength(2)]
        public string ServiceType { get; set; }

        [StringLength(34)]
        public string OriginalPaymentReferenceNumber { get; set; }

        [Required]
        [StringLength(8)]
        public string TransactionStatus { get; set; }

        [Required]
        [StringLength(50)]
        public string ResponseFilename { get; set; }

        [StringLength(98)]
        public string Reason { get; set; }

        public virtual NedbankClientType NedbankClientType { get; set; }

        public override int Key
        {
            get { return NedbankResponseDetailId; }
        }
    }
}