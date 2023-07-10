using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Application.Dto.CashOrder
{
    public class CashOrderDto
    {
        [ScaffoldColumn(false)]
        public int CashOrderId { get; set; }

        [Display(Name = "Seal / Serial Number")]
        public string SealSerialNumber { get; set; }

        public int? UserTypeId { set; get; }

        [Display(Name = "Order type")]
        public int CashOrderTypeId { get; set; }

        public int CashOrderContainerId { get; set; }

        [Display(Name = "Site")]
        public int SiteId { get; set; }

        [Display(Name = "Merchant")]
        public int MerchantId { get; set; }

        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public string SiteName { get; set; }
        public string MerchantName { get; set; }

        public bool IsSubmitted { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public bool IsProcessed { get; set; }
        public DateTime? DateProcessed { get; set; }

        [Display(Name = "Order Date")]
        public DateTime? OrderDate { get; set; }

        public string OrderDateString { get; set; }

        [Display(Name = "Order Reference")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [Display(Name = "Captured Date and Time")]
        public DateTime CapturedDateTime { get; set; }

        public string CapturedDateString { get; set; }
        public DateTime DeliveryDateDto { get; set; }

        [Display(Name = "Container number with cash for Exchange")]
        public string ContainerNumberWithCashForExchange { get; set; }

        [Display(Name = "Bag # to Dispatch Cash to Client")]
        public string BagNumberToDispatchLabel { get; set; }

        [Display(Name = "Empty Container / Bag number")]
        public string EmptyContainerOrBagNumber { get; set; }

        [Display(Name = "Delivery Date")]
        public string DeliveryDate { get; set; }

        [Display(Name = "Cash Order Amount")]
        public decimal? CashOrderAmount { get; set; }

        [Display(Name = "Current Comments")]
        public string CurrentComments { get; set; }

        [Display(Name = "Previous Comments")]
        public string Comments { get; set; }

        public List<string> Attachments { get; set; }
        
        public int InitialCitSerialNumber { get; set; }
        public int CreatedById { get; set; }
        public DateTime CreateDate { get; set; }
        public State EntityState { get; set; }
        public CashOrderContainerDto CashOrderContainer { get; set; }
        public CashOrderTypeDto CashOrderType { get; set; }

        [NotMapped]
        public bool IsSerialNumber { get; set; }
    }
}