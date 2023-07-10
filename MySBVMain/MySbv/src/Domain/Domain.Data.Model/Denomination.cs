using System.Collections.Generic;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Denomination : EntityBase, IIdentity
    {
        #region Mapped

        public int DenominationId { get; set; }
        public int CountryId { get; set; }
        public int DenominationTypeId { get; set; }
        public int ValueInCents { get; set; }
        public virtual DenominationType DenominationType { get; set; }
        public virtual List<ContainerDropItem> ContainerDropItems { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return DenominationId; }
        }

        public string DenominationTypeName
        {
            get { return DenominationType != null ? DenominationType.Name : string.Empty; }
        }

        #endregion
    }
}