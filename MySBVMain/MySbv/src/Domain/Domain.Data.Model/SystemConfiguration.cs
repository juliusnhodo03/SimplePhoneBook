using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class SystemConfiguration : EntityBase, IIdentity
    {
        #region Mapped

        public int SystemConfigurationId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        public string Value { get; set; }

        #endregion
        
        #region Not Mapped

        public override int Key
        {
            get { return SystemConfigurationId; }
        }

        #endregion
        
    }
}