using System;

namespace Domain.Security
{
    /// <summary>
    ///     Provides security and authentication features for ASP.NET Web Pages applications, including the ability to create
    ///     user accounts, log users in and out, reset or change passwords, and perform related tasks.
    /// </summary>
    public interface ISecurity
    {
        /// <summary>
        ///     Represents the key to the enableSimpleMembership value in the
        ///     <see
        ///         cref="P:System.Configuration.ConfigurationManager.AppSettings" />
        ///     property.
        /// </summary>
        string EnableSimpleMembershipKey { get; }

        /// <summary>
        ///     Gets the ID for the current user.
        /// </summary>
        /// <returns>
        ///     The ID for the current user.
        /// </returns>
        int CurrentUserId { get; }

        /// <summary>
        ///     Gets the user name for the current user.
        /// </summary>
        /// <returns>
        ///     The name of the current user.
        /// </returns>
        string CurrentUserName { get; }

        /// <summary>
        ///     Gets a value that indicates whether the current user has a user ID.
        /// </summary>
        /// <returns>
        ///     true if the user has a user ID; otherwise, false.
        /// </returns>
        bool HasUserId { get; }

        /// <summary>
        ///     Gets the authentication status of the current user.
        /// </summary>
        /// <returns>
        ///     true if the current user is authenticated; otherwise, false. The default is false.
        /// </returns>
        bool IsAuthenticated { get; }

        /// <summary>
        ///     Initializes the membership system by connecting to a database that contains user information and optionally creates
        ///     membership tables if they do not already exist.
        /// </summary>
        /// <param name="connectionStringName">
        ///     The name of the connection string for the database that contains user information.
        ///     If you are using SQL Server Compact, this can be the name of the database file (.sdf file) without the .sdf file
        ///     name extension.
        /// </param>
        /// <param name="userTableName">The name of the database table that contains the user profile information.</param>
        /// <param name="userIdColumn">
        ///     The name of the database column that contains user IDs. This column must be typed as an
        ///     integer (int).
        /// </param>
        /// <param name="userNameColumn">
        ///     The name of the database column that contains user names. This column is used to match
        ///     user profile data to membership account data.
        /// </param>
        /// <param name="autoCreateTables">
        ///     true to indicate that user profile and membership tables should be created if they do
        ///     not exist; false to indicate that tables should not be created automatically. Although the membership tables can be
        ///     created automatically, the database itself must already exist.
        /// </param>
        void InitializeDatabaseConnection(string connectionStringName, string userTableName, string userIdColumn,
            string userNameColumn, bool autoCreateTables);

        /// <summary>
        ///     Initializes the membership system by connecting to a database that contains user information by using the specified
        ///     membership or role provider, and optionally creates membership tables if they do not already exist.
        /// </summary>
        /// <param name="connectionString">
        ///     The name of the connection string for the database that contains user information. If
        ///     you are using SQL Server Compact, this can be the name of the database file (.sdf file) without the .sdf file name
        ///     extension.
        /// </param>
        /// <param name="providerName">
        ///     The name of the ADO.NET data provider. If you want to use Microsoft SQL Server, the
        ///     <see
        ///         cref="M:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection(System.String,System.String,System.String,System.String,System.Boolean)" />
        ///     overload is recommended.
        /// </param>
        /// <param name="userTableName">The name of the database table that contains the user profile information.</param>
        /// <param name="userIdColumn">
        ///     The name of the database column that contains user IDs. This column must be typed as an
        ///     integer (int).
        /// </param>
        /// <param name="userNameColumn">
        ///     The name of the database column that contains user names. This column is used to match
        ///     user profile data to membership account data.
        /// </param>
        /// <param name="autoCreateTables">
        ///     true to indicate that user profile and membership tables should be created
        ///     automatically; false to indicate that tables should not be created automatically. Although the membership tables
        ///     can be created automatically, the database itself must already exist.
        /// </param>
        void InitializeDatabaseConnection(string connectionString, string providerName, string userTableName,
            string userIdColumn, string userNameColumn, bool autoCreateTables);

        /// <summary>
        ///     Logs the user in.
        /// </summary>
        /// <returns>
        ///     true if the user was logged in; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="persistCookie">
        ///     (Optional) true to specify that the authentication token in the cookie should be persisted
        ///     beyond the current session; otherwise false. The default is false.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool Login(string userName, string password, bool persistCookie = false);

