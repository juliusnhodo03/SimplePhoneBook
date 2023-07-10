using System;
using System.Collections.Generic;

namespace Application.Dto.CashDeposit
{
    public class ContainerDto
    {
        public ContainerDto()
        {
            ContainerDrops = new List<ContainerDropDto>();
        }
		
        public int ContainerId { get; set; }
        public int CashDepositId { get; set; }
        public int ContainerTypeId { get; set; }

        //[Display(Name = "Container Type")]
        public string ContainerTypeName { get; set; }

        //[Display(Name = "Serial Number")]
		public string SerialNumber { get; set; }

        //[Display(Name = "Seal Number")]
		public string SealNumber { get; set; }

        public bool IsPrimaryContainer { get; set; }

        //[Display(Name = "Container Amount")]
		public decimal Amount { get; set; }

        //[Display(Name = "Discrepancy Total Container Amount")]
		public decimal? DiscrepancyAmount { get; set; }

		public decimal? ActualAmount { get; set; }

        //[Display(Name = "Bag Reference Number")]
		public string ReferenceNumber { get; set; }

        //[Display(Name = "Deposit Type")]
        public string DepositType { get; set; }

		public int LastChangedById { get; set; }
		public bool ActiveStatus { get; set; }
		public bool IsNotDeleted { get; set; }
        public DateTime LastChangedDate { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreateDate { get; set; }

        public ContainerTypeDto ContainerType { get; set; }
		public List<ContainerDropDto> ContainerDrops { get; set; }
    }
}