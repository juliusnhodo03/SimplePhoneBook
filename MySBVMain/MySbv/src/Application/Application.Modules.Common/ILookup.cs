
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading.Tasks;
using Application.Dto.CashOrder;
using Domain.Data.Model;

namespace Application.Modules.Common
{
    public interface ILookup
    {
        /// <summary>
        ///     Get the Status id of the status specified by the lookUpKey
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        Task<int> GetStatusIdAsync(string lookUpKey);

        /// <summary>
        ///     Get the Status id of the status specified by the lookUpKey
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        int GetStatusId(string lookUpKey);

        /// <summary>
        /// get logged in user
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        User GetLoggedUser(string username);

		/// <summary>
		///     Get the Status id of the status specified by the lookUpKey
		/// </summary>
		/// <param name="lookUpKey"></param>
		/// <returns></returns>
		Status GetStatus(string lookUpKey);
		/// <summary>
		///     Get the Status id of the status specified by the lookUpKey
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Status GetStatus(int id);

        /// <summary>
        /// Get the Supplier ID using the Lookup Key
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        int GetSupplierId(string lookUpKey);


        /// <summary>
        /// Get Product Type by LookUpKey
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        int GetProductTypeId(string lookUpKey);

         
        /// <summary>
        /// Get Vault Transaction Type using the lookup key
        /// </summary>
        /// <param name="lookup"></param>
        /// <returns></returns>
        VaultTransactionType GetVaultTransactionType(string lookup);

        /// <summary>
        ///     Get the value of a configuration specified by the lookUpKey
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        string GetConfigurationValue(string lookUpKey);

        /// <summary>
        /// Get an Id of the user type specified by the lookUpKey
        /// </summary>
        /// <param name="lookUpKey"></param>
        /// <returns></returns>
        int GetUserTypeId(string lookUpKey);

        /// <summary>
        ///     Get Titles
        /// </summary>
        /// <returns></returns>
        IEnumerable<Title> Titles();

        /// <summary>
        ///     Get Cash Centers
        /// </summary>
        /// <returns></returns>
        IEnumerable<CashCenter> CashCenters();

        /// <summary>
        ///     Get Merchants
        /// </summary>
        /// <returns></returns>
        IEnumerable<Merchant> Merchants();

        /// <summary>
        ///     Get Sites
        /// </summary>
        /// <returns></returns>
        IEnumerable<Site> Sites();

        /// <summary>
        /// Get All users filtered by the user type
        /// </summary>
        /// <param name="userType"></param>
        IEnumerable<User> GetAllUsers(int userType);

		/// <summary>
		/// Get All CashOrders
		/// </summary>
		IEnumerable<CashOrderType> GetOrderTypes();

		/// <summary>
		/// Get All Denominations filtered by denomination Type Name
		/// </summary>
		/// <param name="denominationTypeName"></param>
		IEnumerable<Denomination> GetDenominations(string denominationTypeName);

		/// <summary>
		/// get Merchants by Teller
		/// </summary>
		/// <param name="user"></param>
		IEnumerable<Merchant> GetMerchantsByTeller(User user);


		/// <summary>
		/// get sites by merchantId
		/// </summary>
		/// <param name="merchantId"></param>
		IEnumerable<Site> GetSites(int merchantId);


		/// <summary>
		/// get sites allowed for cash center by merchantId
		/// </summary>
		/// <param name="merchantId"></param>
		IEnumerable<Site> GetCashCenterTellerAllowedSites(int merchantId);

        /// <summary>
        ///     Get Cities
        /// </summary>
        /// <returns></returns>
        IEnumerable<City> GetCities();

        /// <summary>
        /// Get a report by its Id
        /// </summary>
        /// <returns>a report object</returns>
        User GetApprover();

        /// <summary>
        /// Get Regions (Clusters)
        /// </summary>
        /// <returns></returns>
        IEnumerable<Cluster> GetRegions();

        /// <summary>
        /// Get Address Types
        /// </summary>
        /// <returns></returns>
        IEnumerable<AddressType> GetAddressTypes();

