using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class ApprovalObjects : EntityBase
    {
        #region Mapped

        public int ApprovalObjectsId { get; set; }
        public string NewObject { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return ApprovalObjectsId; }
        }

        #endregion
        
    }
}