using System;
using System.Collections.Generic;
using Application.Dto.CashDeposit;

namespace Application.Dto.CashOrder
{
    public class CashOrderContainerDropDto
	{
		public int CashOrderContainerDropId { get; set; }
		public int CashOrderContainerId { get; set; }
		public decimal Amount { get; set; }
		public decimal? VerifiedAmount { get; set; }
		public decimal? PackedAmount { get; set; }
		public bool IsCashRequiredInExchange { get; set; }
		public bool IsCashForwardedForExchange { get; set; }
        public int LastChangedById { get; set; }
        public bool IsNotDeleted { get; set; }
        public DateTime LastChangedDate { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreateDate { get; set; }
		public StatusDto Status { get; set; }

		public virtual List<CashOrderContainerDropItemDto> CashOrderContainerDropItems { get; set; }
    }
}