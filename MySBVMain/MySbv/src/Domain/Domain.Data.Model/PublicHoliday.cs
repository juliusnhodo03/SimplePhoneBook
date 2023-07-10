using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class PublicHoliday : EntityBase, IIdentity
    {
        #region Mapped

        public int PublicHolidayId { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return PublicHolidayId; }
        }

        #endregion
    }
}