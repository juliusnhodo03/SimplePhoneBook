using Application.Mapper;
using Application.Modules.CashHandling.CashDepositManager;
using Application.Modules.CashHandling.CashProcessing.VaultProcessor;
using Application.Modules.CashHandling.CashProcessing.WebProcessor;
using Application.Modules.CashOrdering.CashOrders;
using Application.Modules.Common;
using Application.Modules.FailedVaultRequest;
using Application.Modules.FinancialManagement;
using Application.Modules.Maintanance.Approval;
using Application.Modules.Maintanance.Bank;
using Application.Modules.Maintanance.BankAccount;
using Application.Modules.Maintanance.Carrier;
using Application.Modules.Maintanance.CashCenter;
using Application.Modules.Maintanance.Cluster;
using Application.Modules.Maintanance.ContainerType;
using Application.Modules.Maintanance.DeviceType;
using Application.Modules.Maintanance.Merchant;
using Application.Modules.Maintanance.Product;
using Application.Modules.Maintanance.ProductType;
using Application.Modules.Maintanance.SalesArea;
using Application.Modules.Maintanance.Site;
using Application.Modules.CashOrdering.Approval;
using Application.Modules.Maintanance.Users.CashCenter;
using Application.Modules.Maintanance.Users.HeadOffice;
using Application.Modules.Maintanance.Users.Merchant;
using Application.Modules.Profile;
using Application.Modules.Reporting;
using Application.Modules.UserAccountValidation;
using Application.Modules.XmlManipulation;
using Domain.Repository;
using Domain.Security;
using Domain.Serializer;
using Infrastructure.Logging;
using Infrastructure.Repository;
using Infrastructure.Security;
using Infrastructure.Serializer;
using Vault.Integration.Msmq.Connector;
using Application.Modules.VaultPayment;

namespace Mysbv.Web
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class LocalUnityConfig
    {
        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterDependencies()
        {
            LocalUnityResolver.InjectStub(new AutoMapperConfigProfile());
            LocalUnityResolver.Register<IMapper, Mapper>();

            LocalUnityResolver.Register<ISecurity, Security>();
            LocalUnityResolver.InjectStub(LogManager.GetLogger<MvcApplication>());

            LocalUnityResolver.Register<IRepository, Repository>();
            LocalUnityResolver.InjectStub(new SecurityConfiguration());

            LocalUnityResolver.Retrieve<SecurityConfiguration>().RegisterSecurity
                (
                    LocalUnityResolver.Retrieve<ISecurity>(),
                    "DefaultConnection",
                    "User",
                    "UserId",
                    "UserName"
                );

            // User Account
            LocalUnityResolver.Register<IUserAccountValidation, UserAccountValidation>();

            // Profile Validation
            LocalUnityResolver.Register<IProfileValidation, ProfileValidation>();

            // Lookup
            LocalUnityResolver.Register<ILookup, Lookup>();

            // Cash Centre User
            LocalUnityResolver.Register<ICashCenterUserValidation, CashCenterUserValidation>();

            // Merchant User
            LocalUnityResolver.Register<IMerchantUserValidation, MerchantUserValidation>();

            // Cash Handling
            LocalUnityResolver.Register<ICashDepositValidation, CashDepositValidation>();

            // Head Office User
            LocalUnityResolver.Register<IHeadOfficeUserValidation, HeadOfficeUserValidation>();

            // Cash Centre
            LocalUnityResolver.Register<ICashCenterValidation, CashCenterValidation>();

            // Bank
            LocalUnityResolver.Register<IBankValidation, BankValidation>();

            // Bank Account
            LocalUnityResolver.Register<IBankAccountValidation, BankAccountValidation>();

            // Approval
            LocalUnityResolver.Register<IApprovalValidation, ApprovalValidation>();

            // Device
            LocalUnityResolver.Register<IDeviceTypeValidation, DeviceTypeValidation>();

            // Reports
            LocalUnityResolver.Register<IReportingValidation, ReportingValidation>();

            // Sbv Cluster
            LocalUnityResolver.Register<IClusterValidation, ClusterValidation>();

            // Sales Area
            LocalUnityResolver.Register<ISalesAreaValidation, SalesAreaValidation>();

            //// Sales Area
            //LocalUnityResolver.Register<IAccountValidation, AccountValidation>();

            // Product Type
            LocalUnityResolver.Register<IProductTypeValidation, ProductTypeValidation>();

            // Container Type
            LocalUnityResolver.Register<IContainerTypeValidation, ContainerTypeValidation>();

            // Cash Ordering
            LocalUnityResolver.Register<ICashOrderingValidation, CashOrderingValidation>();

            // Cash Ordering Approvals
            LocalUnityResolver.Register<ICashOrderApprovalValidation, CashOrderApprovalValidation>();

            //Carrier
            LocalUnityResolver.Register<ICarrierValidation, CarrierValidation>();

            // Web CashDeposit Processing
            LocalUnityResolver.Register<ICashDepositWebProcessingValidation, CashDepositWebProcessingValidation>();

            // Vault CashDeposit Processing
            LocalUnityResolver.Register<ICashDepositVaultProcessingValidation, CashDepositVaultProcessingValidation>();

            // Merchant Maintenance
            LocalUnityResolver.Register<IMerchantValidation, MerchantValidation>();

            // Site
            LocalUnityResolver.Register<ISiteValidation, SiteValidation>();

            // Serializer
            LocalUnityResolver.Register<ISerializer, XmlSerializer>();

            //// Rejected Deposits
            LocalUnityResolver.Register<IRejectedDepositsValidation, RejectedDepositsValidation>();

            // Products
            LocalUnityResolver.Register<IProductValidation, ProductValidation>();

            // MSMQ Connector
            LocalUnityResolver.Register<IMsmqConnector, MsmqConnector>();

            // Failed Requests from Vault
            LocalUnityResolver.Register<IVaultRequestValidation, VaultRequestValidation>();

            // Failed Requests Xml Manipulation
            LocalUnityResolver.Register<IXmlFile, XmlFile>();

            // Vault Payment 
            LocalUnityResolver.Register<IVaultPaymentValidation, VaultPaymentValidation>();

            // Async Repository
            LocalUnityResolver.Register<IAsyncRepository, AsyncRepository>();
        }
    }
}