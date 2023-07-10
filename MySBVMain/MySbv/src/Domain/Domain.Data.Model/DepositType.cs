using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class DepositType : EntityBase, IIdentity
    {
        #region Mapped

        public int DepositTypeId { get; set; }
        public virtual Collection<CashDeposit> CashDeposits { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return DepositTypeId; }
        }

        #endregion
    }
}