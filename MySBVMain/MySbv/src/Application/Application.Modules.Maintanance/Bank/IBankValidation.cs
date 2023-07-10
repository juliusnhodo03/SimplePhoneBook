using System.Collections.Generic;
using Application.Dto.Bank;
using Utility.Core;

namespace Application.Modules.Maintanance.Bank
{
    public interface IBankValidation
    {

        /// <summary>
        /// Return a list of banks
        /// </summary>
        /// <returns></returns>
        IEnumerable<BankDto> All();

        /// <summary>
        /// Add A New Bank
        /// </summary>
        /// <param name="bankDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(BankDto bankDto, string username);

        /// <summary>
        /// Update a new Bank
        /// </summary>
        /// <param name="bankDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(BankDto bankDto, string username);

        /// <summary>
        /// Delete a Bank
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);

        /// <summary>
        /// Find a Bank by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<BankDto> Find(int id);

        /// <summary>
        /// Check if the Bank is still Active before deleting
        /// </summary>
        /// <param name="bankId"></param>
        /// <returns></returns>
        bool IsActive(int bankId);

        /// <summary>
        /// Add A New Bank
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<bool> IsInUse(int id);

        /// <summary>
        /// Check if the Bank is in use by another Bank
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsBankNameInUse(string name);

        /// <summary>
        /// Check if the Branch Code is in use by another Bank
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        bool IsBranchCodeInUse(string code);
        
        /// <summary>
        /// Check if the Branch Code is in use by another Bank (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherBank(string name, int id);

        /// <summary>
        /// Check if the Bank Name is in use by another Bank (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="code"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool CodeUsedByAnotherBank(string code, int id);
        
    }
}