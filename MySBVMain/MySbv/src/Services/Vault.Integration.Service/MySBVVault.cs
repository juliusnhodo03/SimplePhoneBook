using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Web;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Repository;
using Utility.Core;
using Vault.Integration.Service.Contracts;
using Vault.Integration.Service.Infrastructure;

namespace Vault.Integration.Service
{
    #region IMySbvVault Implementation

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
// ReSharper disable once InconsistentNaming
    public class MySBVVault : IMySbvVault
    {
        #region Fields

        private readonly IEnumerable<ProductType> _capturingSources;
        private readonly IEnumerable<ContainerType> _containerTypes;
        private readonly IEnumerable<Denomination> _denominations;
        private readonly IEnumerable<DepositType> _depositTypes;
        private readonly IRepository _repository;
        private readonly IEnumerable<Status> _statuses;

        #endregion

        #region Constructor

        public MySBVVault()
        {
            _repository = new Repository();
            _depositTypes = _repository.All<DepositType>();
            _denominations = _repository.All<Denomination>();
            _capturingSources = _repository.All<ProductType>();
            _statuses = _repository.All<Status>();
            _containerTypes = _repository.All<ContainerType>();
        }

        #endregion

        #region IMySBVVault

        public Accounts Accounts(string citCode)
        {
            var sites = _repository.All<Site>(a => a.Accounts, b => b.Accounts.Select(x => x.Bank), t => t.Accounts.Select(v => v.AccountType));
            Site result = sites.FirstOrDefault(a => a.CitCode == citCode);
            if (result != null)
            {
                var siteAccount = result.Accounts.Where(a => a.SiteId == result.SiteId).ToList().ToCollection();

                if (siteAccount != null)
                {
                    var account = new Accounts
                    {
                        AccountsCollection = 
                            siteAccount.Select(acc => new AccountInformation
                            {
                                AccountId = acc.AccountId,
                                BeneficiaryCode = acc.BeneficiaryCode,
                                BankName = acc.Bank.Name,
                                AccountHolder = acc.AccountHolderName,
                                AccountType = acc.AccountType.Name,
                            }).ToList()
                    };
                    return account;
                }
                throw new WebFaultException<string>("A site does not have any accounts associated with it.",
                    HttpStatusCode.NotFound);
            }
            throw new WebFaultException<string>(string.Format("A site with CIT Code = {0} is not found.", citCode),
                HttpStatusCode.NotFound);
        }

