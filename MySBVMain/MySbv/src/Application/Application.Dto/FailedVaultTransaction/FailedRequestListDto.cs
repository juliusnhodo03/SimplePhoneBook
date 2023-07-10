
using System.ComponentModel.DataAnnotations;

namespace Application.Dto.FailedVaultTransaction
{
	public class FailedRequestListDto
	{
		public int FailedRequestId { get; set; }
		public int Id { get; set; }

		[Display(Name = "Cit Code")]
		public string CitCode { get; set; }


		[Display(Name = "Beneficiary Code")]
		public string BeneficiaryCode { get; set; }


		[Display(Name = "Transaction Type")]
		public string TransactionType { get; set; }


		[Display(Name = "Serial Number")]
		public string BagSerialNumber { get; set; }

		public bool IsCitMessage { get; set; }


		[Display(Name = "Supplier")]
		public string Supplier { get; set; }


		[Display(Name = "Status")]
		public string StatusName { get; set; }


		[Display(Name = "Amount")]
		public string TotalDepositAmount { get; set; }

		[Display(Name = "Errors")]
		public string Errors { get; set; }
	}
}
