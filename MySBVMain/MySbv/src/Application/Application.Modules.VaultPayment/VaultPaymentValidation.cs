using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto.Account;
using Application.Dto.Device;
using Application.Dto.Merchant;
using Application.Dto.Site;
using Application.Dto.VaultPayment;
using Application.Mapper;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Utility.Core;

namespace Application.Modules.VaultPayment
{
    public class VaultPaymentValidation : IVaultPaymentValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IAsyncRepository _repository;
        private readonly IRepository _repositoryResult;

        #endregion

        #region Constructor

        public VaultPaymentValidation(ILookup lookup, IAsyncRepository repository, IMapper mapper, IRepository repositoryResult)
        {
            _lookup = lookup;
            _repository = repository;
            _mapper = mapper;
            _repositoryResult = repositoryResult;
        }

        #endregion

        #region IVaultPayment Validation

        /// <summary>
        ///     Get deposited amount in the Device.
        ///     this retrieves only for an open bag
        /// </summary>
        /// <param name="bagNumber"></param>
        /// <returns></returns>
        public async Task<decimal> GetAmountInDevice(string bagNumber)
        {
            try
            {
                // total deposited in bag.
                IEnumerable<ContainerDrop> cashDrops =
                    await _repository.GetAsync<ContainerDrop>(e => e.BagSerialNumber == bagNumber && e.IsNotDeleted);
                decimal amountDeposited = cashDrops.Sum(x => x.Amount);

                // total paid from bag.
                decimal totalPaid;

                bool anyPaymentsMade =
                    await _repository.AnyAsync<VaultPartialPayment>(e => e.BagSerialNumber == bagNumber && e.IsNotDeleted);

                if (!anyPaymentsMade)
                {
                    totalPaid = 0;
                }
                else
                {
                    IEnumerable<VaultPartialPayment> partialPayments =
                        await _repository.GetAsync<VaultPartialPayment>(e => e.BagSerialNumber == bagNumber && e.IsNotDeleted);
                    totalPaid = partialPayments.Sum(x => x.TotalToBePaid);
                }

                // available funds
                decimal availableFunds = amountDeposited - totalPaid;

                return Math.Round(availableFunds, 2);
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        ///     Release payment to Hyphen
        /// </summary>
        /// <param name="vaultPaymentDto"></param>
        /// <param name="userId"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        public async Task<MethodResult<PaymentResponseDto>> ReleasePayment(VaultPaymentDto vaultPaymentDto, int userId, string userRole)
        {
            string message;
            var paymentResponse = new PaymentResponseDto();

            try
            {
                // get available funds
                decimal availableFunds = await GetAmountInDevice(vaultPaymentDto.BagSerialNumber);

                paymentResponse.AvailableFunds = availableFunds;

                // check if 'Amount to Pay' is above 1 million
                if (vaultPaymentDto.AmountToBePaid > 1000000)
                {
                    message =
                        string.Format(
                            "The amount to be paid cannot be more than R1 Million. Please ask administrator for help!");
                    paymentResponse.ResponseMessage = message;

                    return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
                }

                // check if the device is linked to Vault product
                ProductType productType =
                    await _repository.GetFirstOrDefaultAsync<ProductType>(e => e.LookUpKey == "MYSBV_VAULT");

                bool isLinkedToProduct =
                    await
                        _repository.AnyAsync<Product>(
                            e => e.DeviceId == vaultPaymentDto.DeviceId && e.ProductTypeId == productType.ProductTypeId);

                if (!isLinkedToProduct)
                {
                    message =
                        string.Format(
                            "This device is not linked to mySBV.Vault product type. Please ask administrator for help!");
                    paymentResponse.ResponseMessage = message;
                    paymentResponse.AvailableFunds = 0;

                    return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
                }

                // check if CIT has taken place on the bag in the device
                var cashDeposits = await GetDepositsReleasedByVault(vaultPaymentDto.DeviceId) as List<CashDeposit>;

                if (cashDeposits != null)
                {
                    // try to get the deposit in a container.
                    List<Container> containers = cashDeposits.SelectMany(e => e.Containers).ToList();

                    // get deposit in vault
                    Container depositContainer =
                        containers.FirstOrDefault(e => e.SerialNumber == vaultPaymentDto.BagSerialNumber &&
                                                       !string.IsNullOrWhiteSpace(vaultPaymentDto.BagSerialNumber));

                    if (depositContainer == null)
                    {
                        message = string.Format(
                            "There is no bag in the device {0}. Payment can only happen when you have a bag in the device with at least one deposit made!",
                            vaultPaymentDto.DeviceSerialNumber);
                        paymentResponse.ResponseMessage = message;
                        paymentResponse.AvailableFunds = 0;

                        return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
                    }

                    // no payment is done after cit happens
                    bool hasCitRun = containers.Any(e => e.SerialNumber == vaultPaymentDto.BagSerialNumber &&
                                                         depositContainer.CashDeposit.CitDateTime != null);

                    if (hasCitRun)
                    {
                        message =
                            string.Format(
                                "The bag with serial number {0} is closed, you cannot release payment after CIT has taken place!",
                                vaultPaymentDto.BagSerialNumber);
                        paymentResponse.ResponseMessage = message;
                        paymentResponse.AvailableFunds = 0;

                        return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
                    }
                    
                    // prepare to release payment.
                    // all validation passed at this point

                    if (availableFunds >= vaultPaymentDto.AmountToBePaid && availableFunds > 0)
                    {
                        int statusId = await _lookup.GetStatusIdAsync("PAY_UNCONFIRMED");

                        var user = _repository.Find<User>(userId);

                        Device device = await _repository.GetFirstOrDefaultAsync<Device>(e => e.DeviceId == vaultPaymentDto.DeviceId);

                        // push payment details to the vault partial payment table.
                        var vaultPartialPayment = new VaultPartialPayment
                        {
                            StatusId = statusId,
                            DeviceSerialNumber = device.SerialNumber,
                            PaymentReference = vaultPaymentDto.PaymentReference,
                            TransactionDate = DateTime.Now,
                            BagSerialNumber = vaultPaymentDto.BagSerialNumber,
                            CitCode = vaultPaymentDto.CitCode,
                            BeneficiaryCode = vaultPaymentDto.BeneficiaryCode,
                            TotalToBePaid = vaultPaymentDto.AmountToBePaid,
                            DeviceUserName = user.UserName,
                            DeviceUserRole = userRole,
                            SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Vaultpayments, _repositoryResult),
                            IsNotDeleted = true,
                            LastChangedById = user.UserId,
                            CreatedById = user.UserId,
                            LastChangedDate = DateTime.Now,
                            CreateDate = DateTime.Now,
                            EntityState = State.Added
                        };

                        // update Vault amount in the cash deposit table.

                        CashDeposit cashDeposit = depositContainer.CashDeposit;

                        cashDeposit.VaultAmount = await GetAmountInDevice(vaultPaymentDto.BagSerialNumber) - vaultPaymentDto.AmountToBePaid;
                        cashDeposit.EntityState = State.Modified;

                        // pay 
                        bool isUpdated = await _repository.UpdateAsync(vaultPartialPayment) > 0;

                        // update cash deposit
                        _repository.Update(cashDeposit);

                        if (isUpdated)
                        {
                            message = string.Format("You have successfully requested the following payment:");

                            paymentResponse.AvailableFunds = availableFunds - vaultPaymentDto.AmountToBePaid;
                            paymentResponse.ErrorEncountered = false;
                            paymentResponse.VaultPartialPaymentId = vaultPartialPayment.VaultPartialPaymentId;
                            paymentResponse.ResponseMessage = message;

                            return new MethodResult<PaymentResponseDto>(MethodStatus.Successful, paymentResponse,
                                message);
                        }

                        message = string.Format("Failed to make a payment, please ask administrator for help!");
                        paymentResponse.ResponseMessage = message;

                        return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
                    }
                    message = string.Format("You do not have sufficient funds to make a payment from Container {0}",
                        depositContainer.SerialNumber);
                    paymentResponse.ResponseMessage = message;

                    return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
                }
                message = string.Format("Payment cannot happen on a device that has not released any deposits!");
                paymentResponse.ResponseMessage = message;
                return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal(string.Format("Exception On Method : [VAULT_WEB_PAYMENT]\n[{0}]\nStacktrace\n", ex.Message),
                        ex);

                message = string.Format("An error happened while making a payment, please ask administrator for help!");
                paymentResponse.ResponseMessage = message;

                return new MethodResult<PaymentResponseDto>(MethodStatus.Error, paymentResponse, message);
            }
        }

        /// <summary>
        ///     Build payment model.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isRetailSupervisor"></param>
        /// <returns></returns>
        public async Task<PaymentModel> GeneratePaymentModel(User user, bool isRetailSupervisor)
        {
            List<Merchant> merchants;

            if (isRetailSupervisor)
            {
                // get Supervisor site
                Site site = _lookup.GetMerchantUserSiteByUsername(user);

                // only merchants serviced on SITE can be viewed by the Retail Supervisor.
                merchants = await GetMerchants(site.SiteId);
            }
            else
            {
                // enumerate all merchants, the SBVAdmin can view all merchants.
                merchants = await GetAllMerchants();
            }

            var mappedMerchants = new List<MerchantDto>();

            foreach (Merchant merchant in merchants)
            {
                MerchantDto mapped = _mapper.Map<Merchant, MerchantDto>(merchant);

                mapped.CompanyType = null;
                mapped.MerchantDescription = null;
                mapped.Sites = null;

                mappedMerchants.Add(mapped);
            }

            var model = new PaymentModel
            {
                Payment = new VaultPaymentDto
                {
                    TransactionDate = DateTime.Now.ToShortDateString(),
                    AvailableFunds = Math.Round(0M, 2)
                },
                Merchants = mappedMerchants,
            };

            return model;
        }

        /// <summary>
        ///     get site devices
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<DeviceDto>> GetSiteDevices(int siteId)
        {
            IEnumerable<Device> devices = (from p in await _repository.GetAsync<Product>(e => e.SiteId == siteId)
                join d in await _repository.AllAsync<Device>() on p.DeviceId equals d.DeviceId
                join dt in await _repository.AllAsync<DeviceType>() on d.DeviceTypeId equals dt.DeviceTypeId
                join s in await _repository.AllAsync<Supplier>() on dt.SupplierId equals s.SupplierId
                where s.LookUpKey == "GREYSTONE"
                select d);

            var mappedDevices = new List<DeviceDto>
            {
                new DeviceDto
                {
                    DeviceId = 0,
                    Name = "Please select..."
                }
            };

            foreach (Device device in devices)
            {
                DeviceDto mapped = _mapper.Map<Device, DeviceDto>(device);
                mapped.Name += ", SN: " + mapped.SerialNumber;
                mappedDevices.Add(mapped);
            }

            return mappedDevices;
        }


        /// <summary>
        ///     Get Sites by Merchant
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public async Task<List<SiteDto>> GetSites(int merchantId)
        { 
            var enumeratedSites = (from pt in await _repository.AllAsync<ProductType>()
                join p in await _repository.AllAsync<Product>() on pt.ProductTypeId equals p.ProductTypeId
                                  join s in await _repository.AllAsync<Site>(
                                  o => o.Status,
                                  o => o.Address,
                                  o => o.Address.AddressType,
                                  o => o.Accounts,
                                  o => o.Accounts.Select(x => x.AccountType),
                                  o => o.Accounts.Select(x => x.TransactionType),
                                  o => o.Accounts.Select(x => x.Bank),
                                  o => o.SiteContainers,
                                  o => o.CashCenter,
                                  o => o.Merchant,
                                  o => o.City,
                                  o => o.CashCenter,
                                  o => o.CitCarrier) on p.SiteId equals s.SiteId
                where pt.LookUpKey == "MYSBV_VAULT" && s.MerchantId == merchantId 
                select s)
                .Distinct()
                .ToList();

            var sites = new List<SiteDto>();

            foreach (Site site in enumeratedSites)
            {
                SiteDto mapped = _mapper.Map<Site, SiteDto>(site);

                mapped.Address = null;
                mapped.SiteContainers = null;
                mapped.Accounts = null;
                sites.Add(mapped);
            }

            return sites.OrderBy(a => a.Name).ToList();
        }

        /// <summary>
        ///     get all accounts on site
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<List<AccountDto>> GetSiteAccounts(int siteId)
        {
            // accounts
            var accounts = new List<AccountDto>();

            IEnumerable<Account> accountsEnumeration = await _repository.GetAsync<Account>(e => e.SiteId == siteId,
                o => o.Bank,
                o => o.AccountType,
                o => o.Site,
                o => o.Site.Merchant,
                o => o.Site.City,
                o => o.Site.CitCarrier,
                o => o.Site.CashCenter,
                o => o.Site.Address,
                o => o.Site.SiteContainers,
                o => o.Site.CashCenter,
                o => o.Site.Address.AddressType,
                o => o.Site.Accounts,
                o => o.Site.Accounts.Select(x => x.AccountType),
                o => o.Site.Accounts.Select(x => x.TransactionType),
                o => o.Site.Accounts.Select(x => x.Bank));

            var statusId = await _lookup.GetStatusIdAsync("ACTIVE");

            var collection = accountsEnumeration.Where(e => e.StatusId == statusId);

            foreach (var item in collection)
            {
                AccountDto mapped = _mapper.Map<Account, AccountDto>(item);
                mapped.Bank = null;
                mapped.Site = null;

                accounts.Add(mapped);
            }

            return accounts;
        }

        /// <summary>
        ///     get site information
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public async Task<SiteDto> GetSiteInfo(int siteId)
        {
            Site site = await _repository.GetFirstOrDefaultAsync<Site>(e => e.SiteId == siteId,
                o => o.Status,
                o => o.Address,
                o => o.Address.AddressType,
                o => o.Accounts,
                o => o.Accounts.Select(x => x.AccountType),
                o => o.Accounts.Select(x => x.TransactionType),
                o => o.Accounts.Select(x => x.Bank),
                o => o.SiteContainers,
                o => o.CashCenter,
                o => o.Merchant,
                o => o.City,
                o => o.CashCenter,
                o => o.CitCarrier) ?? new Site
                {
                    Accounts = new Collection<Account>()
                };

            SiteDto mapped = _mapper.Map<Site, SiteDto>(site);
            mapped.IsDefaultReference = true;

            mapped.Accounts = await GetSiteAccounts(siteId);
            return mapped;
        }

        /// <summary>
        /// Get open bag number in device.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<string> GetOpenBagNumber(int deviceId)
        {
            IEnumerable<ContainerDrop> containerDrops = await GetContainerDrops(deviceId);

            ContainerDrop containerDrop = containerDrops.FirstOrDefault();
            string serialNumber = containerDrop != null ? containerDrop.Container.SerialNumber : string.Empty;
            return serialNumber;
        }

        #endregion

        #region Helpers

        public async Task<User> GetLoggedUser(string username)
        {
            IEnumerable<User> user =
                await _repository.GetAsync<User>(a => a.UserName.ToLower() == username.ToLower(),
                    u => u.UserType,
                    o => o.UserSites,
                    m => m.UserNotifications,
                    k => k.UserSites.Select(e => e.Site),
                    z => z.UserSites.Select(e => e.Site.SiteContainers),
                    v => v.UserSites.Select(p => p.Site.CitCarrier));

            User currentuser = user.FirstOrDefault();
            return currentuser;
        }

        public async Task<string> GetSiteContactPersonEmail(string citCode)
        {
            IEnumerable<Site> contactPersonEmail =
                await _repository.GetAsync<Site>(a => a.CitCode == citCode);

            Site email = contactPersonEmail.FirstOrDefault();
            return email != null ? email.ContactPersonEmailAddress1 : string.Empty;
        }

        /// <summary>
        ///     get all merchants.
        /// </summary>
        /// <returns></returns>
        private async Task<List<Merchant>> GetAllMerchants()
        {
            // merchants
            var merchants = (from pt in await _repository.AllAsync<ProductType>()
                join p in await _repository.AllAsync<Product>() on pt.ProductTypeId equals p.ProductTypeId
                join s in await _repository.AllAsync<Site>() on p.SiteId equals s.SiteId
                join m in await _repository.AllAsync<Merchant>(o => o.CompanyType,
                    o => o.MerchantDescription,
                    o => o.Status,
                    o => o.Sites,
                    o => o.Sites.Select(e => e.Address),
                    o => o.Sites.Select(e => e.Accounts),
                    o => o.Sites.Select(e => e.Accounts.Select(x => x.AccountType)),
                    o => o.Sites.Select(e => e.Accounts.Select(x => x.TransactionType)),
                    o => o.Sites.Select(e => e.Accounts.Select(x => x.Bank)),
                    o => o.Sites.Select(e => e.SiteContainers),
                    o => o.Sites.Select(e => e.CashCenter),
                    o => o.Sites.Select(e => e.Address.AddressType)) on s.MerchantId equals m.MerchantId
                where pt.LookUpKey == "MYSBV_VAULT"
                select m)
                .OrderBy(x => x.Name)
                .Distinct()
                .ToList();

            return merchants;
        }

        /// <summary>
        ///     get merchants that are linked to site.
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        private async Task<List<Merchant>> GetMerchants(int siteId)
        {
            var merchants = (from pt in await _repository.AllAsync<ProductType>()
                join p in await _repository.AllAsync<Product>() on pt.ProductTypeId equals p.ProductTypeId
                join s in await _repository.AllAsync<Site>() on p.SiteId equals s.SiteId
                join m in await _repository.AllAsync<Merchant>(o => o.CompanyType,
                    o => o.MerchantDescription,
                    o => o.Status,
                    o => o.Sites,
                    o => o.Sites.Select(e => e.Address),
                    o => o.Sites.Select(e => e.Accounts),
                    o => o.Sites.Select(e => e.Accounts.Select(x => x.AccountType)),
                    o => o.Sites.Select(e => e.Accounts.Select(x => x.TransactionType)),
                    o => o.Sites.Select(e => e.Accounts.Select(x => x.Bank)),
                    o => o.Sites.Select(e => e.SiteContainers),
                    o => o.Sites.Select(e => e.CashCenter),
                    o => o.Sites.Select(e => e.Address.AddressType)) on s.MerchantId equals m.MerchantId
                where pt.LookUpKey == "MYSBV_VAULT" && s.SiteId == siteId
                select m)
                .OrderBy(x => x.Name)
                .Distinct()
                .ToList();

            return merchants;
        }

        /// <summary>
        ///     Get Container drops
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<ContainerDrop>> GetContainerDrops(int deviceId)
        {
            var cashDeposits = await _repository.GetAsync<CashDeposit>(o => o.DeviceId == deviceId && o.CitDateTime == null,
                                        o => o.Status,
                                        o => o.Containers,
                                        o => o.Containers.Select(d => d.ContainerType),
                                        o => o.Containers.Select(d => d.ContainerType.ContainerTypeAttributes),
                                        o => o.Account,
                                        o => o.DepositType,
                                        o => o.Device,
                                        o => o.ErrorCode,
                                        o => o.ProductType,
                                        o => o.Site,
                                        o => o.Site.SiteContainers,
                                        o => o.Site.Address,
                                        o => o.Site.Address.AddressType,
                                        o => o.Status,
                                        o => o.Site.Merchant,
                                        o => o.Site.Accounts,
                                        o => o.Site.Accounts.Select(b => b.Bank),
                                        o => o.Site.Accounts.Select(b => b.TransactionType),
                                        o => o.Site.Accounts.Select(b => b.AccountType),
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.Status)),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems.Select(f => f.Denomination)))
                        );

            var containers = cashDeposits.SelectMany(e => e.Containers);
            var drops = containers.SelectMany(e => e.ContainerDrops).ToList(); 

            var containerDrops = (from deposit in
                    await _repository.GetAsync<CashDeposit>(e => e.DeviceId == deviceId && e.CitDateTime == null)
                    join container in await _repository.AllAsync<Container>() on deposit.CashDepositId equals
                        container.CashDepositId
                    join drop in await _repository.AllAsync<ContainerDrop>(o => o.Container) on container.ContainerId
                        equals drop.ContainerId
                    select drop).AsEnumerable();

            return containerDrops;
        }


        /// <summary>
        ///     deposits released by device
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        private async Task<IEnumerable<CashDeposit>> GetDepositsReleasedByVault(int deviceId)
        {
            IEnumerable<CashDeposit> cashDeposits = await _repository.GetAsync<CashDeposit>(e => e.DeviceId == deviceId,
                o => o.Status,
                o => o.Containers,
                o => o.Containers.Select(d => d.ContainerType),
                o => o.Containers.Select(d => d.ContainerType.ContainerTypeAttributes),
                o => o.Account,
                o => o.DepositType,
                o => o.Device,
                o => o.ErrorCode,
                o => o.ProductType,
                o => o.Site,
                o => o.Site.SiteContainers,
                o => o.Site.Address,
                o => o.Site.Address.AddressType,
                o => o.Status,
                o => o.Site.Merchant,
                o => o.Site.Accounts,
                o => o.Site.Accounts.Select(b => b.Bank),
                o => o.Site.Accounts.Select(b => b.TransactionType),
                o => o.Site.Accounts.Select(b => b.AccountType),
                o => o.Account.Bank,
                o => o.Containers.Select(p => p.ContainerDrops),
                o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)),
                o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.Status)),
                o =>
                    o.Containers.Select(
                        p => p.ContainerDrops.Select(b => b.ContainerDropItems.Select(f => f.Denomination))));
            return cashDeposits.ToList();
        }

        #endregion
    }
}