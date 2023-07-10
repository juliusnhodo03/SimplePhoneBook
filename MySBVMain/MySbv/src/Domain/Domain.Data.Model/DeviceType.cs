using System.Collections.ObjectModel;
using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class DeviceType : EntityBase, IIdentity
    {
        #region Mapped

        public int DeviceTypeId { get; set; }
        public int SupplierId { get; set; }
        public int ManufacturerId { get; set; }
        public string Model { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual Collection<CashDeposit> CashDeposits { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return DeviceTypeId; }
        }

        public string ManufacturerName
        {
            get { return Manufacturer != null ? Manufacturer.Name : string.Empty; }
        }

        #endregion
    }
}