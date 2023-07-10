using System.Collections.Generic;
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

namespace Application.Modules.Maintanance.Users.HeadOffice
{
    public class HeadOfficeUserValidation : IHeadOfficeUserValidation
    {
        #region Fields

        private readonly IUserAccountValidation _accountValidation;
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public HeadOfficeUserValidation(IRepository repository, IMapper mapper, IUserAccountValidation accountValidation, ILookup lookup)
        {
            _repository = repository;
            _mapper = mapper;
            _accountValidation = accountValidation;
            _lookup = lookup;
        }

        #endregion

        #region IHead Office User Validation

        public bool UserExist(string userName)
        {
            return ApplicationHelpers.UserNameExist(_repository, userName);
        }

        public bool UserIdExist(string id, string userName, Function function = Function.Add)
        {
            return ApplicationHelpers.UserIdNumberExist(_repository, userName, id, function);
        }

        public IEnumerable<UserDto> All()
        {
            var userTypeId = _lookup.GetUserTypeId("HEAD_OFFICE_USER");
            var users = _repository.Query<User>(a => a.UserTypeId == userTypeId, b => b.Title, c => c.UserType);
            return users.Select(headOfficeuser => _mapper.Map<User, UserDto>(headOfficeuser)).ToList();
        }

        public MethodResult<UserDto> Find(int id)
        {
            // Get the cash centre user from DB
            User headOfficeUser = _repository.Query<User>(a => a.UserId == id, u => u.UserType).FirstOrDefault();

            if (headOfficeUser != null)
            {
                // Create the DTO from the cash centre user object
                UserDto mapped = _mapper.Map<User, UserDto>(headOfficeUser);
                mapped = _accountValidation.SetUserRoles(mapped);
                return new MethodResult<UserDto>(MethodStatus.Successful, mapped);
            }
            return new MethodResult<UserDto>(MethodStatus.NotFound, null, "User Not Found.");
        }

        public MethodResult<UserDto> Add(UserDto headOfficeUserDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int loggedInUserId = _accountValidation.UserByName(username).UserId;
                headOfficeUserDto.CreatedById = loggedInUserId;
                headOfficeUserDto.UserTypeId = _lookup.GetUserTypeId("HEAD_OFFICE_USER");
                headOfficeUserDto.LastName = headOfficeUserDto.LastName.Trim();

                _accountValidation.CreateUserAndAccount(headOfficeUserDto, loggedInUserId, null, MySBVUserTypes.HeadOffice);

                List<string> userRoles = ExtractRoles(headOfficeUserDto);
                _accountValidation.AddUserToRoles(headOfficeUserDto.UserName, userRoles);
                scope.Complete();
                return new MethodResult<UserDto>(MethodStatus.Successful, headOfficeUserDto,
                    "User Created Successfully.");
            }
            
        }

        public MethodResult<bool> Edit(UserDto userDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int loggedInUserId = _accountValidation.UserByName(username).UserId;

                User mapped = _mapper.Map<UserDto, User>(userDto);
                mapped.LastChangedById = loggedInUserId;
                mapped.EntityState = State.Modified;

                if (_repository.Update(mapped) > 0)
                {
                    List<string> userRoles = ExtractRoles(userDto);
                    _accountValidation.RemoveUserFromRoles(mapped.UserName, _accountValidation.GetUserRoles(mapped.UserName));
                    _accountValidation.AddUserToRoles(mapped.UserName, userRoles);
                    scope.Complete();
                    return new MethodResult<bool>(MethodStatus.Successful, true, "User Updated Successfully.");
                }
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "User Not Updated.");
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

            if (userDto.IsAdmin)
                userRoles.Add(ApplicationRoles.SBVAdmin.Name());
            if (userDto.IsFinanceReviewer)
                userRoles.Add(ApplicationRoles.SBVFinanceReviewer.Name());
            if (userDto.IsApprover)
                userRoles.Add(ApplicationRoles.SBVApprover.Name());
            if (userDto.IsDataCapture)
                userRoles.Add(ApplicationRoles.SbvDataCapture.Name());
            return userRoles;
        }

        #endregion
    }
}