using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Dto.CashDeposit;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.ContainerType
{
    public class ContainerTypeValidation : IContainerTypeValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _accountValidation;

        #endregion

        #region Constructor

        public ContainerTypeValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _repository = repository;
            _accountValidation = accountValidation;
        }

        #endregion

        #region IContainer Type Validation

        public MethodResult<bool> Add(ContainerTypeDto containerTypeDto, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;
            var containerType = _mapper.Map<ContainerTypeDto, Domain.Data.Model.ContainerType>(containerTypeDto);
            containerType.LastChangedById = userId;
            containerType.LastChangedDate = DateTime.Now;
            containerType.CreatedById = userId;
            containerType.CreateDate = DateTime.Now;

            return _repository.Add(containerType) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Container Type Not Added");
        }

        public bool ContainerTypeNameExists(string name)
        {
            return _repository.All<Domain.Data.Model.ContainerType>()
                    .Any(o => o.Name.ToLower() == name.ToLower() && o.IsNotDeleted);
        }

        public MethodResult<bool> Edit(ContainerTypeDto containerTypeDto, string username)
        {
            int userId = _accountValidation.UserByName(username).UserId;
            var tempContainerType =
                _repository.Query<Domain.Data.Model.ContainerType>(
                    a => a.ContainerTypeId == containerTypeDto.ContainerTypeId).FirstOrDefault();

            var mappedContainerType = _mapper.Map<ContainerTypeDto, Domain.Data.Model.ContainerType>(containerTypeDto);
            mappedContainerType.EntityState = State.Modified;
            mappedContainerType.IsNotDeleted = true;
            mappedContainerType.LastChangedDate = DateTime.Now;
            mappedContainerType.LastChangedById = userId;
            if (tempContainerType != null)
            {
                mappedContainerType.CreateDate = tempContainerType.CreateDate;
                mappedContainerType.CreatedById = tempContainerType.CreatedById;
            }

            return _repository.Update(mappedContainerType) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Container Type Not Updated");
        }

        public MethodResult<bool> Delete(int id, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;

            var result = IsInUse(id);
            if (result.Status == MethodStatus.Successful)
            {
                var _result = _repository.Delete<Domain.Data.Model.ContainerType>(id, userId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Container Type Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, result.Message);
        }

        public MethodResult<ContainerTypeDto> Find(int id)
        {
            var containertype = _repository.Query<Domain.Data.Model.ContainerType>(a => a.ContainerTypeId == id, a => a.ContainerTypeAttributes).FirstOrDefault();

            var mappedContainerType = _mapper.Map<Domain.Data.Model.ContainerType, ContainerTypeDto>(containertype);

            return new MethodResult<ContainerTypeDto>(MethodStatus.Successful, mappedContainerType);
        }

        public IEnumerable<ContainerTypeDto> All()
        {
            var containerTypes = _repository.All<Domain.Data.Model.ContainerType>(a => a.ContainerTypeAttributes, c => c.Containers);

            var listOfContainerTypes = new List<ContainerTypeDto>();

            foreach (var containerType in containerTypes)
            {
                var item = _mapper.Map<Domain.Data.Model.ContainerType, ContainerTypeDto>(containerType);
                listOfContainerTypes.Add(item);
            }
            return listOfContainerTypes;
        }

        #endregion

        #region Helpers

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Container>(a => a.ContainerTypeId == id && a.IsNotDeleted) ?
                  new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a container Type that is linked to and active container.") :
              new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsContainerTypeNameInUse(string name, int Id)
        {
            return _repository.Any<Domain.Data.Model.ContainerType>(a => a.Name.Trim().ToLower() == name.Trim().ToLower() && a.ContainerTypeId != Id);
        }

        #endregion

    }
}
