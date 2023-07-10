using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Application.Dto.Account;
using Application.Dto.Bank;
using Application.Dto.Task;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Serializer;
using Utility.Core;
using State = Domain.Data.Core.State;

namespace Application.Modules.Maintanance.BankAccount
{
    public class BankAccountValidation : IBankAccountValidation
    {
        #region Fields

        private readonly IUserAccountValidation _accountValidation;
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly ISerializer _serializer;

        #endregion

        #region Constructor

        public BankAccountValidation(IMapper mapper, ILookup iLookup, IUserAccountValidation accountValidation, ISerializer serializer, IRepository repository)
        {
            _mapper = mapper;
            _lookup = iLookup;
            _repository = repository;
            _accountValidation = accountValidation;
            _serializer = serializer;
        }

        #endregion

        #region IAccountValidation
        
        /// <summary>
        /// Add new Bank Account / Accounts
        /// </summary>
        /// <param name="accountHolder"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<bool> Add(BankAccountHolderDto accountHolder, string username)
        {            
            //Get the logged on user id 
            int userId = _accountValidation.UserByName(username).UserId;
            var site = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == accountHolder.SiteId, a => a.Accounts).FirstOrDefault();

            if (site != null)
            {
                foreach (var account in accountHolder.Accounts.Select(accountDto => _mapper.Map<AccountDto, Account>(accountDto)))
                {
                    var bank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == account.BankId).FirstOrDefault();

                    account.EntityState = State.Added;
                    account.CreatedById = userId;
                    account.CreateDate = DateTime.Now;
                    account.LastChangedById = userId;
                    account.LastChangedDate = DateTime.Now;
                    account.IsNotDeleted = true;
                    account.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);
                    account.IsApproved = false;
                    account.StatusId = _lookup.GetStatusId("SAVED"); //SAVED
                    site.Accounts.Add(account);
                }

