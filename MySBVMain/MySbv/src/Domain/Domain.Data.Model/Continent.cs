using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Continent : EntityBase, IIdentity
    {
        #region Mapped

        public int ContinentId { get; set; }
        public int GeographyId { get; set; }
        public virtual Geography Geography { get; set; }
        public virtual Collection<Country> Countries { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContinentId; }
        }

        public string GeographyName
        {
            get { return Geography != null ? Geography.Name : string.Empty; }
        }

        #endregion
    }
}