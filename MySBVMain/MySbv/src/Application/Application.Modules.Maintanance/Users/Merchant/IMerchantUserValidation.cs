using System.Collections.Generic;
using Application.Dto.Users;
using Application.Modules.Common;
using Utility.Core;

namespace Application.Modules.Maintanance.Users.Merchant
{
    public interface IMerchantUserValidation
    {
        /// <summary>
        /// Check if username already exist of the system
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool UserExist(string userName);

        /// <summary>
        /// Check if the Id Number already exist on the system
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        bool UserIdExist(string id, string userName, Function function = Function.Add);

        /// <summary>
        /// Get all Merchant / Retail Users
        /// </summary>
        /// <returns></returns>
        IEnumerable<MerchantUserDto> All();

        /// <summary>
        /// Find a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<MerchantUserDto> Find(int id);

        /// <summary>
        /// Add a new merchant / retail User
        /// </summary>
        /// <param name="merchantUserDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<MerchantUserDto> Add(MerchantUserDto merchantUserDto, string username);

        /// <summary>
        /// Update a merchant / retail user
        /// </summary>
        /// <param name="merchantUserDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(MerchantUserDto merchantUserDto, string username);

        /// <summary>
        /// Delete a merchant /retail user account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);

    }
}