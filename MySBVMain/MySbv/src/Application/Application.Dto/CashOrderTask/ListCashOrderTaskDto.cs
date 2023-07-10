using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashOrderTask
{
    public class ListCashOrderTaskDto
    {
        [ScaffoldColumn(false)]
        public int CashOrderTaskId { get; set; }
        public int CashOrderId { get; set; }
        
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }
        
        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Site")]
        public int SiteId { get; set; }
        
        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Display(Name = "Link")]
        public string RequestUrl { get; set; }

        [Display(Name = "Total Amount")]
        public string CashOrderAmount { get; set; }
        
    }
}