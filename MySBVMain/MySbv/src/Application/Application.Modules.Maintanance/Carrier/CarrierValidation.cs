using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.Carrier;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.Carrier
{
    public class CarrierValidation : ICarrierValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _accountValidation;

        #endregion

        #region Constructor

        public CarrierValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _repository = repository;
            _accountValidation = accountValidation;
        }

        #endregion

        #region ICarrierValidation

        public IEnumerable<CitCarrierDto> All()
        {
            var carriers = _repository.Query<CitCarrier>(a => a.IsNotDeleted);

            var listOfCarrier = new List<CitCarrierDto>();

            foreach (var carrier in carriers)
            {
                var item = _mapper.Map<CitCarrier, CitCarrierDto>(carrier);

                listOfCarrier.Add(item);
            }

            return listOfCarrier;
        }
        public MethodResult<bool> Add(CitCarrierDto carrierDto, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;

            var carrier = _mapper.Map<CitCarrierDto, CitCarrier>(carrierDto);
            carrier.CreatedById = userId;
            carrier.CreateDate = DateTime.Now;
            carrier.LastChangedById = userId;
            carrier.LastChangedDate = DateTime.Now;

            return _repository.Add(carrier) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Carrier Not Added");
        }
        public MethodResult<bool> Edit(CitCarrierDto carrierDto, string username)
        {
            int userId = _accountValidation.UserByName(username).UserId;
            var tempCarrier =
                _repository.Query<CitCarrier>(a => a.CitCarrierId == carrierDto.CitCarrierId).FirstOrDefault();

            var mappedCitCarrier = _mapper.Map<CitCarrierDto, CitCarrier>(carrierDto);
            mappedCitCarrier.EntityState = State.Modified;
            mappedCitCarrier.IsNotDeleted = true;
            mappedCitCarrier.LastChangedById = userId;
            mappedCitCarrier.LastChangedDate = DateTime.Now;
            if (tempCarrier != null)
            {
                mappedCitCarrier.CreateDate = tempCarrier.CreateDate;
                mappedCitCarrier.CreatedById = tempCarrier.CreatedById;
            }

            return _repository.Update(mappedCitCarrier) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");
        }
        public MethodResult<bool> Delete(int citCarrierId, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;
            var result = IsInUse(citCarrierId);
            if (result.Status == MethodStatus.Successful)
            {
                var _result = _repository.Delete<CitCarrier>(citCarrierId, userId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Cit Carrier Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, result.Message);
        }
        public MethodResult<CitCarrierDto> Find(int id)
        {
            var carrier = _repository.Find<CitCarrier>(id);
            var mappedCarrier = _mapper.Map<CitCarrier, CitCarrierDto>(carrier);

            return new MethodResult<CitCarrierDto>(MethodStatus.Successful, mappedCarrier);
        }
        
        #endregion

        #region Helper

        public MethodResult<bool> IsInUse(int citCarrierId)
        {
            return _repository.Any<Domain.Data.Model.Site>(a => a.CitCarrierId == citCarrierId & a.IsNotDeleted) ?
                new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a Cit Carrier that is linked to and active Site.") :
                new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsCarrierNameInUse(string name)
        {
            return _repository.Any<CitCarrier>(a => a.Name.Trim().ToLower() == name.Trim().ToLower());
        }

        public bool CarrierNameExists(string carrierName)
        {
            return
                _repository.All<CitCarrier>()
                    .Any(o => o.Name.ToLower() == carrierName.ToLower() && o.IsNotDeleted);
        }

        public bool NameUsedByAnotherCarrier(string name, int id)
        {
            return _repository.Any<CitCarrier>(a => a.Name.ToLower() == name.ToLower() && a.CitCarrierId != id);
        }


        #endregion
    }
}