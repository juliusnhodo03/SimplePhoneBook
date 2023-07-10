using System.Collections.Generic;
using Application.Dto.Carrier;
using Utility.Core;

namespace Application.Modules.Maintanance.Carrier
{
    public interface ICarrierValidation
    {
        /// <summary>
        /// Return a list of all Carrier
        /// </summary>
        /// <returns></returns>
        IEnumerable<CitCarrierDto> All();

        /// <summary>
        /// Find a Carrier by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<CitCarrierDto> Find(int id);

        /// <summary>
        /// Add A New Carrier
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<bool> IsInUse(int id);

        /// <summary>
        /// Add A New Carrier
        /// </summary>
        /// <param name="carrierDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(CitCarrierDto carrierDto, string username);

        /// <summary>
        /// Update a new Carrier
        /// </summary>
        /// <param name="carrierDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(CitCarrierDto carrierDto, string username);

        /// <summary>
        /// Delete a Carrier
        /// </summary>
        /// <param name="citCarrierId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int citCarrierId, string username);

        /// <summary>
        /// Is Carrier Name is Use
        /// </summary>
        /// /// <param name="name"></param>
        /// <returns></returns>
        bool IsCarrierNameInUse(string name);

        /// <summary>
        /// Check if the Carrier Name is in use by another Carrier (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherCarrier(string name, int id);
    }
}