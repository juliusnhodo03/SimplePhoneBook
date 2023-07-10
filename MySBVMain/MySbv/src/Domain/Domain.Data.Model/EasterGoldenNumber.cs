using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class EasterGoldenNumber : EntityBase
    {
        #region Mapped

        public int EasterGoldenNumberId { get; set; }
        public int GoldenNumber { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return EasterGoldenNumberId; }
        }

        #endregion
    }
}