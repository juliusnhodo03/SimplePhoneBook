using System.Collections.Generic;

namespace Nedbank.Integration.Request.Data
{
    public interface IRequestData
    {
        /// <summary>
        /// </summary>
        /// <returns></returns>
        IEnumerable<SettlementRecord> GetSettlementRecords(); 

        /// <summary>
        /// </summary>
        void UpdateSettlementStatus();
    }
}