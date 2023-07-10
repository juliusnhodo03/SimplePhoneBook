using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto.Users;
using Domain.Data.Model;
using Utility.Core;

namespace Application.Modules.UserAccountValidation
{
    public enum UserType
    {
        Normal,
        Admin
    }

    public enum MySBVUserTypes
    {
        SBV,
        Retail,
        HeadOffice
    }

    public interface IUserAccountValidation
    {
        /// <summary>
        /// Login to the application
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="userType"></param>
        /// <param name="rememberMe"></param>
        /// <returns></returns>
        MethodResult<bool> LogIn(string username, string password, bool rememberMe = false, UserType userType = UserType.Normal);

        /// <summary>
        /// Logout the current logged in user
        /// </summary>
        void LogOf();

        /// <summary>
        /// Assign a list of roles to a user account
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleNames"></param>
        void AddUserToRoles(string userName, List<string> roleNames);

        /// <summary>
        /// Create a new user together with the account information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="createdBy"></param>
        /// <param name="userTypeId"></param>
        /// <param name="userTypes"></param>
        /// <returns></returns>
        string CreateUserAndAccount(UserDto user, int createdBy, int? userTypeId, MySBVUserTypes userTypes);

        /// <summary>
        /// Change a Users password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="currentPassword"></param>
        /// <param name="newPassword"></param>
        /// <param name="userType"></param>
        /// <returns></returns>
        MethodResult<bool> ChangePassword(string userName, string currentPassword, string newPassword, UserType userType = UserType.Normal);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="emailAddress"></param>
        /// <param name="tokenExpirationInMinutesFromNow">
        /// (Optional) The time in minutes until the password reset token expires.
        ///     The default is 120 (2 hours).
        /// </param>
        /// <returns></returns>
        MethodResult<ForgotPasswordResult> ForgotPassword(string username, string emailAddress, int tokenExpirationInMinutesFromNow = 120);

        /// <summary>
        /// Reset a user's password
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ResetPassword(string userName, string password);

        /// <summary>
        /// Reset a user's password using the reset token
        /// </summary>
        /// <param name="token"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool ResetPasswordFromToken(string token, string password);

        /// <summary>
        /// Delete a user account together with the profile
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deletedById"></param>
        /// <returns></returns>
        bool DeleteUser(int id, int deletedById);

        /// <summary>
        /// Set user roles
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        UserDto SetUserRoles(UserDto user);

        /// <summary>
        /// Remove Roles from a user account
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="getRolesForUser"></param>
        void RemoveUserFromRoles(string userName, string[] getRolesForUser);

        /// <summary>
        /// Get roles for a user given by a user name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        string[] GetUserRoles(string userName);

        /// <summary>
        /// Get the id of a user
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        int GetUserId(string userName);

        /// <summary>
        /// Check if the username belongs to the current logged in user.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        bool IsCurrentUser(string username);

        /// <summary>
        /// Get the user associated with a username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User UserByName(string username);

        /// <summary>
        /// Gets the User type by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns>user type id</returns>
        int? UserTypeId(string username);

        /// <summary>
        /// Get The Currently Logged On User
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Logged on user name</returns>
        User LoggedOnUser(string username);

        Task<User> GetLoggedUserAsync(string username);
    }
}