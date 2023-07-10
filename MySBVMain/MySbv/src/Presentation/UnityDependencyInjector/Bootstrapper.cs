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
using Microsoft.Practices.Unity;

namespace UnityDependencyInjector.Bootstrappers
{
    public static class Bootstrapper
    {
        public static void BuildContainer<T>() where T : class
        {
            var container = new UnityContainer();

            container.RegisterInstance(new AutoMapperConfig());

            container.RegisterType<ISecurity, Security>();
            container.RegisterInstance(LogManager.GetLogger<T>());

            container.RegisterType<IMapper, Mapper>();
            container.RegisterType<IRepository, Repository>();
            container.RegisterInstance(new SecurityConfiguration());

            container.Resolve<SecurityConfiguration>().RegisterSecurity
                (
                    container.Resolve<ISecurity>(),
                    "DefaultConnection",
                    "User",
                    "UserId",
                    "UserName"
                );

            // User Account
            container.RegisterType<IUserAccountValidation, UserAccountValidation>();

            // Profile Validation
            container.RegisterType<IProfileValidation, ProfileValidation>();

            // Lookup
            container.RegisterType<ILookup, Lookup>();

            // Cash Centre User
            container.RegisterType<ICashCenterUserValidation, CashCenterUserValidation>();

            // Merchant User
            container.RegisterType<IMerchantUserValidation, MerchantUserValidation>();

            // Cash Handling
            container.RegisterType<ICashDepositValidation, CashDepositValidation>();

            // Head Office User
            container.RegisterType<IHeadOfficeUserValidation, HeadOfficeUserValidation>();

            // Cash Centre
            container.RegisterType<ICashCenterValidation, CashCenterValidation>();

            // Bank
            container.RegisterType<IBankValidation, BankValidation>();

            // Bank Account
            container.RegisterType<IBankAccountValidation, BankAccountValidation>();

            // Approval
            container.RegisterType<IApprovalValidation, ApprovalValidation>();

            // Device
            container.RegisterType<IDeviceTypeValidation, DeviceTypeValidation>();

            // Reports
            container.RegisterType<IReportingValidation, ReportingValidation>();

            // Sbv Cluster
            container.RegisterType<IClusterValidation, ClusterValidation>();

            // Sales Area
            container.RegisterType<ISalesAreaValidation, SalesAreaValidation>();

            //// Sales Area
            //container.RegisterType<IAccountValidation, AccountValidation>();

            // Product Type
            container.RegisterType<IProductTypeValidation, ProductTypeValidation>();

            // Container Type
            container.RegisterType<IContainerTypeValidation, ContainerTypeValidation>();

            // Cash Ordering
            container.RegisterType<ICashOrderingValidation, CashOrderingValidation>();

            // Cash Ordering Approvals
            container.RegisterType<ICashOrderApprovalValidation, CashOrderApprovalValidation>();

            //Carrier
            container.RegisterType<ICarrierValidation, CarrierValidation>();

            // Web CashDeposit Processing
            container.RegisterType<ICashDepositWebProcessingValidation, CashDepositWebProcessingValidation>();

            // Vault CashDeposit Processing
            container.RegisterType<ICashDepositVaultProcessingValidation, CashDepositVaultProcessingValidation>();

            // Merchant Maintenance
            container.RegisterType<IMerchantValidation, MerchantValidation>();

            // Site
            container.RegisterType<ISiteValidation, SiteValidation>();

            // Serializer
            container.RegisterType<ISerializer, XmlSerializer>();

            //// Rejected Deposits
            container.RegisterType<IRejectedDepositsValidation, RejectedDepositsValidation>();

            // Products
            container.RegisterType<IProductValidation, ProductValidation>();

            // MSMQ Connector
            container.RegisterType<IMsmqConnector, MsmqConnector>();

            // Failed Requests from Vault
            container.RegisterType<IVaultRequestValidation, VaultRequestValidation>();

            // Failed Requests Xml Manipulation
            container.RegisterType<IXmlFile, XmlFile>();

            // Vault Payment 
            container.RegisterType<IVaultPaymentValidation, VaultPaymentValidation>();

            // Async Repository
            container.RegisterType<IAsyncRepository, AsyncRepository>();
        }
    }
}