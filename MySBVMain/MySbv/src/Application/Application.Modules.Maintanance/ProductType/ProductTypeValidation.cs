using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Data.Model;
using Application.Dto.ProductType;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.ProductType
{
    public class ProductTypeValidation : IProductTypeValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IUserAccountValidation _accountValidation;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;


        #endregion

        #region Constructor

        public ProductTypeValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _accountValidation = accountValidation;
            _repository = repository;
        }

        #endregion

        #region IProductType Validation

        public IEnumerable<ProductTypeDto> All()
        {
            var productTypes = _repository.All<Domain.Data.Model.ProductType>();
            return productTypes.Select(productype => _mapper.Map<Domain.Data.Model.ProductType, ProductTypeDto>(productype)).ToList();
        }

        public MethodResult<bool> Add(ProductTypeDto productTypeDto, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;

            var poductType = _mapper.Map<ProductTypeDto, Domain.Data.Model.ProductType>(productTypeDto);
            poductType.CreatedById = userId;
            poductType.LastChangedById = userId;
            poductType.Description = productTypeDto.Name;
			poductType.LookUpKey = poductType.Name.Replace(".", "_").Replace(" ", "_").ToUpper();

	        var date = DateTime.Now;

			((IEntity)poductType).WalkObjectGraph(o =>
			{
				o.CreatedById = userId;
				o.CreateDate = date;
				o.LastChangedById = userId;
				o.LastChangedDate = date; 
				o.IsNotDeleted = true;
				o.EntityState = State.Added;
				return false;
			}, a => { });

            return _repository.Add(poductType) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Product Type Not Added");
        }

        public MethodResult<bool> Edit(ProductTypeDto productTypeDto, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;
            var productType = _repository.Find<Domain.Data.Model.ProductType>(productTypeDto.ProductTypeId);

            productType.Name = productTypeDto.Name;
            productType.LastChangedById = userId;
            productType.LastChangedDate = DateTime.Now;
            productType.CreateDate = productType.CreateDate;
            productType.CreatedById = productType.CreatedById;
            productType.EntityState = State.Modified;

            return _repository.Update(productType) > 0 ?
                 new MethodResult<bool>(MethodStatus.Successful, true) :
                 new MethodResult<bool>(MethodStatus.Error, false, "Product Type Not Updated");
        }

        public MethodResult<bool> Delete(int productTypeId, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;
            var result = IsInUse(productTypeId);
            if (result.Status == MethodStatus.Successful)
            {
                var _result = _repository.Delete<Domain.Data.Model.ProductType>(productTypeId, userId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Product Type Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, result.Message);
        }

        public MethodResult<ProductTypeDto> Find(int id)
        {
            var productType = _repository.Find<Domain.Data.Model.ProductType>(id);

            if (productType == null) return new MethodResult<ProductTypeDto>(MethodStatus.Error, null, "Product Type Not Found.");

            var mappedProductType = _mapper.Map<Domain.Data.Model.ProductType, ProductTypeDto>(productType);

            return new MethodResult<ProductTypeDto>(MethodStatus.Successful, mappedProductType);
        }

        #endregion

        #region Helper

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Domain.Data.Model.Product>(a => a.ProductTypeId == id & a.IsNotDeleted) ?
                new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a Product Type that is linked to an active Product.") :
                new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsProductTypeNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.ProductType>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnother(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.ProductType>(a => a.Name.ToLower() == name.ToLower() && a.ProductTypeId != id);
        }

        #endregion

    }
}