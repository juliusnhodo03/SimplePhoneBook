using System;
using System.ComponentModel.DataAnnotations;
using Domain.Data.Core;

namespace Domain.Data.Model
{
	public class CashOrder : EntityBase
	{
		#region Mapped

		public int CashOrderId { get; set; }
		public int CashOrderTypeId { get; set; }
		public int CashOrderContainerId { get; set; }
		public int SiteId { get; set; }
		public int StatusId { get; set; }
		public bool IsSubmitted { get; set; }
		public bool IsProcessed { get; set; }

		[StringLength(50)]
		[Required]
		public string ReferenceNumber { get; set; }

		public string ContainerNumberWithCashForExchange { get; set; }
		public string EmptyContainerOrBagNumber { get; set; }
		public string DeliveryDate { get; set; }
		public decimal CashOrderAmount { get; set; }
		public DateTime? DateSubmitted { get; set; }
		public DateTime? DateProcessed { get; set; }
		public DateTime? OrderDate { get; set; }
        public string Comments { get; set; }
		public virtual CashOrderContainer CashOrderContainer { get; set; }
		public virtual CashOrderType CashOrderType { get; set; }
		public virtual Site Site { get; set; }
		public virtual Status Status { get; set; }

		#endregion

		#region Not Mapped

		public override int Key
		{
			get { return CashOrderId; }
		}

		public string CashOrderTypeName
		{
			get { return CashOrderType != null ? CashOrderType.Name : string.Empty; }
		}

		public string SiteName
		{
			get { return Site != null ? Site.Name : string.Empty; }
		}

		public string CitCode
		{
			get { return Site != null ? Site.CitCode : string.Empty; }
		}

		public string StatusName
		{
			get { return Status != null ? Status.Name : string.Empty; }
		}

		public string MerchantName
		{
			get { return Site != null ? Site.MerchantName : string.Empty; }
		}

		public int MerchantId
		{
			get { return Site != null ? Site.MerchantId : 0; }
		}

		#endregion
	}
}