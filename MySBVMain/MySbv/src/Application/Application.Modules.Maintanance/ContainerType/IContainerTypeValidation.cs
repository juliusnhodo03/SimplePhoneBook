using System.Collections.Generic;
using Application.Dto.CashDeposit;
using Utility.Core;

namespace Application.Modules.Maintanance.ContainerType
{
    public interface IContainerTypeValidation
    {
        /// <summary>
        /// Return a list of all ContainerType
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContainerTypeDto> All();
        
        /// <summary>
        /// Find a ContainerType by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<ContainerTypeDto> Find(int id);

        /// <summary>
        /// Add A New ContainerType
        /// </summary>
        /// <param name="containerTypeDtoDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(ContainerTypeDto containerTypeDtoDto, string username);

        /// <summary>
        /// Update a new ContainerType
        /// </summary>
        /// <param name="containerTypeDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(ContainerTypeDto containerTypeDto, string username);

        /// <summary>
        /// Delete a ContainerType
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);
        
        /// <summary>
        /// Check if Container Type Name already Exists in the system
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool ContainerTypeNameExists(string name);
        
        /// <summary>
        /// Check if Container Type Name In use by another Product
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsContainerTypeNameInUse(string name, int Id);
        
    }
}