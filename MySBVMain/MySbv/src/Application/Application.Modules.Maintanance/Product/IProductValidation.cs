using System.Collections.Generic;
using Application.Dto.Product;
using Utility.Core;

namespace Application.Modules.Maintanance.Product
{
    public interface IProductValidation
    {
        /// <summary>
        ///     Get a list of all products
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductDto> All();

        /// <summary>
        ///     Add a new Product
        /// </summary>
        /// <param name="productDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<int> Add(ProductDto productDto, string username);

        /// <summary>
        ///     Update a product
        /// </summary>
        /// <param name="productDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(ProductDto productDto, string username);

        /// <summary>
        /// Edit Product Fee
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        void DeleteProductFees(int productId, string username);

        /// <summary>
        ///     Find a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<ProductDto> Find(int id);

        /// <summary>
        ///     delete Product
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Delete(int id, string username);

        /// <summary>
        /// Check if the Device Name is in use by another Device (EDIT SCREEN)
        /// Once it has been edited
        /// </summary>
        /// <param name="serialNumber"></param>
        /// /// <param name="id"></param>
        /// <returns></returns>
        bool SerialNumberUsedByAnotherDevice(string serialNumber, int id);
        
        /// <summary>
        /// Check if the Serial Number is in use by another Device
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        bool IsSerialNumberInUse(string serialNumber);


    }
}