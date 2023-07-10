using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class SiteContainer : EntityBase
    {
        #region Mapped

        [Column(Order = 1), Key, ForeignKey("ContainerType")]
        public int ContainerTypeId { get; set; }

        [Column(Order = 0), Key, ForeignKey("Site")]
        public int SiteId { get; set; }
        public virtual Site Site { get; set; }
        public virtual ContainerType ContainerType { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContainerTypeId; }
        }

        public string Name
        {
            get { return Site != null ? Site.Name : string.Empty; }
        }

        public string ContainerTypeName
        {
            get { return ContainerType != null ? ContainerType.Name : string.Empty; }
        }

        #endregion
    }
}