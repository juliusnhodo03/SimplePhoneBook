using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class AccountType : EntityBase, IIdentity
    {
        #region Mapped

        public int AccountTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public virtual Collection<Account> Accounts { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return AccountTypeId; }
        }

        #endregion
        
    }
}