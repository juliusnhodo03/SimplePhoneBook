using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class City : EntityBase, IIdentity
    {
        #region Mapped

        public int CityId { get; set; }
        public int ProvinceId { get; set; }
        public virtual Province Province { get; set; }
        public virtual Collection<CashCenter> CashCenters { get; set; }
        public virtual Collection<Site> Sites { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CityId; }
        }

        public string ProvinceName
        {
            get { return Province != null ? Province.Name : string.Empty; }
        }

        #endregion
    }
}