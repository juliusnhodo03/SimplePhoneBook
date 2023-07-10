using System.ComponentModel.DataAnnotations;
using Application.Dto.Site;

namespace Application.Dto.CashOrder
{
    public class ListCashOrderDto
    {
        [ScaffoldColumn(false)]
        public int CashOrderId { get; set; }

        [Display(Name = "Transaction Number")]
		public string ReferenceNumber { get; set; } 

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

		[Display(Name = "Site Name")]
		public string SiteName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; } 
        
        [Display(Name = "CIT Code")]
        public string CitCode { get; set; }

        [Display(Name = "Delivery Date")]
        public string DeliveryDate { get; set; }

        [Display(Name = "Bag number")]
        public string BagNumber { get; set; }

        [Display(Name = "Order type")]
        public string OrderType { get; set; }

        [Display(Name = "Total Amount")]
		public string CashOrderAmount { get; set; }
    }
}