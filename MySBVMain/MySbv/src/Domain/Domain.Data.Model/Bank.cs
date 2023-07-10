using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Bank : EntityBase, IIdentity
    {
        #region Mapped

        public int BankId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public string BranchCode { get; set; }

        public virtual Collection<Account> Accounts { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return BankId; }
        }

        #endregion
        
    }
}