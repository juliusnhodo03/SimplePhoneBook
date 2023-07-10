using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashProcessing
{
    public class ProcessingContainerDropDto
    {
        public ProcessingContainerDropDto()
        {
            ContainerDropItems = new List<ProcessingContainerDropItemDto>();
        }

        public int ContainerDropId { get; set; }

        public int ContainerId { get; set; }

        [Display(Name = "Discrepancy Reason")]
        public int? DiscrepancyReasonId { get; set; }

        [Display(Name = "Deposit Reference")]
        public string Narrative { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Bag Serial Number")]
        public string BagSerialNumber { get; set; }

        [Display(Name = "Container Type")]
        public string ContainerTypeName { get; set; }

        public decimal Amount { get; set; }

        [Display(Name = "Discrepancy Amount")]
        public decimal DiscrepancyAmount { get; set; }

        [Display(Name = "Actual Amount")]
        public decimal ActualAmount { get; set; }

        public string Comment { get; set; }

        public int Number { get; set; }

		public bool HasDiscrepancy { get; set; } 
		public bool IsNotDeleted { get; set; }

        public int CreatedById { get; set; }

        public int LastChangedById { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastChangedDate { get; set; }

        public virtual List<ProcessingContainerDropItemDto> ContainerDropItems { get; set; }
    }
}