using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class UserType : EntityBase, IIdentity
    {
        #region Mapped

        public int UserTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return UserTypeId; }
        }

        #endregion
    }
}