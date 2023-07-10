using Domain.Data.Core;

namespace Domain.Data.Model
{
	public class VaultBeneficiary : EntityBase
	{
        #region Mapped

        public int VaultBeneficiaryId { get; set; }
		public int CashDepositId { get; set; }
		public int ContainerDropId { get; set; }
		public int? AccountId { get; set; }
		public string DeviceUserName { get; set; }
		public string DeviceUserRole { get; set; }
		public Account Account { get; set; }
		public ContainerDrop ContainerDrop { get; set; }
		public CashDeposit CashDeposit { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return VaultBeneficiaryId; }
        }

        #endregion
        
	}
}
