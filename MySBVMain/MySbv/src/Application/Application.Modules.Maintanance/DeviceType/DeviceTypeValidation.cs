using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.DeviceType;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.DeviceType
{
    public class DeviceTypeValidation : IDeviceTypeValidation
    {

        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _accountValidation;

        #endregion

        #region Constructor

        public DeviceTypeValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _repository = repository;
            _accountValidation = accountValidation;
        }

        #endregion

        #region IDevice Validation

        public IEnumerable<DeviceTypeDto> All()
        {
            var deviceTypes = _repository.All<Domain.Data.Model.DeviceType>();
            return deviceTypes.Select(deviceType => _mapper.Map<Domain.Data.Model.DeviceType, DeviceTypeDto>(deviceType)).ToList();
        }

        public MethodResult<bool> Add(DeviceTypeDto deviceDto, string username)
        {
            int loggedInUserId = _accountValidation.UserByName(username).UserId;
            var supplier = _repository.Query<Supplier>(e => e.SupplierId == deviceDto.SupplierId).FirstOrDefault();

            var deviceType = _mapper.Map<DeviceTypeDto, Domain.Data.Model.DeviceType>(deviceDto);
            deviceType.IsNotDeleted = true;
            deviceType.Description = deviceType.Description.ToUpper();
            if (supplier != null) deviceType.LookUpKey = supplier.LookUpKey.ToUpper();
            deviceType.CreatedById = loggedInUserId;
            deviceType.CreateDate = DateTime.Now;
            deviceType.LastChangedById = loggedInUserId;
            deviceType.LastChangedDate = DateTime.Now;

            return _repository.Add(deviceType) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Device Not Added");
        }

        public MethodResult<bool> Edit(DeviceTypeDto deviceDto, string username)
        {
            var tempDeviceType = _repository.Query<Domain.Data.Model.DeviceType>(a => a.DeviceTypeId == deviceDto.DeviceTypeId).FirstOrDefault();

            int userId = _accountValidation.UserByName(username).UserId;
            var supplier = _repository.Query<Supplier>(e => e.SupplierId == deviceDto.SupplierId).FirstOrDefault();

            Domain.Data.Model.DeviceType mappedDevice = _mapper.Map<DeviceTypeDto, Domain.Data.Model.DeviceType>(deviceDto);
            mappedDevice.EntityState = State.Modified;
            mappedDevice.LastChangedById = userId;
            mappedDevice.LastChangedDate = DateTime.Now;
            mappedDevice.IsNotDeleted = true;
            mappedDevice.Description = mappedDevice.Description.ToUpper();
            if (supplier != null) mappedDevice.LookUpKey = supplier.LookUpKey.ToUpper();
            if (tempDeviceType != null)
            {
                mappedDevice.CreateDate = tempDeviceType.CreateDate;
                mappedDevice.CreatedById = tempDeviceType.CreatedById;
            }

            return _repository.Update(mappedDevice) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Device Not Updated");
        }

        public MethodResult<bool> Delete(int id, string username)
        {
            var user = _accountValidation.UserByName(username);

            var result = IsInUse(id);
            if (result.Status == MethodStatus.Successful)
            {
                var _result = _repository.Delete<Domain.Data.Model.DeviceType>(id, user.UserId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Device Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, result.Message);
        }

        public MethodResult<DeviceTypeDto> Find(int id)
        {
            var device = _repository.Find<Domain.Data.Model.DeviceType>(id);
            var mappedDevice = _mapper.Map<Domain.Data.Model.DeviceType, DeviceTypeDto>(device);

            return new MethodResult<DeviceTypeDto>(MethodStatus.Successful, mappedDevice);
        }

        public bool IsActive(int deviceId)
        {
            var devices = _repository.Query<Domain.Data.Model.DeviceType>(a => a.IsNotDeleted)
                .Where(e => e.DeviceTypeId == deviceId);

            return devices.Any();
        }

        #endregion

        #region Helper

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Domain.Data.Model.Product>(a => a.DeviceId == id && a.IsNotDeleted) ?
                  new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a device that is linked to and active Product.") :
              new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsDeviceNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.Device>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnotherDevice(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.Device>(a => a.Name.ToLower() == name.ToLower() && a.DeviceId != id);
        }

        #endregion

    }
}