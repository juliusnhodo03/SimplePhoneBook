using System;
using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Product : EntityBase
    {
        #region Mapped

        public int ProductId { get; set; }
        public int ProductTypeId { get; set; }
        public int? DeviceTypeId { get; set; }
        public int? DeviceId { get; set; }
        public int SiteId { get; set; }
        public int ServiceTypeId { get; set; }
        public int SettlementTypeId { get; set; }
        public int StatusId { get; set; }
        public bool PublicHolidayInclInFeeFlag { get; set; }
        public DateTime ImplementationDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public virtual ProductType ProductType { get; set; }
        public virtual DeviceType DeviceType { get; set; }
        public virtual Device Device { get; set; }
        public virtual Site Site { get; set; }
        public virtual ServiceType ServiceType { get; set; }
        public virtual SettlementType SettlementType { get; set; }
        public virtual Status Status { get; set; }
        public virtual Collection<ProductFee> ProductFees { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ProductId; }
        }


        public string ProductTypeName
        {
            get { return ProductType != null ? ProductType.Name : string.Empty; }
        }

        public string ServiceTypeName
        {
            get { return ServiceType != null ? ServiceType.Name : string.Empty; }
        }

        public string SettlementTypeName
        {
            get { return SettlementType != null ? SettlementType.Name : string.Empty; }
        }

        public string StatusName
        {
            get { return Status != null ? Status.Name : string.Empty; }
        }

        public string SiteName
        {
            get { return Site != null ? Site.Name : string.Empty; }
        }


        #endregion
    }
}