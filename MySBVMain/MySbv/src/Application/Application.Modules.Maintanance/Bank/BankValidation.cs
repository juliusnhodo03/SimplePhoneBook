using System;
using System.Collections.Generic;
using System.Linq;
using Application.Dto.Bank;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.Maintanance.Bank
{
    public class BankValidation : IBankValidation
    {

        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _accountValidation;

        #endregion

        #region Constructor

        public BankValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _repository = repository;
            _accountValidation = accountValidation;
        }

        #endregion

        #region IBank Validation

        public IEnumerable<BankDto> All()
        {
            var banks = _repository.All<Domain.Data.Model.Bank>();
            var listOfBanks = new List<BankDto>();

            foreach (var bank in banks)
            {
                var item = _mapper.Map<Domain.Data.Model.Bank, BankDto>(bank);
                listOfBanks.Add(item);
            }
            return listOfBanks;
        }

        public MethodResult<bool> Add(BankDto bankDto, string username)
        {
           
            int loggedInUserId = _accountValidation.UserByName(username).UserId;

            var bank = _mapper.Map<BankDto, Domain.Data.Model.Bank>(bankDto);
            bank.CreatedById = loggedInUserId;
            bank.LastChangedById = loggedInUserId;
            bank.IsNotDeleted = true;
            bank.LookUpKey = bank.Name.ToUpper();

            return _repository.Add(bank) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Bank Not Added");
        }

        public MethodResult<bool> Edit(BankDto bankDto, string username)
        {
            int userId = _accountValidation.UserByName(username).UserId;
            var tempBank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == bankDto.BankId).FirstOrDefault();

            Domain.Data.Model.Bank mappedBank = _mapper.Map<BankDto, Domain.Data.Model.Bank>(bankDto);
            mappedBank.EntityState = State.Modified;
            mappedBank.LastChangedById = userId;
            mappedBank.LastChangedDate = DateTime.Now;
            mappedBank.IsNotDeleted = true;
            mappedBank.LookUpKey = bankDto.Name.ToUpper();

            if (tempBank != null)
            {
                mappedBank.CreateDate = tempBank.CreateDate;
                mappedBank.CreatedById = tempBank.CreatedById;
            }
            
            return _repository.Update(mappedBank) > 0 ?
                new MethodResult<bool>(MethodStatus.Successful, true) :
                new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");
        }

        public MethodResult<bool> Delete(int id, string username)
        {
            var userId = _accountValidation.UserByName(username).UserId;

            var result = IsInUse(id);
            if (result.Status == MethodStatus.Successful)
            {
                var _result = _repository.Delete<Domain.Data.Model.Bank>(id, userId) > 0;
                return _result ? new MethodResult<bool>(MethodStatus.Successful, true) :
                    new MethodResult<bool>(MethodStatus.Error, false, "Bank Not Found");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, result.Message);
        }

        public MethodResult<BankDto> Find(int id)
        {
            var bank = _repository.Find<Domain.Data.Model.Bank>(id);
            var mappedBank = _mapper.Map<Domain.Data.Model.Bank, BankDto>(bank);

            return new MethodResult<BankDto>(MethodStatus.Successful, mappedBank);
        }

        public bool IsActive(int bankId)
        {
            var banks = _repository.Query<Domain.Data.Model.Bank>(a => a.IsNotDeleted)
                .Where(e => e.BankId == bankId);

            return banks.Any();
        }

        #endregion

        #region Helper

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<Domain.Data.Model.Account>(a => a.BankId == id && a.IsNotDeleted) ?
                  new MethodResult<bool>(MethodStatus.Error, true, "Cannot delete a bank that is linked to and active account.") :
              new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool IsBankNameInUse(string name)
        {
            return _repository.Any<Domain.Data.Model.Bank>(a => a.Name.ToLower() == name.ToLower());
        }

        public bool NameUsedByAnotherBank(string name, int id)
        {
            return _repository.Any<Domain.Data.Model.Bank>(a => a.Name.ToLower() == name.ToLower() && a.BankId != id);
        }

        public bool CodeUsedByAnotherBank(string code, int id)
        {
            return _repository.Any<Domain.Data.Model.Bank>(a => a.BranchCode.ToLower() == code.ToLower() && a.BankId != id);
        }

        public bool IsBranchCodeInUse(string code)
        {
            return _repository.Any<Domain.Data.Model.Bank>(a => a.BranchCode.ToLower() == code.ToLower());
        }


        #endregion

    }
}