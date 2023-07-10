using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Country : EntityBase, IIdentity
    {
        #region Mapped

        public int CountryId { get; set; }
        public int ContinentId { get; set; }
        public virtual Continent Continent { get; set; }
        public virtual Collection<Currency> Currencies { get; set; }
        public virtual Collection<Province> Provinces { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CountryId; }
        }

        public string ContinentName
        {
            get { return Continent != null ? Continent.Name : string.Empty; }
        }

        #endregion
    }
}