using System;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.Device;
using Application.Dto.Product;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Utility.Core;
using State = Domain.Data.Core.State;
using TransactionScope = System.Transactions.TransactionScope;
 
namespace Application.Modules.Maintanance.Product
{
    public class ProductValidation : IProductValidation
    {
        #region Fields

        private readonly IRepository _repository;
        private readonly IUserAccountValidation _userAccountValidation;
        private readonly IMapper _mapper;
        private readonly ILookup _lookup;

        #endregion
        
        #region Constructor

        public ProductValidation(IRepository repository, IUserAccountValidation userAccountValidation, IMapper mapper, ILookup lookup)
        {
            _repository = repository;
            _userAccountValidation = userAccountValidation;
            _mapper = mapper;
            _lookup = lookup;
        }

        #endregion

        #region IProduct Validation

        public IEnumerable<ProductDto> All()
        {
            var products = _repository.Query<Domain.Data.Model.Product>(a => a.IsNotDeleted, s => s.Site, pt => pt.ProductType, st => st.SettlementType, sta => sta.Status, st => st.ServiceType, d => d.DeviceType, d => d.Device, pf => pf.ProductFees);

            var list = new List<ProductDto>();

            foreach (var product in products)
            {
                var item = _mapper.Map<Domain.Data.Model.Product, ProductDto>(product);
                list.Add(item);
            }
            return list;
        }

        public MethodResult<int> Add(ProductDto productDto, string username)
        {
            using (var scope = new TransactionScope())
            {
                int productVault = _lookup.GetProductTypeId("MYSBV_VAULT");

                var userId = _userAccountValidation.UserByName(username).UserId;
                var mappedProduct = _mapper.Map<ProductDto, Domain.Data.Model.Product>(productDto);
                mappedProduct.CreatedById = userId;
                mappedProduct.CreateDate = DateTime.Now;
                mappedProduct.LastChangedById = userId;
                mappedProduct.LastChangedDate = DateTime.Now;
                
                //If Product = Valut (Create Device AND Get Device ID)
                if (productDto.ProductTypeId == productVault)
                {
                    var results = AddNewDevice(productDto, userId);
                    mappedProduct.DeviceId = results.EntityResult;
                }

                
                if (_repository.Add(mappedProduct) > 0)
                {
                    //ADD PRODUCT FEES
                    if (productDto.ProductFees != null)
                    {
                        AddNewProductFees(productDto, mappedProduct, userId);
                    }
                    
                    scope.Complete();
                    return new MethodResult<int>(MethodStatus.Successful, mappedProduct.ProductId,
                        "Product was successfully Added.");
                }
            }
            return new MethodResult<int>(MethodStatus.Error, 0, "Product was not successfully Added.");
            
        }

