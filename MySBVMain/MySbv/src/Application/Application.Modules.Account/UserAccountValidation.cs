using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Security;
using Application.Dto.Users;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Security;
using Utility.Core;

namespace Application.Modules.UserAccountValidation
{
    public class UserAccountValidation : IUserAccountValidation
    {
        #region Fields

        private readonly IRepository _repository;
        private readonly IAsyncRepository _asyncRepository;
        private readonly ISecurity _security;
        private User _user;

        #endregion

        #region Constructor

        public UserAccountValidation(IRepository repository, ISecurity security, IAsyncRepository asyncRepository)
        {
            _repository = repository;
            _security = security;
            _asyncRepository = asyncRepository;
        }

        #endregion

        #region IAccount Validation

        public MethodResult<bool> LogIn(string username, string password, bool rememberMe, UserType userType)
        {
			User user = _repository.Query<User>(a => a.UserName.ToLower() == username.ToLower(),
						u => u.UserType, 
						o => o.UserSites,
                        o => o.UserSites.Select( b => b.Site),
						o => o.UserSites.Select(e => e.Site.CitCarrier)).FirstOrDefault();

            if (user != null)
            {
                if (user.LockedStatus)
                {
                    return new MethodResult<bool>(MethodStatus.Warning, false,
                        string.Format("Account for {0} is locked. Contact SBV for more information.", username));
                }

                switch (userType)
                {
                    case UserType.Normal:
                        if (_security.Login(username, password, rememberMe))
                        {
                            return new MethodResult<bool>(MethodStatus.Successful, true);
                        }
                        break;
                    case UserType.Admin:
                        if (user.UserTypeId == null && user.TitleId == null && user.MerchantId == null)
                        {
                            if (_security.Login(username, password))
                            {
                                return new MethodResult<bool>(MethodStatus.Successful, true);
                            }
                        }
                        break;
                    default:
                        throw new ArgumentException("Invalid User Type");
                }
            }

            return new MethodResult<bool>(MethodStatus.NotFound, false,
                "The user name or password provided is incorrect.");
        }

        public void LogOf()
        {
            _security.Logout();
            _user = null;
        }

        public void AddUserToRoles(string userName, List<string> roleNames)
        {
            var roles = new string[roleNames.Count];

            int count = 0;
            foreach (string role in roleNames)
            {
                if (!Roles.IsUserInRole(userName, role))
                    roles[count] = role;
                count++;
            }

            Roles.AddUserToRoles(userName, roles);
        }

        public string CreateUserAndAccount(UserDto user, int createdBy, int? id, MySBVUserTypes userTypes)
        {
            return _security.CreateUserAndAccount(user.UserName, user.Password, new
            {
                user.TitleId,
                user.UserTypeId,
                user.FirstName,
                MerchantId = userTypes == MySBVUserTypes.Retail ? id : null,
                CashCenterId = userTypes == MySBVUserTypes.SBV ? id : null, 
                user.CanMakeVaultPayment,
                user.LastName,
                user.IdNumber,
                user.PassportNumber,
                user.EmailAddress,
                user.CellNumber,
                user.OfficeNumber,
                user.FaxNumber,
                LockedStatus = user.IsLocked,
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                LastChangedById = createdBy,
                CreatedById = createdBy,
                CreateDate = DateTime.Now
            });
        }

        public MethodResult<bool> ChangePassword(string userName, string currentPassword, string newPassword,
            UserType userType)
        {
            switch (userType)
            {
                case UserType.Normal:
                    if (_security.ChangePassword(userName, currentPassword, newPassword))
                    {
                        LogOf();
                        return new MethodResult<bool>(MethodStatus.Successful, true,
                            "Your Password Was Successfully Changed. Log in With Your New Password");
                    }
                    break;
                case UserType.Admin:
                    User user =
                        _repository.Query<User>(a => a.UserName.ToLower() == userName.ToLower())
                            .FirstOrDefault();
                    if (user != null)
                    {
                        if (user.UserTypeId == null && user.TitleId == null && user.MerchantId == null)
                        {
                            if (_security.ChangePassword(userName, currentPassword, newPassword))
                            {
                                LogOf();
                                return new MethodResult<bool>(MethodStatus.Successful, true,
                                    "Your Admin Password Was Successfully Changed. Log in With Your New Password");
                            }
                            return new MethodResult<bool>(MethodStatus.Successful, false,
                                    "Invalid Credentials. Current Password. Try again.");
                        }

                        LogOf();
                        return new MethodResult<bool>(MethodStatus.Error, false,
                            "Current logged in user is not a system administrator");
                    }
                    return new MethodResult<bool>(MethodStatus.Error, false, "User Not found.");
                default:
                    throw new ArgumentException("Invalid User Type");
            }

            return new MethodResult<bool>(MethodStatus.Error, false,
                "The current password is incorrect or the new password is invalid.");
        }

