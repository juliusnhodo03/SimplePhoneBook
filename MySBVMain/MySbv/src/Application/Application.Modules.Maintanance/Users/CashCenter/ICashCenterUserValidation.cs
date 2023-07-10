using System.Collections.Generic;
using Application.Dto.Users;
using Application.Modules.Common;
using Utility.Core;

namespace Application.Modules.Maintanance.Users.CashCenter
{
    public interface ICashCenterUserValidation
    {
        /// <summary>
        ///     Check if a user name already exists on the system
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool UserNameExist(string userName);

        /// <summary>
        ///     Check if the ID Number already exists on the system.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        bool UserIdNumberExist(string id, string username, Function function = Function.Add);

        /// <summary>
        ///     Return all Cash Center Users.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CashCenterUserDto> All();

        /// <summary>
        ///     Find a cash center user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<CashCenterUserDto> Find(int id);

        /// <summary>
        ///     Add a cash center user
        /// </summary>
        /// <param name="cashCenterUserDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<CashCenterUserDto> Add(CashCenterUserDto cashCenterUserDto, string username);

        /// <summary>
        ///     Update a cash center user
        /// </summary>
        /// <param name="cashCenterUserDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(CashCenterUserDto cashCenterUserDto, string username);

        /// <summary>
        ///     Delete a cash center user.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);
    }
}