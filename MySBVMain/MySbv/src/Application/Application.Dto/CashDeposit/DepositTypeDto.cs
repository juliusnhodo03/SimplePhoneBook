using System;
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashDeposit
{
    public class DepositTypeDto
    {
        public int DepositTypeId { get; set; }

        [Display(Name = "Deposit Type")]
        public string Name { get; set; }

        [Display(Name = "Deposit Type Description")]
        public string Description { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreateDate { get; set; }
    }
}