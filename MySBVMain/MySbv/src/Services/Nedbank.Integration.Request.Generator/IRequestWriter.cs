using Domain.Data.Nedbank.Model;

namespace Nedbank.Integration.Request.Generator
{
    public interface IRequestWriter
    {
        /// <summary>
        /// Get Deposits Awaiting settlement.
        /// This happens:
        /// 1.  When deposits captured on web are processed at a cash center.
        /// 2.  When a Payment message has been send from a device.
        /// 3.  When CIT message from device is send to close a bag.
        /// </summary>
        /// <param name="batch"></param>
        void CollectRecordsToSettle(ref NedbankBatchFile batch);

        /// <summary>
        /// This runs after an instruction file has been droped to Connect Direct.
        /// Update Deposits send to Nedbank as "Pending Settlement".
        /// </summary>
        void UpdateSettlementStatus();
    }
}