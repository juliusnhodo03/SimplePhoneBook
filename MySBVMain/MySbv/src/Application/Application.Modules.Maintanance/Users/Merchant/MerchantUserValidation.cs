using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Transactions;
using Application.Dto.Users;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.Users.Merchant
{
    public class MerchantUserValidation : IMerchantUserValidation
    {
        #region Fields

        private readonly IUserAccountValidation _accountValidation;
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public MerchantUserValidation(IRepository repository, IMapper mapper, IUserAccountValidation accountValidation, ILookup lookup)
        {
            _repository = repository;
            _mapper = mapper;
            _accountValidation = accountValidation;
            _lookup = lookup;
        }

        #endregion

        #region IMerchant User Validation

        public bool UserExist(string userName)
        {
            return ApplicationHelpers.UserNameExist(_repository, userName);
        }

        public bool UserIdExist(string id, string username, Function function = Function.Add)
        {
            return ApplicationHelpers.UserIdNumberExist(_repository, username, id,
                function);
        }

        public IEnumerable<MerchantUserDto> All()
        {
            var userTypeId = _lookup.GetUserTypeId("MERCHANT_USER");
            IEnumerable<User> merchantUsers = _repository.Query<User>(a => a.IsNotDeleted && a.UserTypeId == userTypeId,
                    c => c.CashCenter, t => t.Title, u => u.UserType, m => m.Merchant, u => u.UserSites).ToList();

            return merchantUsers.Select(user => _mapper.Map<User, MerchantUserDto>(user)).ToList(); ;
        }

        public MethodResult<MerchantUserDto> Find(int id)
        {
            User merchantUser = _repository.Query<User>(a => a.UserId == id, 
                c => c.CashCenter, t => t.Title, u => u.UserType, m => m.Merchant, u => u.UserSites, n => n.UserNotifications)
                .FirstOrDefault(a => a.UserId == id);

            if (merchantUser != null)
            {
                if (merchantUser.MerchantId == null)
                    return new MethodResult<MerchantUserDto>(MethodStatus.Error, null,
                        "User is not a retail client or does not have a merchant linked to their profile.");

                MerchantUserDto mapped = _mapper.Map<User, MerchantUserDto>(merchantUser);
                mapped.SiteIds = merchantUser.UserSites.Select(a => a.SiteId).ToList();
                mapped.UserDto = _accountValidation.SetUserRoles(mapped.UserDto);

                mapped.IsEmailNotificationType =
                    merchantUser.UserNotifications.Any(
                        a => a.NotificationTypeId == (int) ApplicationNotificationType.EMAIL && a.IsNotDeleted);
                mapped.IsSmsNotificationType =
                    merchantUser.UserNotifications.Any(
                        a => a.NotificationTypeId == (int) ApplicationNotificationType.SMS && a.IsNotDeleted);
                mapped.IsFaxNotificationType =
                    merchantUser.UserNotifications.Any(
                        a => a.NotificationTypeId == (int) ApplicationNotificationType.FAX && a.IsNotDeleted);

                return new MethodResult<MerchantUserDto>(MethodStatus.Successful, mapped);
            }
            return new MethodResult<MerchantUserDto>(MethodStatus.NotFound, null, "Merchant user not found.");
        }

        public MethodResult<MerchantUserDto> Add(MerchantUserDto merchantUserDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int loggedInUserId = _accountValidation.UserByName(username).UserId;
                merchantUserDto.UserDto.UserTypeId = _lookup.GetUserTypeId("MERCHANT_USER");
                merchantUserDto.UserDto.LastName = merchantUserDto.UserDto.LastName.Trim();
                _accountValidation.CreateUserAndAccount(merchantUserDto.UserDto, loggedInUserId, merchantUserDto.MerchantId, MySBVUserTypes.Retail);
                var user = _repository.Query<User>(a => a.UserName.ToLower() == merchantUserDto.UserDto.UserName).FirstOrDefault();

                if (user != null)
                {
                    merchantUserDto.UserDto.UserId = user.UserId;
                    List<string> userRoles = ExtractRoles(merchantUserDto.UserDto);
                    _accountValidation.AddUserToRoles(merchantUserDto.UserDto.UserName, userRoles);

                    User mappedMerchantUser = _mapper.Map<MerchantUserDto, User>(merchantUserDto);
                    AddNewUserSites(merchantUserDto, mappedMerchantUser, loggedInUserId);
                    SetNotifications(merchantUserDto, mappedMerchantUser, loggedInUserId);
                    if (_repository.Update(mappedMerchantUser) > 0)
                    {
                        scope.Complete();
                        return new MethodResult<MerchantUserDto>(MethodStatus.Successful, merchantUserDto,
                            "User Created Successfully.");
                    }  
                }
            }
            return new MethodResult<MerchantUserDto>(MethodStatus.Error, null, "User Not Created. Server Error.");
        }

        public MethodResult<bool> Edit(MerchantUserDto merchantUserDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int loggedInUserId = _accountValidation.UserByName(username).UserId;

                merchantUserDto.UserSites =
                    _repository.Query<UserSite>(a => a.UserId == merchantUserDto.UserDto.UserId)
                        .Select(a => _mapper.Map<UserSite, UserSiteDto>(a))
                        .ToList();
                
                User mappedMerchantUser = _mapper.Map<MerchantUserDto, User>(merchantUserDto);
                mappedMerchantUser.CanMakeVaultPayment = merchantUserDto.UserDto.CanMakeVaultPayment;
                mappedMerchantUser.LastChangedById = loggedInUserId;
                mappedMerchantUser.EntityState = State.Modified;

                IEnumerable<int> added = merchantUserDto.SiteIds.Except(mappedMerchantUser.UserSites.Select(a => a.SiteId));
                IEnumerable<int> removed = mappedMerchantUser.UserSites.Select(a => a.SiteId).Except(merchantUserDto.SiteIds.Select(a => a));

                AddNewUserSitesOnUpdate(added, mappedMerchantUser, loggedInUserId);
                MarkRemovedUserSites(removed, mappedMerchantUser, loggedInUserId);

                if (_repository.Update(mappedMerchantUser) > 0)
                {
                    List<string> userRoles = ExtractRoles(merchantUserDto.UserDto);
                    _accountValidation.RemoveUserFromRoles(mappedMerchantUser.UserName,
                        _accountValidation.GetUserRoles(mappedMerchantUser.UserName));
                    _accountValidation.AddUserToRoles(mappedMerchantUser.UserName, userRoles);

                    List<UserNotification> userNotificationCollection =
                        _repository.Query<UserNotification>(a => a.UserId == mappedMerchantUser.UserId,
                            n => n.NotificationType).ToList();
                    foreach (UserNotification userNotification in userNotificationCollection)
                    {
                        mappedMerchantUser.UserNotifications.Add(userNotification);
                    }

                    SetEmailNotification(merchantUserDto, mappedMerchantUser);
                    SetFaxNotification(merchantUserDto, mappedMerchantUser);
                    SetSmsNotification(merchantUserDto, mappedMerchantUser);


                    ICollection<UserNotification> tempuserNotifications = mappedMerchantUser.UserNotifications;
                    mappedMerchantUser.UserNotifications = new Collection<UserNotification>();

                    foreach (UserNotification userNotification in tempuserNotifications)
                    {
                        mappedMerchantUser.UserNotifications.Add(
                            _mapper.Map<UserNotification, UserNotification>(userNotification));
                    }

                    foreach (UserNotification userNotification in mappedMerchantUser.UserNotifications)
                    {
                        userNotification.NotificationType = null;
                        userNotification.User = null;
                        _repository.Update(userNotification);
                    }
                    scope.Complete();
                    return new MethodResult<bool>(MethodStatus.Successful, true);
                }   
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "User not updated.");
        }

        public MethodResult<bool> Delete(int id, string username)
        {
            var user = _accountValidation.UserByName(username);
            return _accountValidation.DeleteUser(id, user.UserId)
                ? new MethodResult<bool>(MethodStatus.Successful, true, "User Deleted Successfully.")
                : new MethodResult<bool>(MethodStatus.Error, false, "User Not Deleted.");
        }

        #endregion

        #region Helpers

        private static List<string> ExtractRoles(UserDto userDto)
        {
            var userRoles = new List<string>();

            if (userDto.IsSupervisor)
                userRoles.Add(ApplicationRoles.RetailSupervisor.Name());
            if (userDto.IsUser)
                userRoles.Add(ApplicationRoles.RetailUser.Name());
            if (userDto.IsViewer)
                userRoles.Add(ApplicationRoles.RetailViewer.Name());
            return userRoles;
        }

        private UserNotification CreateNotification(int lastChangedById, int userId, int notificationTypeid,
            bool activeStatus = true)
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

        private void SetNotifications(MerchantUserDto merchantUserDto, User mappedMerchantUser, int loggedInUserId)
        {
            mappedMerchantUser.UserNotifications = new Collection<UserNotification>();

            if (merchantUserDto.IsEmailNotificationType)
            {
                mappedMerchantUser.UserNotifications.Add(
                    CreateNotification(loggedInUserId,
                        mappedMerchantUser.UserId, (int) ApplicationNotificationType.EMAIL));
            }
            if (merchantUserDto.IsFaxNotificationType)
            {
                mappedMerchantUser.UserNotifications.Add(
                    CreateNotification(loggedInUserId,
                        mappedMerchantUser.UserId, (int) ApplicationNotificationType.FAX));
            }

            if (merchantUserDto.IsSmsNotificationType)
            {
                mappedMerchantUser.UserNotifications.Add(
                    CreateNotification(loggedInUserId,
                        mappedMerchantUser.UserId, (int) ApplicationNotificationType.SMS));
            }
        }

        private static void AddNewUserSites(MerchantUserDto merchantUserDto, User mappedMerchantUser, int loggedInUserId)
        {
            foreach (int siteId in merchantUserDto.SiteIds)
            {
                mappedMerchantUser.UserSites.Add(new UserSite
                {
                    SiteId = siteId,
                    UserId = merchantUserDto.UserDto.UserId,
                    IsNotDeleted = true,
                    LastChangedById = loggedInUserId,
                    CreatedById = loggedInUserId,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    EntityState = State.Added
                });
            }
        }

        private void SetSmsNotification(MerchantUserDto merchantUserDto, User mappedMerchantUser)
        {
            if (merchantUserDto.IsSmsNotificationType)
            {
                if (!mappedMerchantUser.UserNotifications.Any(a => a.NotificationName == "SMS"))
                {
                    mappedMerchantUser.UserNotifications.Add(
                        CreateNotification(mappedMerchantUser.LastChangedById.Value,
                            mappedMerchantUser.UserId, (int) ApplicationNotificationType.SMS));
                }
                else if (
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationName == "SMS")
                        .IsNotDeleted ==
                    false)
                {
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .IsNotDeleted = true;
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .EntityState = State.Modified;
                }
            }
            else
            {
                if (mappedMerchantUser.UserNotifications.Any(a => a.NotificationName == "SMS"))
                {
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .IsNotDeleted = false;
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "SMS")
                        .EntityState = State.Modified;
                }
            }
        }

        private void SetFaxNotification(MerchantUserDto merchantUserDto, User mappedMerchantUser)
        {
            if (merchantUserDto.IsFaxNotificationType)
            {
                if (!mappedMerchantUser.UserNotifications.Any(a => a.NotificationName == "FAX"))
                {
                    mappedMerchantUser.UserNotifications.Add(
                        CreateNotification(mappedMerchantUser.LastChangedById.Value,
                            mappedMerchantUser.UserId, (int) ApplicationNotificationType.FAX));
                }
                else if (
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationName == "FAX")
                        .IsNotDeleted ==
                    false)
                {
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .IsNotDeleted = true;
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .EntityState = State.Modified;
                }
            }
            else
            {
                if (mappedMerchantUser.UserNotifications.Any(a => a.NotificationName == "FAX"))
                {
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .IsNotDeleted = false;
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "FAX")
                        .EntityState = State.Modified;
                }
            }
        }

        private void SetEmailNotification(MerchantUserDto merchantUserDto, User mappedMerchantUser)
        {
            if (merchantUserDto.IsEmailNotificationType)
            {
                if (!mappedMerchantUser.UserNotifications.Any(a => a.NotificationName == "EMAIL"))
                {
                    mappedMerchantUser.UserNotifications.Add(
                        CreateNotification(mappedMerchantUser.LastChangedById.Value,
                            mappedMerchantUser.UserId, (int) ApplicationNotificationType.EMAIL));
                }
                else if (
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationName == "EMAIL")
                        .IsNotDeleted ==
                    false)
                {
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .IsNotDeleted = true;
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .EntityState = State.Modified;
                }
            }
            else
            {
                if (mappedMerchantUser.UserNotifications.Any(a => a.NotificationName == "EMAIL"))
                {
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .IsNotDeleted = false;
                    mappedMerchantUser.UserNotifications.FirstOrDefault(a => a.NotificationType.Name == "EMAIL")
                        .EntityState = State.Modified;
                }
            }
        }

        private void MarkRemovedUserSites(IEnumerable<int> removed, User mappedMerchantUser,  int loggedInUserId)
        {
            foreach (int i in removed)
            {
                mappedMerchantUser.UserSites.FirstOrDefault(a => a.SiteId == i).LastChangedById = loggedInUserId;
                mappedMerchantUser.UserSites.FirstOrDefault(a => a.SiteId == i).LastChangedDate = DateTime.Now;
                mappedMerchantUser.UserSites.FirstOrDefault(a => a.SiteId == i).IsNotDeleted = false;
                mappedMerchantUser.UserSites.FirstOrDefault(a => a.SiteId == i).EntityState = State.Modified;
            }
        }

        private static void AddNewUserSitesOnUpdate(IEnumerable<int> added, User mappedMerchantUser, int loggedInUserId)
        {
            foreach (int variable in added)
            {
                mappedMerchantUser.UserSites.Add(new UserSite
                {
                    SiteId = variable,
                    UserId = loggedInUserId,
                    IsNotDeleted = true,
                    LastChangedById = loggedInUserId,
                    CreatedById = loggedInUserId,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    EntityState = State.Added
                });
            }
        }

        #endregion
    }
}