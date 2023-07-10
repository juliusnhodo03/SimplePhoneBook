using Domain.Data.Core;

namespace Domain.Data.Model
{
    public class Currency : EntityBase, IIdentity
    {
        #region Mapped

        public int CurrencyId { get; set; }
        public int CountryId { get; set; }
        public string Code { get; set; }
        public string Symbol { get; set; }
        public virtual Country Country { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string LookUpKey { get; set; }

        #endregion

        #region Not Mapped

        public override int Key
        {
            get { return CurrencyId; }
        }

        #endregion
    }
}