using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class CashCenter : EntityBase, IIdentity
    {
        #region Mapped

        public int CashCenterId { get; set; }
        public int CityId { get; set; }
        public int AddressId { get; set; }
        public int ClusterId { get; set; }
        public string Number { get; set; }
        public string TelephoneNumber { get; set; }
        public string ContactPerson { get; set; }
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        public string EmailAddress3 { get; set; }
        public virtual Cluster Cluster { get; set; }
        public virtual Address Address { get; set; }
        public virtual City City { get; set; }
        public virtual Collection<User> Users { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CashCenterId; }
        }

        public string ClusterName
        {
            get { return Cluster != null ? Cluster.Name : string.Empty; }
        }

        public string CityName
        {
            get { return City != null ? City.Name : string.Empty; }
        }

        #endregion
    }
}