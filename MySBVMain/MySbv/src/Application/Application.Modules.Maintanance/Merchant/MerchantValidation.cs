using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.Merchant;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Serializer;
using Utility.Core;

namespace Application.Modules.Maintanance.Merchant
{
    public class MerchantValidation : IMerchantValidation
    {
        #region Fields

        private readonly IUserAccountValidation _userAccountValidation;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly ILookup _lookup;
        private readonly ISerializer _serializer;

        #endregion

        #region Constructor

        public MerchantValidation(IRepository repository,ILookup lookup, IMapper mapper, ISerializer serializer, IUserAccountValidation userAccountValidation)
        {
            _mapper = mapper;
            _repository = repository;
            _lookup = lookup;
            _serializer = serializer;
            _userAccountValidation = userAccountValidation;
        }

        #endregion

        #region IMerchant Validation

        public IEnumerable<ListMerchantDto> All()
        {
            var merchants = _repository.All<Domain.Data.Model.Merchant>( b => b.Status, c => c.CompanyType, d => d.MerchantDescription);
            return merchants.Select(merchant => _mapper.Map<Domain.Data.Model.Merchant, ListMerchantDto>(merchant)).ToList();
        }

        public MethodResult<MerchantDto> Find(int id)
        {
            var merchant = _repository.Query<Domain.Data.Model.Merchant>(a => a.MerchantId == id, 
                c => c.CompanyType, 
                c => c.MerchantDescription, 
                c => c.Sites,
                c => c.Sites.Select(e => e.Address),
                c => c.Sites.Select(a => a.SiteContainers),
                c => c.Sites.Select(a => a.Accounts),
                c => c.Sites.Select(a => a.Accounts.Select(b => b.AccountType)),
                c => c.Sites.Select(a => a.Accounts.Select(b => b.TransactionType)),
                c => c.Sites.Select(a => a.Accounts.Select(b => b.Bank))
                ).FirstOrDefault(a => a.MerchantId == id);

            if (merchant != null)
            {
                var mappedMerchant = _mapper.Map<Domain.Data.Model.Merchant, MerchantDto>(merchant);
                mappedMerchant.Sites = mappedMerchant.Sites.Where(o => o.IsNotDeleted).ToList();

                return new MethodResult<MerchantDto>(MethodStatus.Successful, mappedMerchant);
            }
            return new MethodResult<MerchantDto>(MethodStatus.NotFound, null, "Merchant Not Found.");
            
        }

        public MethodResult<int> Add(MerchantDto merchantDto, string username)
        {
            int userId = _userAccountValidation.UserByName(username).UserId;
            var merchant = _mapper.Map<MerchantDto, Domain.Data.Model.Merchant>(merchantDto);
            merchant.StatusId = _lookup.GetStatusId("ACTIVE");  //SAVED
            merchant.LastChangedById = userId;
            merchant.LastChangedDate = DateTime.Now;
            merchant.CreatedById = userId;
            merchant.CreateDate = DateTime.Now;
            var result = _repository.Add(merchant) > 0;

            return result ? new MethodResult<int>(MethodStatus.Successful, merchant.MerchantId) :
                new MethodResult<int>(MethodStatus.Error, 0, "Merchant Not added. Server Error.");
        }

        public MethodResult<bool> Edit(MerchantDto merchantDto, string username)
        {
            //Get the logged on user id 
            int userId = _userAccountValidation.UserByName(username).UserId;

            // Only update the approval status from ACTIVE to ACTIVE_BUT_AMENDED
            // Then save the object
            var tmp = _repository.Find<Domain.Data.Model.Merchant>(merchantDto.MerchantId);

            if(tmp == null)
                return new MethodResult<bool>(MethodStatus.Error, false, "Error, Merchant Not Found.");
            
                var merchant = _mapper.Map<MerchantDto, Domain.Data.Model.Merchant>(merchantDto);
                merchant.StatusId = _lookup.GetStatusId("ACTIVE");
                merchant.CreatedById = tmp.CreatedById;
                merchant.CreateDate = tmp.CreateDate;
                merchant.LastChangedById = userId;
                merchant.LastChangedDate = DateTime.Now;
                merchant.IsNotDeleted = true;
                merchant.EntityState = State.Modified;

                return (_repository.Update(merchant) > 0)
                                ? new MethodResult<bool>(MethodStatus.Successful, true, "Merchant was successfully Updated.")
                                : new MethodResult<bool>(MethodStatus.Error, false, "Merchant was not successfully Updated.");
                  
        }

