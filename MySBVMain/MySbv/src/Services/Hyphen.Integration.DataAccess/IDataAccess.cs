using System.Collections.Generic;

namespace Hyphen.Integration.DataAccess
{
    public interface IDataAccess
    {
        bool HasRejectedDeposits { get; }

        void UpdateDeposit();

        void UpdateSettlementStatus();

        IEnumerable<SettlementFile> GetProcessedDeposites();

        void AddDepositId(string depositId, DepositSettlementDetails settlementDetails);
    }
}