        public MethodResult<bool> Edit(ProductDto productDto, string username)
        {
            int userId = _userAccountValidation.UserByName(username).UserId;
            int productVault = _lookup.GetProductTypeId("MYSBV_VAULT");

            var tempProduct =
                _repository.Query<Domain.Data.Model.Product>(a => a.ProductId == productDto.ProductId).FirstOrDefault();

            productDto.ProductFees = _repository.Query<ProductFee>(a => a.ProductId == productDto.ProductId && a.IsActive == true)
                       .Select(a => _mapper.Map<ProductFee, ProductFeeDto>(a))
                       .ToList();

            var implementationDate = Convert.ToDateTime(productDto.ImplementationDateString);
            var terminationDate = DateTime.Now; //Set the Termination Date to now to prevent errors
            var terminationChaged = false;

            if (!string.IsNullOrWhiteSpace(productDto.TerminationDateString))
            {
                terminationDate = Convert.ToDateTime(productDto.TerminationDateString);
                terminationChaged = true;
            }

            Domain.Data.Model.Product mappedProduct = _mapper.Map<ProductDto, Domain.Data.Model.Product>(productDto);
            mappedProduct.EntityState = State.Modified;
            mappedProduct.LastChangedById = userId;
            mappedProduct.LastChangedDate = DateTime.Now;
            mappedProduct.IsNotDeleted = true;
            mappedProduct.ImplementationDate = implementationDate;
            if (tempProduct != null)
            {
                mappedProduct.CreateDate = tempProduct.CreateDate;
                mappedProduct.CreatedById = tempProduct.CreatedById;
            }

            if (terminationChaged)
            {
                mappedProduct.TerminationDate = terminationDate;
            }
            else
            {
                mappedProduct.TerminationDate = null;
            }

            if (productDto.FeeCodes != null)
            {
                var feeIds = productDto.FeeCodes.Select(feeCode => _lookup.GetFee(feeCode).FeeId).ToList();

                IEnumerable<int> added = feeIds.Except(mappedProduct.ProductFees.Select(a => a.FeeId));
                IEnumerable<int> removed = mappedProduct.ProductFees.Select(a => a.FeeId).Except(feeIds.Select(a => a));

                AddNewProductFeessOnUpdate(added, mappedProduct, userId);
                MarkRemovedProductFees(removed, mappedProduct, userId);
            }
            else
            {
                IEnumerable<int> feeIDsToBeRemoved = productDto.ProductFees.Select(a => a.FeeId);
                MarkRemovedProductFees(feeIDsToBeRemoved, mappedProduct, userId);
            }

            if (_repository.Update(mappedProduct) > 0)
            {
                if (productDto.ProductTypeId == productVault)
                {
                    UpdateDeviceInfo(productDto, userId);
                }

                return new MethodResult<bool>(MethodStatus.Successful, true);
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Product Not Updated");

        }

        public MethodResult<ProductDto> Find(int id)
        {
            var product =
                _repository.Query<Domain.Data.Model.Product>(a => a.ProductId == id,
                        si => si.Site,
                        pd => pd.ProductType,
                        st => st.ServiceType,
                        stt => stt.SettlementType,
                        dt => dt.DeviceType,
                        d => d.Device,
                        stu => stu.Status).FirstOrDefault(a => a.ProductTypeId == id);


            var mappedProduct = _mapper.Map<Domain.Data.Model.Product, ProductDto>(product);
            if (product != null) mappedProduct.CitCode = product.Site.CitCode;
            if (product != null) mappedProduct.SerialNumber = product.Device.SerialNumber;

            return new MethodResult<ProductDto>(MethodStatus.Successful, mappedProduct);
        }

        public MethodResult<bool> Delete(int id, string username)
        {
            using (var scope = new TransactionScope())
            {
                var userId = _userAccountValidation.UserByName(username).UserId;
                var product = _repository.Query<Domain.Data.Model.Product>(a => a.ProductId == id).FirstOrDefault();

                if (_repository.Delete<Domain.Data.Model.Product>(id, userId) > 0)
                {
                    //Delete the products
                    DeleteProductFees(id, username);

                    //Delete the Device for Product Type Valut
                    if (product != null && product.ProductTypeId == 2)
                    {
                        if (product.DeviceId != null)
                            _repository.Delete<Domain.Data.Model.Device>(product.DeviceId.Value, userId);
                    }

                    scope.Complete();
                    return new MethodResult<bool>(MethodStatus.Successful, true, "Product deleted successfully");
                }
            }

            return new MethodResult<bool>(MethodStatus.Error, false, "Product not deleted. contact SBV for support");
        }
        
        public void DeleteProductFees(int productId, string username)
        {
            int userId = _userAccountValidation.UserByName(username).UserId;

            var productFees = _repository.Query<ProductFee>(a => a.ProductId == productId)
                       .Select(a => _mapper.Map<ProductFee, ProductFeeDto>(a))
                       .ToList();

            foreach (var productFee in productFees)
            {
                ProductFeeDto productFeeDto = new ProductFeeDto
                {
                    ProductId = productId,
                    FeeId = productFee.FeeId,
                    IsActive = false
                };

                ProductFee mappedProductFee = _mapper.Map<ProductFeeDto, ProductFee>(productFeeDto);
                mappedProductFee.EntityState = State.Modified;
                mappedProductFee.LastChangedById = userId;
                mappedProductFee.IsNotDeleted = false;
                mappedProductFee.LastChangedDate = DateTime.Now;

                _repository.Update(mappedProductFee);
            }
        }
        
        public void AddNewProductFees(ProductDto productDto, Domain.Data.Model.Product mappedProduct, int loggedInUserId)
        {
            foreach (string feeCode in productDto.FeeCodes)
            {
                var fee = _lookup.GetFee(feeCode);
                var productId = mappedProduct.ProductId;
                var date = DateTime.Now;

                this.Log().Debug(string.Format("ProductId - [{0}] FeeId - [{1}]", productId, fee.FeeId));
                
                var productFeeDto = new ProductFeeDto
                {
                    ProductId = productId,
                    FeeId = fee.FeeId,
                    IsActive = true
                };

                var mappedProductFees = _mapper.Map<ProductFeeDto, ProductFee>(productFeeDto);
                ((IEntity)mappedProductFees).WalkObjectGraph(o =>
                {
                    o.CreatedById = loggedInUserId;
                    o.CreateDate = date;
                    o.LastChangedById = loggedInUserId;
                    o.LastChangedDate = date;
                    o.IsNotDeleted = true;
                    o.EntityState = State.Added;
                    return false;
                }, a => { });

                if (_repository != null) _repository.Add(mappedProductFees);
            }
        }

        public MethodResult<int> AddNewDevice(ProductDto productDto, int loggedInUserId)
        {
            if (productDto.DeviceTypeId != null)
            {
                var deviceType =
                    _repository.Query<Domain.Data.Model.DeviceType>(a => a.DeviceTypeId == productDto.DeviceTypeId).FirstOrDefault();

                if (deviceType != null)
                {
                    var deviceDto = new DeviceDto
                    {
                        DeviceTypeId = productDto.DeviceTypeId.Value,
                        SerialNumber = productDto.SerialNumber,
                        Name = deviceType.Name,
                        Description = deviceType.Description,
                        LookUpKey = deviceType.LookUpKey
                    };


                    var mappedDevice = _mapper.Map<DeviceDto, Device>(deviceDto);
                    ((IEntity)mappedDevice).WalkObjectGraph(o =>
                    {
                        o.CreatedById = loggedInUserId;
                        o.CreateDate = DateTime.Now;
                        o.LastChangedById = loggedInUserId;
                        o.LastChangedDate = DateTime.Now;
                        o.IsNotDeleted = true;
                        o.EntityState = State.Added;
                        return false;
                    }, a => { });
                    
                    if (_repository != null)
                    {
                        var result = _repository.Add(mappedDevice) > 0;

                        return result ? new MethodResult<int>(MethodStatus.Successful, mappedDevice.DeviceId) :
                    new MethodResult<int>(MethodStatus.Error, 0, "Device Not added. Server Error.");
                        
                    }
                    return new MethodResult<int>(MethodStatus.Error, 0, "Device Not added. Server Error.");
                }
                return new MethodResult<int>(MethodStatus.Error, 0, "Device Not added. Server Error.");
            }
            return new MethodResult<int>(MethodStatus.Error, 0, "Device Not added. Server Error.");
        }
        
        private void AddNewProductFeessOnUpdate(IEnumerable<int> added, Domain.Data.Model.Product mappedProduct, int loggedInUserId)
        {
            foreach (int feeId in added)
            {
                
                var productId = mappedProduct.ProductId;
                var date = DateTime.Now;

                ProductFeeDto productFeeDto = new ProductFeeDto
                {
                    ProductId = productId,
                    FeeId = feeId,
                    IsActive = true
                };

                ProductFee recordAlreadyExist = _repository.Query<ProductFee>(a => a.IsActive == false && a.ProductId == productId && a.FeeId == feeId).FirstOrDefault();

                var mappedProductFees = _mapper.Map<ProductFeeDto, ProductFee>(productFeeDto);
                
                 if (recordAlreadyExist == null)
                 {
                     ((IEntity)mappedProductFees).WalkObjectGraph(o =>
                     {
                         o.CreatedById = loggedInUserId;
                         o.CreateDate = date;
                         o.LastChangedById = loggedInUserId;
                         o.LastChangedDate = date;
                         o.IsNotDeleted = true;
                         o.EntityState = State.Added;
                         return false;
                     }, a => { });

                     if (_repository != null) _repository.Add(mappedProductFees);
                 }
                 else
                 {
                     ((IEntity)mappedProductFees).WalkObjectGraph(o =>
                     {
                         o.CreatedById = loggedInUserId;
                         o.CreateDate = date;
                         o.LastChangedById = loggedInUserId;
                         o.LastChangedDate = date;
                         o.IsNotDeleted = true;
                         o.EntityState = State.Modified;
                         return false;
                     }, a => { });

                     if (_repository != null) _repository.Update(mappedProductFees);
                 }
            }
        }

        private void MarkRemovedProductFees(IEnumerable<int> removed, Domain.Data.Model.Product mappedProductFees, int userId)
        {
            foreach (int feeId in removed)
            {
                ProductFee temp = _repository.Query<ProductFee>(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).FirstOrDefault();

                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).LastChangedById = userId;
                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).LastChangedDate = DateTime.Now;
                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).CreatedById = temp.CreatedById;
                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).CreateDate = temp.CreateDate;
                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).IsActive = false;
                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).IsNotDeleted = true;
                mappedProductFees.ProductFees.FirstOrDefault(a => a.ProductId == mappedProductFees.ProductId && a.FeeId == feeId).EntityState = State.Modified;
            }
        }
        
        public void UpdateDeviceInfo(ProductDto productDto, int loggedInUserId)
        {
            var deviceType =
                _repository.Query<Domain.Data.Model.DeviceType>(a => a.DeviceTypeId == productDto.DeviceTypeId).FirstOrDefault();

            var tempDevice = _repository.Query<Device>(a => a.DeviceId == productDto.DeviceId).FirstOrDefault();

            if (deviceType != null && productDto.DeviceId != null)
            {
                var deviceDto = new DeviceDto
                {
                    DeviceId = productDto.DeviceId.Value,
                    DeviceTypeId = deviceType.DeviceTypeId,
                    SerialNumber = productDto.SerialNumber,
                    Name = deviceType.Name,
                    Description = deviceType.Description,
                    LookUpKey = deviceType.LookUpKey
                };
                
                Device mappedDevice = _mapper.Map<DeviceDto, Device>(deviceDto);
                mappedDevice.EntityState = State.Modified;
                mappedDevice.LastChangedById = loggedInUserId;
                mappedDevice.LastChangedDate = DateTime.Now;
                if (tempDevice != null)
                {
                    mappedDevice.CreateDate = tempDevice.CreateDate;
                    mappedDevice.CreatedById = tempDevice.CreatedById;
                }
                mappedDevice.IsNotDeleted = true;
                mappedDevice.LastChangedDate = DateTime.Now;

                _repository.Update(mappedDevice);
            }
        }
        
        #endregion
        
        #region Helper

        public bool SerialNumberUsedByAnotherDevice(string serialNumber, int id)
        {
            return _repository.Any<Domain.Data.Model.Device>(a => a.SerialNumber.ToLower() == serialNumber.ToLower() && a.DeviceId != id);
        }

        public bool IsSerialNumberInUse(string serialNumber)
        {
            return _repository.Any<Domain.Data.Model.Device>(a => a.SerialNumber.ToLower() == serialNumber.ToLower());
        }

        #endregion

    }
}
