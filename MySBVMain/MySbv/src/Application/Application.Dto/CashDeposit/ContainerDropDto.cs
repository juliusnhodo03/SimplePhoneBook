using System;
using System.Collections.Generic;

namespace Application.Dto.CashDeposit
{
    public class ContainerDropDto
    {
        public ContainerDropDto()
        {
            ContainerDropItems = new List<ContainerDropItemDto>();
            ActiveStatus = true;
            LastChangedDate = DateTime.Now;
        }

		public int ContainerDropId { get; set; }
		public int ContainerId { get; set; }

        //[Display(Name = "Discrepancy Reason")]
		public int? DiscrepancyReasonId { get; set; }
		public int StatusId { get; set; }

        //[Display(Name = "Client Deposit Reference")]
		public string Narrative { get; set; }
		
        //[Display(Name = "Status")]
		public string Name { get; set; } 

        //[Display(Name = "Drop Transaction Reference")]
		public string ReferenceNumber { get; set; }

        //[Display(Name = "Drop Serial Number")]
		public string BagSerialNumber { get; set; }

		public decimal Amount { get; set; }
		public decimal DiscrepancyAmount { get; set; }
		public decimal ActualAmount { get; set; }
		public string Comment { get; set; }
		public int Number { get; set; }
		public bool IsNotDeleted { get; set; }
		public int? ErrorCodeId { get; set; }
		public DateTime? SettlementDateTime { get; set; }
		public DateTime? SendDateTime { get; set; }
		public DateTime LastChangedDate { get; set; }
		public bool ActiveStatus { get; set; }
        public virtual List<ContainerDropItemDto> ContainerDropItems { get; set; }
    }
}