        /// <summary>
        /// Get All Cities filtered by the user type
        /// </summary>
        /// <param name="provinceId"></param>
        IEnumerable<City> GetCitiesInProvince(int provinceId);

        /// <summary>
        /// Get Merchant Descriptions
        /// </summary>
        /// <returns></returns>
        IEnumerable<MerchantDescription> GetMerchantDescriptions();

        /// <summary>
        /// Get Company Type
        /// </summary>
        /// <returns></returns>
        IEnumerable<CompanyType> GetCompanyTypes();

        /// <summary>
        /// Get Settlement types
        /// </summary>
        /// <returns></returns>
        IEnumerable<SettlementType> SettlementTypes();

        /// <summary>
        /// Get Service Types
        /// </summary>
        /// <returns></returns>
        IEnumerable<ServiceType> ServiceTypes();

        /// <summary>
        /// Get Settlement Types
        /// </summary>
        /// <returns></returns>
        IEnumerable<SettlementType> GetSettlementTypes();

        /// <summary>
        /// Get Cit Carriers
        /// </summary>
        /// <returns></returns>
        IEnumerable<CitCarrier> GetCitCarries();

        /// <summary>
        /// Returns the Server Address
        /// </summary>
        /// <returns></returns>
        string GetServerAddress();

        /// <summary>
        /// Get Merchant By MerchantId
        /// </summary>
        /// <returns></returns>
        Merchant GetMerchantById(int merchantId);

        /// <summary>
        /// Get User information from the UserName
        /// <param name="username"></param>
        /// </summary>
        /// <returns></returns>
        User GetUser(string username);

        /// <summary>
        /// Get User By ID
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetUserById(int userId);

        /// <summary>
        ///  Get User that has submitted the Task for Approval
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        User SubmittedByUser(int accountId);

        /// <summary>
		/// Get Merchant By Username
		/// </summary>
		/// <returns></returns>
		Merchant GetMerchantByUsername(User user);
        
        /// <summary>
        /// Get Manufacturers
        /// </summary>
        /// <returns></returns>
        IEnumerable<Manufacturer> Manufactures();


        /// <summary>
        /// Get Devices
        /// </summary>
        /// <returns></returns>
        IEnumerable<Device> Devices();

        /// <summary>
        /// Get Device Types
        /// </summary>
        /// <returns></returns>
        IEnumerable<DeviceType> DeviceTypes();

        /// <summary>
        /// Get Product Types
        /// </summary>
        /// <returns></returns>
        IEnumerable<ProductType> ProductTypes();

		/// <summary>
		/// Get Merchant User Site
		/// </summary>
		/// <returns></returns>
		Site GetMerchantUserSiteByUsername(User user);


		/// <summary>
		/// Get Site DepositReference
		/// <param name="siteId"></param>
		/// </summary>
		/// <returns></returns>
		Dictionary<int, string> GetSiteDepositReference(int siteId);


		/// <summary>
		/// Get Site Containers
		/// <param name="siteId"></param>
		/// </summary>
		/// <returns></returns>
		IEnumerable<ContainerType> GetSiteContainers(int siteId);


		/// <summary>
		/// Get DepositTypes
		/// </summary>
		/// <returns></returns>
		IEnumerable<DepositType> GetDepositTypes();

        /// <summary>
        /// Get Cash Deposit Type
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        DepositType GetDepositType(string key);

        /// <summary>
        /// Get ContainerTypess
        /// </summary>
        /// <returns></returns>
        IEnumerable<ContainerType> GetContainerTypes();


		/// <summary>
		/// Get Site SettlementAccounts
		/// <param name="siteId"></param>
		/// </summary>
		/// <returns></returns>
	    IEnumerable<Bank> GetBanksServicedBySite(int siteId);


	    /// <summary>
		/// Get Site Settlement Accounts
		/// <param name="bankId"></param>
		/// <param name="siteId"></param>
	    /// </summary>
	    /// <returns></returns>
	    IEnumerable<Account> GetSiteSettlementAccounts(int bankId, int siteId);


		/// <summary>
		/// Get Merchants serviced by CashCenter
		/// <param name="user"></param>
		/// </summary>
		/// <returns></returns>
	    IEnumerable<Merchant> GetMerchantsServicedByCashCenter(User user);


