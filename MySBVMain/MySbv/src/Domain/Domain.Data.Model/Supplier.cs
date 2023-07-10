using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Supplier : EntityBase, IIdentity
    {
        #region Mapped

        public int SupplierId { get; set; }
        public virtual Collection<Device> Devices { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return SupplierId; }
        }

        #endregion
    }
}