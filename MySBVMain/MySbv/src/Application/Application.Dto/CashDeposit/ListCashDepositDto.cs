using System.ComponentModel.DataAnnotations;

namespace Application.Dto.CashDeposit
{
    public class ListCashDepositDto
    {
        public int CashDepositId { get; set; }

        [Display(Name = "Deposit Number")]
		public string TransactionReference { get; set; } 

        [Display(Name = "Serial Number")]
        public string ContainerSerialNumber { get; set; }

        [Display(Name = "Deposit Type")]
        public string CashDepositType { get; set; }

        [Display(Name = "Deposit Date")]
        public string CaptureDate { get; set; }

        [Display(Name = "Amount")]
        public string TotalDepositAmount { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Site Name")]
        public string SiteName { get; set; }

        [Display(Name = "Account")]
        public string BankAccount { get; set; }

        public int? UserTypeId { get; set; }
    }
}