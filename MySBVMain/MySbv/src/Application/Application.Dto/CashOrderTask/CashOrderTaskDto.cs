using System;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Account;
using Application.Dto.CashDeposit;
using Application.Dto.Site;
using Application.Dto.Users;

namespace Application.Dto.CashOrderTask
{
    public class CashOrderTaskDto
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

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }
        
        [Display(Name = "User")]
        public int UserId { get; set; }

        [Display(Name = "Link")]
        public string RequestUrl { get; set; }

        public virtual SiteDto Site { get; set; }
        public virtual UserDto User { get; set; }
        public virtual StatusDto Status { get; set; }
        
    }
}