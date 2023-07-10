using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Nedbank.Model
{
    [Table("UnpaidOrNaedo", Schema = "Nedbank")]
    public class NedbankUnpaidOrNaedo : EntityBase 
    {
        public int NedbankUnpaidOrNaedoId { get; set; }

        [Required]
        [StringLength(2)]
        public string RecordIdentifier { get; set; }

        [Required]
        [StringLength(2)]
        public string RecordType { get; set; }
        
        [StringLength(2)]
        public string NedbankUnpaidReasonId { get; set; }

        [StringLength(34)]
        public string PaymentReferenceNumber { get; set; }

        [StringLength(8)]
        public string NedbankReferenceNumber { get; set; }

        [StringLength(3)]
        public string RejectingBankCode { get; set; }

        [StringLength(6)]
        public string RejectingBankBranchCode { get; set; }

        [StringLength(6)]
        public string NewDestinationBranchCode { get; set; }

        [StringLength(16)]
        public string NewDestinationAccountNumber { get; set; }

        [StringLength(1)]
        public string NewDestinationAccountType { get; set; }

        [StringLength(8)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Reason { get; set; }

        [StringLength(30)]
        public string UnpaidsUserReference { get; set; }

        [StringLength(30)]
        public string NaedosUserReference { get; set; } 

        [StringLength(11)]
        public string OriginalHomingAccountNumber { get; set; }

        [StringLength(1)]
        public string OriginalAccountType { get; set; }

        [StringLength(12)]
        public string Amount { get; set; }

        [StringLength(6)]
        public string OriginalActionDate { get; set; }

        [StringLength(2)]
        public string Class { get; set; }

        [StringLength(1)]
        public string TaxCode { get; set; }

        [StringLength(2)]
        public string ReasonCode { get; set; }

        [StringLength(30)]
        public string OriginalHomingAccountName { get; set; }

        [StringLength(6)]
        public string NewSequenceNumber { get; set; }

        [StringLength(2)]
        public string NumberOfTimesRedirected { get; set; }

        [StringLength(6)]
        public string NewActionDate { get; set; }

        [StringLength(50)]
        public string ResponseFilename { get; set; }

        public string SettlementIdentifier { get; set; }  

        public override int Key
        {
            get { return NedbankUnpaidOrNaedoId; }
        }
    }
}