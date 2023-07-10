using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.Task
{
    public class ListTaskDto
    {
        [ScaffoldColumn(false)]
        public int TaskId { get; set; }

        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Site")]
        public int SiteId { get; set; }

        [Display(Name = "Approval Objects")]
        public int? ApprovalObjectsId { get; set; }

        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Display(Name = "Merchant Name")]
        public string MerchantName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Status")]
        public int StatusId { get; set; }

        [Display(Name = "Status Name")]
        public string StatusName { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [Display(Name = "User")]
        public string UserId { get; set; }

        [Display(Name = "Module")]
        public string Module { get; set; }

    }
}