using System.Collections.Generic;
using Domain.Data.Nedbank.Model;

namespace Nedbank.Integration.SettlementHub
{
    public interface ISettlement
    {
        /// <summary>
        /// Apply settlements to Cash deposits from ACK, NACK, DUPLICATE, SECOND-SCK Files.
        /// </summary>
        /// <param name="details"></param>
        void ApplySettlements(IEnumerable<NedbankResponseDetail> details);
        
        /// <summary>
        /// All unpaid transactions are set to SETTLEMENT_REJECTED.
        /// The UnPaid Reason is also supplied.
        /// </summary>
        /// <param name="unpaids"></param>
        void ApplyUnPaidRejections(IEnumerable<NedbankUnpaidOrNaedo> unpaids);

        /// <summary>
        /// Apply duplicate rejections.
        /// First see see if another file ran updates before.
        /// if applied just log a duplicate entry without updating deposits, otherwise update.
        /// </summary>
        /// <param name="duplicateFile"></param>
        void ApplyDuplicateRejections(NedbankDuplicate duplicateFile);
    }
}