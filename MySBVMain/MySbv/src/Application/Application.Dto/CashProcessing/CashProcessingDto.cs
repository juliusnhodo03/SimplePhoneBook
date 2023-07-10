using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashProcessing
{
    public class CashProcessingDto
    {
        public int? UserTypeId { get; set; }

        public int CashDepositId { get; set; }

        public int DepositTypeId { get; set; }

        public int SiteId { get; set; }

        public int? AccountId { get; set; }

        public int ProductTypeId { get; set; }

        public int? DeviceId { get; set; }

        [Display(Name = "Product Type")]
        public string ProductType { get; set; }

        [Display(Name = "Deposit Reference")]
        public string Narrative { get; set; }

        [Display(Name = "Transaction Number")]
        public string TransactionReference { get; set; }

        [Display(Name = "Total Deposit Amount")]
        public decimal DepositedAmount { get; set; }

        [Display(Name = "Total Deposit Amount (Actual)")]
        public decimal ActualAmount { get; set; }

        [Display(Name = "Discrepancy Amount")]
        public decimal DiscrepancyAmount { get; set; }

        [Display(Name = "DepositType")]
        public string DepositTypeName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Product Type")]
        public string ProductTypeName { get; set; }

        [Display(Name = "Capture Date")]
        public DateTime? CreateDate { get; set; }

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

        [Display(Name = "Contract Number")]
        public string ContractNumber { get; set; }

        [Display(Name = "Seal/Serial Number")]
        [Required(ErrorMessage = "Enter the Bag Seal Or Serial Number")]
        public string SealSerialNumber { get; set; }

        public bool IsDescripency { get; set; }

        public bool Confirm { get; set; }

        public DateTime? CitDateTime { get; set; }

        public DateTime? SubmitDateTime { get; set; }

        public int? SupervisorId { get; set; }

        public bool IsNotDeleted { get; set; }

        public int CreatedById { get; set; }

        public int LastChangedById { get; set; }

        public decimal VaultAmount { get; set; }

        public string VaultSource { get; set; }

        public string SettlementIdentifier { get; set; }

        public string iTramsUserName { get; set; }

        public DateTime? SendDateTime { get; set; }

        public DateTime LastChangedDate { get; set; }

        public virtual List<ProcessingContainerDto> Containers { get; set; }
    }
}