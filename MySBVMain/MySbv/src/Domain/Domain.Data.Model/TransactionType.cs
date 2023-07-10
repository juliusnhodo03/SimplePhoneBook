using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class TransactionType : EntityBase, IIdentity
    {
        #region Mapped

        public int TransactionTypeId { get; set; }
        public virtual Collection<Account> Accounts { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return TransactionTypeId; }
        }

        #endregion
    }
}