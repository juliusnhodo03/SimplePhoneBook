using System;
using System.ComponentModel.DataAnnotations;
using Application.Dto.Account;
using Application.Dto.CashDeposit;
using Application.Dto.Merchant;
using Application.Dto.Site;
using Application.Dto.Users;
using Domain.Data.Model;

namespace Application.Dto.Task
{
    public class TaskDto
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

        [Display(Name = "Account")]
        public int AccountId { get; set; }

        [Display(Name = "Approval Objects")]
        public int? ApprovalObjectsId { get; set; }


        [Display(Name = "Merchant")]
        public int MerchantId { get; set; }

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
        public int UserId { get; set; }

        [Display(Name = "Module")]
        public string Module { get; set; }

        [Display(Name = "Link")]
        public string Link { get; set; }

        [Display(Name = "Is Executed")]
        public bool IsExecuted { get; set; }

        public virtual AccountDto Account { get; set; }
        public virtual MerchantDto Merchant { get; set; }
        public virtual SiteDto Site { get; set; }
        public virtual UserDto User { get; set; }
        public virtual StatusDto Status { get; set; }
        public virtual ApprovalObjects ApprovalObjects { get; set; }
        


    }
}