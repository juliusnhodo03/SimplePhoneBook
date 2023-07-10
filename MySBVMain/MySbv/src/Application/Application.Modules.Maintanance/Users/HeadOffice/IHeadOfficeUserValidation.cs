using System.Collections.Generic;
using Application.Dto.Users;
using Application.Modules.Common;
using Utility.Core;

namespace Application.Modules.Maintanance.Users.HeadOffice
{
    public interface IHeadOfficeUserValidation
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
        /// <param name="function"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool UserIdExist(string id, string userName, Function function = Function.Add);
        
        /// <summary>
        /// Get All Head office users
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserDto> All();
        
        /// <summary>
        /// Find a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<UserDto> Find(int id);

        /// <summary>
        /// Add a new Head Office user
        /// </summary>
        /// <param name="headOfficeUserDto"></param>
        /// <param name="usename"></param>
        /// <returns></returns>
        MethodResult<UserDto> Add(UserDto headOfficeUserDto, string usename);

        /// <summary>
        /// Update an existing head office user
        /// </summary>
        /// <param name="cashCenterUserDto"></param>
        /// <param name="usename"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(UserDto cashCenterUserDto, string usename);

        /// <summary>
        /// Delete an existing head office user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="usename"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string usename);
         
    }
}