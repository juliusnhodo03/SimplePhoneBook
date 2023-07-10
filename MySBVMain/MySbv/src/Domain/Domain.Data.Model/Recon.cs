using System;
using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Recon : EntityBase
    {
        public int ReconId { get; set; }

        [Required]
        [StringLength(50)]
        public string BankType { get; set; }

        [Required]
        [StringLength(150)]
        public string ClientReference { get; set; }

        [Required]
        [StringLength(100)]
        public string Amount { get; set; }

        public DateTime DateActioned { get; set; }

        [Required]
        [StringLength(100)]
        public string ClientSite { get; set; }

        [StringLength(100)]
        public string MySbvReference { get; set; }

        [Required]
        [StringLength(100)]
        public string StatusCode { get; set; }

        [StringLength(100)]
        public string UniqueUserCode { get; set; }

        [Required]
        [StringLength(100)]
        public string BatchNumber { get; set; }

        public DateTime DateSent { get; set; }

        [Required]
        [StringLength(100)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string BranchCode { get; set; }

        [Required]
        public int AccountTypeId { get; set; }

        public SettlementStatus SettlementStatus { get; set; }
        public AccountType AccountType { get; set; }

        public override int Key
        {
            get { return ReconId; }
        }
    }
}