        /// <summary>
        ///     Logs the user out.
        /// </summary>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        void Logout();

        /// <summary>
        ///     Changes the password for the specified user.
        /// </summary>
        /// <returns>
        ///     true if the password is successfully changed; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <param name="currentPassword">The current password for the user.</param>
        /// <param name="newPassword">The new password.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool ChangePassword(string userName, string currentPassword, string newPassword);

        /// <summary>
        ///     Confirms that an account is valid and activates the account.
        /// </summary>
        /// <returns>
        ///     true if the account is confirmed; otherwise, false.
        /// </returns>
        /// <param name="accountConfirmationToken">A confirmation token to pass to the authentication provider.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool ConfirmAccount(string accountConfirmationToken);

        /// <summary>
        ///     Creates a new membership account using the specified user name and password and optionally lets you specify that
        ///     the user must explicitly confirm the account.
        /// </summary>
        /// <returns>
        ///     A token that can be sent to the user to confirm the account.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="requireConfirmationToken">
        ///     (Optional) true to specify that the account must be confirmed by using the token
        ///     return value; otherwise, false. The default is false.
        /// </param>
        /// <exception cref="T:System.Web.Security.MembershipCreateUserException">
        ///     <paramref name="username" /> is empty.-or-<paramref name="username" /> already has a membership account.-or-
        ///     <paramref
        ///         name="password" />
        ///     is empty.-or-<paramref name="password" /> is too int.-or-The database operation failed.
        /// </exception>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        string CreateAccount(string userName, string password, bool requireConfirmationToken = false);

        /// <summary>
        ///     Creates a new user profile entry and a new membership account.
        /// </summary>
        /// <returns>
        ///     A token that can be sent to the user to confirm the user account.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password for the user.</param>
        /// <param name="propertyValues">(Optional) A dictionary that contains additional user attributes. The default is null.</param>
        /// <param name="requireConfirmationToken">
        ///     (Optional) true to specify that the user account must be confirmed; otherwise,
        ///     false. The default is false.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        string CreateUserAndAccount(string userName, string password, object propertyValues = null,
            bool requireConfirmationToken = false);

        /// <summary>
        ///     Generates a password reset token that can be sent to a user in email.
        /// </summary>
        /// <returns>
        ///     A token to send to the user.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <param name="tokenExpirationInMinutesFromNow">
        ///     (Optional) The time in minutes until the password reset token expires.
        ///     The default is 1440 (24 hours).
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        string GeneratePasswordResetToken(string userName, int tokenExpirationInMinutesFromNow = 1440);

        /// <summary>
        ///     Returns a value that indicates whether the specified user exists in the membership database.
        /// </summary>
        /// <returns>
        ///     true if the <paramref name="username" /> exists in the user profile table; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool UserExists(string userName);

        /// <summary>
        ///     Returns the ID for a user based on the specified user name.
        /// </summary>
        /// <returns>
        ///     The user ID.
        /// </returns>
        /// <param name="userName">The user name.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        int GetUserId(string userName);

        /// <summary>
        ///     Returns a user ID from a password reset token.
        /// </summary>
        /// <returns>
        ///     The user ID.
        /// </returns>
        /// <param name="token">The password reset token.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        int GetUserIdFromPasswordResetToken(string token);

        /// <summary>
        ///     Returns a value that indicates whether the user name of the logged-in user matches the specified user name.
        /// </summary>
        /// <returns>
        ///     true if the logged-in user name matches <paramref name="userName" />; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name to compare the logged-in user name to.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool IsCurrentUser(string userName);

        /// <summary>
        ///     Returns a value that indicates whether the user has been confirmed.
        /// </summary>
        /// <returns>
        ///     true if the user is confirmed; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name.</param>
        bool IsConfirmed(string userName);

        /// <summary>
        ///     If the user is not authenticated, sets the HTTP status to 401 (Unauthorized).
        /// </summary>
        void RequireAuthenticatedUser();

        /// <summary>
        ///     If the specified user is not logged on, sets the HTTP status to 401 (Unauthorized).
        /// </summary>
        /// <param name="userId">The ID of the user to compare.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        void RequireUser(int userId);

        /// <summary>
        ///     If the current user does not match the specified user name, sets the HTTP status to 401 (Unauthorized).
        /// </summary>
        /// <param name="userName">The name of the user to compare.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        void RequireUser(string userName);

