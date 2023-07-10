using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class FeeType : EntityBase, IIdentity
    {
        #region Mapped

        public int FeeTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }
        //public virtual Collection<Fee> Fees { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return FeeTypeId; }
        }

        #endregion
    }
}