		/// <summary>
		/// Get Merchants for Retail Users
		/// <param name="user"></param>
		/// </summary>
		/// <returns></returns>
		IEnumerable<Site> GetSitesForRetailUsers(User user);


		/// <summary>
		/// Get site ContainerTypes by SiteId
		/// <param name="siteId"></param>
		/// </summary>
		/// <returns></returns>
		IEnumerable<ContainerType> GetSiteContainerTypesBySiteId(int siteId);
		

		/// <summary>
		/// Get Cit Carrier initial digit
		/// <param name="siteId"></param>
		/// </summary>
		/// <returns></returns>
	    int GetCitInitialDigit(int siteId);

	    /// <summary>
	    /// Get ContainerType Attributes
	    /// <param name="containerTypeId"></param>
	    /// </summary>
	    /// <returns></returns>
	    ContainerTypeAttribute GetContainerTypeAttributes(int containerTypeId);

        /// <summary>
        /// Get Suppliers
        /// </summary>
        /// <returns></returns>
        IEnumerable<Supplier> Suppliers();

        /// <summary>
        /// Get All Fees
        /// </summary>
        /// <returns></returns>
        IEnumerable<Fee> Fees();

		Merchant GetMerchantBySiteId(int siteId);
	    IEnumerable<AccountType> GetAccountTypes();
	    IEnumerable<Bank> GetBanks();
	    Bank GetBankCode(int bankId);
	    Site GetCitCode(int siteId);

        /// <summary>
        /// Get a DB identity value for a giving error code
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        int GetErrorCodeId(string errorCode);

        /// <summary>
        /// Get a list of Discrepancy Reasons
        /// </summary>
        /// <returns></returns>
        IEnumerable<DiscrepancyReason> DiscrepancyReasons();

        /// <summary>
        /// Return a list of denomination types
        /// </summary>
        /// <returns></returns>
        IEnumerable<DenominationType> DenominationTypes();

        /// <summary>
        /// Get a list of all denominations
        /// </summary>
        /// <returns></returns>
        IEnumerable<Denomination> Denominations();


        /// <summary>
        /// Get a DB identity value for a giving error code
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        Fee GetFee(string code);


		/// <summary>
		/// Get Site
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>
		Site GetSite(int siteId);

        /// <summary>
        /// Get All Reports
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        List<Report> GetReports(string username);

        /// <summary>
        /// Get a report by its Id
        /// </summary>
        /// <param name="reportId"></param>
        /// <returns>a report object</returns>
        Report GetReportById(int reportId);

		/// <summary>
		///     get a list of emailAddresses for contact persons at a cash center
		/// </summary>
		/// <param name="siteId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
	    List<string> GetEmailAddresses(int siteId, User user);

        /// <summary>
        /// Get a list of all transaction Types
        /// </summary>
        /// <returns></returns>
        List<TransactionType> GetTransactionTypes();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Account GetDefaultAccount(int siteId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        IEnumerable<ContainerDrop> GetContainerDropsByDate();

        /// <summary>
        /// Generate Transaction number for DROP.
        /// </summary>
        /// <param name="siteId"></param>
        string GenerateTransactionNumberForDrop(int siteId);

        /// <summary>
        /// Generate transaction reference number
        /// </summary>
        /// <param name="siteId"></param>
        string GenerateTransactionNumber(int siteId);

        /// <summary>
        /// generate bag reference number
        /// </summary>
        /// <param name="siteId"></param>
        string GenerateTransactionNumberForContainer(int siteId);

        /// <summary>
        /// Check if Order type is EFT
        /// </summary>
        /// <param name="cashOrderTypeId"></param>
        bool IsEft(int cashOrderTypeId);

        /// <summary>
        /// Create Physical Folder
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <param name="userName"></param>
        string CreateTemporaryFolder(int cashOrderId, string userName);


        /// <summary>
        /// Get a list of email addresses for the sbv Finance Reviewer
        /// </summary>
        List<string> GetFinanceReviewerEmail();

        Task<List<Denomination>> GetDenominationsAsync(string denominationType);  
    }
}