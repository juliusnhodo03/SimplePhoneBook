using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;
using Domain.Data.Model;

namespace Domain.Data.Nedbank.Model
{
    [Table("FileItem", Schema = "Nedbank")]
    public class NedbankFileItem : EntityBase
    {
        public int NedbankFileItemId { get; set; }

        public int NedbankBatchFileId { get; set; }

        [Required]
        [StringLength(2)]
        public string NedbankClientTypeId { get; set; }

        public int AccountId { get; set; }

        [Required]
        [StringLength(2)]
        public string RecordIdentifier { get; set; }

        [Required]
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
        [StringLength(16)]
        public string ChargesAccountNumber { get; set; }

        [Required]
        [StringLength(2)]
        public string ServiceType { get; set; }

        [StringLength(34)]
        public string OriginalPaymentReferenceNumber { get; set; }
        
        [StringLength(2)]
        public string EntryClass { get; set; }

        [Required]
        [StringLength(30)]
        public string NominatedAccountReference { get; set; }

        [StringLength(1)]
        public string BDFIndicator { get; set; }

        [Required]
        public string SettlementIdentifier { get; set; }
        
        [NotMapped]
        [StringLength(75)]
        public string Filler { get; set; }

        public virtual Account Account { get; set; }

        public virtual NedbankEntryCode NedbankEntryCode { get; set; }

        public virtual NedbankBatchFile NedbankBatchFile { get; set; }

        public virtual NedbankClientType NedbankClientType { get; set; }

        public virtual NedbankServiceType NedbankServiceType { get; set; }

        public virtual NedbankTransactionType NedbankTransactionType { get; set; }

        public override int Key
        {
            get { return NedbankFileItemId; }
        }
    }
}