using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Device : EntityBase, IIdentity
    {
        #region Mapped

        public int DeviceId { get; set; }
        public int? DeviceTypeId { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public virtual DeviceType DeviceType { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return DeviceId; }
        }

        public string DeviceTypeName
        {
            get { return DeviceType != null ? DeviceType.Name : string.Empty; }
        }

        #endregion
    }
}