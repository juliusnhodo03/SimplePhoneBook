using System.Collections.Generic;
using Application.Dto.CashCenter;
using Utility.Core;

namespace Application.Modules.Maintanance.CashCenter
{
    public interface ICashCenterValidation
    {
        /// <summary>
        /// Return a list of all Cash Center
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListCashCenterDto> All();

        /// <summary>
        /// Find a Cash Center by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<CashCentreDto> Find(int id);

        /// <summary>
        /// Add A New Cash Center
        /// </summary>
        /// <param name="cashCenter"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(CashCentreDto cashCenter, string username);

        /// <summary>
        /// Update a new Cash Center
        /// </summary>
        /// <param name="cashCenter"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(CashCentreDto cashCenter, string username);

        /// <summary>
        /// Delete a Cash Center
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);

        /// <summary>
        /// Check if Cash Center Exists by Name is Use
        /// </summary>
        /// /// <param name="name"></param>
        /// <returns></returns>
        bool ExistsByName(string name);
        
        /// <summary>
        /// Check if Cash Center Exists by Number is Use
        /// </summary>
        /// /// <param name="number"></param>
        /// <returns></returns>
        bool ExistsByNumber(string number);

        /// <summary>
        /// Check if Cash Center Name used by another Bank
        /// </summary>
        /// /// <param name="name"></param>
        /// /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherCashCenter(string name, int id);
    }
}