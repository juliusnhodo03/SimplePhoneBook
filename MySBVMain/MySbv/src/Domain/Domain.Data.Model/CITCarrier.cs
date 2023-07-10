using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CitCarrier : EntityBase, IIdentity
    {
        #region Mapped

        public int CitCarrierId { get; set; }
        public int SerialStartNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        public virtual Collection<Site> Sites { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CitCarrierId; }
        }

        #endregion
    }
}