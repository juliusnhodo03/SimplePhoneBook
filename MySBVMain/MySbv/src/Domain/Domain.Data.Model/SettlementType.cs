﻿using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class SettlementType : EntityBase, IIdentity
    {
        #region Mapped

        public int SettlementTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public virtual Collection<Product> Products { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return SettlementTypeId; }
        }

        #endregion
    }
}