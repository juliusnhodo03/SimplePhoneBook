using System.Collections.Generic;
using Application.Dto.Task;
using Utility.Core;

namespace Application.Modules.Maintanance.Approval
{
    public interface IApprovalValidation
    {

        /// <summary>
        /// Return a list of banks
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListTaskDto> All();

        /// <summary>
        /// Find a Bank by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<TaskDto> Find(int id);

    }
}