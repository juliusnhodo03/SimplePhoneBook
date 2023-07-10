using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Fee : EntityBase, IIdentity
    {
        #region Mapped

        public int FeeId { get; set; }
        public int FeeTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
        public double Value { get; set; }
        public string LookUpKey { get; set; }
        public virtual FeeType FeeType { get; set; }
        
        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return FeeId; }
        }

        #endregion
    }
}