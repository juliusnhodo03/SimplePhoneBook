﻿using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Province : EntityBase, IIdentity
    {
        #region Mapped

        public int ProvinceId { get; set; }
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual Collection<City> Cities { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ProvinceId; }
        }

        public string CountryName
        {
            get { return Country != null ? Country.Name : string.Empty; }
        }

        #endregion
    }
}