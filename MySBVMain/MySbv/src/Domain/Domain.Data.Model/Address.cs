using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Address : EntityBase
    {
        #region Mapped

        public int AddressId { get; set; }

        public int AddressTypeId { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string AddressLine3 { get; set; }

        public string PostalCode { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public virtual AddressType AddressType { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return AddressId; }
        }

        public string AddessTypName 
        {
            get { return AddressType != null ? AddressType.Name : string.Empty; }
        }

        #endregion
    }
}