using System.Collections.Generic;
using Application.Dto.ProductType;
using Utility.Core;


namespace Application.Modules.Maintanance.ProductType
{
    public interface IProductTypeValidation
    {
        /// <summary>
        /// Return a list of all Product Type
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductTypeDto> All();

        /// <summary>
        /// Add A New Product Type
        /// </summary>
        /// <param name="productTypeDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Add(ProductTypeDto productTypeDto, string username);

        /// <summary>
        /// Update a new Product Type
        /// </summary>
        /// <param name="productTypeDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(ProductTypeDto productTypeDto, string username);

        /// <summary>
        /// Delete a Product Type
        /// </summary>
        /// <param name="productTypeId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int productTypeId, string username);
        
        /// <summary>
        /// Find a Product Type by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<ProductTypeDto> Find(int id);

        /// <summary>
        /// Add A New Product Type
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<bool> IsInUse(int id);
        
        /// <summary>
        /// Check if the Product Type Name is in use by another Product Type (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool NameUsedByAnother(string name, int id);
        
        /// <summary>
        /// Check if the Product Type Name is in use by another Product Type (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool IsProductTypeNameInUse(string name);
        
    }
}
