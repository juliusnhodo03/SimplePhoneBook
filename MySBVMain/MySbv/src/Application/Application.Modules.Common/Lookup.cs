using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Security;
using Application.Dto.CashOrder;
using Domain.Data.Model;
using Domain.Repository;
using Task = Domain.Data.Model.Task;
using System.IO;

namespace Application.Modules.Common
{
    [Export(typeof (ILookup))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class Lookup : ILookup
    {
        #region Fields

        private IEnumerable<AccountType> _accountType;
        private IEnumerable<AddressType> _addressTypes;
        private IEnumerable<City> _cities;
        private IEnumerable<CompanyType> _companyType;
        private IEnumerable<ContainerType> _containerTypes;
        private IEnumerable<DenominationType> _denominationTypes;
        private IEnumerable<Denomination> _denominations;
        private IEnumerable<Status> _depositStatuses;
        private IEnumerable<DepositType> _depositTypes;
        private IEnumerable<ErrorCode> _errorCodes;
        private IEnumerable<Fee> _fees;
        private IEnumerable<Manufacturer> _manufacturers;
        private IEnumerable<MerchantDescription> _merchantDescription;
        private IEnumerable<ProductType> _productTypes;
        private IEnumerable<DiscrepancyReason> _reasons;
        private IEnumerable<Report> _reports;
        public IRepository _repository;
        public IAsyncRepository _asyncRepository;
        private IEnumerable<RoleReport> _roleReport;
        private IEnumerable<ServiceType> _serviceTypes;
        private IEnumerable<SettlementType> _settlementTypes;
        private IEnumerable<Supplier> _suppliers;
        private IEnumerable<Title> _titles;
        private IEnumerable<TransactionType> _transactionTypes;
        private IEnumerable<VaultTransactionType> _vaultTransactionTypes;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public Lookup(IRepository repository, IAsyncRepository asyncRepository)
        {
            _repository = repository;
            _asyncRepository = asyncRepository;
        }

        #endregion

        #region Properties

        #endregion

        #region ILookup

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lookUpKey"></param>
        public int GetStatusId(string lookUpKey)
        {
            _depositStatuses = _depositStatuses ?? _repository.All<Status>();
            Status firstOrDefault = _depositStatuses.FirstOrDefault(a => a.LookUpKey == (lookUpKey));
            return (firstOrDefault != null) ? firstOrDefault.StatusId : -1;
        }

        public async Task<int> GetStatusIdAsync(string lookUpKey)
        {
            _depositStatuses = _depositStatuses ?? await _asyncRepository.AllAsync<Status>();

            Status firstOrDefault = _depositStatuses.FirstOrDefault(a => a.LookUpKey == (lookUpKey));

            return (firstOrDefault != null) ? firstOrDefault.StatusId : -1;
        }

        public Status GetStatus(string lookUpKey)
        {
            _depositStatuses = _depositStatuses ?? _repository.All<Status>();
            Status firstOrDefault = _depositStatuses.FirstOrDefault(a => a.LookUpKey == (lookUpKey));
            return firstOrDefault ?? null;
        }

        public Status GetStatus(int id)
        {
            _depositStatuses = _depositStatuses ?? _repository.All<Status>();
            Status firstOrDefault = _depositStatuses.FirstOrDefault(a => a.StatusId == id);
            return firstOrDefault ?? null;
        }


        public int GetSupplierId(string lookUpKey)
        {
            _suppliers = _suppliers ?? _repository.All<Supplier>();
            Supplier firstOrDefault = _suppliers.FirstOrDefault(a => a.LookUpKey == lookUpKey);
            return (firstOrDefault != null) ? firstOrDefault.SupplierId : -1;
        }

        public int GetProductTypeId(string lookUpKey)
        {
            _productTypes = _productTypes ?? _repository.All<ProductType>();
            ProductType firstOrDefault = _productTypes.FirstOrDefault(a => a.LookUpKey == lookUpKey);
            return (firstOrDefault != null) ? firstOrDefault.ProductTypeId : -1;
        }

        public VaultTransactionType GetVaultTransactionType(string lookup)
        {
            _vaultTransactionTypes = _vaultTransactionTypes ?? _repository.All<VaultTransactionType>();
            return _vaultTransactionTypes.FirstOrDefault(a => a.LookUpKey == lookup);
        }


        public User GetLoggedUser(string username)
        {
            User user = _repository.Query<User>(a => a.UserName.ToLower() == username.ToLower(),
                u => u.UserType,
                o => o.UserSites,
                m => m.UserNotifications,
                k => k.UserSites.Select(e => e.Site),
                z => z.UserSites.Select(e => e.Site.SiteContainers),
                v => v.UserSites.Select(p => p.Site.CitCarrier)).FirstOrDefault();

            return user;
        }

        public User GetApprover()
        {
            string[] user = Roles.GetUsersInRole("SBVApprover");

            return
                user.Select(s => _repository.Query<User>(a => a.UserName.ToLower() == s.ToLower()).FirstOrDefault())
                    .FirstOrDefault();
        }

        public string GetConfigurationValue(string lookUpKey)
        {
            SystemConfiguration configurationItem =
                _repository.Query<SystemConfiguration>(a => a.LookUpKey == lookUpKey).FirstOrDefault();
            return configurationItem != null ? configurationItem.Value : string.Empty;
        }

        public int GetUserTypeId(string lookUpKey)
        {
            UserType userType = _repository.Query<UserType>(a => a.LookUpKey == lookUpKey).FirstOrDefault();
            return userType != null ? userType.UserTypeId : -1;
        }

        public IEnumerable<Title> Titles()
        {
            _titles = _titles ?? _repository.All<Title>();
            return _titles;
        }

        public IEnumerable<CashCenter> CashCenters()
        {
            return _repository.All<CashCenter>();
        }

        public IEnumerable<Merchant> Merchants()
        {
            Status status = _repository.Query<Status>(a => a.LookUpKey == "ACTIVE").FirstOrDefault();

            IEnumerable<Merchant> merchants = _repository.Query<Merchant>(a => a.StatusId == status.StatusId)
                .OrderBy(e => e.Name);

            return merchants;
        }

        public IEnumerable<Site> Sites()
        {
            Status status = _repository.Query<Status>(a => a.LookUpKey == "ACTIVE").FirstOrDefault();

            IEnumerable<Site> sites = _repository.Query<Site>(a => a.StatusId == status.StatusId, b => b.CitCarrier)
                .OrderBy(x => x.Name)
                .Distinct()
                .ToList();

            return sites;
        }

        public IEnumerable<User> GetAllUsers(int userType)
        {
            IEnumerable<User> users = _repository.Query<User>(a => a.UserTypeId == userType, a => a.Title,
                u => u.UserType, m => m.Merchant, c => c.CashCenter);
            return users;
        }

        public IEnumerable<CashOrderType> GetOrderTypes()
        {
            IEnumerable<CashOrderType> cashOrders = _repository.All<CashOrderType>();
            return cashOrders;
        }

        public IEnumerable<City> GetCities()
        {
            _cities = _cities ?? _repository.All<City>().OrderBy(a => a.Name);
            return _cities;
        }

        public IEnumerable<Cluster> GetRegions()
        {
            return _repository.All<Cluster>();
        }

        public IEnumerable<AddressType> GetAddressTypes()
        {
            _addressTypes = _addressTypes ?? _repository.All<AddressType>();
            return _addressTypes;
        }

        public IEnumerable<City> GetCitiesInProvince(int provinceId)
        {
            IEnumerable<City> cities = _repository.Query<City>(a => a.ProvinceId == provinceId);
            return cities;
        }

        public IEnumerable<MerchantDescription> GetMerchantDescriptions()
        {
            _merchantDescription = _merchantDescription ?? _repository.All<MerchantDescription>();
            return _merchantDescription;
        }

        public IEnumerable<CompanyType> GetCompanyTypes()
        {
            _companyType = _companyType ?? _repository.All<CompanyType>();
            return _companyType;
        }

        public IEnumerable<SettlementType> GetSettlementTypes()
        {
            _settlementTypes = _settlementTypes ?? _repository.All<SettlementType>();
            return _settlementTypes;
        }

        public IEnumerable<CitCarrier> GetCitCarries()
        {
            return _repository.All<CitCarrier>();
        }

        public string GetServerAddress()
        {
            return _repository.Query<SystemConfiguration>(a => a.LookUpKey == "SERVER_ADDRESS")
                .FirstOrDefault()
                .Value;
        }

        public Merchant GetMerchantById(int merchantId)
        {
            return _repository.Find<Merchant>(merchantId);
        }

        public Merchant GetMerchantByUsername(User user)
        {
            return (from u in _repository.All<User>()
                join m in _repository.All<Merchant>() on u.MerchantId equals m.MerchantId
                where u.UserName == user.UserName
                select m).FirstOrDefault();
        }

        public IEnumerable<SettlementType> SettlementTypes()
        {
            return _settlementTypes = _settlementTypes ?? _repository.All<SettlementType>();
        }

        public IEnumerable<ServiceType> ServiceTypes()
        {
            return _serviceTypes = _serviceTypes ?? _repository.All<ServiceType>();
        }

        public IEnumerable<ProductType> ProductTypes()
        {
            return _productTypes = _productTypes ?? _repository.All<ProductType>();
        }

        public Site GetMerchantUserSiteByUsername(User user)
        {
            UserSite site = user.UserSites.FirstOrDefault();
            return (site == null) ? new Site() : site.Site;
        }

        public Dictionary<int, string> GetSiteDepositReference(int siteId)
        {
            var site = _repository.Find<Site>(siteId);

            var siteDictionary = new Dictionary<int, string>();
            if (site != null)
            {
                siteDictionary.Add(site.SiteId, site.DepositReference);
                if (site.DepositReferenceIsEditable)
                {
                    siteDictionary.Add(0, "Custom Deposit Reference");
                }
                return siteDictionary;
            }
            siteDictionary.Add(0, "Please Select");
            return siteDictionary;
        }

        public ContainerTypeAttribute GetContainerTypeAttributes(int containerTypeId)
        {
            ContainerTypeAttribute containerType =
                _repository.Query<ContainerTypeAttribute>(e => e.ContainerTypeId == containerTypeId).FirstOrDefault();
            return containerType;
        }

        public IEnumerable<Supplier> Suppliers()
        {
            return _repository.All<Supplier>();
        }

        public IEnumerable<Fee> Fees()
        {
            _fees = _fees ?? _repository.All<Fee>(f => f.FeeType).ToList();
            return _fees;
        }

        public Merchant GetMerchantBySiteId(int siteId)
        {
            if (siteId <= 0)
            {
                return new Merchant();
            }

            return (from m in _repository.All<Merchant>()
                join s in _repository.All<Site>() on m.MerchantId equals s.MerchantId
                where s.SiteId == siteId
                select m
                ).FirstOrDefault();
        }

        public IEnumerable<ContainerType> GetSiteContainers(int siteId)
        {
            IEnumerable<ContainerType> containerTypes = (from containerType in _repository.All<ContainerType>()
                join siteContainer in _repository.All<SiteContainer>() on containerType.ContainerTypeId equals
                    siteContainer.ContainerTypeId
                where siteContainer.SiteId == siteId
                select containerType);
            return containerTypes;
        }

        public DepositType GetDepositType(string key)
        {
            _depositTypes = _depositTypes ?? _repository.All<DepositType>();
            return _depositTypes.FirstOrDefault(a => a.LookUpKey == key);
        }

        public IEnumerable<ContainerType> GetContainerTypes()
        {
            _containerTypes = _containerTypes ?? _repository.All<ContainerType>();
            return _containerTypes;
        }

        public IEnumerable<DepositType> GetDepositTypes()
        {
            _depositTypes = _depositTypes ?? _repository.All<DepositType>();
            return _depositTypes;
        }

        public IEnumerable<Bank> GetBanksServicedBySite(int siteId)
        {
            if (siteId <= 0)
            {
                return new List<Bank>();
            }

            IEnumerable<Bank> banks =
                _repository.Query<Account>(e => e.SiteId == siteId, o => o.Bank)
                .Select(p => p.Bank)
                .OrderBy(e => e.Name);

            return banks.ToList().Distinct();
        }

        public IEnumerable<Account> GetSiteSettlementAccounts(int bankId, int siteId)
        {
            Status status = _repository.Query<Status>(e => e.LookUpKey == "ACTIVE").FirstOrDefault();

            IEnumerable<Account> siteAccounts = _repository.Query<Account>(e =>
                e.BankId == bankId &&
                e.SiteId == siteId &&
                e.StatusId == status.StatusId, b => b.Bank)
                .OrderBy(e => e.AccountNumber);

            return siteAccounts.ToList().Distinct();
        }

        public IEnumerable<Merchant> GetMerchantsServicedByCashCenter(User user)
        {
            IEnumerable<Site> sites =
                _repository.Query<Site>(
                    a => a.CashCenterId == user.CashCenterId && a.IsCashCentreAllowedDepositCapturing, a => a.Merchant);
            IEnumerable<Merchant> merchants = sites.Select(b => b.Merchant).Distinct();
            return merchants.ToList();
        }

        public IEnumerable<Site> GetSitesForRetailUsers(User user)
        {
            IEnumerable<UserSite> userSites = _repository.Query<UserSite>(o => o.UserId == user.UserId,
                v => v.Site,
                k => k.Site.CitCarrier);

            return userSites.Select(o => o.Site);
        }

        public IEnumerable<ContainerType> GetSiteContainerTypesBySiteId(int siteId)
        {
            return (from sc in _repository.All<SiteContainer>()
                join ct in _repository.All<ContainerType>()
                    on sc.ContainerTypeId equals ct.ContainerTypeId
                where sc.SiteId == siteId
                select ct
                ).ToList();
        }

        public int GetCitInitialDigit(int siteId)
        {
            if (siteId <= 0)
            {
                return 0;
            }
            return
                _repository.Query<Site>(o => o.SiteId == siteId, u => u.CitCarrier)
                    .FirstOrDefault()
                    .CitCarrier.SerialStartNumber;
        }

        public IEnumerable<Device> Devices()
        {
            return _repository.All<Device>();
        }

        public IEnumerable<DeviceType> DeviceTypes()
        {
            return _repository.All<DeviceType>();
        }

        public IEnumerable<Manufacturer> Manufactures()
        {
            _manufacturers = _manufacturers ?? _repository.All<Manufacturer>();
            return _manufacturers;
        }

        public IEnumerable<Bank> GetBanks()
        {
            return _repository.All<Bank>();
        }

        public User GetUser(string username)
        {
            return _repository.Query<User>(a => a.UserName.ToLower() == username.ToLower()).FirstOrDefault();
        }

        public User GetUserById(int userId)
        {
            return _repository.Query<User>(a => a.UserId == userId).FirstOrDefault();
        }

        public User SubmittedByUser(int accountId)
        {
            Task tempTask = _repository.Query<Task>(a => a.AccountId == accountId
                ).FirstOrDefault(a => a.AccountId == accountId);

            return _repository.Query<User>(a => a.UserId == tempTask.UserId).FirstOrDefault();
        }


        public IEnumerable<AccountType> GetAccountTypes()
        {
            _accountType = _accountType ?? _repository.All<AccountType>();
            return _accountType;
        }

        public IEnumerable<Denomination> Denominations()
        {
            _denominations = _denominations ?? _repository.All<Denomination>(o => o.DenominationType);
            return _denominations;
        }

        public Bank GetBankCode(int bankId)
        {
            return _repository.Find<Bank>(bankId);
        }

        public Site GetCitCode(int siteId)
        {
            return _repository.Find<Site>(siteId);
        }

        public int GetErrorCodeId(string errorCode)
        {
            _errorCodes = _errorCodes ?? _repository.All<ErrorCode>();
            ErrorCode code = _errorCodes.FirstOrDefault(e => e.Code == errorCode);
            if (code != null)
                return code.ErrorCodeId;
            return -1;
        }

        public Fee GetFee(string code)
        {
            _fees = _fees ?? _repository.All<Fee>();
            return _fees.FirstOrDefault(e => e.Code.ToLower() == code.ToLower());
        }

        public Site GetSite(int siteId)
        {
            return _repository.Find<Site>(siteId);
        }

        public List<Report> GetReports(string username)
        {
            _reports = _reports ?? _repository.All<Report>();
            _roleReport = _roleReport ?? _repository.All<RoleReport>();

            User user = _repository.Query<User>(a => a.UserName == username).FirstOrDefault();
            string query = string.Format("SELECT * FROM [dbo].[udf_UserRoleIds] ({0})", user.UserId);
            List<int> listOfRoleIds = _repository.ExecuteQueryCommand(query);

            IEnumerable<Report> reports = (from roleReport in _roleReport
                join report in _reports on roleReport.ReportId equals report.ReportId
                where listOfRoleIds.Contains(roleReport.RoleId)
                select report);

            return reports.Distinct().ToList();
        }

        public Report GetReportById(int reportId)
        {
            return _reports.FirstOrDefault(a => a.ReportId == reportId);
        }

        public IEnumerable<DiscrepancyReason> DiscrepancyReasons()
        {
            _reasons = _reasons ?? _repository.All<DiscrepancyReason>();
            return _reasons;
        }

        public IEnumerable<DenominationType> DenominationTypes()
        {
            _denominationTypes = _denominationTypes ?? _repository.All<DenominationType>();
            return _denominationTypes;
        }

        public IEnumerable<Denomination> GetDenominations(string denominationTypeName)
        {
            return Denominations().Where(e => e.DenominationTypeName == denominationTypeName);
        }


        public async Task<List<Denomination>> GetDenominationsAsync(string denominationType)
        {
            var denominations = await _asyncRepository.GetAsync<Denomination>(
                e => e.DenominationType.Name==denominationType, e => e.DenominationType);
            return denominations.ToList();
        }

        public IEnumerable<Merchant> GetMerchantsByTeller(User user)
        {
            var merchants = new List<Merchant>();

            if (user.UserType.LookUpKey == "SBV_USER")
            {
                // for cash center users
                merchants = _repository.Query<Site>(a => a.CashCenterId == user.CashCenterId, o => o.Merchant)
                    .Select(b => b.Merchant)
                    .Distinct()
                    .ToList();
            }
            else if (user.UserType.LookUpKey == "MERCHANT_USER")
            {
                // for MERCHANT_USER
                merchants = _repository.Query<UserSite>(a => a.UserId == user.UserId, o => o.Site, o => o.Site.Merchant)
                    .Select(b => b.Site.Merchant)
                    .OrderBy(x => x.Name)
                    .Distinct()
                    .ToList();
            }
            else if (user.UserType.LookUpKey == "HEAD_OFFICE_USER")
            {
                // for HEAD_OFFICE_USER
                merchants = _repository.All<Merchant>()
                    .OrderBy(x => x.Name)
                    .Distinct()
                    .ToList();
            }
            return merchants.ToList();
        }

        public List<string> GetEmailAddresses(int siteId, User user)
        {
            var recipients = new List<string> {user.EmailAddress};

            if (user.UserType.LookUpKey == "MERCHANT_USER")
            {
                CashCenter cashCenter = _repository.Query<Site>
                    (a => a.SiteId == siteId,
                        o => o.CashCenter,
                        o => o.CitCarrier).Select(b => b.CashCenter
                    ).FirstOrDefault();

                if (cashCenter == null) return recipients;

                if (cashCenter.EmailAddress1 != null)
                {
                    recipients.Add(cashCenter.EmailAddress1);
                }

                if (cashCenter.EmailAddress2 != null)
                {
                    recipients.Add(cashCenter.EmailAddress2);
                }

                if (cashCenter.EmailAddress3 != null)
                {
                    recipients.Add(cashCenter.EmailAddress3);
                }
            }
            else
            {
                Site site = _repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault();

                if (site == null) return recipients;


                if (site.ContactPersonEmailAddress1 != null)
                {
                    recipients.Add(site.ContactPersonEmailAddress1);
                }

                if (site.ContactPersonEmailAddress2 != null)
                {
                    recipients.Add(site.ContactPersonEmailAddress2);
                }
            }
            return recipients;
        }

        public List<TransactionType> GetTransactionTypes()
        {
            _transactionTypes = _transactionTypes ?? _repository.All<TransactionType>();
            return _transactionTypes.ToList();
        }

        public Account GetDefaultAccount(int siteId)
        {
            IEnumerable<Account> accounts = _repository.Query<Account>(a => a.SiteId == siteId);
            return accounts.FirstOrDefault(a => a.DefaultAccount);
        }


        /// <summary>
        ///     Get Sites by Merchant
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        public IEnumerable<Site> GetSites(int merchantId)
        {
            return Sites().Where(o => o.MerchantId == merchantId)
                .OrderBy(x => x.Name)
                .Distinct()
                .ToList();
        }

        public IEnumerable<Site> GetCashCenterTellerAllowedSites(int merchantId)
        {
            return Sites().Where(o => o.MerchantId == merchantId && o.IsCashCentreAllowedDepositCapturing);
        }

        public IEnumerable<ContainerDrop> GetContainerDropsByDate()
        {
            return _repository.All<ContainerDrop>(a => (a.CreateDate.Value.Minute - 15) == DateTime.Now.Minute);
        }

        #endregion

        #region Generate Transaction Number

        private string GetRandomCharacters()
        {
            var guid = Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();

            while (guid.StartsWith("0"))
            {
                guid = Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
            }
            return guid;
        }

        private string Generate(string citCode)
        {
            var date = DateTime.Now;

            string depositDate = string.Concat(date.Year.ToString().Substring(2, 2),
                date.Month.ToString().PadLeft(2, '0'),
                date.Day.ToString().PadLeft(2, '0'));

            string transactionReferenceNumber = string.Concat(GetRandomCharacters(),
                citCode, depositDate);

            return transactionReferenceNumber;
        }

        /// <summary>
        /// generate transaction reference number
        /// </summary>
        /// <param name="siteId"></param>
        public string GenerateTransactionNumber(int siteId)
        {
            var citCode = _repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault().CitCode;
            var transactionReferenceNumber = Generate(citCode);

            while (true)
            {
                if (!_repository.Any<CashDeposit>(a => a.TransactionReference == transactionReferenceNumber && a.IsNotDeleted))
                {
                    return transactionReferenceNumber;
                }
                transactionReferenceNumber = Generate(citCode);
            }
        }

        /// <summary>
        /// generate bag reference number
        /// </summary>
        /// <param name="siteId"></param>
        public string GenerateTransactionNumberForContainer(int siteId)
        {
            string citCode = _repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault().CitCode;
            string transactionReferenceNumber = Generate(citCode);

            while (true)
            {
                if (!_repository.Any<Container>(a => a.ReferenceNumber == transactionReferenceNumber && a.IsNotDeleted))
                {
                    return transactionReferenceNumber;
                }
                transactionReferenceNumber = Generate(citCode);
            }
        }

        /// <summary>
        /// Check if Order type is EFT
        /// </summary>
        /// <param name="cashOrderTypeId"></param>
        public bool IsEft(int cashOrderTypeId) 
        {
            var orderType = _repository.Find<CashOrderType>(cashOrderTypeId);
            return orderType.LookUpKey.ToUpper() == "EFT";
        }

        /// <summary>
        /// Generate container drop reference number
        /// </summary>
        /// <param name="siteId"></param>
        public string GenerateTransactionNumberForDrop(int siteId)
        {
            var citCode = _repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault().CitCode;
            var referenceNumber = Generate(citCode);

            while (true)
            {
                if (!_repository.Any<ContainerDrop>(a => a.ReferenceNumber == referenceNumber && a.IsNotDeleted))
                {
                    return referenceNumber;
                }
                referenceNumber = Generate(citCode);
            }
        }


        /// <summary>
        /// Create Physical Folder
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <param name="userName"></param>
        public string CreateTemporaryFolder(int cashOrderId, string userName)
        {
            var rootLocationUrl = GetConfigurationValue("CASH_ORDER_EFT_ATTACHMENTS_URL");

            var folder = (cashOrderId == 0) ? userName : cashOrderId.ToString();

            var physicalPath = Path.Combine(rootLocationUrl, folder);

            var exists = Directory.Exists(physicalPath);

            // Check if Directory Exists
            if (!Directory.Exists(physicalPath))
            {
                // Create Directory
               // Directory.CreateDirectory(physicalPath);
            }

            return physicalPath;
        }


        /// <summary>
        /// Get a list of email addresses for the sbv Finance Reviewer
        /// </summary>
        public List<string> GetFinanceReviewerEmail()
        {
            var collection = new List<string>();

            const string sql = @"    
                            SELECT DISTINCT U.*
                            FROM webpages_Roles R INNER JOIN
                                 webpages_UsersInRoles UR ON R.RoleId = UR.RoleId INNER JOIN
	                             [User] U ON U.UserId = UR.UserId
                            WHERE R.RoleName = 'SBVFinanceReviewer'";

            var endUsers = _repository.ExecuteQueryCommand<User>(sql);

            foreach (var endUser in endUsers)
            {
                if (string.IsNullOrWhiteSpace(endUser.EmailAddress) == false)
                {
                    collection.Add(endUser.EmailAddress);
                }
            }
            return collection;
        }


        #endregion
    }
}