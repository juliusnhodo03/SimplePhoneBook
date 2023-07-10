
using System.Collections.Generic;
using Application.Dto.Vault;
using Vault.Integration.DataContracts;

namespace Application.Dto.FailedVaultTransaction
{
	public class FailedRequestDto
	{
		public int Id { get; set; }
		public RequestMessage RequestMessage { get; set; }
		public string IndexUrl { get; set; }
		public string PostUrl { get; set; }
		public string ApproveUrl { get; set; }
		public string DeclineUrl { get; set; }
		public string SelectedVaultTransactionType { get; set; }
		public bool IsApprover { get; set; }
		public bool IsWrongDate { get; set; }
		public bool SameAsCapturer { get; set; }
		public int? UserId { get; set; }
		public bool IsPending { get; set; }
		public bool HasErrorOnCitCode { get; set; }
		public bool HasErrorOnBeneficiaryCode{ get; set; }
		public bool HasErrorOnTransactionDate { get; set; }
		public bool HasErrorOnTransactionType { get; set; }
		public bool HasErrorOnDeviceSerialNumber { get; set; }
		public bool HasErrorOnCurrencyCode { get; set; }
		public bool HasErrorOnBagSerialNumber { get; set; }
		public bool HasErrorOnUserReferance { get; set; }
		public List<string> Currencies { get; set; }
		public List<string> VaultTransactionTypes { get; set; }
		public List<string> ErrorMessages { get; set; }
		public bool IsGptRequest { get; set; }
		public bool IsCitRequest { get; set; }
	}
}