        public DepositResult SubmitDeposit(iTramsDeposit deposit)
        {
            ValidateTotals(deposit);
            ValidateFields(deposit);

            // All My SBV vault deposit will be captured by the system account. SBVAdmin.
            // However, for Audit purpose, iTrams must always give us the unique username
            // of the person that made the deposit on the device.
            User user = _repository.Query<User>(a => a.UserId == 1).FirstOrDefault();

            if (user != null)
            {
                Site site = _repository.Query<Site>(a => a.CitCode == deposit.CitCode).FirstOrDefault();
                if (site == null)
                    throw new WebFaultException<string>(
                        string.Format("Site with CITCode = {0} was not found.", deposit.CitCode),
                        HttpStatusCode.NotFound);

                var devices = _repository.All<Device>();
                Device device = devices.FirstOrDefault(a => a.SerialNumber == deposit.DeviceSerialNumber);
                if (device == null)
                    throw new WebFaultException<string>(
                        string.Format("Device with serial number =  {0} was not found.", deposit.DeviceSerialNumber),
                        HttpStatusCode.NotFound);

                Account account = _repository.Query<Account>(a => a.BeneficiaryCode == deposit.BeneficiaryCode).FirstOrDefault();
                if (account == null)
                    throw new WebFaultException<string>(string.Format("Account With Beneficiary Code [{0}] was not found.", deposit.BeneficiaryCode),
                        HttpStatusCode.NotFound);

                DepositType depositType = _depositTypes.FirstOrDefault(a => a.LookUpKey == "MULTI_DROP");
                if (depositType == null)
                    throw new WebFaultException<string>("Invalid Deposit Type", HttpStatusCode.InternalServerError);

                ProductType capturingSource = _capturingSources.FirstOrDefault(a => a.LookUpKey == "MYSBV_VAULT");
                if (capturingSource == null)
                    throw new WebFaultException<string>("Invalid Capture Source", HttpStatusCode.InternalServerError);

                Status status = _statuses.FirstOrDefault(a => a.LookUpKey == "CONFIRMED");
                if (status == null)
                    throw new WebFaultException<string>("Invalid Status", HttpStatusCode.InternalServerError);

                ContainerType containerType = _containerTypes.FirstOrDefault(a => a.LookUpKey == "DROP_SAFE_CONTAINER");
                if (containerType == null)
                    throw new WebFaultException<string>("Invalid Container Type", HttpStatusCode.InternalServerError);

                if (deposit.IsNewBag)
                {
                    var cashDeposit = new CashDeposit
                    {
                        DepositTypeId = depositType.DepositTypeId,
                        SiteId = site.SiteId,
#pragma warning disable 618
                        AccountId = deposit.AccountId != 0 ? deposit.AccountId : account.AccountId,
#pragma warning restore 618
                        DeviceId = device.DeviceId,
                        Narrative = deposit.iTramsReference,
                        TransactionReference = GenerateTransactionNumber(site.SiteId),
                        DepositedAmount = Convert.ToDecimal(deposit.TotalAmount),
                        ActualAmount = Convert.ToDecimal(deposit.TotalAmount),
                        IsSubmitted = true,
                        SubmitDateTime = DateTime.Now,
                        ProductTypeId = capturingSource.ProductTypeId,
                        iTransUserName = deposit.UserName,
                        IsProcessed = true,
                        //CitCollectionDate = DateTime.Now, // Not a true reflection. To be investigated
                        StatusId = status.StatusId,
                        IsNotDeleted = true,
                        LastChangedById = user.UserId,
                        CreatedById = user.UserId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now,
                        Containers = new Collection<Container>
                        {
                            new Container
                            {
                                ContainerTypeId = containerType.ContainerTypeId,
                                ReferenceNumber = GenerateTransactionNumberForContainer(site.SiteId),
                                SerialNumber = deposit.NoteBagSealNumber,
                                SealNumber = deposit.NoteBagSealNumber,
                                IsPrimaryContainer = true,
                                Amount = Convert.ToDecimal(deposit.TotalAmount),
                                ActualAmount = Convert.ToDecimal(deposit.TotalAmount),
                                IsNotDeleted = true,
                                CreatedById = user.UserId,
                                LastChangedById = user.UserId,
                                LastChangedDate = DateTime.Now,
                                CreateDate = DateTime.Now,
                                ContainerDrops = new Collection<ContainerDrop>
                                {
                                    new ContainerDrop
                                    {
                                        StatusId = status.StatusId,
                                        ReferenceNumber = GenerateTransactionNumberForDrop(site.SiteId),
                                        BagSerialNumber = deposit.NoteBagSealNumber,
                                        Number = 1,
                                        Narrative = deposit.iTramsReference,
                                        Amount = Convert.ToDecimal(deposit.TotalAmount),
                                        ActualAmount = Convert.ToDecimal(deposit.TotalAmount),
                                        IsNotDeleted = true,
                                        LastChangedById = user.UserId,
                                        CreatedById = user.UserId,
                                        LastChangedDate = DateTime.Now,
                                        CreateDate = DateTime.Now,
                                        ContainerDropItems = CreateDropItems(deposit, user)
                                    }
                                },
                            }
                        }
                    };

                    if (_repository.Add(cashDeposit) > 0)
                    {
                        return new DepositResult() { Status = status.Description };
                    }
                }
                else
                {
                    Container container =
                        _repository.Query<Container>(a => a.SerialNumber == deposit.NoteBagSealNumber, 
                        c => c.CashDeposit,
                        c => c.CashDeposit.Containers,
                        c => c.ContainerDrops,
                        c => c.ContainerDrops.Select(b => b.ContainerDropItems)).FirstOrDefault();

                    if (container != null)
                    {
                        container.ContainerDrops.Add(new ContainerDrop
                        {
                            StatusId = status.StatusId,
                            ReferenceNumber = GenerateTransactionNumberForDrop(site.StatusId),
                            BagSerialNumber = deposit.NoteBagSealNumber,
                            Number = 1,
                            Narrative = deposit.iTramsReference,
                            Amount = Convert.ToDecimal(deposit.TotalAmount),
                            ActualAmount = Convert.ToDecimal(deposit.TotalAmount),
                            IsNotDeleted = true,
                            LastChangedById = user.UserId,
                            CreatedById = user.UserId,
                            LastChangedDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            EntityState = State.Added,
                            ContainerDropItems = CreateDropItems(deposit, user)
                        });

                        var cashDeposit = ComputeActuals(container);

                        if (_repository.Update(cashDeposit) > 0)
                        {
                            return new DepositResult() { Status = status.Description };
                        }
                    }
                    throw new WebFaultException<string>(string.Format("Note Bag Serial Number [{0}] Not Found.",
                        deposit.NoteBagSealNumber), HttpStatusCode.NotFound);
                }
            }

            throw new WebFaultException<string>(
                string.Format("User with name {0} was not found.", deposit.UserName), HttpStatusCode.NotFound);
        }