        public MethodResult<Task> Delete(int id, string username)
        {
            var result = IsInUse(id);

            if (result.Status == MethodStatus.Successful)
            {
                //Get the logged on user ID
                var userId = _userAccountValidation.UserByName(username).UserId;
                var merchant = _repository.Query<Domain.Data.Model.Merchant>(a => a.MerchantId == id).FirstOrDefault();

                if (merchant != null)
                {

                    if (_repository.Delete<Domain.Data.Model.Merchant>(id, userId) > 0)
                    {
                        return new MethodResult<Task>(MethodStatus.Successful, new Task(),
                            "Merchant was successfully deleted.");
                    }
                }
                return new MethodResult<Task>(MethodStatus.Error, null, "Merchant was not deleted successfully.");
            }
            return new MethodResult<Task>(MethodStatus.Error, null, result.Message);
        }

        public MethodResult<int> Submit(MerchantDto merchantDto, string username)
        {
            int userId = _userAccountValidation.UserByName(username).UserId;
            Domain.Data.Model.Merchant merchant = _mapper.Map<MerchantDto, Domain.Data.Model.Merchant>(merchantDto);
            merchant.StatusId = _lookup.GetStatusId("ACTIVE"); // Pending
            merchant.CreatedById = merchant.LastChangedById;
            merchant.LastChangedById = userId; 
            merchant.IsNotDeleted = true;
            return (_repository.Add(merchant) > 0)
                ? new MethodResult<int>(MethodStatus.Successful, merchant.MerchantId, "Merchant was successfully Added.")
                : new MethodResult<int>(MethodStatus.Error, 0,
                    "Merchant was not successfully Added.");
        }

        public MethodResult<bool> IsRejected(RejectMerchantArgumentsDto rejectParameters, MerchantDto merchantDto, string username)
        {;
        int userId = _userAccountValidation.UserByName(username).UserId;
            var mappedMerchant = _mapper.Map<MerchantDto, Domain.Data.Model.Merchant>(merchantDto);

            mappedMerchant.Comments = rejectParameters.Comments;
            mappedMerchant.StatusId = _lookup.GetStatusId("REJECT");
            mappedMerchant.LastChangedById = userId;
            mappedMerchant.LastChangedDate = DateTime.Now;
            mappedMerchant.CreateDate = DateTime.Now;
            mappedMerchant.CreatedById = userId;
            mappedMerchant.IsNotDeleted = true;
            mappedMerchant.EntityState = State.Modified;

            var result = _repository.Update(mappedMerchant) > 0;
            return result ? new MethodResult<bool>(MethodStatus.Successful, true, "Merchant was successfully. Rejected") :
                new MethodResult<bool>(MethodStatus.Error, false, "Merchant Not Rejected. Server Error");
        }
        
        #endregion

        #region Helpers

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Domain.Data.Model.Site>(a => a.MerchantId == id && a.IsNotDeleted) ?
                new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a merchant that is linked to and active site.") :
                new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsContractNumberUsed(string contractNumber)
        {
            return _repository.Any<Domain.Data.Model.Merchant>(o => o.ContractNumber.ToLower() == contractNumber.ToLower());
        }

        public bool IsContractNumberUsedByAnother(string contractNumber, int merchantId)
        {
            return _repository.Any<Domain.Data.Model.Merchant>(o => o.ContractNumber.ToLower() == contractNumber.ToLower() && o.MerchantId != merchantId);
        }


        public bool IsRegistrationNumberUsed(string registrationNumber)
        {
            return _repository.Any<Domain.Data.Model.Merchant>(o => o.RegistrationNumber.ToLower() == registrationNumber.ToLower());
        }

        public bool IsRegistrationNumberUsedByAnother(string registrationNumber, int merchantId)
        {
            return _repository.Any<Domain.Data.Model.Merchant>(o => o.RegistrationNumber.ToLower() == registrationNumber.ToLower() && o.MerchantId != merchantId);
        }


        public bool IsNameUsed(string name)
        {
            return _repository.Any<Domain.Data.Model.Merchant>(o => o.Name.ToLower() == name.ToLower());
        }

        public bool IsNameUsedByAnother(string name, int merchantId)
        {
            return _repository.Any<Domain.Data.Model.Merchant>(o => o.Name.ToLower() == name.ToLower() && o.MerchantId != merchantId);
        }

        #endregion
    }
}