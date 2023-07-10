using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Geography : EntityBase, IIdentity
    {
        #region Mapped

        public int GeographyId { get; set; }
        public virtual Collection<Continent> Continents { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return GeographyId; }
        }

        #endregion
    }
}