        #endregion

        #region Helpers

        private void ValidateFields(iTramsDeposit deposit)
        {
            int count;
            double amount;
            bool isValid;

            if(!_repository.Any<Account>(a => a.AccountId == deposit.AccountId))
                throw new WebFaultException<string>("Account Not Found", HttpStatusCode.NotFound);

            if (string.IsNullOrEmpty(deposit.BeneficiaryCode) || string.IsNullOrWhiteSpace(deposit.BeneficiaryCode))
                throw new WebFaultException<string>("Beneficiary Code Cannot be NULL/Empty", HttpStatusCode.NotAcceptable);

            if (string.IsNullOrEmpty(deposit.NoteBagSealNumber) || string.IsNullOrWhiteSpace(deposit.NoteBagSealNumber))
                throw new WebFaultException<string>("Note Bag Serial Number Cannot be NULL/Empty", HttpStatusCode.NotAcceptable);

            if (deposit.IsNewBag)
            {
                if(_repository.Any<Container>(a => a.SerialNumber == deposit.NoteBagSealNumber))
                    throw new WebFaultException<string>(string.Format("Bag with serial number [{0}] already exist.", deposit.NoteBagSealNumber), HttpStatusCode.ExpectationFailed);
            }

            if (!deposit.IsNewBag)
            {
                if (!_repository.Any<Container>(a => a.SerialNumber == deposit.NoteBagSealNumber))
                    throw new WebFaultException<string>(string.Format("Bag with serial number [{0}] was not found.", deposit.NoteBagSealNumber), HttpStatusCode.NotFound);
            }

            if (string.IsNullOrEmpty(deposit.DeviceSerialNumber) || string.IsNullOrWhiteSpace(deposit.DeviceSerialNumber))
                throw new WebFaultException<string>("Device Serial Number Cannot be NULL/Empty", HttpStatusCode.NotAcceptable);

            if (string.IsNullOrEmpty(deposit.iTramsReference) || string.IsNullOrWhiteSpace(deposit.iTramsReference))
                throw new WebFaultException<string>("iTrams Reference Cannot be NULL/Empty", HttpStatusCode.NotAcceptable);

            if (string.IsNullOrEmpty(deposit.UserName) || string.IsNullOrWhiteSpace(deposit.UserName))
                throw new WebFaultException<string>("Invalid Username [username cannot be null]", HttpStatusCode.NotAcceptable);

            if (!bool.TryParse(deposit.IsNewBag.ToString(), out isValid))
                throw new WebFaultException<string>(string.Format("Invalid Valid Value for IsNewBag [{0}]",
                    CheckValue(deposit.IsNewBag.ToString())), HttpStatusCode.NotAcceptable);
            
            if(!double.TryParse(deposit.TotalAmount.ToString(CultureInfo.InvariantCulture), out amount))
                throw new WebFaultException<string>(string.Format("Invalid Total Amount [{0}]",
                    CheckValue(deposit.TotalAmount.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR5.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR5 [{0}]", 
                    CheckValue(deposit.ZAR5.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR10.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR10 [{0}]",
                    CheckValue(deposit.ZAR10.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR20.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR20 [{0}]",
                    CheckValue(deposit.ZAR20.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR50.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR50 [{0}]",
                    CheckValue(deposit.ZAR50.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR100.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR100 [{0}]",
                    CheckValue(deposit.ZAR100.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR200.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR200 [{0}]",
                    CheckValue(deposit.ZAR200.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR500.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR500 [{0}]",
                    CheckValue(deposit.ZAR500.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR1000.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR1000 [{0}]",
                    CheckValue(deposit.ZAR1000.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR2000.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR2000 [{0}]",
                    CheckValue(deposit.ZAR2000.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR5000.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR5000 [{0}]",
                    CheckValue(deposit.ZAR5000.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR10000.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR10000 [{0}]",
                    CheckValue(deposit.ZAR10000.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);

            if (!int.TryParse(deposit.ZAR20000.ToString(CultureInfo.InvariantCulture), out count))
                throw new WebFaultException<string>(string.Format("Invalid denomination for ZAR20000 [{0}]",
                    CheckValue(deposit.ZAR20000.ToString(CultureInfo.InvariantCulture))), HttpStatusCode.NotAcceptable);
        }

        private string CheckValue(string value)
        {
            if (string.IsNullOrEmpty(value)) return "NULL";
            if (string.IsNullOrWhiteSpace(value)) return "NULL";
            return value;
        }

        private void ValidateTotals(iTramsDeposit deposit)
        {
            Dictionary<string, iTramsDenomination> capturedDenominations = ReadCapturedDenominations(deposit);
            var total = capturedDenominations.Sum(a => CalculateDenominationValue(a));
            if(total != Convert.ToDecimal(deposit.TotalAmount))
                throw new WebFaultException<string>(string.Format("The Total Computation Of The Supplied All Denomination Count [{0}] Does Not Amount To The Total Amount [{1}]."
                    , total, deposit.TotalAmount),
                    HttpStatusCode.ExpectationFailed);
        }

        private CashDeposit ComputeActuals(Container container)
        {
            var total = container.ContainerDrops.Sum(a => a.ActualAmount);
            container.Amount = total;
            container.ActualAmount = total;
            container.EntityState = State.Modified;
            container.CashDeposit.ActualAmount = total;
            container.CashDeposit.DepositedAmount = total;
            container.CashDeposit.EntityState = State.Modified;
            return container.CashDeposit;
        }

        private Collection<ContainerDropItem> CreateDropItems(iTramsDeposit deposit, User user)
        {
            Dictionary<string, iTramsDenomination> capturedDenominations = ReadCapturedDenominations(deposit);
            return capturedDenominations.Select(capturedDenomination => new ContainerDropItem
            {
                DenominationId =
                    _denominations.FirstOrDefault(a => a.LookUpKey == capturedDenomination.Key).DenominationId,
                ValueInCents =
                    _denominations.FirstOrDefault(a => a.LookUpKey == capturedDenomination.Key).ValueInCents,
                Count = capturedDenomination.Value.Count,
                Value = CalculateDenominationValue(capturedDenomination),
                ActualCount = capturedDenomination.Value.Count,
                ActualValue = CalculateDenominationValue(capturedDenomination),
                DenominationType = Enum.GetName(typeof(Infrastructure.DenominationType),
                    capturedDenomination.Value.DenominationType),
                IsNotDeleted = true,
                CreatedById = user.UserId,
                LastChangedById = user.UserId,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
                EntityState = State.Added
            }).ToList().ToCollection();
        }

        private decimal CalculateDenominationValue(KeyValuePair<string, iTramsDenomination> capturedDenomination)
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
                    return 0;
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*5);
                case "R2":
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*2);
                    return 0;
                case "R1":
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*1);
                    return 0;
                case "50C":
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*(50/100));
                    return 0;
                case "20C":
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*(20/100));
                    return 0;
                case "10C":
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*(10/100));
                    return 0;
                case "5C":
                    //return Convert.ToDecimal(capturedDenomination.Value.Count*(5/100));
                    return 0;
                default:
                    throw new FaultException("Invalid Denomination Type");
            }
        }

        private Dictionary<string, iTramsDenomination> ReadCapturedDenominations(iTramsDeposit deposit)
        {
            // Store the name of the denomination together with the count
            // into a list.
            var capturedDenominations = new Dictionary<string, iTramsDenomination>();

            if (deposit.ZAR5 > 0)
                capturedDenominations.Add("5C",
                    new iTramsDenomination { Count = deposit.ZAR5, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR10 > 0)
                capturedDenominations.Add("10C",
                    new iTramsDenomination { Count = deposit.ZAR10, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR20 > 0)
                capturedDenominations.Add("20C",
                    new iTramsDenomination { Count = deposit.ZAR20, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR50 > 0)
                capturedDenominations.Add("50C",
                    new iTramsDenomination { Count = deposit.ZAR50, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR100 > 0)
                capturedDenominations.Add("R1",
                    new iTramsDenomination { Count = deposit.ZAR100, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR200 > 0)
                capturedDenominations.Add("R2",
                    new iTramsDenomination { Count = deposit.ZAR200, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR500 > 0)
                capturedDenominations.Add("R5",
                    new iTramsDenomination { Count = deposit.ZAR500, DenominationType = Infrastructure.DenominationType.Coins });
            if (deposit.ZAR1000 > 0)
                capturedDenominations.Add("R10",
                    new iTramsDenomination { Count = deposit.ZAR1000, DenominationType = Infrastructure.DenominationType.Notes });
            if (deposit.ZAR2000 > 0)
                capturedDenominations.Add("R20",
                    new iTramsDenomination { Count = deposit.ZAR2000, DenominationType = Infrastructure.DenominationType.Notes });
            if (deposit.ZAR5000 > 0)
                capturedDenominations.Add("R50",
                    new iTramsDenomination { Count = deposit.ZAR5000, DenominationType = Infrastructure.DenominationType.Notes });
            if (deposit.ZAR10000 > 0)
                capturedDenominations.Add("R100",
                    new iTramsDenomination { Count = deposit.ZAR10000, DenominationType = Infrastructure.DenominationType.Notes });
            if (deposit.ZAR20000 > 0)
                capturedDenominations.Add("R200",
                    new iTramsDenomination {Count = deposit.ZAR20000, DenominationType = Infrastructure.DenominationType.Notes});
            return capturedDenominations;
        }

        #endregion

        #region Generate Transaction Number

        private string GetRandomCharacters()
        {
            var hashSet = new HashSet<string>();
            var array = new[]
            {
                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "R", "S", "T", "U", "V",
                "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
            };
            while (hashSet.Count < 3)
            {
                hashSet.Add(array[new Random().Next(0, 34)]);
            }

            return hashSet.ToHashString();
        }

        private string Generate(string citCode)
        {
            string depositDate = string.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            string transactionReferenceNumber = string.Concat(GetRandomCharacters(),
                citCode, depositDate);
            return transactionReferenceNumber;
        }

        private string GenerateTransactionNumber(int siteId)
        {
            DateTime endDate = DateTime.Now.AddDays(1);
            string depositDate = string.Concat(DateTime.Now.Year, "-", DateTime.Now.Month, "-", DateTime.Now.Day);

            DateTime date = Convert.ToDateTime(depositDate);

            string citCode =
                _repository.Query<Site>(a => a.IsNotDeleted && a.SiteId == siteId).FirstOrDefault().CitCode;
            string transactionReferenceNumber = Generate(citCode);

            while (true)
            {
                if (!_repository.Any<CashDeposit>(a => a.TransactionReference == transactionReferenceNumber
                                                       && a.IsNotDeleted && a.CreateDate.HasValue
                                                       && a.CreateDate >= date && a.CreateDate < endDate))
                {
                    return transactionReferenceNumber;
                }
                transactionReferenceNumber = Generate(citCode);
            }
        }

        private string GenerateTransactionNumberForContainer(int siteId)
        {
            DateTime endDate = DateTime.Now.AddDays(1);
            string depositDate = string.Concat(DateTime.Now.Year, "-", DateTime.Now.Month, "-", DateTime.Now.Day);

            DateTime date = Convert.ToDateTime(depositDate);

            string citCode =
                _repository.Query<Site>(a => a.IsNotDeleted && a.SiteId == siteId).FirstOrDefault().CitCode;
            string transactionReferenceNumber = Generate(citCode);

            while (true)
            {
                if (!_repository.Any<Container>(a => a.ReferenceNumber == transactionReferenceNumber
                                                     && a.IsNotDeleted && a.CreateDate.HasValue
                                                     && a.CreateDate >= date && a.CreateDate < endDate))
                {
                    return transactionReferenceNumber;
                }
                transactionReferenceNumber = Generate(citCode);
            }
        }

        private string GenerateTransactionNumberForDrop(int siteId)
        {
            DateTime endDate = DateTime.Now.AddDays(1);
            string depositDate = string.Concat(DateTime.Now.Year, "-", DateTime.Now.Month, "-", DateTime.Now.Day);

            DateTime date = Convert.ToDateTime(depositDate);

            string citCode =
                _repository.Query<Site>(a => a.IsNotDeleted && a.SiteId == siteId).FirstOrDefault().CitCode;
            string transactionReferenceNumber = Generate(citCode);

            while (true)
            {
                if (!_repository.Any<ContainerDrop>(a => a.ReferenceNumber == transactionReferenceNumber
                                                         && a.IsNotDeleted && a.CreateDate.HasValue
                                                         && a.CreateDate >= date && a.CreateDate < endDate))
                {
                    return transactionReferenceNumber;
                }
                transactionReferenceNumber = Generate(citCode);
            }
        }

        #endregion
    }

    #endregion

}