                return _repository.Update(site) > 0
                    ? new MethodResult<bool>(MethodStatus.Successful, true)
                    : new MethodResult<bool>(MethodStatus.Error, false, "Account Not Added");
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Account Not Added. Site Not Found");
        }
        
        /// <summary>
        /// Checkl if a Default Account already exist
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool DefaultAccountAlreadyExist(int siteId)
        {
            return _repository.Any<Account>(a => a.SiteId == siteId && a.DefaultAccount);
        }
        
        /// <summary>
        /// Get transaction Type ID by LookUpKey
        /// </summary>
        /// <param name="bankKey"></param>
        /// <returns></returns>
        private int GetTransactionTypeId(string bankKey)
        {
            //============================================
            // BANK TRANSACTION TYPES
            // FNB - FNB01
            // ABSA - ABS01
            // STANDARD BANK - SBS01
            //============================================

            var transactionTypes = _lookup.GetTransactionTypes();
            
            if(transactionTypes.Count <= 0)
                throw new ArgumentNullException("No Transaction Types were retrieved from the database");

            switch (bankKey)
            {
                case "FNB":
                    // SET FNB Transaction Type
                    return transactionTypes.FirstOrDefault(a => a.LookUpKey == "FNB_HOST").TransactionTypeId; 
                case "ABSA":
                    // SET ABSA Transaction Type
                    return transactionTypes.FirstOrDefault(a => a.LookUpKey == "ABSA_HOST").TransactionTypeId;
                case "SBSA":                
                    // SET SBSA Transaction Type
                    return transactionTypes.FirstOrDefault(a => a.LookUpKey == "SBSA_HOST").TransactionTypeId;
                case "NEDBANK":
                    // SET NEDBANK Transaction Type
                    return transactionTypes.FirstOrDefault(a => a.LookUpKey == "NED_HOST").TransactionTypeId;
                case "AFRICAN BANK":
                    // SET NEDBANK CROSS BANKING Transaction Type
                    return transactionTypes.FirstOrDefault(a => a.LookUpKey == "NED_CROSS_BANK").TransactionTypeId;
                default :
                    //============================================
                    // FOR ALL THE OTHER BANKS, USE THE CONGIFURED PARENT
                    // HOST BANK
                    //============================================
                    return transactionTypes.FirstOrDefault(a => a.LookUpKey == "PARENT_HOST").TransactionTypeId;
            }
        }

        /// <summary>
        /// Add New Bank Account and Submit for Approval
        /// </summary>
        /// <param name="accountHolder"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<bool> AddOnContinue(BankAccountHolderDto accountHolder, string username)
        {
            //Get the logged on user id 
            int userId = _accountValidation.UserByName(username).UserId;
            var site = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == accountHolder.SiteId, a => a.Accounts).FirstOrDefault();
            
            if (site != null)
            {
                foreach (var account in accountHolder.Accounts.Select(accountDto => _mapper.Map<AccountDto, Account>(accountDto)))
                {
                    var bank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == account.BankId).FirstOrDefault();

                    account.EntityState = State.Added;
                    account.CreatedById = userId;
                    account.LastChangedById = userId;
                    account.IsNotDeleted = true;
                    if (bank != null) account.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);
                    account.IsApproved = false;
                    account.StatusId = _lookup.GetStatusId("PENDING"); //SAVED
                    account.Comments = "Addition Approval";

                    if (_repository.Add(account) > 0)
                    {
                        var currentAccount = _repository.Query<Account>(a => a.BeneficiaryCode == account.BeneficiaryCode).FirstOrDefault();
                        if (currentAccount != null) account.AccountId = currentAccount.AccountId;

                        // Serialize the object in to xml
                        string newObject = _serializer.Serialize(account);

                        // Save the serialized object and save it
                        int key = _repository.Add(new ApprovalObjects
                        {
                            NewObject = newObject,
                            IsNotDeleted = true,
                            CreatedById = userId,
                            LastChangedById = userId,
                            CreateDate = DateTime.Now,
                            LastChangedDate = DateTime.Now
                        });

                        string serverAddress = _lookup.GetServerAddress();

                        // Create a new task
                        string refNumber = GenerateTaskRefNumber();
                        string url = serverAddress + "/BankAccount/Approve/" + "?id=" + key;

                        var task = new Task
                        {
                            Title = "Bank Account Add",
                            ReferenceNumber = refNumber,
                            Date = DateTime.Now,
                            MerchantId = site.MerchantId,
                            ApprovalObjectsId = key,
                            SiteId = site.SiteId,
                            AccountId = account.AccountId,
                            UserId = userId,
                            Module = "Bank Account",
                            StatusId = _lookup.GetStatusId("PENDING"),
                            Link = url,
                            IsExecuted = false,
                            CreatedById = userId,
                            LastChangedById = userId,
                            CreateDate = DateTime.Now,
                            LastChangedDate = DateTime.Now,
                            IsNotDeleted = true
                        };
                        _repository.Add(task);
                    }
                }
                return new MethodResult<bool>(MethodStatus.Successful, true);
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Account Not Added. Site Not Found");
        }

        /// <summary>
        /// Unmark all Old Default Accounts
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="userId"></param>
        /// <param name="accountId"></param>
        private void UncheckOldDefauls(int siteId, int userId, int accountId)
        {
            var bankAccounts = _repository.Query<Account>(a => a.DefaultAccount && a.SiteId == siteId);

            foreach (var bankAccount in bankAccounts)
            {
                if (bankAccount.AccountId != accountId)
                {
                    var bank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == bankAccount.BankId).FirstOrDefault();

                    bankAccount.EntityState = State.Modified;
                    bankAccount.DefaultAccount = false;
                    bankAccount.LastChangedById = userId;
                    bankAccount.CreateDate = bankAccount.CreateDate;
                    bankAccount.CreatedById = bankAccount.CreatedById;
                    bankAccount.IsNotDeleted = true;
                    bankAccount.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);

                    _repository.Update(bankAccount);
                }
            }

        }
        
        /// <summary>
        /// Edit a Bank Account
        /// </summary>
        /// <param name="accountDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<bool> Edit(AccountDto accountDto, string username)
        {
            //Get the logged on user id 
            int userId = _accountValidation.UserByName(username).UserId;
            
            Account temp = _repository.Query<Account>(a => a.AccountId == accountDto.AccountId,
                                    a => a.Site.Merchant,
                                    a => a.Site.Accounts
                                    ).FirstOrDefault(a => a.AccountId == accountDto.AccountId);
            
            var bank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == accountDto.BankId).FirstOrDefault();

            if (temp != null && temp.StatusId == _lookup.GetStatusId("ACTIVE"))
            {
                Account mappedAccount = _mapper.Map<AccountDto, Account>(accountDto);
                mappedAccount.EntityState = State.Modified;
                mappedAccount.LastChangedById = userId;
                mappedAccount.LastChangedDate = DateTime.Now;
                mappedAccount.CreateDate = temp.CreateDate;
                mappedAccount.CreatedById = temp.CreatedById;
                mappedAccount.IsNotDeleted = true;
                if (bank != null) mappedAccount.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);
                mappedAccount.IsApproved = false;
                mappedAccount.StatusId = _lookup.GetStatusId("PENDING");
                mappedAccount.Comments = "Updating Approval";

                if (_repository.Update(mappedAccount) > 0)
                {
                    // Serialize the object in to xml
                    string newObject = _serializer.Serialize(mappedAccount);

                    // Save the serialized object and save it
                    int key = _repository.Add(new ApprovalObjects
                    {
                        NewObject = newObject,
                        IsNotDeleted = true,
                        CreatedById = userId,
                        LastChangedById = userId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now
                    });

                    string serverAddress = _lookup.GetServerAddress();

                    // Create a new task
                    string refNumber = GenerateTaskRefNumber();
                    string url = serverAddress + "/BankAccount/Approve/" + "?id=" + key;

                    var task = new Task
                    {
                        Title = "Bank Account Update",
                        ReferenceNumber = refNumber,
                        Date = DateTime.Now,
                        MerchantId = temp.Site.MerchantId,
                        ApprovalObjectsId = key,
                        SiteId = temp.Site.SiteId,
                        AccountId = mappedAccount.AccountId,
                        UserId = userId,
                        Module = "Bank Account",
                        StatusId = _lookup.GetStatusId("PENDING"),
                        Link = url,
                        IsExecuted = false,
                        CreatedById = userId,
                        LastChangedById = userId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now,
                        IsNotDeleted = true
                    };
                    
                    _repository.Add(task);

                    return new MethodResult<bool>(MethodStatus.Successful, true,
                        "Bank Account was successfully updated");
                }
            }

             //This will happen when the user approves a bank account that was flagged for deletion
            else if (temp != null && (temp.StatusId == _lookup.GetStatusId("PENDING") && temp.ToBeDeleted))
            {
                if (_repository.Delete<Account>(temp.AccountId, userId) > 0)
                {
                    Task tempTask = _repository.Query<Task>(a => a.AccountId == temp.AccountId
                                 ).FirstOrDefault(a => a.AccountId == temp.AccountId);

                    if (tempTask != null)
                    {
                        var task = new TaskDto
                        {
                            TaskId = tempTask.TaskId,
                            Title = "Bank Account Approve",
                            ReferenceNumber = tempTask.ReferenceNumber,
                            Date = DateTime.Now,
                            ApprovalObjectsId = tempTask.ApprovalObjectsId,
                            MerchantId = temp.Site.MerchantId,
                            SiteId = temp.Site.SiteId,
                            AccountId = temp.AccountId,
                            UserId = userId,
                            StatusId = _lookup.GetStatusId("INACTIVE"),
                            Module = "Bank Account",
                            Link = tempTask.Link,
                            IsExecuted = false
                        };

                        Task mappedTask = _mapper.Map<TaskDto, Task>(task);
                        mappedTask.EntityState = State.Modified;
                        mappedTask.LastChangedDate = DateTime.Now;
                        mappedTask.LastChangedById = userId;
                        mappedTask.CreateDate = tempTask.CreateDate;
                        mappedTask.CreatedById = tempTask.CreatedById;
                        mappedTask.IsNotDeleted = false;
                        mappedTask.StatusId = _lookup.GetStatusId("INACTIVE");

                        _repository.Update(mappedTask);
                    }

                    return new MethodResult<bool>(MethodStatus.Successful, true);
                }
            }

            else if (temp != null && (temp.StatusId == _lookup.GetStatusId("PENDING") || temp.StatusId == _lookup.GetStatusId("DECLINED")))
            {
                Account mappedAccount = _mapper.Map<AccountDto, Account>(accountDto);
                mappedAccount.EntityState = State.Modified;
                mappedAccount.LastChangedById = userId;
                mappedAccount.CreateDate = temp.CreateDate;
                mappedAccount.CreatedById = temp.CreatedById;
                mappedAccount.IsNotDeleted = true;
                mappedAccount.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);
                
                mappedAccount.Comments = accountDto.PreviousComments + Environment.NewLine + "Approved";

                Task tempTask = _repository.Query<Task>(a => a.AccountId == accountDto.AccountId
                                   ).FirstOrDefault(a => a.AccountId == accountDto.AccountId);

                if (tempTask != null)
                {
                    var task = new TaskDto
                    {
                        TaskId = tempTask.TaskId,
                        Title = "Bank Account Approve",
                        ReferenceNumber = tempTask.ReferenceNumber,
                        Date = DateTime.Now,
                        MerchantId = temp.Site.MerchantId,
                        SiteId = temp.Site.SiteId,
                        AccountId = temp.AccountId,
                        UserId = userId,
                        StatusId = _lookup.GetStatusId("INACTIVE"),
                        Module = "Bank Account",
                        Link = tempTask.Link,
                        IsExecuted = false
                    };

                    Task mappedTask = _mapper.Map<TaskDto, Task>(task);
                    mappedTask.EntityState = State.Modified;
                    mappedTask.LastChangedById = userId;
                    mappedTask.CreateDate = temp.CreateDate;
                    mappedTask.CreatedById = temp.CreatedById;

                    if (temp.StatusId == _lookup.GetStatusId("PENDING"))
                    {
                        if (accountDto.DefaultAccount)
                        {
                            UncheckOldDefauls(accountDto.SiteId, userId, accountDto.AccountId);
                        }

                        mappedTask.StatusId = _lookup.GetStatusId("INACTIVE");
                        mappedAccount.StatusId = _lookup.GetStatusId("ACTIVE");
                        mappedTask.IsNotDeleted = false;
                        mappedAccount.IsApproved = true;
                    }
                    else
                    {
                        mappedTask.StatusId = _lookup.GetStatusId("PENDING");
                        mappedAccount.StatusId = _lookup.GetStatusId("PENDING");
                        mappedTask.IsNotDeleted = true;
                        mappedAccount.IsApproved = false;
                    }

                    return _repository.Update(mappedAccount) > 0 && _repository.Update(mappedTask) > 0
                        ? new MethodResult<bool>(MethodStatus.Successful, true)
                        : new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");
                }
            }
            else
            {
                if (accountDto.DefaultAccount)
                {
                    UncheckOldDefauls(accountDto.SiteId, userId, accountDto.AccountId);
                }

                Account mappedAccount = _mapper.Map<AccountDto, Account>(accountDto);
                mappedAccount.EntityState = State.Modified;
                mappedAccount.LastChangedById = userId;
                if (temp != null)
                {
                    mappedAccount.CreateDate = temp.CreateDate;
                    mappedAccount.CreatedById = temp.CreatedById;
                }
                mappedAccount.IsNotDeleted = true;
                if (bank != null) mappedAccount.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);
                mappedAccount.StatusId = _lookup.GetStatusId("SAVED");

                return _repository.Update(mappedAccount) > 0
                    ? new MethodResult<bool>(MethodStatus.Successful, true)
                    : new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");
            }

            return new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");
        }

        /// <summary>
        /// Submit Bank Account for Approval
        /// </summary>
        /// <param name="accountDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<bool> Submit(AccountDto accountDto, string username)
        {
            //Get the logged on user id 
            int userId = _accountValidation.UserByName(username).UserId;

            //Bank Account Status Before Updates

            Account temp = _repository.Query<Account>(a => a.AccountId == accountDto.AccountId,
                        a => a.Site.Merchant,
                        a => a.Site.Accounts
                        ).FirstOrDefault(a => a.AccountId == accountDto.AccountId);
            var bank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == accountDto.BankId).FirstOrDefault();
            
            if (temp != null)
            {
                var actualAccountStatus = temp.StatusId;

                //OBJECT TO BE APPROVED 
                Account mappedAccount = _mapper.Map<AccountDto, Account>(accountDto);
                mappedAccount.EntityState = State.Modified;
                mappedAccount.LastChangedById = userId;
                mappedAccount.IsNotDeleted = true;
                if (bank != null) mappedAccount.TransactionTypeId = GetTransactionTypeId(bank.LookUpKey);
                mappedAccount.CreateDate = temp.CreateDate;
                mappedAccount.CreatedById = temp.CreatedById;
                mappedAccount.StatusId = _lookup.GetStatusId("PENDING");
                string comment = "";
            
                switch (accountDto.StatusId)
                {
                    case 1:     //Active Status
                        comment = string.IsNullOrEmpty(temp.Comments) ? "Updating Approval" : accountDto.PreviousComments + Environment.NewLine + "Updating Approval";
                        mappedAccount.Comments = comment;
                        break;
                    case 7:     //Declined Status
                        comment = string.IsNullOrEmpty(temp.Comments) ? "Updating Approval" : accountDto.PreviousComments + Environment.NewLine + "Updating Approval";
                        mappedAccount.Comments = comment;
                        break;

                    default:
                        mappedAccount.Comments = "Addition Approval";
                        break;
                }
            
                //KEEP OLD VALUES BUT UPDATE MINOR FIELDS
                temp.StatusId = _lookup.GetStatusId("PENDING");
                temp.Comments = comment;
                temp.EntityState = State.Modified;

                if (_repository.Update(temp) > 0)
                {
                    // Serialize the object in to xml
                    string newObject = _serializer.Serialize(mappedAccount);

                    // Save the serialized object and save it
                    int key = _repository.Add(new ApprovalObjects
                    {
                        NewObject = newObject,
                        IsNotDeleted = true,
                        CreatedById = userId,
                        LastChangedById = userId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now
                    });

                    string serverAddress = _lookup.GetServerAddress();

                    // Create a new task
                    string refNumber = GenerateTaskRefNumber();
                    string url = serverAddress + "/BankAccount/Approve/" + "?id=" + key;


                    if (actualAccountStatus == _lookup.GetStatusId("DECLINED"))
                    {
                        Task tempTask = _repository.Query<Task>(a => a.AccountId == accountDto.AccountId
                            ).FirstOrDefault(a => a.AccountId == accountDto.AccountId);

                        if (tempTask != null)
                        {
                            var task = new TaskDto
                            {
                                TaskId = tempTask.TaskId,
                                Title = "Bank Account Approve",
                                ReferenceNumber = tempTask.ReferenceNumber,
                                Date = DateTime.Now,
                                MerchantId = temp.Site.MerchantId,
                                ApprovalObjectsId = key,
                                SiteId = temp.Site.SiteId,
                                AccountId = temp.AccountId,
                                UserId = userId,
                                StatusId = _lookup.GetStatusId("PENDING"),
                                Module = "Bank Account",
                                Link = tempTask.Link,
                                IsExecuted = false
                            };

                            Task mappedTask = _mapper.Map<TaskDto, Task>(task);
                            mappedTask.EntityState = State.Modified;
                            mappedTask.LastChangedById = userId;
                            mappedTask.CreateDate = temp.CreateDate;
                            mappedTask.CreatedById = temp.CreatedById;
                            mappedTask.IsNotDeleted = true;
                            mappedTask.StatusId = _lookup.GetStatusId("PENDING");

                            _repository.Update(mappedTask);
                        }

                    }
                    else
                    {
                        var task = new Task
                        {
                            Title = "Bank Account Update",
                            ReferenceNumber = refNumber,
                            Date = DateTime.Now,
                            MerchantId = temp.Site.MerchantId,
                            ApprovalObjectsId = key,
                            SiteId = temp.Site.SiteId,
                            AccountId = temp.AccountId,
                            UserId = userId,
                            StatusId = _lookup.GetStatusId("PENDING"),
                            Module = "Bank Account",
                            Link = url,
                            IsExecuted = false,
                            CreatedById = userId,
                            LastChangedById = userId,
                            CreateDate = DateTime.Now,
                            LastChangedDate = DateTime.Now,
                            IsNotDeleted = true,
                        };
                        _repository.Add(task);
                    }

                    return new MethodResult<bool>(MethodStatus.Successful, true,
                        "Bank Account was successfully Submitted. An email will be sent for Approval.");
                }
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Bank Account Not Submitted");
        }

        /// <summary>
        /// Delete Bank Account
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<bool> Delete(int id, string username)
        {
            //Get the logged on user id 
            int userId = _accountValidation.UserByName(username).UserId;

            //Check if the Bank Account is in use
            MethodResult<bool> results = IsInUse(id);
            if (results.Status != MethodStatus.Successful)
            {
                return new MethodResult<bool>(MethodStatus.Error, true, results.Message);
            }

            Account bankAccount = _repository.Query<Account>(a => a.AccountId == id,
                         a => a.Site.Merchant,
                         a => a.Site.Accounts
                         ).FirstOrDefault(a => a.AccountId == id);


            if (bankAccount != null && (bankAccount.StatusId == _lookup.GetStatusId("SAVED") ||
                                        bankAccount.StatusId == _lookup.GetStatusId("DECLINED")))
            {
                if (_repository.Delete<Account>(id, userId) > 0)
                {
                    Task tempTask = _repository.Query<Task>(a => a.AccountId == bankAccount.AccountId
                                 ).FirstOrDefault(a => a.AccountId == bankAccount.AccountId);
                    
                    if (tempTask != null)
                    {
                        var task = new TaskDto
                        {
                            TaskId = tempTask.TaskId,
                            Title = "Bank Account Approve",
                            ReferenceNumber = tempTask.ReferenceNumber,
                            Date = DateTime.Now,
                            MerchantId = bankAccount.Site.MerchantId,
                            SiteId = bankAccount.Site.SiteId,
                            AccountId = bankAccount.AccountId,
                            UserId = userId,
                            StatusId = _lookup.GetStatusId("INACTIVE"),
                            Module = "Bank Account",
                            Link = tempTask.Link,
                            IsExecuted = false
                        };

                        Task mappedTask = _mapper.Map<TaskDto, Task>(task);
                        mappedTask.EntityState = State.Modified;
                        mappedTask.LastChangedDate = DateTime.Now;
                        mappedTask.LastChangedById = userId;
                        mappedTask.CreateDate = bankAccount.CreateDate;
                        mappedTask.CreatedById = bankAccount.CreatedById;
                        mappedTask.IsNotDeleted = false;
                        mappedTask.StatusId = _lookup.GetStatusId("INACTIVE");

                        _repository.Update(mappedTask);
                    }

                    return new MethodResult<bool>(MethodStatus.Successful, true, "Bank Account Deleted Successful");
                }
            }
            else if (bankAccount != null && bankAccount.StatusId == _lookup.GetStatusId("PENDING"))
            {
                return new MethodResult<bool>(MethodStatus.Error, false, "Bank Account cannot be deleted because it is pending approval.");
            }
            else if (bankAccount != null && bankAccount.StatusId == _lookup.GetStatusId("ACTIVE"))
            {
                var accountDto = new AccountDto
                {
                    AccountId = bankAccount.AccountId,
                    SiteId = bankAccount.SiteId,
                    AccountTypeId = bankAccount.AccountTypeId,
                    BankId = bankAccount.BankId,
                    TransactionTypeId = bankAccount.TransactionTypeId,
                    AccountNumber = bankAccount.AccountNumber,
                    AccountHolderName = bankAccount.AccountHolderName,
                    DefaultAccount = bankAccount.DefaultAccount,
                    BeneficiaryCode = bankAccount.BeneficiaryCode
                    
                };

                Account mappedAccount = _mapper.Map<AccountDto, Account>(accountDto);
                mappedAccount.EntityState = State.Modified;
                mappedAccount.LastChangedById = userId;
                mappedAccount.IsNotDeleted = true;
                mappedAccount.ToBeDeleted = true;
                mappedAccount.CreateDate = bankAccount.CreateDate;
                mappedAccount.CreatedById = bankAccount.CreatedById;
                mappedAccount.StatusId = _lookup.GetStatusId("PENDING");
                mappedAccount.Comments = bankAccount.Comments + Environment.NewLine +
                                         "Deletion Approval";

                if (_repository.Update(mappedAccount) > 0)
                {
                    // Serialize the object in to xml
                    string newObject = _serializer.Serialize(mappedAccount);

                    // Save the serialized object and save it
                    int key = _repository.Add(new ApprovalObjects
                    {
                        NewObject = newObject,
                        IsNotDeleted = true,
                        CreatedById = userId,
                        LastChangedById = userId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now
                    });

                    string serverAddress = _lookup.GetServerAddress();

                    // Create a new task
                    string refNumber = GenerateTaskRefNumber();
                    string url = serverAddress + "/BankAccount/Approve/" + "?id=" + key;
                    
                    var task = new Task
                    {
                        Title = "Bank Account Delete",
                        ReferenceNumber = refNumber,
                        Date = DateTime.Now,
                        ApprovalObjectsId = key,
                        MerchantId = bankAccount.Site.MerchantId,
                        SiteId = bankAccount.Site.SiteId,
                        AccountId = mappedAccount.AccountId,
                        UserId = userId,
                        StatusId = _lookup.GetStatusId("PENDING"),
                        Module = "Bank Account",
                        Link = url,
                        IsExecuted = false,
                        CreatedById = userId,
                        LastChangedById = userId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now,
                        IsNotDeleted = true
                    };

                    _repository.Add(task);
                    return new MethodResult<bool>(MethodStatus.Successful, true, "Bank Account was successfully marked for deletion. A notification email was sent to the approver for approval.");
                }
                return new MethodResult<bool>(MethodStatus.Error, false, "Account Not Updated");

            }

            return new MethodResult<bool>(MethodStatus.Error, false, results.Message);
        }

        /// <summary>
        /// Find Specifit Bank Account Information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MethodResult<AccountDto> Find(int id)
        {
          Account account = _repository.Query<Account>(a => a.AccountId == id,
                                                a => a.Site.Merchant,
                                                a => a.Site.Address,
                                                a => a.Site.Accounts,
                                                a => a.Site.Accounts.Select(b => b.Bank),
                                                a => a.Site.Accounts.Select(b => b.AccountType),
                                                a => a.Site.Accounts.Select(b => b.TransactionType),
                                                a => a.Site.Accounts.Select(b => b.Status),
                                                u => u.Site.SiteContainers,
                                                o => o.Site.SiteContainers.Select(a => a.ContainerType),
                                                cta => cta.Site.SiteContainers.Select(a => a.ContainerType.ContainerTypeAttributes)
                                                ).FirstOrDefault(a => a.AccountId == id);

            if (account == null) return new MethodResult<AccountDto>(MethodStatus.Error, null, "Bank Account Not Found.");

            AccountDto mappedAccount = _mapper.Map<Account, AccountDto>(account);
            mappedAccount.BranchCode = account.Bank.BranchCode;
            mappedAccount.CitCode = account.Site.CitCode;

            return new MethodResult<AccountDto>(MethodStatus.Successful, mappedAccount);
        }
        
        /// <summary>
        /// Find Bank Account information on Approval
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MethodResult<AccountDto> ApprovalFind(int id)
        {
            var approvalObject = _repository.Query<ApprovalObjects>(a => a.ApprovalObjectsId == id).FirstOrDefault();

            if (approvalObject != null)
            {
                string byteOrderMarkUtf8 = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
                if (approvalObject.NewObject.StartsWith(byteOrderMarkUtf8))
                {
                    var doc = approvalObject.NewObject.Remove(0, byteOrderMarkUtf8.Length);

                    XDocument xml2 = XDocument.Parse(doc);
                    var account = xml2.Element("Account");
                    if (account != null)
                    {
                        var accountDto = new AccountDto
                        {
                            AccountId = Convert.ToInt32(account.Element("AccountId").Value),
                            SiteId = Convert.ToInt32(account.Element("SiteId").Value),
                            BankId = Convert.ToInt32(account.Element("BankId").Value),
                            StatusId = Convert.ToInt32(account.Element("StatusId").Value),
                            TransactionTypeId = Convert.ToInt32(account.Element("TransactionTypeId").Value),
                            AccountTypeId = Convert.ToInt32(account.Element("AccountTypeId").Value),
                            AccountHolderName = account.Element("AccountHolderName").Value,
                            AccountNumber = account.Element("AccountNumber").Value,
                            DefaultAccount = Convert.ToBoolean(account.Element("DefaultAccount").Value),
                            ToBeDeleted = Convert.ToBoolean(account.Element("ToBeDeleted").Value),
                            BeneficiaryCode = account.Element("BeneficiaryCode").Value,
                            PreviousComments = account.Element("Comments").Value,
                        };

                        var site = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == accountDto.SiteId).FirstOrDefault();
                        var bank = _repository.Query<Domain.Data.Model.Bank>(a => a.BankId == accountDto.BankId).FirstOrDefault();

                        if (bank != null) accountDto.BranchCode = bank.BranchCode;
                        if (site != null) accountDto.CitCode = site.CitCode;

                        return new MethodResult<AccountDto>(MethodStatus.Successful, accountDto);
                    }

                    return new MethodResult<AccountDto>(MethodStatus.Error, null, "Bank Account Not Found");
                }
                return new MethodResult<AccountDto>(MethodStatus.Error, null, "Bank Account Not Found");
            }
            return new MethodResult<AccountDto>(MethodStatus.Error, null, "Bank Account Not Found");
        }
        
        /// <summary>
        /// Find AllActive Bank Accounts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ListAccountDto> All()
        {
            IEnumerable<Account> accounts = _repository.All<Account>(b => b.Site, c => c.Bank, s => s.Status);
            List<ListAccountDto> list = accounts.Select(site => _mapper.Map<Account, ListAccountDto>(site)).ToList();

            return list;
        }
        
        /// <summary>
        /// Check if Bank Account Is Rejected from the Bank Account Approval Module
        /// </summary>
        /// <param name="rejectParameters"></param>
        /// <param name="bankAccountDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool IsBankAccountRejected(RejectAccountArgumentsDto rejectParameters, AccountDto bankAccountDto, string username)
        {
            int userId = _accountValidation.UserByName(username).UserId;

            Account temp = _repository.Query<Account>(a => a.AccountId == rejectParameters.Id,
                a => a.Site.Merchant,
                a => a.Site.Accounts
                ).FirstOrDefault(a => a.AccountId == rejectParameters.Id);

            Account mappedBankAccount = _mapper.Map<AccountDto, Account>(bankAccountDto);
            

            mappedBankAccount.StatusId = _lookup.GetStatusId("DECLINED");
            mappedBankAccount.IsApproved = false;
            mappedBankAccount.IsNotDeleted = true;
            mappedBankAccount.ToBeDeleted = false;
            mappedBankAccount.LastChangedById = userId;
            mappedBankAccount.LastChangedDate = DateTime.Now;
            if (temp != null)
            {
                mappedBankAccount.CreateDate = temp.CreateDate;
                mappedBankAccount.CreatedById = temp.CreatedById;
            }
            mappedBankAccount.EntityState = State.Modified;
            mappedBankAccount.Comments = rejectParameters.PreviousComments + Environment.NewLine +
                                         "Declined: " + rejectParameters.CurrentComments;

            // Serialize the object in to xml
            string newObject = _serializer.Serialize(mappedBankAccount);

            mappedBankAccount.Site = _repository.Query<Domain.Data.Model.Site>(a => a.SiteId == bankAccountDto.SiteId).FirstOrDefault();
            if (_repository.Update(mappedBankAccount) > 0)
            {
                Task tempTask = _repository.Query<Task>(a => a.AccountId == mappedBankAccount.AccountId
                                   ).FirstOrDefault(a => a.AccountId == mappedBankAccount.AccountId);

                if (tempTask != null)
                {
                    if (mappedBankAccount.Site != null)
                    {
                        // Save the serialized object and save it
                        int key = _repository.Add(new ApprovalObjects
                        {
                            NewObject = newObject,
                            IsNotDeleted = true,
                            CreatedById = userId,
                            LastChangedById = userId,
                            CreateDate = DateTime.Now,
                            LastChangedDate = DateTime.Now
                        });

                        var task = new TaskDto
                        {
                            TaskId = tempTask.TaskId,
                            Title = "Bank Account Approve",
                            ReferenceNumber = tempTask.ReferenceNumber,
                            Date = DateTime.Now,
                            ApprovalObjectsId = key,
                            MerchantId = mappedBankAccount.Site.MerchantId,
                            SiteId = mappedBankAccount.SiteId,
                            AccountId = mappedBankAccount.AccountId,
                            UserId = userId,
                            StatusId = _lookup.GetStatusId("DECLINED"),
                            Module = "Bank Account",
                            Link = tempTask.Link,
                            IsExecuted = false
                        };

                        Task mappedTask = _mapper.Map<TaskDto, Task>(task);
                        mappedTask.EntityState = State.Modified;
                        mappedTask.LastChangedById = userId;
                        mappedTask.LastChangedDate = DateTime.Now;
                        mappedTask.CreateDate = mappedBankAccount.CreateDate;
                        mappedTask.CreatedById = mappedBankAccount.CreatedById;
                        mappedTask.IsNotDeleted = true;
                        mappedTask.StatusId = _lookup.GetStatusId("DECLINED");

                        return _repository.Update(mappedTask) > 0;
                    }
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// Generate Bank Account Beneficiary Code
        /// </summary>
        /// <returns></returns>
        public string GenerateBeneficiaryCode()
        {
            string beneficiaryCode = GenerateCode();
            while (_repository.Any<Account>(a => a.BeneficiaryCode == beneficiaryCode))
            {
                beneficiaryCode = GenerateCode();
            }
            return beneficiaryCode;
        }

        /// <summary>
        /// Generate Code
        /// </summary>
        /// <returns></returns>
        private static string GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numerics = "0123456789";

            var stringChars = new char[3];
            var stringNumerics = new char[3];

            var random = new Random();

            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
                stringNumerics[i] = numerics[random.Next(numerics.Length)];
            }

            string finalString = new String(stringChars) + new String(stringNumerics);
            return finalString;
        }
        
        //Get Task Ref Number
        public string GetTaskRef(int accountId)
        {
            var task = _repository.Query<Task>(a => a.AccountId == accountId && a.IsNotDeleted).FirstOrDefault();

            if (task != null)
            {
                return task.ReferenceNumber;
            }

            return "";
        }

        /// <summary>
        /// Get Approval Object
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <returns></returns>
        public int GetApprovalObjectIdByRefNumber(string referenceNumber)
        {
            var task = _repository.Query<Task>(a => a.ReferenceNumber == referenceNumber && a.IsNotDeleted).FirstOrDefault();

            if (task != null && task.ApprovalObjectsId != null)
            {
                return (int) task.ApprovalObjectsId;
            }

            return 0;
        }
        
        /// <summary>
        /// Generate Task Ref Number
        /// </summary>
        /// <returns></returns>
        public string GenerateTaskRefNumber()
        {
            string code = string.Concat("CD", Guid.NewGuid().ToString("N").Substring(0, 10));
            while (_repository.Any<Task>(a => a.ReferenceNumber == code))
            {
                code = string.Concat("CD", Guid.NewGuid().ToString("N").Substring(0, 15));
            }
            return code.ToUpper();
        }
        
        /// <summary>
        /// Get Account By Beneficiary Code
        /// </summary>
        /// <param name="beneficiaryCode"></param>
        /// <returns></returns>
        public int GetAccountByBeneficiaryCode(string beneficiaryCode)
        {
            var user = _repository.Query<Account>(a => a.BeneficiaryCode == beneficiaryCode).FirstOrDefault();
            return user != null ? user.AccountId : 0;
        }

        /// <summary>
        /// Check If The Account is the Defauilt Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool IsAccountDefault(int accountId, int siteId)
        {
            return _repository.Any<Account>(a => a.AccountId == accountId && a.DefaultAccount && a.SiteId == siteId);
        }

        /// <summary>
        /// Check if there is a Default Account
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public bool AreThereDefaultAccount(int accountId, int siteId)
        {
            return _repository.Any<Account>(a => a.AccountId != accountId && a.DefaultAccount && a.SiteId == siteId);
        }

        /// <summary>
        /// Check the Default Account By Status
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="siteId"></param>
        /// <param name="statusId"></param>
        /// <returns></returns>
        public bool DefaultAccountByStatus(int accountId, int siteId, int statusId)
        {
            return _repository.Any<Account>(a => a.AccountId != accountId && a.DefaultAccount && a.SiteId == siteId && a.StatusId == statusId);
        }

        #endregion

        #region Helpers

        public MethodResult<bool> IsInUse(int id)
        {
            return _repository.Any<CashDeposit>(a => a.AccountId == id && a.IsNotDeleted)
                ? new MethodResult<bool>(MethodStatus.Error, true,
                    "Cannot delete a Account that is linked to and active cash deposit.")
                : new MethodResult<bool>(MethodStatus.Successful);
        }

        public bool UserApprovingOwnTask(int userId, int accountId)
        {
            return _repository.All<Task>().Any(o => o.UserId == userId && o.AccountId == accountId && o.StatusId == _lookup.GetStatusId("PENDING"));
        }

        public bool IsBankAccountInApprovalState(int accountId)
        {
            return
                _repository.All<Account>()
                    .Any(a => a.AccountId == accountId && a.StatusId == _lookup.GetStatusId("PENDING") ||
                              a.AccountId == accountId && a.StatusId == _lookup.GetStatusId("DECLINED"));
        }
        
        public bool DuplicateBankAccounts(int siteId, string accountNumber, int bankId)
        {
            return _repository.Any<Account>(a => a.SiteId == siteId && a.AccountNumber == accountNumber && a.BankId == bankId);
        }
        
        public bool AccountInUseByCurrentSite(int accountId, int siteId, string accountNumber)
        {
            return _repository.Any<Account>(a => a.AccountId != accountId && a.SiteId == siteId && a.AccountNumber == accountNumber);
        }

        public List<string> SiteBankAccounts(int siteId)
        {
            var accounts = _repository.Query<Account>(a => a.SiteId == siteId); 
            var accountNumbers = new List<string>();

            foreach (var bank in accounts)
            {
                accountNumbers.Add(bank.AccountNumber);
            }
            return accountNumbers;
        }
       
        #endregion
    }
}