        public MethodResult<ForgotPasswordResult> ForgotPassword(string username, string emailaddress,
            int tokenExpirationInMinutesFromNow = 120)
        {
            string _username = string.Empty;
            string _fullNames = string.Empty;
            string _emailAddress = string.Empty;

            if (!string.IsNullOrEmpty(emailaddress) && !string.IsNullOrEmpty(username))
            {
                var errorMessages = new List<string>();
                User userByEmail =
                    _repository.Query<User>(a => a.EmailAddress.ToLower() == emailaddress.ToLower())
                        .FirstOrDefault();

                if (userByEmail == null)
                {
                    errorMessages.Add(string.Format("User with email address {0} was not found.", emailaddress));
                }

                User userByName =
                    _repository.Query<User>(a => a.UserName.ToLower() == username.ToLower())
                        .FirstOrDefault();
                if (userByName == null)
                {
                    errorMessages.Add(string.Format("User with username {0} was not found.", username));
                }

                if (!errorMessages.Any())
                {
                    _username = userByEmail.UserName;
                    _fullNames = userByEmail.FirstName + " " + userByEmail.LastName;
                    _emailAddress = userByEmail.EmailAddress;
                    string resetToken = _security.GeneratePasswordResetToken(username, tokenExpirationInMinutesFromNow);
                    return new MethodResult<ForgotPasswordResult>(MethodStatus.Successful, new ForgotPasswordResult
                    {
                        Username = _username,
                        FullName = _fullNames,
                        EmaillAddress = _emailAddress,
                        PasswordResetToken = resetToken
                    });
                }

                string message = errorMessages.Aggregate(string.Empty,
                    (current, errorMessage) => current + (errorMessage + "\n"));
                return new MethodResult<ForgotPasswordResult>(MethodStatus.Error, null, message);
            }
            return new MethodResult<ForgotPasswordResult>(MethodStatus.Error, null,
                "Please supply email address and a username for us to reset your password.");
        }

        public bool ResetPassword(string userName, string password)
        {
            string resetToken = _security.GeneratePasswordResetToken(userName);
            return _security.ResetPassword(resetToken, password);
        }

        public bool ResetPasswordFromToken(string token, string password)
        {
            return _security.ResetPassword(token, password);
        }

        public bool DeleteUser(int id, int deletedById)
        {
            using (var scope = new TransactionScope())
            {
                User user = _repository.Query<User>(a => a.UserId == id).FirstOrDefault();

                if (user != null)
                {
                    if (_repository.Delete<User>(id, deletedById) > 0)
                    {
                        Roles.RemoveUserFromRoles(user.UserName, Roles.GetRolesForUser(user.UserName));
                        scope.Complete();
                        return true;
                    }
                }
            }
            
            return false;
        }

        public UserDto SetUserRoles(UserDto user)
        {
            string[] userRoles = Roles.GetRolesForUser(user.UserName);

            foreach (string role in userRoles)
            {
                switch (role)
                {
                    case "SBVRecon":
                        user.IsRecon = true;
                        break;
                    case "SBVTeller":
                        user.IsTeller = true;
                        break;
                    case "SBVTellerSupervisor":
                        user.IsTellerSupervisor = true;
                        break;
                    case "SBVAdmin":
                        user.IsAdmin = true;
                        break;
                    case "SBVApprover":
                        user.IsApprover = true;
                        break;
                    case "SBVFinanceReviewer":
                        user.IsFinanceReviewer = true;
                        break;
                    case "SBVDataCapture":
                        user.IsDataCapture = true;
                        break;
                    case "RetailSupervisor":
                        user.IsSupervisor = true;
                        break;
                    case "RetailUser":
                        user.IsUser = true;
                        break;
                    case "RetailViewer":
                        user.IsViewer = true;
                        break;
                }
            }
            return user;
        }

        public void RemoveUserFromRoles(string userName, string[] roles)
        {
            Roles.RemoveUserFromRoles(userName, roles);
        }

        public string[] GetUserRoles(string userName)
        {
            return Roles.GetRolesForUser(userName);
        }

        public int GetUserId(string userName)
        {
            return _security.GetUserId(userName);
        }

        public bool IsCurrentUser(string username)
        {
            return _security.IsCurrentUser(username);
        }

        public User UserByName(string username)
        {
            var userId = _security.GetUserId(username);
            return _repository.Query<User>(u => u.UserId == userId && u.UserName.ToLower() == username.ToLower(), o => o.UserType).FirstOrDefault();
        }

        public int? UserTypeId(string username)
        {
            var userId = _security.GetUserId(username);
            var user = _repository.Query<User>(a => a.UserId == userId).FirstOrDefault();
            return user != null ? user.UserTypeId : null;
        }

        public User LoggedOnUser(string username)
        {
            var userId = _security.GetUserId(username);
            return _repository.Query<User>(a => a.UserId == userId).FirstOrDefault();
        }


        public async Task<User> GetLoggedUserAsync(string username)
        {
            var user = await _asyncRepository.GetFirstOrDefaultAsync<User>(
                e => e.UserName == username, e => e.UserType);
            return user;
        }

        #endregion
    }

    public class ForgotPasswordResult
    {
        public string Username { get; set; }
        public string EmaillAddress { get; set; }
        public string PasswordResetToken { get; set; }
        public string FullName { get; set; }
    }
}