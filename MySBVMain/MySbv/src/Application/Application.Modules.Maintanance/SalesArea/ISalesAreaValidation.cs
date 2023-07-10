using System.Collections.Generic;
using Application.Dto.SalesArea;
using Utility.Core;


namespace Application.Modules.Maintanance.SalesArea
{
    public interface ISalesAreaValidation
    {

        /// <summary>
        /// Return a list of all Sales Area
        /// </summary>
        /// <returns></returns>
        IEnumerable<SalesAreaDto> All();

        /// <summary>
        /// Add A New Sales Area
        /// </summary>
        /// <param name="salesAreaDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(SalesAreaDto salesAreaDto, string username);

        /// <summary>
        /// Update a new Sales Area
        /// </summary>
        /// <param name="salesAreaDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(SalesAreaDto salesAreaDto, string username);

        /// <summary>
        /// Delete a Sales Area
        /// </summary>
        /// <param name="salesAreaId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int salesAreaId, string username);
        
        /// <summary>
        /// Find a Sales Area by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<SalesAreaDto> Find(int id);
        
        /// <summary>
        /// Check if the Sales Area is in use or not
        /// </summary>
        /// <returns></returns>
        bool IsInUse(string name);

        /// <summary>
        /// Check if the Area is in use by another Area
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsAreaNameInUse(string name);

        /// <summary>
        /// Check if the Sales Area is in use by another Sales Area (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnotherArea(string name, int id);

    }
}
