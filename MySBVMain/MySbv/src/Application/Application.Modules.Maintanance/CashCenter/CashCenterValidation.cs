using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using Application.Dto.Address;
using Application.Dto.CashCenter;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.CashCenter
{
    public class CashCenterValidation : ICashCenterValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _accountValidation;

        #endregion

        #region Constructor

        public CashCenterValidation(ILookup lookup, IUserAccountValidation accountValidation,  IRepository repository, IMapper mapper)
        {
            _lookup = lookup;
            _mapper = mapper;
            _repository = repository;
            _accountValidation = accountValidation;
        }

        #endregion

        #region ICashCenterValidation

        public MethodResult<bool> Add(CashCentreDto cashCenterDto, string username)
        {
            int userId = _accountValidation.UserByName(username).UserId;

            var address = new Address
            {
                AddressTypeId = cashCenterDto.Address.AddressTypeId,
                AddressLine1 = cashCenterDto.Address.AddressLine1,
                AddressLine2 = cashCenterDto.Address.AddressLine2,
                AddressLine3 = cashCenterDto.Address.AddressLine3,
                PostalCode = cashCenterDto.Address.PostalCode,
                Latitude = cashCenterDto.Address.Latitude,
                Longitude = cashCenterDto.Address.Longitude,
                LastChangedById = userId,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = userId,
                IsNotDeleted = true,
                EntityState = State.Added
            };
            
            var mappedCashCenter = _mapper.Map<CashCentreDto, Domain.Data.Model.CashCenter>(cashCenterDto);

            mappedCashCenter.Address = address;
            mappedCashCenter.LastChangedById = userId;
            mappedCashCenter.LastChangedDate = DateTime.Now;
            mappedCashCenter.CreateDate = DateTime.Now;
            mappedCashCenter.CreatedById = userId;

            return _repository.Add(mappedCashCenter) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Account Not Added");
        }

        public MethodResult<CashCentreDto> Find(int id)
        {
            var cashCenter = _repository.Find<Domain.Data.Model.CashCenter>(id);
            var address = _repository.Find<Address>(cashCenter.AddressId);
            var addressType = _repository.Find<AddressType>(address.AddressTypeId);

            address.AddressType = addressType;
            cashCenter.Address = address;
            
            var mappedCashCenter = _mapper.Map<Domain.Data.Model.CashCenter, CashCentreDto>(cashCenter);

            return new MethodResult<CashCentreDto>(MethodStatus.Successful, mappedCashCenter);
        }

        public IEnumerable<ListCashCenterDto> All()
        {
            var cashCenters = _repository.Query<Domain.Data.Model.CashCenter>(a => a.IsNotDeleted, b => b.Address, c => c.Cluster).OrderByDescending(o => o.CreateDate);
            return cashCenters.Select(cashCenter => _mapper.Map<Domain.Data.Model.CashCenter, ListCashCenterDto>(cashCenter));
        }

        public MethodResult<bool> Edit(CashCentreDto cashCenterDto, string username)
        {
            int userId = _accountValidation.UserByName(username).UserId;
            var tempCashCentre =
                _repository.Query<Domain.Data.Model.CashCenter>(a => a.CashCenterId == cashCenterDto.CashCenterId)
                    .FirstOrDefault();
            
            var mappedCashCenter = _mapper.Map<CashCentreDto, Domain.Data.Model.CashCenter>(cashCenterDto);
            mappedCashCenter.EntityState = State.Modified;
            mappedCashCenter.IsNotDeleted = true;
            mappedCashCenter.Address.LastChangedById = userId;
            mappedCashCenter.Address.EntityState = State.Modified;
            mappedCashCenter.Address.IsNotDeleted = true;
            mappedCashCenter.LastChangedById = userId;
            mappedCashCenter.LastChangedDate = DateTime.Now;
            if (tempCashCentre != null)
            {
                mappedCashCenter.CreateDate = tempCashCentre.CreateDate;
                mappedCashCenter.CreatedById = tempCashCentre.CreatedById;
            }

            return _repository.Update(mappedCashCenter) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Cash Center Not Updated");
        }

        public MethodResult<bool> Delete(int centerId, string username)
        {
            var results = IsInUse(centerId);
            if (results.Status == MethodStatus.Successful)
            {
                int userId = _accountValidation.UserByName(username).UserId;
                var _result = _repository.Delete<Domain.Data.Model.CashCenter>(centerId, userId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Cash Center Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Cash Center In Use");
        }

        #endregion

        #region Helpers

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Domain.Data.Model.Site>(a => a.CashCenterId == id && a.IsNotDeleted) ?
                new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a Cash Center that is linked to and active Site.") :
                new MethodResult<bool>(MethodStatus.Successful);
        }
        
        public bool IsCashCenterNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.CashCenter>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnotherCashCenter(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.CashCenter>(a => a.Name.ToLower() == name.ToLower() && a.CashCenterId != id);
        }

        public bool ExistsByName(string name)
        {
            return _repository.Any<Domain.Data.Model.CashCenter>(o => o.Name.ToLower() == name.ToLower() && o.IsNotDeleted);
        }

        public bool ExistsByNumber(string number)
        {
            return _repository.Any<Domain.Data.Model.CashCenter>(o => o.Number.ToLower() == number.ToLower() && o.IsNotDeleted);
        }
        
        #endregion

    }
}