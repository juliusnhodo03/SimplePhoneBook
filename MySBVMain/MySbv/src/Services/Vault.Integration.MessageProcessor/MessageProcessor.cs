using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Transactions;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.DataContracts.Contracts;
using Vault.Integration.MessageProcessor.Infrastructure;
using Denomination = Domain.Data.Model.Denomination;
using DenominationType = Vault.Integration.MessageProcessor.Infrastructure.DenominationType;
using User = Domain.Data.Model.User;

namespace Vault.Integration.MessageProcessor
{
    /// <summary>
    ///     .
    /// </summary>
    [Export(typeof (IMessageProcessor))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class MessageProcessor : IMessageProcessor
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly IEnumerable<ProductType> _capturingSources;

        /// <summary>
        /// </summary>
        private readonly IEnumerable<ContainerType> _containerTypes;

        /// <summary>
        /// </summary>
        private readonly IEnumerable<Denomination> _denominations;

        /// <summary>
        /// </summary>
        private readonly IEnumerable<DepositType> _depositTypes;

        /// <summary>
        /// </summary>
        private readonly IRepository _repository;

        /// <summary>
        /// </summary>
        private readonly ILookup _lookup;

        private readonly IAsyncRepository _asyncRepository;

        /// <summary>
        /// </summary>
        private readonly IEnumerable<Status> _statuses;

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="lookup"></param>
        [ImportingConstructor]
        public MessageProcessor(IRepository repository, ILookup lookup)
        {
            _repository = repository;
            _lookup = lookup;
            _depositTypes = _repository.All<DepositType>();
            _denominations = _repository.All<Denomination>();
            _capturingSources = _repository.All<ProductType>();
            _statuses = _repository.All<Status>();
            _containerTypes = _repository.All<ContainerType>();
        }

        #endregion

        #region IMessageProcessor Implementation

        /// <summary>
        ///     Submits a deposit
        /// </summary>
        /// <param name="request"></param>
        /// <param name="vaultSource"></param>
        public MethodResult<bool> SubmitRequest(RequestMessage request, VaultSource vaultSource)
        {
            if (IsResending(request))
            {
                // return true to enable the duplicate request to be popped from the queue
                return new MethodResult<bool>(MethodStatus.Successful, true, null, HttpStatusCode.Created);
            }

            User user = _repository.Query<User>(a => a.UserName == "SBVAdmin").FirstOrDefault();
            short code = Convert.ToInt16(request.TransactionType.Code);
            VaultTransactionType transactionType =
                _repository.Query<VaultTransactionType>(a => a.Code == code).FirstOrDefault();
            Device device =
                _repository.Query<Device>(a => a.SerialNumber.Trim().ToLower() == request.DeviceSerial.Trim().ToLower())
                    .FirstOrDefault();
            IEnumerable<CashDeposit> cashDeposits = QueryCashDeposits(device.DeviceId);

            if (transactionType.LookUpKey == XmlTransactionType.DEPOSIT.Name())
            {
                // CashDeposit message

                // Empty Container signifies a new transaction
                Container container = cashDeposits.SelectMany(o => o.Containers)
                    .FirstOrDefault(
                        o => o.SerialNumber == request.CollectionUnits.CollectionUnit.FirstOrDefault().Value);

                if (container != null)
                {
                    MethodResult<CashDeposit> depositResult = AddContainerDrop(request, container, user);
                    return ReturnStatus(depositResult, "Failed to Create New ContainerDrop");
                }
                else
                {
                    MethodResult<CashDeposit> depositResult = CreateCashDeposit(request, user, vaultSource);
                    return ReturnStatus(depositResult, "Failed to Create New Container in new Deposit");
                }
            }

            if (transactionType.LookUpKey == XmlTransactionType.CIT.Name())
            {
                // Cit message
                MethodResult<CashDeposit> citResult = CloseBag(cashDeposits, request, user); 
                return ReturnStatus(citResult, "Failed to Run Cit Operation");
            }

            if (transactionType.LookUpKey == XmlTransactionType.PAYMENT.Name())
            {
                // Payment message
                MethodResult<CashDeposit> paymentResult = PayBeneficiary(cashDeposits, request, user);
                return ReturnStatus(paymentResult, "Failed to Pay Beneficiary");
            }
            return new MethodResult<bool>(MethodStatus.Error, false,
                "Wrong operation on Request: Operations -> 'Deposit', 'Payment', 'CIT'",
                HttpStatusCode.ExpectationFailed);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="bagSerialNumber"></param>
        private bool HasVaultAmountLeftOnDevice(string bagSerialNumber)
        {
            var container =
                _repository.Query<Container>(e => e.SerialNumber == bagSerialNumber, o => o.CashDeposit)
                    .FirstOrDefault();

            if (container != null)
            {
                return container.CashDeposit.VaultAmount > 0;
            }

            return false;
        }

        
        /// <summary>
        /// Saves the Cit Request details
        /// </summary>
        /// <param name="request"></param>
        public void SaveCitRequest(RequestMessage request)
        {
            try
            {
                var cashDepositId = GetCashDepositId(request);
                var date = DateTime.Now;
                var user = _repository.Query<User>(a => a.UserName == "SBVAdmin").FirstOrDefault();

                var bagSerialNumber = request.CollectionUnits.CollectionUnit.FirstOrDefault().Value;

                if (HasVaultAmountLeftOnDevice(bagSerialNumber))
                {
                    var citRequestDetail = new CitRequestDetail
                    {
                        CashDepositId = cashDepositId,
                        BeneficiaryCode = request.BeneficiaryCode,
                        CitCode = request.CitCode,
                        DeviceSerialNumber = request.DeviceSerial,
                        BagSerialNumber = bagSerialNumber,
                        TransactionDate = request.TransactionDate,
                        UserReferance = request.UserReferance,
                        ItramsReference = request.ItramsReference,
                        IsReceiptPrinted = false,
                        IsNotDeleted = true,
                        EntityState = State.Added,
                        CreatedById = user.UserId,
                        CreateDate = date,
                        LastChangedById = user.UserId,
                        LastChangedDate = date,
                    };
                    _repository.Add(citRequestDetail);
                }
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception on method: [VAULT MESSAGE PROCESSOR] => [SAVE CIT REQUEST]", ex);
            }
        }

        #endregion

        #region Internal
        
        /// <summary>
        /// Get Cash deposit Id
        /// </summary>
        /// <param name="request"></param>
        private long GetCashDepositId(RequestMessage request)
        {
            var serialNumber = request.CollectionUnits.CollectionUnit.FirstOrDefault();

            var bagNumber = serialNumber != null ? serialNumber.Value : "NOT FOUND";

            this.Log().Info(String.Format("Container Serial Number = [{0}]", bagNumber));

            var container = (from d in _repository.All<CashDeposit>()
                             join c in _repository.All<Container>() on d.CashDepositId equals c.CashDepositId
                             where c.SerialNumber == bagNumber
                             select c).FirstOrDefault();
            return container != null ? container.CashDepositId : 0;
        }


        /// <summary>
        ///     Construct a Check string to be used to validate if duplicate request
        /// </summary>
        /// <param name="request"></param>
        private string CreateDuplicateCheckSum(RequestMessage request)
        {
            string serialNumbers = string.Empty;
            foreach (CollectionUnit unit in request.CollectionUnits.CollectionUnit)
                serialNumbers = serialNumbers + unit.Value;
            return string.Concat(request.TransactionDate, request.DeviceSerial, serialNumbers,
                request.Currencies.Denominations.TotalValue);
        }

        /// <summary>
        ///     Evaluate duplicate Requests
        /// </summary>
        /// <param name="request"></param>
        private bool IsResending(RequestMessage request)
        {
            string checksum = CreateDuplicateCheckSum(request);
            IEnumerable<ContainerDrop> result = _repository.Query<ContainerDrop>(o => o.DuplicateChecksum == checksum);
            return result.Any();
        }

        /// <summary>
        ///     Check if Gpt deposit
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsGpt(RequestMessage request)
        {
            Device device =
                _repository.Query<Device>(a => a.SerialNumber.Trim().ToLower() == request.DeviceSerial.Trim().ToLower(),
                    o => o.DeviceType, o => o.DeviceType.Supplier).FirstOrDefault();
            if (device != null)
            {
                Supplier supplier = device.DeviceType.Supplier;
                return supplier.LookUpKey == "GPT" || supplier.LookUpKey == "GREYSTONE";
            }
            return false;
        }

        /// <summary>
        ///     Check if Gpt deposit
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsGreystone(RequestMessage request)
        {
            Device device =
                _repository.Query<Device>(a => a.SerialNumber.Trim().ToLower() == request.DeviceSerial.Trim().ToLower(),
                    o => o.DeviceType, o => o.DeviceType.Supplier).FirstOrDefault();
            if (device != null)
            {
                Supplier supplier = device.DeviceType.Supplier;
                return supplier.LookUpKey == "GREYSTONE";
            }
            return false;
        }

        /// <summary>
        ///     Gets all deposits from the same devices
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private IEnumerable<CashDeposit> QueryCashDeposits(int deviceId)
        {
            IEnumerable<CashDeposit> cashDeposits = _repository.Query<CashDeposit>(a => a.DeviceId == deviceId,
                c => c.VaultBeneficiaries,
                c => c.Containers,
                c => c.Containers.Select(o => o.ContainerDrops),
                c => c.Containers.Select(o => o.ContainerDrops.Select(b => b.ContainerDropItems)));
            return cashDeposits.ToList();
        }

        /// <summary>
        ///     Apply Payment message details
        ///     to indicate that the cashdeposit Cit  message was send to close a transaction
        /// </summary>
        /// <param name="cashDeposits"></param>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private MethodResult<CashDeposit> PayBeneficiary(IEnumerable<CashDeposit> cashDeposits, RequestMessage request,
            User user)
        {
            // Payment message
            try
            {
                Container container = cashDeposits.SelectMany(o => o.Containers)
                    .FirstOrDefault(o => o.SerialNumber == request.CollectionUnits.CollectionUnit.FirstOrDefault().Value);

                VaultPartialPayment payment = GeneratePartialPayment(request, user);

                CashDeposit deposit = container.CashDeposit;
                deposit.VaultAmount = CalculateVaultAmount(request, deposit);
                deposit.EntityState = State.Modified;

                bool saved;
                using (var scope = new TransactionScope())
                {
                    _repository.Update(deposit);
                    saved = _repository.Add(payment) > 0;
                    scope.Complete();
                }

                return saved
                    ? new MethodResult<CashDeposit>(MethodStatus.Successful, deposit)
                    : new MethodResult<CashDeposit>(MethodStatus.Error);
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal("Exception On Method : [RUN_PAYMENT", ex);
                throw;
            }
        }

        /// <summary>
        ///     Set CIT operation values
        ///     to indicate that the cashdeposit Cit  message was send to close a transaction
        /// </summary>
        /// <param name="cashDeposits"></param>
        /// <param name="request"></param>
        /// <param name="user"></param>
        private MethodResult<CashDeposit> CloseBag(IEnumerable<CashDeposit> cashDeposits, RequestMessage request, User user)
        {
            CashDeposit deposit = null;

            try
            {
                foreach (CollectionUnit unit in request.CollectionUnits.CollectionUnit)
                {
                    Container data =
                        cashDeposits.SelectMany(o => o.Containers.Where(e => e.SerialNumber == unit.Value))
                            .FirstOrDefault();
                    if (data != null)
                    {
                        deposit = data.CashDeposit;
                    }
                }

                if (deposit != null)
                {
                    Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "PAY_UNCONFIRMED");

                    foreach (ContainerDrop drop in deposit.Containers.SelectMany(container => container.ContainerDrops))
                    {
                        drop.StatusId = status.StatusId;
                    }

                    deposit.IsConfirmed = true;
                    deposit.StatusId = status.StatusId;
                    deposit.CitDateTime = Convert.ToDateTime(request.TransactionDate);
                    deposit.Narrative = GetCitNarrative(request); 
                    deposit.AccountId = IsGpt(request) ? GetAccountId(request) : null;
                    deposit.LastChangedById = user.UserId;
                    deposit.LastChangedDate = DateTime.Now;
                    deposit.EntityState = State.Modified;
                    deposit.SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Cashdeposit,
                        _repository);

                    return _repository.Update(deposit) > 0
                        ? new MethodResult<CashDeposit>(MethodStatus.Successful, deposit)
                        : new MethodResult<CashDeposit>(MethodStatus.Error);
                }
                return new MethodResult<CashDeposit>(MethodStatus.Error);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method => [CLOSE BAG => CIT MESSAGE]", ex);
                throw;
            }
        }


        /// <summary>
        /// Get Narrative on CIT.
        /// </summary>
        /// <param name="request"></param>
        private string GetCitNarrative(RequestMessage request)
        {
            if (IsGreystone(request))
            {
                var site = _repository.Query<Site>(e => e.CitCode == request.CitCode).FirstOrDefault();
                return site.DepositReference;
            }
            return request.UserReferance;
        }


        /// <summary>
        ///     Creates a deposit object from Request message.
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="user"></param>
        /// <param name="vaultSource"></param>
        private MethodResult<CashDeposit> CreateCashDeposit(RequestMessage deposit, User user, VaultSource vaultSource)
        {
            try
            {
                Site site = _repository.Query<Site>(a => a.CitCode == deposit.CitCode).FirstOrDefault();
                Device device = _repository.Query<Device>(a => a.SerialNumber == deposit.DeviceSerial).FirstOrDefault();

                DepositType depositType = _depositTypes.FirstOrDefault(a => a.LookUpKey == "MULTI_DROP");
                ProductType capturingSource = _capturingSources.FirstOrDefault(a => a.LookUpKey == "MYSBV_VAULT");
                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "ACTIVE");
                ContainerType containerType =
                    _repository.Query<ContainerType>(a => a.LookUpKey == "DROP_SAFE_CONTAINER").FirstOrDefault();

                var cashDeposit = new CashDeposit
                {
                    DepositTypeId = depositType.DepositTypeId,
                    SiteId = site.SiteId,
                    DeviceId = device.DeviceId,
                    Narrative = deposit.UserReferance,
                    TransactionReference = _lookup.GenerateTransactionNumber(site.SiteId),
                    DepositedAmount = Convert.ToDecimal(deposit.Currencies.Denominations.TotalValue),
                    VaultAmount = Convert.ToDecimal(deposit.Currencies.Denominations.TotalValue),
                    VaultSource = vaultSource.Name(),
                    IsSubmitted = true,
                    IsConfirmed = true,
                    SubmitDateTime = DateTime.Now,
                    ProductTypeId = capturingSource.ProductTypeId,
                    iTramsUserName = deposit.Users.User.Value,
                    StatusId = status.StatusId,
                    IsNotDeleted = true,
                    LastChangedById = user.UserId,
                    CreatedById = user.UserId,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                    AccountId = IsGpt(deposit) ? GetAccountId(deposit) : null,
                    EntityState = State.Added,
                    SettlementIdentifier =
                        ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Cashdeposit, _repository),
                    VaultBeneficiaries = new Collection<VaultBeneficiary> {CreateBeneficiary(deposit, user)},
                    Containers = new Collection<Container>
                    {
                        new Container
                        {
                            ContainerTypeId = containerType.ContainerTypeId,
                            ReferenceNumber = _lookup.GenerateTransactionNumberForContainer(site.SiteId),
                            SerialNumber = deposit.CollectionUnits.CollectionUnit.FirstOrDefault().Value,
                            SealNumber = deposit.CollectionUnits.CollectionUnit.FirstOrDefault().Value,
                            IsPrimaryContainer = true,
                            Amount = Convert.ToDecimal(deposit.Currencies.Denominations.TotalValue),
                            IsNotDeleted = true,
                            CreatedById = user.UserId,
                            LastChangedById = user.UserId,
                            LastChangedDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            EntityState = State.Added,
                            ContainerDrops = new Collection<ContainerDrop>
                            {
                                new ContainerDrop
                                {
                                    StatusId = status.StatusId,
                                    ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(site.SiteId),
                                    BagSerialNumber = deposit.CollectionUnits.CollectionUnit.FirstOrDefault().Value,
                                    Number = 1,
                                    Narrative = deposit.UserReferance,
                                    Amount = Convert.ToDecimal(deposit.Currencies.Denominations.TotalValue),
                                    DuplicateChecksum = CreateDuplicateCheckSum(deposit),
                                    IsNotDeleted = true,
                                    LastChangedById = user.UserId,
                                    CreatedById = user.UserId,
                                    TransactionDateTime = deposit.TransactionDate.ToShortDateString(),
                                    LastChangedDate = DateTime.Now,
                                    CreateDate = DateTime.Now,
                                    EntityState = State.Added,
                                    ContainerDropItems = CreateDropItems(deposit, user)
                                }
                            },
                        }
                    }
                };

                return _repository.Add(cashDeposit) > 0
                    ? new MethodResult<CashDeposit>(MethodStatus.Successful, cashDeposit)
                    : new MethodResult<CashDeposit>(MethodStatus.Error);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception on method => [CREATE_CASH_DEPOSIT]", ex);
                throw;
            }
        }

        /// <summary>
        ///     Generates a new ContainerDrop
        /// </summary>
        /// <param name="request"></param>
        /// <param name="container"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private MethodResult<CashDeposit> AddContainerDrop(RequestMessage request, Container container, User user)
        {
            Site site = _repository.Query<Site>(a => a.CitCode == request.CitCode).FirstOrDefault();
            Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "ACTIVE");

            try
            {
                var containerDrop = new ContainerDrop
                {
                    StatusId = status.StatusId,
                    ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(site.SiteId),
                    BagSerialNumber = request.CollectionUnits.CollectionUnit.FirstOrDefault().Value,
                    Number = container.ContainerDrops.Count + 1,
                    Narrative = request.UserReferance,
                    Amount = Convert.ToDecimal(request.Currencies.Denominations.TotalValue),
                    IsNotDeleted = true,
                    LastChangedById = user.UserId,
                    CreatedById = user.UserId,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    TransactionDateTime = request.TransactionDate.ToShortDateString(),
                    EntityState = State.Added,
                    DuplicateChecksum = CreateDuplicateCheckSum(request),
                    ContainerDropItems = CreateDropItems(request, user)
                };
                container.ContainerDrops.Add(containerDrop);

                CashDeposit cashDeposit = ComputeActuals(container);
                cashDeposit.DepositedAmount += containerDrop.Amount;
                cashDeposit.VaultAmount += containerDrop.Amount;

                cashDeposit.SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Cashdeposit,
                    _repository);

                cashDeposit.AccountId = IsGpt(request) ? GetAccountId(request) : null;
                bool saved;
                using (var scope = new TransactionScope())
                {
                    _repository.Update(cashDeposit);

                    VaultBeneficiary beneficiary = CreateBeneficiary(request, user);
                    beneficiary.ContainerDropId = containerDrop.ContainerDropId;
                    beneficiary.CashDepositId = cashDeposit.CashDepositId;

                    saved = _repository.Add(beneficiary) > 0;
                    scope.Complete();
                }

                return saved
                    ? new MethodResult<CashDeposit>(MethodStatus.Successful, cashDeposit)
                    : new MethodResult<CashDeposit>(MethodStatus.Error);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method => [CREATE_CONTAINER_DROP]", ex);
                throw;
            }
        }

        /// <summary>
        ///     Runs for Partial Payments
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private VaultPartialPayment GeneratePartialPayment(RequestMessage deposit, User user)
        {
            Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "PAY_UNCONFIRMED");

            return new VaultPartialPayment
            {
                StatusId = status.StatusId,
                DeviceSerialNumber = deposit.DeviceSerial,
                BagSerialNumber = deposit.CollectionUnits.CollectionUnit.FirstOrDefault().Value,
                CitCode = deposit.CitCode,
                BeneficiaryCode = deposit.BeneficiaryCode,
                TotalToBePaid = Convert.ToDecimal(deposit.Currencies.Denominations.TotalValue),
                DeviceUserName = deposit.Users.User.Value,
                DeviceUserRole = deposit.Users.User.Type,
                SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Vaultpayments, _repository),
                IsNotDeleted = true,
                PaymentReference = deposit.UserReferance,
                TransactionDate = deposit.TransactionDate,
                LastChangedById = user.UserId,
                CreatedById = user.UserId,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                EntityState = State.Added
            };
        }

        /// <summary>
        ///     Calculates total to be paid
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="cashDeposit"></param>
        /// <returns></returns>
        private decimal CalculateVaultAmount(RequestMessage deposit, CashDeposit cashDeposit)
        {
            decimal requestedAmount = Convert.ToDecimal(deposit.Currencies.Denominations.TotalValue);
            decimal vaultAmount = Convert.ToDecimal(cashDeposit.VaultAmount.Value) - requestedAmount;
            return vaultAmount;
        }

        /// <summary>
        ///     Creates Beneficiciary.
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="user"></param>
        private VaultBeneficiary CreateBeneficiary(RequestMessage deposit, User user)
        {
            return new VaultBeneficiary
            {
                AccountId = GetAccountId(deposit),
                DeviceUserName = deposit.Users.User.Value,
                DeviceUserRole = deposit.Users.User.Type,
                IsNotDeleted = true,
                LastChangedById = user.UserId,
                CreatedById = user.UserId,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                EntityState = State.Added
            };
        }

        /// <summary>
        ///     Gets Beneficiary Account details
        /// </summary>
        /// <param name="message"></param>
        private int? GetAccountId(RequestMessage message)
        {
            this.Log().Debug(message.BeneficiaryCode);

            Account account =
                _repository.Query<Account>(a => a.BeneficiaryCode.ToLower() == message.BeneficiaryCode.ToLower(),
                    a => a.Site,
                    a => a.Site.Accounts).FirstOrDefault();

            if (account != null)
            {
                if (IsGpt(message))
                {
                    this.Log().Debug(string.Format("Number of accounts [{0}]", account.Site.Accounts.Count));

                    foreach (Account account1 in account.Site.Accounts)
                    {
                        this.Log()
                            .Debug(string.Format("Account Holder [{0}] - Beneficiary Code [{1}] - Default [{2}]",
                                account1.AccountHolderName, account1.BeneficiaryCode, account1.DefaultAccount));
                    }

                    Account defaultAccont = account.Site.Accounts.FirstOrDefault(a => a.DefaultAccount);

                    this.Log().Debug(defaultAccont != null);

                    if (defaultAccont != null)
                    {
                        this.Log().Debug(message.BeneficiaryCode);
                        return defaultAccont.AccountId;
                    }
                }
                else return account.AccountId;
            }
            this.Log().Debug(-1);
            return null;
        }

        #endregion

        #region Helpers

        /// <summary>
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private CashDeposit ComputeActuals(Container container)
        {
            decimal total = container.ContainerDrops.Sum(a => a.Amount);
            container.Amount = total;
            container.EntityState = State.Modified;
            container.CashDeposit.EntityState = State.Modified;
            return container.CashDeposit;
        }

        /// <summary>
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private Collection<ContainerDropItem> CreateDropItems(RequestMessage deposit, User user)
        {
            Dictionary<string, VaultDenomination> capturedDenominations = ReadCapturedDenominations(deposit);

            var items = new List<ContainerDropItem>();
            foreach (var denomination in capturedDenominations)
            {
                var containerDropItem = new ContainerDropItem
                {
                    DenominationId = GetDenomination(denomination.Key),
                    ValueInCents = _denominations.FirstOrDefault(a => a.LookUpKey == denomination.Key).ValueInCents,
                    Count = denomination.Value.Count,
                    Value = CalculateDenominationValue(denomination),
                    DenominationName = GetDenominationName(GetDenomination(denomination.Key)),
                    DenominationType = Enum.GetName(typeof (DenominationType), denomination.Value.DenominationType),
                    IsNotDeleted = true,
                    CreatedById = user.UserId,
                    LastChangedById = user.UserId,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                    EntityState = State.Added
                };
                items.Add(containerDropItem);
            }
            return items.ToCollection();
        }

        /// <summary>
        ///     Gets the DenominationID when given the Denomination Key e.g. R10
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        private int GetDenomination(string lookUpKey)
        {
            Denomination denomination = _repository.Query<Denomination>(e => e.LookUpKey == lookUpKey).FirstOrDefault();
            return denomination != null ? denomination.DenominationId : 0;
        }

        /// <summary>
        ///     Gets denomination Name by ID
        /// </summary>
        /// <param name="denominationId"></param>
        /// <returns></returns>
        private string GetDenominationName(int denominationId)
        {
            return _repository.Query<Denomination>(o => o.DenominationId == denominationId).FirstOrDefault().Description;
        }

        /// <summary>
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        private Dictionary<string, VaultDenomination> ReadCapturedDenominations(RequestMessage deposit)
        {
            // Store the name of the denomination together with the count
            // into a list.
            var capturedDenominations = new Dictionary<string, VaultDenomination>();

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 5))
            {
                capturedDenominations.Add("5C", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 5).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 10))
            {
                capturedDenominations.Add("10C", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 10).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 20))
            {
                capturedDenominations.Add("20C", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 20).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 50))
            {
                capturedDenominations.Add("50C", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 50).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 100))
            {
                capturedDenominations.Add("R1", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 100).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 200))
            {
                capturedDenominations.Add("R2", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 200).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 500))
            {
                capturedDenominations.Add("R5", new VaultDenomination
                {
                    Count = deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 500).Count,
                    DenominationType = DenominationType.Coins
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 1000))
            {
                capturedDenominations.Add("R10", new VaultDenomination
                {
                    Count =
                        deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 1000).Count,
                    DenominationType = DenominationType.Notes
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 2000))
            {
                capturedDenominations.Add("R20", new VaultDenomination
                {
                    Count =
                        deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 2000).Count,
                    DenominationType = DenominationType.Notes
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 5000))
            {
                capturedDenominations.Add("R50", new VaultDenomination
                {
                    Count =
                        deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 5000).Count,
                    DenominationType = DenominationType.Notes
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 10000))
            {
                capturedDenominations.Add("R100", new VaultDenomination
                {
                    Count =
                        deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 10000).Count,
                    DenominationType = DenominationType.Notes
                });
            }

            if (deposit.Currencies.Denominations.Fit.Denomination.Any(e => e.Value == 20000))
            {
                capturedDenominations.Add("R200", new VaultDenomination
                {
                    Count =
                        deposit.Currencies.Denominations.Fit.Denomination.FirstOrDefault(e => e.Value == 20000).Count,
                    DenominationType = DenominationType.Notes
                });
            }
            return capturedDenominations;
        }

        /// <summary>
        /// </summary>
        /// <param name="capturedDenomination"></param>
        /// <returns></returns>
        private decimal CalculateDenominationValue(KeyValuePair<string, VaultDenomination> capturedDenomination)
        {
            switch (capturedDenomination.Key)
            {
                case "R200":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*200);
                case "R100":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*100);
                case "R50":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*50);
                case "R20":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*20);
                case "R10":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*10);
                case "R5":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*5);
                case "R2":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*2);
                case "R1":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*1);
                case "50C":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*(50/100));
                case "20C":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*(20/100));
                case "10C":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*(10/100));
                case "5C":
                    return Convert.ToDecimal(capturedDenomination.Value.Count*(5/100));
                default:
                    return 0;
            }
        }


        /// <summary>
        ///     Configure Return-Status
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="depositResult"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private MethodResult<bool> ReturnStatus<T>(MethodResult<T> depositResult, string message)
        {
            if (depositResult.Status == MethodStatus.Successful)
            {
                return new MethodResult<bool>(MethodStatus.Successful, true, null, HttpStatusCode.Created);
            }
            return new MethodResult<bool>(MethodStatus.Error, false, message, HttpStatusCode.ExpectationFailed);
        }

        #endregion
    }
}