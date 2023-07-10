using System;
using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CitRequestDetail : EntityBase
    {
        public int CitRequestDetailId { get; set; }

        public long CashDepositId { get; set; }

        [Required]
        [StringLength(20)]
        public string BeneficiaryCode { get; set; }
        
        [Required]
        [StringLength(20)]
        public string CitCode { get; set; }

        [Required]
        [StringLength(100)]
        public string DeviceSerialNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string BagSerialNumber { get; set; } 

        public DateTime TransactionDate { get; set; }

        [StringLength(100)]
        public string ItramsReference { get; set; }
        
        [StringLength(100)]
        public string UserReferance { get; set; }

        public bool IsReceiptPrinted { get; set; }

        public override int Key
        {
            get { return CitRequestDetailId; }
        }

        public CashDeposit CashDeposit { get; set; } 

    }
}