using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CompanyType : EntityBase, IIdentity
    {
        #region Mapped

        public int CompanyTypeId { get; set; }
        public virtual Collection<Merchant> Merchants { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CompanyTypeId; }
        }

        #endregion
    }
}