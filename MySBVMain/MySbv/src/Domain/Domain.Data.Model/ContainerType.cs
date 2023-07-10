using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class ContainerType : EntityBase, IIdentity
    {
        #region Mapped

        public int ContainerTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        public virtual Collection<Container> Containers { get; set; }
        public virtual Collection<ContainerTypeAttribute> ContainerTypeAttributes { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ContainerTypeId; }
        }

        #endregion
    }
}