        /// <summary>
        ///     If the current user is not in all of the specified roles, sets the HTTP status code to 401 (Unauthorized).
        /// </summary>
        /// <param name="roles">The roles to check. The current user must be in all of the roles that are passed in this parameter.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        void RequireRoles(params string[] roles);

        /// <summary>
        ///     Resets a password by using a password reset token.
        /// </summary>
        /// <returns>
        ///     true if the password was changed; otherwise, false.
        /// </returns>
        /// <param name="passwordResetToken">A password reset token.</param>
        /// <param name="newPassword">The new password.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool ResetPassword(string passwordResetToken, string newPassword);

        /// <summary>
        ///     Returns a value that indicates whether the specified membership account is temporarily locked because of too many
        ///     failed password attempts in the specified number of seconds.
        /// </summary>
        /// <returns>
        ///     true if the membership account is locked; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name of the membership account.</param>
        /// <param name="allowedPasswordAttempts">
        ///     The number of password attempts the user is permitted before the membership
        ///     account is locked.
        /// </param>
        /// <param name="intervalInSeconds">
        ///     The number of seconds to lock a user account after the number of password attempts exceeds the value in the
        ///     <paramref
        ///         name="allowedPasswordAttempts" />
        ///     parameter.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, int intervalInSeconds);

        /// <summary>
        ///     Returns a value that indicates whether the specified membership account is temporarily locked because of too many
        ///     failed password attempts in the specified time span.
        /// </summary>
        /// <returns>
        ///     true if the membership account is locked; otherwise, false.
        /// </returns>
        /// <param name="userName">The user name of the membership account.</param>
        /// <param name="allowedPasswordAttempts">
        ///     The number of password attempts the user is permitted before the membership
        ///     account is locked.
        /// </param>
        /// <param name="interval">
        ///     The number of seconds to lock out a user account after the number of password attempts exceeds the value in the
        ///     <paramref
        ///         name="allowedPasswordAttempts" />
        ///     parameter.
        /// </param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        bool IsAccountLockedOut(string userName, int allowedPasswordAttempts, TimeSpan interval);

        /// <summary>
        ///     Returns the number of times that the password for the specified account was incorrectly entered since the last
        ///     successful login or since the membership account was created.
        /// </summary>
        /// <returns>
        ///     The count of failed password attempts for the specified account.
        /// </returns>
        /// <param name="userName">The user name of the account.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        int GetPasswordFailuresSinceLastSuccess(string userName);

        /// <summary>
        ///     Returns the date and time when the specified membership account was created.
        /// </summary>
        /// <returns>
        ///     The date and time that the membership account was created, or <see cref="F:System.DateTime.MinValue" /> if the
        ///     account creation date is not available.
        /// </returns>
        /// <param name="userName">The user name for the membership account.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        DateTime GetCreateDate(string userName);

        /// <summary>
        ///     Returns the date and time when the password was most recently changed for the specified membership account.
        /// </summary>
        /// <returns>
        ///     The date and time when the password was most recently changed, or <see cref="F:System.DateTime.MinValue" /> if the
        ///     password has not been changed for this account.
        /// </returns>
        /// <param name="userName">The user name of the account.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        DateTime GetPasswordChangedDate(string userName);

        /// <summary>
        ///     Returns the date and time when an incorrect password was most recently entered for the specified account.
        /// </summary>
        /// <returns>
        ///     The date and time when an incorrect password was most recently entered for this account, or
        ///     <see
        ///         cref="F:System.DateTime.MinValue" />
        ///     if an incorrect password has not been entered for this account.
        /// </returns>
        /// <param name="userName">The user name of the membership account.</param>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The
        ///     <see
        ///         cref="M:WebMatrix.WebData.SimpleMembershipProvider.Initialize(System.String,System.Collections.Specialized.NameValueCollection)" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="Overload:WebMatrix.WebData.WebSecurity.InitializeDatabaseConnection" />
        ///     method was not called.-or-The
        ///     <see
        ///         cref="T:WebMatrix.WebData.SimpleMembershipProvider" />
        ///     membership provider is not registered in the configuration of your site. For more information, contact your site's
        ///     system administrator.
        /// </exception>
        DateTime GetLastPasswordFailureDate(string userName);
    }
}