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

namespace Application.Modules.Maintanance.Users.CashCenter
{
    public class CashCenterUserValidation : ICashCenterUserValidation
    {
        #region Fields

        private readonly IUserAccountValidation _accountValidation;
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public CashCenterUserValidation(IRepository repository, IMapper mapper, IUserAccountValidation accountValidation, ILookup lookup)
        {
            _repository = repository;
            _mapper = mapper;
            _accountValidation = accountValidation;
            _lookup = lookup;
        }

        #endregion

        #region ICash Center User Validation

        public bool UserNameExist(string userName)
        {
            return ApplicationHelpers.UserNameExist(_repository, userName);
        }

        public bool UserIdNumberExist(string id, string username, Function function = Function.Add)
        {
            return ApplicationHelpers.UserIdNumberExist(_repository, username, id, function);
        }

        public IEnumerable<CashCenterUserDto> All()
        {
            var userTypeId = _lookup.GetUserTypeId("SBV_USER");
            IEnumerable<User> cashCenterUsers = _repository.Query<User>(a => a.UserTypeId == userTypeId, c => c.CashCenter);
            var list = cashCenterUsers.Select(cashCenterUser => _mapper.Map<User, CashCenterUserDto>(cashCenterUser));
            return list;
        }

        public MethodResult<CashCenterUserDto> Find(int id)
        {
            User cashCenterUser = _repository.Query<User>(a => a.UserId == id, b => b.CashCenter).FirstOrDefault();

            if (cashCenterUser != null)
            {
                // Create the DTO from the cash centre user object
                CashCenterUserDto mapped = _mapper.Map<User, CashCenterUserDto>(cashCenterUser);
                mapped.UserDto = _accountValidation.SetUserRoles(mapped.UserDto);
                return new MethodResult<CashCenterUserDto>(MethodStatus.Successful, mapped);
            }
            return new MethodResult<CashCenterUserDto>(MethodStatus.NotFound, null, "User Not Found.");
        }

        public MethodResult<CashCenterUserDto> Add(CashCenterUserDto cashCenterUserDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int loggedInUserId = _accountValidation.UserByName(username).UserId;
                cashCenterUserDto.UserDto.CreatedById = loggedInUserId;
                cashCenterUserDto.UserDto.UserTypeId = _lookup.GetUserTypeId("SBV_USER");
                cashCenterUserDto.UserDto.LastName = cashCenterUserDto.UserDto.LastName.Trim();

                _accountValidation.CreateUserAndAccount(cashCenterUserDto.UserDto, loggedInUserId, cashCenterUserDto.CashCenterId, MySBVUserTypes.SBV);

                List<string> userRoles = ExtractRoles(cashCenterUserDto);
                _accountValidation.AddUserToRoles(cashCenterUserDto.UserDto.UserName, userRoles);
                scope.Complete();
                return new MethodResult<CashCenterUserDto>(MethodStatus.Successful, cashCenterUserDto,
                    "User Created Successfully.");   
            }
        }

        public MethodResult<bool> Edit(CashCenterUserDto cashCenterUserDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int loggedInUserId = _accountValidation.UserByName(username).UserId;

                User mapped = _mapper.Map<CashCenterUserDto, User>(cashCenterUserDto);
                mapped.LastChangedById = loggedInUserId;
                mapped.EntityState = State.Modified;

                if (_repository.Update(mapped) > 0)
                {
                    List<string> userRoles = ExtractRoles(cashCenterUserDto);
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

        private static List<string> ExtractRoles(CashCenterUserDto cashCenterUserDto)
        {
            var userRoles = new List<string>();

            if (cashCenterUserDto.UserDto.IsRecon)
                userRoles.Add(ApplicationRoles.SBVRecon.Name());
            if (cashCenterUserDto.UserDto.IsTeller)
                userRoles.Add(ApplicationRoles.SBVTeller.Name());
            if (cashCenterUserDto.UserDto.IsTellerSupervisor)
                userRoles.Add(ApplicationRoles.SBVTellerSupervisor.Name());
            return userRoles;
        }

        #endregion
    }
}