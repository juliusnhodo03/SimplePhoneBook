using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Security;
using Application.Dto.Profile;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Profile
{
    public class ProfileValidation : IProfileValidation
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IUserAccountValidation _accountValidation;
        private readonly ILookup _lookup;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public ProfileValidation(IRepository repository, IMapper mapper, IUserAccountValidation accountValidation, ILookup lookup)
        {
            _repository = repository;
            _mapper = mapper;
            _accountValidation = accountValidation;
            _lookup = lookup;
        }

        #endregion

        #region IProfileValidation

        public MethodResult<UserProfileDto> ProfileDetails(string username)
        {
            User loggedInUser = _accountValidation.UserByName(username);
            User user = _repository.Query<User>(a => a.UserId == loggedInUser.UserId && a.IsNotDeleted,
                a => a.CashCenter, 
                a => a.UserSites, 
                a => a.Merchant,
                a => a.UserType, 
                a => a.Title, 
                a => a.UserNotifications).FirstOrDefault();

            if (user != null)
            {
                UserProfileDto profile = _mapper.Map<User, UserProfileDto>(user);

                // Set User Roles
                string[] userRoles = Roles.GetRolesForUser(username);

                foreach (string role in userRoles)
                {
                    switch (role)
                    {
                        case "SBVAdmin":
                            profile.IsAdmin = true;
                            break;
                        case "SBVApprover":
                            profile.IsApprover = true;
                            break;
                        case "SBVFinanceReviewer":
                            profile.IsFinanceReviewer = true;
                            break;
                        case "SBVRecon":
                            profile.IsRecon = true;
                            break;
                        case "SBVTeller":
                            profile.IsTeller = true;
                            break;
                        case "SBVTellerSupervisor":
                            profile.IsTellerSupervisor = true;
                            break;
                        case "RetailSupervisor":
                            profile.IsSupervisor = true;
                            break;
                        case "RetailUser":
                            profile.IsUser = true;
                            break;
                        case "RetailViewer":
                            profile.IsViewer = true;
                            break;
                        case "SBVDataCapture":
                            profile.IsDataCapture = true;
							break;
                    }
                }

                var userTypeId = _lookup.GetUserTypeId("MERCHANT_USER");
                if (profile.UserTypeId == userTypeId)
                {
                    List<UserNotification> userNotifications =
                        _repository.Query<UserNotification>(a => a.UserId == profile.UserId).ToList();

                    profile.IsEmailNotificationType =
                        userNotifications.Any(
                            a => a.NotificationTypeId == (int) ApplicationNotificationType.EMAIL && a.IsNotDeleted);
                    profile.IsSmsNotificationType =
                        userNotifications.Any(
                            a => a.NotificationTypeId == (int) ApplicationNotificationType.SMS && a.IsNotDeleted);
                    profile.IsFaxNotificationType =
                        userNotifications.Any(
                            a => a.NotificationTypeId == (int) ApplicationNotificationType.FAX && a.IsNotDeleted);
                }

                return new MethodResult<UserProfileDto>(MethodStatus.Successful, profile);
            }
            return new MethodResult<UserProfileDto>(MethodStatus.Error, null, "User Not Found.");
        }

        public MethodResult<bool> Update(UserProfileDto model, string loggedInUser)
        {
            User _loggedInUser = _accountValidation.UserByName(loggedInUser);
            User user = _mapper.Map<UserProfileDto, User>(model);
            user.LastChangedById = _loggedInUser.UserId;
            user.LastChangedDate = DateTime.Now;
            user.EntityState = State.Modified;
            user.IsNotDeleted = true;

            var userTypeId = _lookup.GetUserTypeId("MERCHANT_USER");
            if (user.UserTypeId == userTypeId)
            {
                UpdateUserNotifications(user, model);
            }

            bool result = (_repository.Update(user) > 0);
            return result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Error Updating User Profile Information. Server Error");
        }

        private void UpdateUserNotifications(User user, UserProfileDto model)
        {
            var results = _repository.Query<UserNotification>(a => a.UserId == user.UserId, o => o.NotificationType).ToList();
            user.UserNotifications = new Collection<UserNotification>();
            results.ForEach(a => user.UserNotifications.Add(a));

            #region Email Notification

            if (model.IsEmailNotificationType)
            {
                if (!user.UserNotifications.Any(a => a.NotificationName == "EMAIL"))
                {
                    user.UserNotifications.Add(
                        CreateNotification(user.LastChangedById.Value,
                            user.UserId, (int) ApplicationNotificationType.EMAIL));
                }
                else if (
                    user.UserNotifications.FirstOrDefault(a => a.NotificationName == "EMAIL")
                        .IsNotDeleted == false)
                {
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .IsNotDeleted = true;
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .EntityState = State.Modified;
                }
            }
            else
            {
                if (user.UserNotifications.Any(a => a.NotificationName == "EMAIL"))
                {
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .IsNotDeleted = false;
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .EntityState = State.Modified;
                }
            }

            #endregion

            #region Fax Notification

            if (model.IsFaxNotificationType)
            {
                if (!user.UserNotifications.Any(a => a.NotificationName == "FAX"))
                {
                    user.UserNotifications.Add(
                        CreateNotification(user.LastChangedById.Value,
                            user.UserId, (int) ApplicationNotificationType.FAX));
                }
                else if (
                    user.UserNotifications.FirstOrDefault(a => a.NotificationName == "FAX")
                        .IsNotDeleted == false)
                {
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .IsNotDeleted = true;
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .EntityState = State.Modified;
                }
            }
            else
            {
                if (user.UserNotifications.Any(a => a.NotificationName == "FAX"))
                {
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .IsNotDeleted = false;
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .EntityState = State.Modified;
                }
            }

            #endregion

            #region SMS Notification

            if (model.IsSmsNotificationType)
            {
                if (!user.UserNotifications.Any(a => a.NotificationName == "SMS"))
                {
                    user.UserNotifications.Add(
                        CreateNotification(user.LastChangedById.Value,
                            user.UserId, (int) ApplicationNotificationType.SMS));
                }
                else if (
                    user.UserNotifications.FirstOrDefault(a => a.NotificationName == "SMS")
                        .IsNotDeleted == false)
                {
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .IsNotDeleted = true;
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .EntityState = State.Modified;
                }
            }
            else
            {
                if (user.UserNotifications.Any(a => a.NotificationName == "SMS"))
                {
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .IsNotDeleted = false;
                    user.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .EntityState = State.Modified;
                }
            }

            #endregion

            ICollection<UserNotification> tempuserNotifications = user.UserNotifications;
            user.UserNotifications = new Collection<UserNotification>();

            foreach (UserNotification userNotification in tempuserNotifications)
            {
                user.UserNotifications.Add(
                    _mapper.Map<UserNotification, UserNotification>(userNotification));
            }

            foreach (UserNotification userNotification in user.UserNotifications)
            {
                userNotification.NotificationType = null;
                userNotification.User = null;
                _repository.Update(userNotification);
            }
        }

        private UserNotification CreateNotification(int lastChangedById, int userId, int notificationTypeid, bool activeStatus = true)
        {
            return new UserNotification
            {
                IsNotDeleted = activeStatus,
                LastChangedById = lastChangedById,
                CreatedById = lastChangedById,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
                UserId = userId,
                NotificationTypeId = notificationTypeid,
                EntityState = State.Added
            };
        }

        #endregion
    }
}