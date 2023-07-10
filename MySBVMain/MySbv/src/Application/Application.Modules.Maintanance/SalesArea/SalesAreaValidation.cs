using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.SalesArea;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.SalesArea
{
    public class SalesAreaValidation : ISalesAreaValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IUserAccountValidation _userAccountValidation;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public SalesAreaValidation(IMapper mapper, IUserAccountValidation userAccountValidation, ILookup iLookup, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _userAccountValidation = userAccountValidation;
            _repository = repository;
        }

        #endregion

        #region ISalesArea Validation

        public IEnumerable<SalesAreaDto> All()
        {
            var salesareas = _repository.All<Domain.Data.Model.SalesArea>();

            var listOfSalesArea = new List<SalesAreaDto>();

            foreach (var salesArea in salesareas)
            {
                var item = _mapper.Map<Domain.Data.Model.SalesArea, SalesAreaDto>(salesArea);
                listOfSalesArea.Add(item);
            }
            return listOfSalesArea;
        }

        public MethodResult<bool> Add(SalesAreaDto salesAreaDto, string username)
        {
            var userId = _userAccountValidation.UserByName(username).UserId;

            var salesArea = _mapper.Map<SalesAreaDto, Domain.Data.Model.SalesArea>(salesAreaDto);
            salesArea.CreatedById = userId;
            salesArea.CreateDate = DateTime.Now;
            salesArea.LastChangedById = userId;
            salesArea.LastChangedDate = DateTime.Now;

            return _repository.Add(salesArea) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Sales Area Not Added");
        }

        public MethodResult<bool> Edit(SalesAreaDto salesAreaDto, string username)
        {
            //Get the logged on user id 
            var userId = _userAccountValidation.UserByName(username).UserId;
            var tempSalesArea =
                _repository.Query<Domain.Data.Model.SalesArea>(a => a.SalesAreaId == salesAreaDto.SalesAreaId)
                    .FirstOrDefault();

            var mappedSalesArea = _mapper.Map<SalesAreaDto, Domain.Data.Model.SalesArea>(salesAreaDto);
            mappedSalesArea.EntityState = State.Modified;
            mappedSalesArea.IsNotDeleted = true;
            mappedSalesArea.LastChangedById = userId;
            mappedSalesArea.LastChangedDate = DateTime.Now;
            if (tempSalesArea != null)
            {
                mappedSalesArea.CreateDate = tempSalesArea.CreateDate;
                mappedSalesArea.CreatedById = tempSalesArea.CreatedById;
            }

            return _repository.Update(mappedSalesArea) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");
        }

        public MethodResult<bool> Delete(int salesAreaId, string username)
        {
            var userId = _userAccountValidation.UserByName(username).UserId;
            if (_repository.Delete<Domain.Data.Model.SalesArea>(salesAreaId, userId) > 0)
            {
                return new MethodResult<bool>(MethodStatus.Successful, true);
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Sales Area Not Found");
        }

        public MethodResult<SalesAreaDto> Find(int id)
        {
            var salesArea = _repository.Find<Domain.Data.Model.SalesArea>(id);

            if (salesArea == null) return new MethodResult<SalesAreaDto>(MethodStatus.Error, null, "Sales Area Not Found.");
            var mappedSalesArea = _mapper.Map<Domain.Data.Model.SalesArea, SalesAreaDto>(salesArea);

            return new MethodResult<SalesAreaDto>(MethodStatus.Successful, mappedSalesArea);
        }

        public bool IsInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.SalesArea>(a => a.Name.ToLower() == name.ToLower());
        }

        #endregion

        #region Helpers

        public bool IsAreaNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.SalesArea>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnotherArea(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.SalesArea>(a => a.Name.ToLower() == name.ToLower() && a.SalesAreaId != id);
        }
        
        #endregion

    }
}