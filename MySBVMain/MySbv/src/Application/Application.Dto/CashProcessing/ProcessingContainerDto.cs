using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashProcessing
{
    public class ProcessingContainerDto
    {
        public ProcessingContainerDto()
        {
            ContainerDrops = new List<ProcessingContainerDropDto>();
        }

        public int ContainerId { get; set; }

        public int CashDepositId { get; set; }

        public int? ContainerTypeId { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Seal Number")]
        public string SealNumber { get; set; }

        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Display(Name = "Discrepancy Amount")]
        public decimal DiscrepancyAmount { get; set; }

        [Display(Name = "Actual Amount")]
        public decimal ActualAmount { get; set; }

        [Display(Name = "IsPrimary Container")]
        public bool IsPrimaryContainer { get; set; }

        [Display(Name = "Container Type")]
        public string ContainerTypeName { get; set; }

        public bool IsNotDeleted { get; set; }

        public int CreatedById { get; set; }

        public int LastChangedById { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastChangedDate { get; set; }

        public virtual List<ProcessingContainerDropDto> ContainerDrops { get; set; }
    }
}