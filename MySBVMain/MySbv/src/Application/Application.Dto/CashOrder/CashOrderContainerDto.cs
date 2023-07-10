using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashOrder
{
    public class CashOrderContainerDto
	{
		public int CashOrderContainerId { get; set; }

		[Display(Name = "Serial Number")]
		public string SerialNumber { get; set; }

		[Display(Name = "Total Container Amount")]
		public decimal Amount { get; set; }
		public decimal VerifiedAmount { get; set; }
		public decimal PackedAmount { get; set; }
        public int LastChangedById { get; set; }
        public bool IsNotDeleted { get; set; } 
        public int CreatedById { get; set; }
		public DateTime CreateDate { get; set; }
		public virtual List<CashOrderContainerDropDto> CashOrderContainerDrops { get; set; }

    }
}