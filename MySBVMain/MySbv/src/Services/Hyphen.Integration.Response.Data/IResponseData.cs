using System.Collections.Generic;

namespace Hyphen.Integration.Response.Data
{
    public interface IResponseData
    {
        //void AddDeposit(string settlementIdentifier, DepositSettlement settlementDetails);
        void AddDeposit(List<DepositSettlement> settlementDetails);

        void UpdateDeposit();

        bool HasRejectedDeposits { get; }

    }
}