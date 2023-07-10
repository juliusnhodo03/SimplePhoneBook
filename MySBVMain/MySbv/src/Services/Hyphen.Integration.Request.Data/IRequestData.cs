using System.Collections.Generic;

namespace Hyphen.Integration.Request.Data
{
    public interface IRequestData
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerable<SettlementTransaction> GetSettlementTransactions();

        /// <summary>
        /// 
        /// </summary>
        void UpdateSettlementStatus();
    }
}