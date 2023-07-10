

namespace Vault.Integration.Service.Infrastructure
{
    #region Enums

    public enum DenominationType
    {
        Notes,
        Coins
    }

    #endregion

// ReSharper disable once InconsistentNaming
    public class iTramsDenomination
    {
        public int Count { get; set; }
        public DenominationType DenominationType { get; set; }
    }
}