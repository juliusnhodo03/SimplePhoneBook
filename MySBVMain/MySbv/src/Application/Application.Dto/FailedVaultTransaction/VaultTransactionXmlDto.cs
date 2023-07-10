
using System;
using System.Xml;

namespace Application.Dto.FailedVaultTransaction
{
	public class VaultTransactionXmlDto
	{
		public int VaultTransactionXmlId { get; set; }
		public int StatusId { get; set; }
		public string BagSerialNumber { get; set; }
		public string ErrorMessages { get; set; }
		public string XmlMessage { get; set; }
		public string XmlAwaitingApproval { get; set; }
		public int? ApprovedById { get; set; }
		public DateTime? ApprovedDate { get; set; }
		public bool IsNotDeleted { get; set; }
		public int? LastChangedById { get; set; }
		public DateTime LastChangedDate { get; set; }
		public int? CreatedById { get; set; }
		public DateTime? CreateDate { get; set; }
	}
}
