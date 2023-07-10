namespace Nedbank.Integration.ResponseManager
{
    public interface ISettlementResponseHandling
    {
        bool HasRejectedDeposits { get; }

        void AddDeposit(string settlementIdentifier, DepositSettlement settlementDetails);

        void UpdateDeposit();
    }
}