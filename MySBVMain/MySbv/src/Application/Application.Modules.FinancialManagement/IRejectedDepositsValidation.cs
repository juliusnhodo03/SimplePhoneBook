using System.Collections.Generic;
using Application.Dto.FinancialManagement;
using Utility.Core;

namespace Application.Modules.FinancialManagement 
{ 
    public interface IRejectedDepositsValidation
    {
        /// <summary>
        /// Get All Rejected Deposits
        /// </summary>
        /// <returns></returns>
        MethodResult<IEnumerable<RejectedDepositDto>> All();

        /// <summary>
        /// Find rejected Transaction by its ID
        /// </summary>
        /// <param name="cashDepositId"></param>
        /// <param name="containerDropId"></param>
        /// <param name="paymentId"></param>
        MethodResult<RejectedDepositDto> FindRejectedRecord(int cashDepositId, int containerDropId, int paymentId);

        /// <summary>
        ///     Resubmit a deposit or payment.
        /// </summary>
        /// <param name="cashDepositId"></param>
        /// <param name="paymentId"></param>
        /// <param name="userId"></param>
        /// <param name="containerDropId"></param>
        MethodResult<bool> Submit(int cashDepositId, int containerDropId, int paymentId, int userId);
    }
}