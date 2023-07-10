using System.Collections.Generic;
using Application.Dto.CashOrderTask;

namespace Application.Modules.CashOrdering.Approval
{
    public interface ICashOrderApprovalValidation
    {

        /// <summary>
        /// Return a list of Cash Order Tasks
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListCashOrderTaskDto> All();
        
    }
}
