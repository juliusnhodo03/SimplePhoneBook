using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class AddressType : EntityBase, IIdentity
    {
        #region Mapped

        public int AddressTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public virtual Collection<Address> Addresses { get; set; }

        #endregion
        
        #region Not Mapped

        public override int Key
        {
            get { return AddressTypeId; }
        }

        #endregion
        
    }
}