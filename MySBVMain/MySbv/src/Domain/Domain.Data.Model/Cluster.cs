using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Cluster : EntityBase, IIdentity
    {
        #region Mapped

        public int ClusterId { get; set; }
        public int? RegionManagerId { get; set; }
        public virtual Collection<CashCenter> CashCenters { get; set; }

        [ForeignKey("RegionManagerId")]
        public virtual User User { get; set; }
        public string Name { get; set; }
        public string Description { set; get; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ClusterId; }
        }

        public string RegionManager
        {
            get { return User != null ? User.FirstName + " " + User.LastName : string.Empty; }
        }

        #endregion
    }
}