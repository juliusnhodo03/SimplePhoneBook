using System.ComponentModel.DataAnnotations;

namespace Application.Dto.FailedVaultTransaction
{
	public class FailedTransactionHeaderDto
	{
		public int? Id { get; set; }

		[Display(Name = "Beneficiary Code")]
		public string BeneficiaryCode { get; set; }

		[Display(Name = "Cit Code")]
		public string CitCode { get; set; }

		public bool IsCitMessage { get; set; } 

		[Display(Name = "Serial Number")]
		public string SerialNumber { get; set; }

		[Display(Name = "Supplier")]
		public string Supplier { get; set; }

		[Display(Name = "Cit Status")]
		public string CitReceivedStatus { get; set; } 
	}
}