using System.Collections.Generic;
using Application.Dto.DeviceType;
using Utility.Core;

namespace Application.Modules.Maintanance.DeviceType
{
    public interface IDeviceTypeValidation
    {

        /// <summary>
        /// Return a list of Devices
        /// </summary>
        /// <returns></returns>
        IEnumerable<DeviceTypeDto> All();

        /// <summary>
        /// Add A New Device
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(DeviceTypeDto deviceDto, string username);

        /// <summary>
        /// Update a new Device
        /// </summary>
        /// <param name="deviceDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(DeviceTypeDto deviceDto, string username);

        /// <summary>
        /// Delete a Device
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);

        /// <summary>
        /// Find a Device by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<DeviceTypeDto> Find(int id);

        /// <summary>
        /// Check if the Device is still Active before deleting
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        bool IsActive(int deviceId);

        /// <summary>
        /// Add A New Device
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<bool> IsInUse(int id);

        /// <summary>
        /// Check if the Device is in use by another Device
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsDeviceNameInUse(string name);

        /// <summary>
        /// Check if the Branch Code is in use by another Device (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherDevice(string name, int id);



    }
}