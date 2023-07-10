using System;
using System.Data.Entity.Migrations;
using System.Web.Security;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;
using Infrastructure.Repository.Database;
using WebMatrix.WebData;

namespace Infrastructure.Repository.Migrations
{
    public class MigrationConfiguration : DbMigrationsConfiguration<Context>
    {
        public MigrationConfiguration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(Context context)
        {
            //if (System.Diagnostics.Debugger.IsAttached == false)
            //    System.Diagnostics.Debugger.Launch();

            #region Security Config

            #region Create The Default User

            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "User", "UserId", "UserName", true);

            if (!WebSecurity.UserExists("SBVAdmin"))
                WebSecurity.CreateUserAndAccount("SBVAdmin", "Pass@123",
                    new
                    {
                        LockedStatus = false,
                        CanMakeVaultPayment = false,
                        IsNotDeleted = true,
                        LastChangedDate = DateTime.Now,
                        CreateDate = DateTime.Now
                    });

            #endregion

            #region Configure Roles

            if (!Roles.RoleExists("SBVAdmin"))
                Roles.CreateRole("SBVAdmin");

            if (!Roles.RoleExists("RetailUser"))
                Roles.CreateRole("RetailUser");

            if (!Roles.RoleExists("RetailViewer"))
                Roles.CreateRole("RetailViewer");

            if (!Roles.RoleExists("RetailSupervisor"))
                Roles.CreateRole("RetailSupervisor");

            if (!Roles.RoleExists("SBVTeller"))
                Roles.CreateRole("SBVTeller");

            if (!Roles.RoleExists("SBVTellerSupervisor"))
                Roles.CreateRole("SBVTellerSupervisor");

            if (!Roles.RoleExists("SBVApprover"))
                Roles.CreateRole("SBVApprover");

            if (!Roles.RoleExists("SBVRecon"))
                Roles.CreateRole("SBVRecon");

            if (!Roles.RoleExists("SBVFinanceReviewer"))
                Roles.CreateRole("SBVFinanceReviewer");

            if (!Roles.RoleExists("SBVAdmin"))
                Roles.CreateRole("SBVAdmin");

            if (!Roles.RoleExists("SBVDataCapture"))
                Roles.CreateRole("SBVDataCapture");

            #endregion

            #region Add Default User To System Administrator Role

            if (!Roles.IsUserInRole("SBVAdmin", "SBVAdmin"))
                Roles.AddUserToRole("SBVAdmin", "SBVAdmin");

            #endregion

            #endregion

            #region System Configuration

            context.SystemConfigurations.AddOrUpdate(new SystemConfiguration()
            {
                SystemConfigurationId = 1,
                IsNotDeleted = true,
                Name = "Server Address",
                Description = "The address of the server where mysbv is running",
                LookUpKey = "SERVER_ADDRESS",
                Value = "http://so-dev-mysbv/MySbv",
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            }, 
            new SystemConfiguration
            {
                SystemConfigurationId = 2,
                Name = "DropPath",
                Description = "File Drop Path",
                LookUpKey = "DROP_PATH",
                Value = @"D:/hyphen/OUT",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            }, 
            new SystemConfiguration
            {
                SystemConfigurationId = 3,
                Name = "PickUpPath",
                Description = "File Pick Up Path",
                LookUpKey = "PICKUP_PATH",
                Value = @"D:/hyphen/IN",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            }, 
            new SystemConfiguration
            {
                SystemConfigurationId = 4,
                Name = "ArchivePath",
                Description = "File Archive Path",
                LookUpKey = "ARCHIVE_PATH",
                Value = @"D:/hyphen/ARCHIVE",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new SystemConfiguration()
            {
                SystemConfigurationId = 5,
                Name = "RejectedDepositEmail",
                Description = "Rejected Deposit Email Template",
                LookUpKey = "REJECTED_DEPOSIT_EMAIL",
                Value = "tefom@sbv.co.za",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            }, 
            new SystemConfiguration()
            {
                SystemConfigurationId = 6,
                Name  = "ResponseMessageXMLPath",
                Description = "Response Message XML Save Path",
                LookUpKey = "RESPONSE_MESSAGE_XML_PATH",
                Value = @"D:\\ResponseMessage\\",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new SystemConfiguration
            {
                SystemConfigurationId = 7,
                Name = "Nedbank Archive Directory",
                Description = "Nedbank Archive Directory",
                LookUpKey = "NEDBANK_ARCHIVE_PATH",
                Value = @"C:\CONNECTDIRECT\ARCHIVE",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new SystemConfiguration
            {
                SystemConfigurationId = 8,
                Name = "Nedbank Request Directory",
                Description = "Nedbank Request Directory",
                LookUpKey = "NEDBANK_DROP_PATH",
                Value = @"C:\CONNECTDIRECT\OUT",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new SystemConfiguration
            {
                SystemConfigurationId = 9,
                Name = "Nedbank Response Directory",
                Description = "Nedbank Response Directory",
                LookUpKey = "NEDBANK_PICKUP_PATH",
                Value = @"C:\CONNECTDIRECT\IN",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new SystemConfiguration
            {
                SystemConfigurationId = 10,
                Name = "Nedbank Server",
                Description = "Nedbank Server",
                LookUpKey = "NEDBANK_SERVER_ENVIRONMENT",
                Value = "UAT",
                IsNotDeleted = true,
                LastChangedById = 1,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            });
            
            #endregion

			#region Manufactures
			context.Manufacturers.AddOrUpdate(new Manufacturer
			{
				ManufacturerId = 1,
				Name = "UnKnown",
				Description = "UnKnown",
				LookUpKey = "UNKNOWN",
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			});
			#endregion

            #region  CashOrderTypes

            context.CashOrderTypes.AddOrUpdate(new CashOrderType
            {
                CashOrderTypeId = 1,
                Name = "Cash for Cash",
                Description = "Cash For Cash",
                LookUpKey = "CASH_FOR_CASH",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
                new CashOrderType
                {
                    CashOrderTypeId = 2,
                    Name = "EFT",
                    Description = "Electronic funds transfer",
                    LookUpKey = "EFT",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                });

            #endregion

            //#region CashOrderTask

            //context.CashOrderTasks.AddOrUpdate(new CashOrderTask
            //{
            //    CashOrderTaskId = 1,
            //    CashOrderId = 1,
            //    ReferenceNumber = "CD85FCACE33E",
            //    Date = DateTime.Now,
            //    SiteId = 93,
            //    UserId = 5128,
            //    StatusId = 13,
            //    RequestUrl = "",
            //    IsNotDeleted = true,
            //    LastChangedById = 1,
            //    LastChangedDate = DateTime.Now,
            //    CreateDate = DateTime.Now,
            //    CreatedById = 1
            //});

            //#endregion

            #region PublicHolidays

            context.PublicHolidays.AddOrUpdate(new PublicHoliday
            {
                PublicHolidayId = 1,
                Name = "NewYearsDay",
                Description = "New Year's Day",
                LookUpKey = "NEW_YEARS_DAY",
                Month = 1,
                Day = 1,
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            },
                new PublicHoliday
                {
                    PublicHolidayId = 2,
                    Name = "Human Rights Day",
                    Description = "Human Rights Day",
                    LookUpKey = "HUMAN_RIGHTS_DAY",
                    Month = 3,
                    Day = 21,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 5,
                    Name = "FreedomDay",
                    Description = "Freedom Day",
                    LookUpKey = "FREEDOM_DAY",
                    Month = 4,
                    Day = 27,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 6,
                    Name = "WorkersDay",
                    Description = "Workers Day",
                    LookUpKey = "WORKERS_DAY",
                    Month = 5,
                    Day = 1,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 7,
                    Name = "Youth Day",
                    Description = "Youth Day",
                    LookUpKey = " YOUTH_DAY",
                    Month = 6,
                    Day = 16,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 8,
                    Name = "NationalWomensDay",
                    Description = "National Woman's Day",
                    LookUpKey = "NATIONAL_WOMANS_DAY",
                    Month = 8,
                    Day = 9,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 9,
                    Name = "Heritage Day",
                    Description = "Heritage Day",
                    LookUpKey = "HERITAGE_DAY",
                    Month = 9,
                    Day = 24,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 10,
                    Name = "Day of Reconciliation",
                    Description = "Day of Reconciliation",
                    LookUpKey = "DAY_OF_RECONCILIATION",
                    Month = 12,
                    Day = 16,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 11,
                    Name = "Christmas Day",
                    Description = "Christmas Day",
                    LookUpKey = "CHRISTMAS_DAY",
                    Month = 12,
                    Day = 25,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new PublicHoliday
                {
                    PublicHolidayId = 12,
                    Name = "DayofGoodwill",
                    Description = "Day of Goodwill",
                    LookUpKey = "DAY_OF_GOODWILL",
                    Month = 12,
                    Day = 26,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                });

            #endregion

            #region UserType

            context.UserTypes.AddOrUpdate(new UserType
            {
                UserTypeId = 1,
                Name = "SBVUser",
                Description = "SBV Cash Centre User",
                LookUpKey = "SBV_USER",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            },
                new UserType
                {
                    UserTypeId = 2,
                    Name = "MerchantUser",
                    Description = "Retail Merchant User",
                    LookUpKey = "MERCHANT_USER",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new UserType
                {
                    UserTypeId = 3,
                    Name = "HeadOfficeUser",
                    Description = "SBV Head Office User",
                    LookUpKey = "HEAD_OFFICE_USER",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                });

            #endregion

            #region Titles

            context.Titles.AddOrUpdate(
                new Title
                {
                    TitleId = 1,
                    Name = "Mr",
                    Description = "Mr",
                    LookUpKey = "MR",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now
                },
                new Title
                {
                    CreatedById = 1,
                    TitleId = 2,
                    Name = "Mrs",
                    Description = "Mrs",
                    LookUpKey = "MRS",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now
                },
                new Title
                {
                    CreatedById = 1,
                    TitleId = 3,
                    Name = "Miss",
                    Description = "Miss",
                    LookUpKey = "MISS",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now
                },
                new Title
                {
                    CreatedById = 1,
                    TitleId = 4,
                    Name = "Dr",
                    Description = "Dr",
                    LookUpKey = "DR",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now
                },
                new Title
                {
                    CreatedById = 1,
                    TitleId = 5,
                    Name = "Other",
                    Description = "Other",
                    LookUpKey = "OTHER",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now
                });

            #endregion

            #region Geography

            context.Geographies.AddOrUpdate(new Geography
            {
                CreatedById = 1,
                GeographyId = 1,
                Name = "Africa",
                Description = "Africa",
                LookUpKey = "AFRICA",
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreateDate = DateTime.Now
            });

            #endregion

            #region Continent

            context.Continents.AddOrUpdate(new Continent
            {
                CreatedById = 1,
                ContinentId = 1,
                Name = "Africa",
                Description = "Africa",
                GeographyId = 1,
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreateDate = DateTime.Now
            });

            #endregion

            #region Country

            context.Countries.AddOrUpdate(new Country
            {
                CreatedById = 1,
                CountryId = 1,
                Name = "South Africa",
                Description = "South Africa",
                LookUpKey = "SOUTH_AFRICA",
                ContinentId = 1,
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                CreateDate = DateTime.Now
            });

            #endregion

            #region Provinces

            context.Provinces.AddOrUpdate(new Province
            {
                ProvinceId = 1,
                Name = "Gauteng",
                Description = "Gauteng",
                LookUpKey = "GAUTENG",
                CountryId = 1,
                LastChangedDate = DateTime.Now,
                LastChangedById = 1,
                IsNotDeleted = true,
                CreateDate = DateTime.Now,
                CreatedById = 1
            },
                new Province
                {
                    ProvinceId = 2,
                    Name = "Limpopo",
                    Description = "Limpopo",
                    LookUpKey = "LIMPOPO",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 3,
                    Name = "Free State",
                    Description = "Free State",
                    LookUpKey = "FREESTATE",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 4,
                    Name = "KwaZulu-Natal",
                    Description = "KwaZulu-Natal",
                    LookUpKey = "KWAZULU_NATAL",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 5,
                    Name = "Eastern Cape",
                    Description = "Eastern Cape",
                    LookUpKey = "EASTERN_CAPE",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 6,
                    Name = "Mpumalanga",
                    Description = "Mpumalanga",
                    LookUpKey = "MPUMALANGA",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 7,
                    Name = "North West",
                    Description = "North West",
                    LookUpKey = "NORTH_WEST",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 8,
                    Name = "Northern Cape",
                    Description = "Northern Cape",
                    LookUpKey = "NORTHERN_CAPE",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    IsNotDeleted = true,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 9,
                    Name = "Western Cape",
                    Description = "Western Cape",
                    LookUpKey = "WESTERN_CAPE",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Province
                {
                    ProvinceId = 10,
                    Name = "Other",
                    Description = "Other",
                    LookUpKey = "OTHER",
                    CountryId = 1,
                    LastChangedDate = DateTime.Now,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                });

            #endregion

            #region Merchant Descriptions

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 1,
                Name = "Agriculture And Processing",
                Description = "Agriculture & Agri-processing",
                LookUpKey = "AGRICULTURE_AND_AGRIPROCESSING",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 2,
                Name = "Aquaculture And Mariculture",
                Description = "Aquaculture & Mariculture",
                LookUpKey = "AQUACULTURE_AND_MARICULTURE",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 3,
                Name = "Auto mobiles",
                Description = "Automobile",
                LookUpKey = "AUTOMOBILE",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 4,
                Name = "Banking",
                Description = "",
                LookUpKey = "BANKING",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 5,
                Name = "Chemicals",
                Description = "",
                LookUpKey = "CHEMICALS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 6,
                Name = "Clothing And Textiles",
                Description = "Clothing & Textiles",
                LookUpKey = "CLOTHING_AND_TEXTILES",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 7,
                Name = "Construction And Materials",
                Description = "Construction & Materials",
                LookUpKey = "CONSTRUCTION_AND_MATERIAL",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 8,
                Name = "Containers And Packaging",
                Description = "Containers & Packaging",
                LookUpKey = "CONTAINERS_AND_PACKAGING",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 9,
                Name = "Delivery Services Or Logistics",
                Description = "Delivery Services Or Logistics",
                LookUpKey = "DELIVERY_SERVICES",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 10,
                Name = "Development Finance Or Micro Loans",
                Description = "Development Finance Or MicroLoans",
                LookUpKey = "DEVELOPMENT_FINANCE",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 11,
                Name = "Education",
                Description = "Education",
                LookUpKey = "EDUCATION",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 12,
                Name = "Electronics",
                Description = "Electronics",
                LookUpKey = "ELECTRONICS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 13,
                Name = "Energy",
                Description = "Energy",
                LookUpKey = "ENERGY",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 14,
                Name = "Environment Waste And Recycling",
                Description = "Environment, Waste & Recycling",
                LookUpKey = "ENVIRONMENTAL_WASTE_AND_RECYCLING",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 15,
                Name = "Forestry And Paper",
                Description = "Forestry & Paper",
                LookUpKey = "FORESTRY_AND_PAPER",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 16,
                Name = "Healthcare And Pharmaceuticals",
                Description = "Healthcare & Pharmaceuticals",
                LookUpKey = "HEALTHCARE_AND_PHARMACEUTICALS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 17,
                Name = "Information And Communication Technology",
                Description = "ICT - Information & Communication",
                LookUpKey = "ICT",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });


            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 18,
                Name = "Insurance",
                Description = "Insurance",
                LookUpKey = "INSURANCE",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 19,
                Name = "Manufacturing",
                Description = "Manufacturing",
                LookUpKey = "MANUFACTURING",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 20,
                Name = "Media And Publications",
                Description = "Media & Publications",
                LookUpKey = "MEDIA_AND_PUBLICATION",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 21,
                Name = "Mining And Metals",
                Description = "Mining & Metals",
                LookUpKey = "MINING_AND_METAL",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 22,
                Name = "Oil, Gas And Fuel",
                Description = "Oil, Gas & Fuel",
                LookUpKey = "OIL_GAS_AND_FUEL",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 23,
                Name = "Household Goods",
                Description = "Household Goods",
                LookUpKey = "HOUSEHOLD_GOODS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 24,
                Name = "Pharmaceutical",
                Description = "Pharmaceutical",
                LookUpKey = "PHARMACEUTICAL",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 25,
                Name = "Public Sector Entity",
                Description = "Public Sector Entity",
                LookUpKey = "PUBLIC_SECTOR_ENTITY",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 26,
                Name = "Retail",
                Description = "Retail",
                LookUpKey = "RETAIL",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 27,
                Name = "Security",
                Description = "Security",
                LookUpKey = "SECURITY",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 28,
                Name = "Sport",
                Description = "Sport",
                LookUpKey = "SPORT",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 29,
                Name = "Telecommunications",
                Description = "Telecommunications",
                LookUpKey = "TELECOMMUNICATIONS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 30,
                Name = "Tourism And Leisure",
                Description = "Tourism & Leisure",
                LookUpKey = "TOURISM_AND_LEISURE",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 31,
                Name = "Transportation",
                Description = "Transportation",
                LookUpKey = "TRANSPORTATION",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.MerchantDescriptions.AddOrUpdate(new MerchantDescription
            {
                MerchantDescriptionId = 32,
                Name = "Other",
                Description = "Other",
                LookUpKey = "OTHER",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            #endregion

            #region Company Types

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 1,
                IsNotDeleted = true,
                Name = "Company",
                Description = "Company",
                LookUpKey = "COMPANY",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 2,
                IsNotDeleted = true,
                Name = "Close Corporation",
                Description = "Close Corporation",
                LookUpKey = "CLOSED_CORPORATION",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 3,
                IsNotDeleted = true,
                Name = "SoleProprietorship",
                Description = "Sole Proprietorship",
                LookUpKey = "SOLEPROPRIETORSHIP",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 4,
                IsNotDeleted = true,
                Name = "Trust",
                Description = "Trust",
                LookUpKey = "TRUST",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 5,
                IsNotDeleted = true,
                Name = "Partnership",
                Description = "Partnership",
                LookUpKey = "PARTNERSHIP",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 6,
                IsNotDeleted = true,
                Name = "Parastatal",
                Description = "Parastatal",
                LookUpKey = "PARASTATAL",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 7,
                IsNotDeleted = true,
                Name = "Government Body",
                Description = "Government Body",
                LookUpKey = "GOVERNMENT_BODY",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 8,
                IsNotDeleted = true,
                Name = "Bank",
                Description = "Bank",
                LookUpKey = "BANK",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            context.CompanyTypes.AddOrUpdate(new CompanyType
            {
                CompanyTypeId = 9,
                IsNotDeleted = true,
                Name = "Other",
                Description = "Other",
                LookUpKey = "OTHER",
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            #endregion
			
            #region Address Type

            context.AddressTypes.AddOrUpdate(new AddressType
            {
                AddressTypeId = 1,
                Name = "PostalAddress",
                Description = "Postal Address",
                LookUpKey = "POSTAL_ADDRESS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            },
                new AddressType
                {
                    AddressTypeId = 2,
                    Name = "ResidentialAddress",
                    Description = "Residential Address",
                    LookUpKey = "RESIDENTIAL_ADDRESS",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                }
                );

            #endregion

            #region Cluster

            context.Cluster.AddOrUpdate(new Cluster
            {
                ClusterId = 1,
                LastChangedById = 1,
                Name = "Gauteng",
                Description = "Gauteng",
                LookUpKey = "GAUTENG",
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            }, new Cluster
            {
                ClusterId = 2,
                LastChangedById = 1,
                Name = "Limpopo",
                Description = "Limpopo",
                LookUpKey = "LIMPOPO",
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            }, new Cluster
            {
                ClusterId = 3,
                LastChangedById = 1,
                Name = "Inland",
                Description = "Inland",
                LookUpKey = "INLAND",
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            },
                new Cluster
                {
                    ClusterId = 4,
                    LastChangedById = 1,
                    Name = "KZN",
                    Description = "KZN",
                    LookUpKey = "KZN",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                }, new Cluster
                {
                    ClusterId = 5,
                    LastChangedById = 1,
                    Name = "Eastern Cape",
                    Description = "Eastern Cape",
                    LookUpKey = "EASTERN_CAPE",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                }, new Cluster
                {
                    ClusterId = 6,
                    LastChangedById = 1,
                    Name = "Western Cape",
                    Description = "Western Cape",
                    LookUpKey = "WESTERN_CAPE",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                }, new Cluster
                {
                    ClusterId = 7,
                    LastChangedById = 1,
                    Name = "Mpumalanga",
                    Description = "Mpumalanga",
                    LookUpKey = "MPUMALANGA",
                    IsNotDeleted = true,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                });

            #endregion

			#region Settlement Type

            context.SettlementTypes.AddOrUpdate(new SettlementType
            {
                SettlementTypeId = 1,
                Name = "Net Settlement",
                Description = "Net Settlement",
                LookUpKey = "NET_SETTLEMENT",
                LastChangedById = 1,
                CreatedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                IsNotDeleted = true
            }, new SettlementType
            {
                SettlementTypeId = 2,
                Name = "Gross Settlement",
                Description = "Gross Settlement",
                LookUpKey = "GROSS_SETTLEMENT",
                LastChangedById = 1,
                CreatedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                IsNotDeleted = true
            });

            #endregion

            #region AccountType

            context.AccountTypes.AddOrUpdate(new AccountType
            {
                AccountTypeId = 1,
                Name = "Cheque Account",
                Description = "Cheque Account",
                LookUpKey = "CHECK_ACCONT",

                IsNotDeleted = true,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now
            },
                new AccountType
                {
                    AccountTypeId = 2,
                    Name = "Savings Account",
                    Description = "Savings Account",
                    LookUpKey = "SAVINGS_ACCOUNT",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }, new AccountType
                {
                    AccountTypeId = 3,
                    Name = "Transmission",
                    Description = "Transmission Account",
                    LookUpKey = "TRANSMISSION_ACOUNT",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                },
                new AccountType
                {
                    AccountTypeId = 4,
                    Name = "Mzansi Account",
                    Description = "Mzansi Account",
                    LookUpKey = "MZANSI_ACCOUNT",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                });

            #endregion

            #region Banks

            context.Banks.AddOrUpdate(new Bank
            {
                BankId = 1,
                Name = "FNB",
                Description = "First National Bank SA",
                LookUpKey = "FNB",
				BranchCode = "250655",
                IsNotDeleted = true,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                LastChangedDate = DateTime.Now
            }, new Bank
            {
                BankId = 2,
                Name = "ABSA",
                Description = "ABSA Group Limited",
                LookUpKey = "ABSA",
				BranchCode = "632005",
                IsNotDeleted = true,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                LastChangedDate = DateTime.Now
            }, new Bank
            {
                BankId = 3,
                Name = "SBSA",
                Description = "Standard Bank SA",
                LookUpKey = "SBSA",
				BranchCode = "051001",
                IsNotDeleted = true,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                LastChangedDate = DateTime.Now
            }, new Bank
            {
                BankId = 4,
                Name = "Nedbank",
                Description = "Nedbank Group",
                LookUpKey = "NEDBANK",
				BranchCode = "198765",
                IsNotDeleted = true,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                CreatedById = 1,
                LastChangedDate = DateTime.Now
            });

            #endregion

            #region Product Types

            context.ProductType.AddOrUpdate(new ProductType
            {
                ProductTypeId = 1,
                Name = "mySBV.Deposit",
                Description = "mySBV.Deposit",
                LookUpKey = "MYSBV_DEPOSIT",
                LastChangedById = 1,
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            }, new ProductType
            {
                ProductTypeId = 2,
                Name = "mySBV.Vault",
                Description = "mySBV.Vault",
                LookUpKey = "MYSBV_VAULT",
                LastChangedById = 1,
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            }, new ProductType
            {
                ProductTypeId = 3,
                Name = "mySBV.CIT",
                Description = "mySBV.CIT",
                LookUpKey = "MYSBV_CIT",
                LastChangedById = 1,
                IsNotDeleted = true,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            });

            #endregion

            #region NotificationTypes

            context.NotificationTypes.AddOrUpdate(new NotificationType
            {
                NotificationTypeId = 1,
                Name = "SMS",
                Description = "SMS Notification",
                LookUpKey = "SMS",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now,
                CreatedById = 1
            },
                new NotificationType
                {
                    NotificationTypeId = 2,
                    Name = "FAX",
                    Description = "FAX Notification",
                    LookUpKey = "FAX",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new NotificationType
                {
                    NotificationTypeId = 3,
                    Name = "EMAIL",
                    Description = "EMAIL Notification",
                    LookUpKey = "EMAIL",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                });

            #endregion

            #region DenominationTypes

            context.DenominationTypes.AddOrUpdate(
                new DenominationType
                {
                    DenominationTypeId = 1,
                    Name = "Notes",
                    Description = "Notes",
                    LookUpKey = "NOTES",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new DenominationType
                {
                    DenominationTypeId = 2,
                    Name = "Coins",
                    Description = "Coins",
                    LookUpKey = "COINS",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                });

            #endregion

            #region Denominations

            context.Denominations.AddOrUpdate(
                new Denomination
                {
                    DenominationId = 1,
                    CountryId = 1,
                    Name = "200",
                    Description = "R200",
                    LookUpKey = "R200",
                    DenominationTypeId = 1,
                    ValueInCents = 20000,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Denomination
                {
                    DenominationId = 2,
                    CountryId = 1,
                    Name = "100",
                    Description = "R100",
                    LookUpKey = "R100",
                    DenominationTypeId = 1,
                    ValueInCents = 10000,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                    CreatedById = 1
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 3,
                    CountryId = 1,
                    Name = "50",
                    Description = "R50",
                    LookUpKey = "R50",
                    DenominationTypeId = 1,
                    ValueInCents = 5000,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 4,
                    CountryId = 1,
                    Name = "20",
                    Description = "R20",
                    LookUpKey = "R20",
                    DenominationTypeId = 1,
                    ValueInCents = 2000,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 5,
                    CountryId = 1,
                    Name = "10",
                    Description = "R10",
                    LookUpKey = "R10",
                    DenominationTypeId = 1,
                    ValueInCents = 1000,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 6,
                    CountryId = 1,
                    Name = "5",
                    Description = "R5",
                    LookUpKey = "R5",
                    DenominationTypeId = 2,
                    ValueInCents = 500,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 7,
                    CountryId = 1,
                    Name = "2",
                    Description = "R2",
                    LookUpKey = "R2",
                    DenominationTypeId = 2,
                    ValueInCents = 200,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 8,
                    CountryId = 1,
                    Name = "1",
                    Description = "R1",
                    LookUpKey = "R1",
                    DenominationTypeId = 2,
                    ValueInCents = 100,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 9,
                    CountryId = 1,
                    Name = "50",
                    Description = "50C",
                    LookUpKey = "50C",
                    DenominationTypeId = 2,
                    ValueInCents = 50,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 10,
                    CountryId = 1,
                    Name = "20",
                    Description = "20C",
                    LookUpKey = "20C",
                    DenominationTypeId = 2,
                    ValueInCents = 20,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 11,
                    CountryId = 1,
                    Name = "10",
                    Description = "10C",
                    LookUpKey = "10C",
                    DenominationTypeId = 2,
                    ValueInCents = 10,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Denomination
                {
                    CreatedById = 1,
                    DenominationId = 12,
                    CountryId = 1,
                    Name = "5",
                    Description = "5C",
                    LookUpKey = "5C",
                    DenominationTypeId = 2,
                    ValueInCents = 5,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                });

            #endregion

            #region Deposit Types

            context.DepositTypes.AddOrUpdate(
                new DepositType
                {
                    CreatedById = 1,
                    DepositTypeId = 1,
                    Name = "Single Deposit",
                    Description = "Single Deposit",
                    LookUpKey = "SINGLE_DEPOSIT",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new DepositType
                {
                    CreatedById = 1,
                    DepositTypeId = 2,
                    Name = "Multi Deposit",
                    Description = "Multi Deposit",
                    LookUpKey = "MULTI_DEPOSIT",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new DepositType
                {
                    CreatedById = 1,
                    DepositTypeId = 3,
                    Name = "Multi Drop",
                    Description = "Multi Drop",
                    LookUpKey = "MULTI_DROP",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                });

            #endregion

            #region Status

            context.Status.AddOrUpdate(new Status
            {
                CreatedById = 1,
                StatusId = 1,
                Name = "Active",
                Description = "Active",
                LookUpKey = "ACTIVE",
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 2,
                    Name = "InActive",
                    Description = "InActive",
                    LookUpKey = "INACTIVE",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 3,
                    Name = "Submitted",
                    Description = "Submitted",
                    LookUpKey = "SUBMITTED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 4,
                    Name = "Saved",
                    Description = "Saved",
                    LookUpKey = "SAVED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 5,
                    Name = "On Hold",
                    Description = "On Hold",
                    LookUpKey = "ONHOLD",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 6,
                    Name = "Approved",
                    Description = "Approved",
                    LookUpKey = "APPROVED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 7,
                    Name = "Declined",
                    Description = "Declined",
                    LookUpKey = "DECLINED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 8,
                    Name = "Canceled",
                    Description = "Canceled",
                    LookUpKey = "CANCELLED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 9,
                    Name = "Bag Open",
                    Description = "Bag Open",
                    LookUpKey = "BAG_OPEN",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 10,
                    Name = "Bag Closed",
                    Description = "Bag Closed",
                    LookUpKey = "BAG_CLOSED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 11,
                    Name = "Bag Not In Use",
                    Description = "Bag Not In Use",
                    LookUpKey = "BAG_NOT_IN_USE",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 12,
                    Name = "Amended",
                    Description = "Active But Amended",
                    LookUpKey = "AMENDED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 13,
                    Name = "Pending",
                    Description = "Pending Approval",
                    LookUpKey = "PENDING",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 14,
                    Name = "Confirmed",
                    Description = "Confirmed",
                    LookUpKey = "CONFIRMED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 15,
                    Name = "Discrepancy",
                    Description = "Discrepancy",
                    LookUpKey = "DISCREPANCY",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 16,
                    Name = "Pending Settlement",
                    Description = "Submitted for Settlement",
                    LookUpKey = "PENDING_SETTLEMENT",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                }, new Status
                {
                    CreatedById = 1,
                    StatusId = 17,
                    Name = "Settled",
                    Description = "Settled",
                    LookUpKey = "SETTLED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 18,
                    Name = "Settlement Rejected",
                    Description = "Rejected Settlement",
                    LookUpKey = "SETTLEMENT_REJECTED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 19,
                    Name = "Resubmitted",
                    Description = "Resubmitted",
                    LookUpKey = "RESUBMITTED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 20,
                    Name = "Processed",
                    Description = "Processed",
                    LookUpKey = "PROCESSED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                },
                new Status
                {
                    CreatedById = 1,
                    StatusId = 21,
                    Name = "Pay Unconfirmed",
                    Description = "Pay a deposit even though is not yet confirmed",
                    LookUpKey = "PAY_UNCONFIRMED",
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreateDate = DateTime.Now
                });

            #endregion

            #region Transaction Types

            context.TransactionTypes.AddOrUpdate(
			new TransactionType
            {
                TransactionTypeId = 1,
                Name = "FNB01",
                Description = "EFT/Credit",
                LookUpKey = "FNB_HOST",
                IsNotDeleted = true,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now
            },
            new TransactionType
            {
                TransactionTypeId = 2,
                Name = "FNB02",
                Description = "Debit",
                LookUpKey = "FNB02",
                IsNotDeleted = true,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now
            },
            new TransactionType
            {
                TransactionTypeId = 3,
                Name = "ABS01",
                Description = "EFT/Credit",
                LookUpKey = "ABSA_HOST",
                IsNotDeleted = true,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now
            },
            new TransactionType
            {
                TransactionTypeId = 4,
                Name = "SBS01",
                Description = "EFT/Credit",
                LookUpKey = "SBSA_HOST",
                IsNotDeleted = true,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now
            });

            #endregion
			
            #region Supplier

            context.Suppliers.AddOrUpdate(new Supplier()
            {
                SupplierId = 1,
                Name = "GPT",
                Description = "Global Payment Technologies",
                LookUpKey = "GPT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new Supplier()
            {
                SupplierId = 2,
                Name = "Cash Connect",
                Description = "Cash Connect Technologies",
                LookUpKey = "CASH_CONNECT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            });

            #endregion

			#region Reports

			context.Reports.AddOrUpdate(new Report
			{
				ReportId = 1,
				Name = "CashDepositAuditTrail",
				Description = "Cash Deposit Audit Trail",
				LookUpKey = "Cash_Deposit_Audit_Trail",
				Path = "/ReportServer/Pages/ReportViewer.aspx?/mySBV Reports/CashDepositAuditTrail&rs:Command=Render&UserID=",
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new Report
			{
				ReportId = 2,
				Name = "CashOrderAuditTrail",
				Description = "Cash Order Audit Trail",
				LookUpKey = "Cash_Order_Audit_Trail",
				Path = "/ReportServer/Pages/ReportViewer.aspx?/mySBV Reports/CashOrderAuditTrail&rs:Command=Render&UserID=",
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new Report
			{
				ReportId = 3,
				Name = "DepositSettlementTransactions",
				Description = "Deposit Settlement Transactions",
				LookUpKey = "Deposit_Settlement_Transactions",
				Path = "/ReportServer/Pages/ReportViewer.aspx?/mySBV Reports/DepositSettlementTransactions&rs:Command=Render&UserID=",
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new Report
			{
				ReportId = 4,
				Name = "ClientTransactionSearch",
				Description = "Client Transaction Search",
				LookUpKey = "Client_Transaction_Search",
				Path = "/ReportServer/Pages/ReportViewer.aspx?/mySBV Reports/ClientTransactionSearch&rs:Command=Render&UserID=",
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new Report
			{
				ReportId = 5,
				Name = "OutstandingBags",
				Description = "Outstanding Bags",
				LookUpKey = "Outstanding_Bags",
				Path = "/ReportServer/Pages/ReportViewer.aspx?/mySBV Reports/OutstandingBags&rs:Command=Render&UserID=",
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			});

			#endregion
			
			#region RoleReports

			context.RoleReports.AddOrUpdate(new RoleReport
	        {
				RoleReportId = 1,
				RoleId = 1,
		        ReportId = 1,
		        IsNotDeleted = true,
		        CreatedById = 1,
		        LastChangedById = 1,
		        CreateDate = DateTime.Now,
		        LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 2,
				RoleId = 1,
				ReportId = 2,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 3,
				RoleId = 1,
				ReportId = 3,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 4,
				RoleId = 1,
				ReportId = 4,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 5,
				RoleId = 7,
				ReportId = 4,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 6,
				RoleId = 9,
				ReportId = 1,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 7,
				RoleId = 9,
				ReportId = 2,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 8,
				RoleId = 9,
				ReportId = 3,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 9,
				RoleId = 8,
				ReportId = 1,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 10,
				RoleId = 8,
				ReportId = 2,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 11,
				RoleId = 8,
				ReportId = 3,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 12,
				RoleId = 6,
				ReportId = 1,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 13,
				RoleId = 6,
				ReportId = 2,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 14,
				RoleId = 4,
				ReportId = 4,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 15,
				RoleId = 2,
				ReportId = 4,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			},
			new RoleReport
			{
				RoleReportId = 16,
				RoleId = 3,
				ReportId = 4,
				IsNotDeleted = true,
				CreatedById = 1,
				LastChangedById = 1,
				CreateDate = DateTime.Now,
				LastChangedDate = DateTime.Now
			});
			#endregion

			#region DiscrepancyReasons

			context.DiscrepancyReasons.AddOrUpdate(new DiscrepancyReason
            {
                DiscrepancyReasonId = 1,
                Name = "Surplus",
                Description = "Surplus",
                LookUpKey = "SURPLUS",
                IsNotDeleted = true,
                CreatedById = 1,
                CreateDate = DateTime.Now,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now
            },
                new DiscrepancyReason
                {
                    DiscrepancyReasonId = 2,
                    Name = "Shortage",
                    Description = "Shortage",
                    LookUpKey = "SHORTAGE",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }, new DiscrepancyReason
                {
                    DiscrepancyReasonId = 3,
                    Name = "Tampered",
                    Description = "Tampered",
                    LookUpKey = "TAMPERED",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }, new DiscrepancyReason
                {
                    DiscrepancyReasonId = 4,
                    Name = "Missing Containers",
                    Description = "Missing Containers",
                    LookUpKey = "MISSING_CONTAINERS",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }, new DiscrepancyReason
                {
                    DiscrepancyReasonId = 5,
                    Name = "Missing Drops",
                    Description = "Missing Drops",
                    LookUpKey = "MISSING_DROPS",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }, new DiscrepancyReason
                {
                    DiscrepancyReasonId = 6,
                    Name = "Missing Deposits",
                    Description = "Missing Deposits",
                    LookUpKey = "MISSING_DEPOSITS",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }
                , new DiscrepancyReason
                {
                    DiscrepancyReasonId = 7,
                    Name = "Rectify Denomination",
                    Description = "Rectify Denomination",
                    LookUpKey = "RECTIFY_DENOMINATION",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now
                }
                );

            #endregion

            #region Easter Golden Numbers

            context.EasterGoldenNumbers.AddOrUpdate(new EasterGoldenNumber
            {
                EasterGoldenNumberId = 1,
                GoldenNumber = 0,
                Month = 3,
                Day = 27,
                IsNotDeleted = true,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreatedById = 1,
                CreateDate = DateTime.Now
            },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 2,
                    GoldenNumber = 1,
                    Month = 4,
                    Day = 14,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 3,
                    GoldenNumber = 2,
                    Month = 4,
                    Day = 3,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 4,
                    GoldenNumber = 3,
                    Month = 3,
                    Day = 23,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 5,
                    GoldenNumber = 4,
                    Month = 4,
                    Day = 11,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 6,
                    GoldenNumber = 5,
                    Month = 3,
                    Day = 31,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 7,
                    GoldenNumber = 6,
                    Month = 4,
                    Day = 18,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 8,
                    GoldenNumber = 7,
                    Month = 4,
                    Day = 8,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 9,
                    GoldenNumber = 8,
                    Month = 3,
                    Day = 28,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 10,
                    GoldenNumber = 9,
                    Month = 4,
                    Day = 16,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 11,
                    GoldenNumber = 10,
                    Month = 4,
                    Day = 5,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 12,
                    GoldenNumber = 11,
                    Month = 3,
                    Day = 25,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 13,
                    GoldenNumber = 12,
                    Month = 4,
                    Day = 13,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 14,
                    GoldenNumber = 13,
                    Month = 4,
                    Day = 2,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 15,
                    GoldenNumber = 14,
                    Month = 3,
                    Day = 22,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 16,
                    GoldenNumber = 15,
                    Month = 4,
                    Day = 10,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 17,
                    GoldenNumber = 16,
                    Month = 3,
                    Day = 30,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 18,
                    GoldenNumber = 17,
                    Month = 4,
                    Day = 17,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 19,
                    GoldenNumber = 18,
                    Month = 4,
                    Day = 7,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                },
                new EasterGoldenNumber
                {
                    EasterGoldenNumberId = 20,
                    GoldenNumber = 19,
                    Month = 3,
                    Day = 27,
                    IsNotDeleted = true,
                    LastChangedById = 1,
                    LastChangedDate = DateTime.Now,
                    CreatedById = 1,
                    CreateDate = DateTime.Now
                });

            #endregion
            
            #region Error Codes

            context.ErrorCodes.AddOrUpdate(new ErrorCode
            {
                ErrorCodeId = 1,
                Code = "0016",
                Description = "The transaction type is blank or invalid",
                LookUpKey = "0016",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
                new ErrorCode
                {
                    ErrorCodeId = 2,
                    Code = "0020",
                    Description = "Document type is blank or invalid",
                    LookUpKey = "0020",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 3,
                    Code = "0024",
                    Description = "Invalid transaction type",
                    LookUpKey = "0024",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 4,
                    Code = "0080",
                    Description = "Amount is less than zero",
                    LookUpKey = "0080",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 5,
                    Code = "1009",
                    Description = "Invalid bank account number",
                    LookUpKey = "1009",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 6,
                    Code = "1014",
                    Description = "Bank account details not transmitted",
                    LookUpKey = "1014",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 7,
                    Code = "1042",
                    Description = "Client branch code invalid for FEDI transaction",
                    LookUpKey = "1042",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 8,
                    Code = "1047",
                    Description = "FEDI not available for this bank at present",
                    LookUpKey = "1047",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 9,
                    Code = "1054",
                    Description = "Invalid branch code",
                    LookUpKey = "1054",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 10,
                    Code = "1055",
                    Description = "Invalid bank acc number/invalid acc number acc type combination",
                    LookUpKey = "1055",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 11,
                    Code = "1056",
                    Description = "Account type not valid for this branch",
                    LookUpKey = "1056",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 12,
                    Code = "1057",
                    Description = "Invalid branch code or blank",
                    LookUpKey = "1057",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 13,
                    Code = "1058",
                    Description = "FNB saving account, debit order not allowed",
                    LookUpKey = "1058",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 14,
                    Code = "1059",
                    Description = "Account number too short/long",
                    LookUpKey = "1059",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 15,
                    Code = "1078",
                    Description = "Duplicate transaction sent to Hyphen",
                    LookUpKey = "1078",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 16,
                    Code = "1084",
                    Description = "Bond account type '4' must be loaded as '1' ",
                    LookUpKey = "1084",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 17,
                    Code = "2061",
                    Description = "Transaction cannot be created on credit card account",
                    LookUpKey = "2061",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 18,
                    Code = "2062",
                    Description = "Credit card account number in invalid",
                    LookUpKey = "2062",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 19,
                    Code = "2080",
                    Description = "Invalid branch",
                    LookUpKey = "2080",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 20,
                    Code = "2138",
                    Description = "Record inactive on master file (Nominated payments)",
                    LookUpKey = "2138",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 21,
                    Code = "2147",
                    Description = "Value exceeded for NPS limit",
                    LookUpKey = "2147",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 22,
                    Code = "2150",
                    Description = "Reference not loaded on master file (Nominated payments)",
                    LookUpKey = "2150",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 23,
                    Code = "2201",
                    Description = "Amount greater than R5000 for NAEDO transactions",
                    LookUpKey = "2201",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 24,
                    Code = "2202",
                    Description = "Service branch. EFT not allowed",
                    LookUpKey = "2202",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 25,
                    Code = "2203",
                    Description = "Zero amount received in mail",
                    LookUpKey = "2203",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 26,
                    Code = "7777",
                    Description = "Technical error on Hyphen system. Please contact Hyphen Help Desk",
                    LookUpKey = "7777",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 27,
                    Code = "8888",
                    Description = "Redirect information. Update line of business with new account details",
                    LookUpKey = "8888",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 28,
                    Code = "HB",
                    Description = "Item Limit exceeded",
                    LookUpKey = "HB",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 29,
                    Code = "HC",
                    Description = "Non participating branch",
                    LookUpKey = "HC",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 30,
                    Code = "HD",
                    Description = "Invalid account type for NAEDO",
                    LookUpKey = "HD",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 31,
                    Code = "HA",
                    Description = "General PACS load report error",
                    LookUpKey = "HA",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 32,
                    Code = "HE",
                    Description = "All transactions rejected",
                    LookUpKey = "HE",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 33,
                    Code = "HF",
                    Description = "Aggregate limit exceeded",
                    LookUpKey = "HF",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 34,
                    Code = "HG",
                    Description = "Hom brn/acc no. invalid",
                    LookUpKey = "HG",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 35,
                    Code = "HH",
                    Description = "X Border Txn Not allowed",
                    LookUpKey = "HH",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 36,
                    Code = "H1",
                    Description = "Manual Unpaid",
                    LookUpKey = "H1",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 37,
                    Code = "02",
                    Description = "Not Provided For/not enough money in the account",
                    LookUpKey = "02",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 38,
                    Code = "03",
                    Description = "Debits not Allowed on Account/debit order for registered accounts not allowed",
                    LookUpKey = "03",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 39,
                    Code = "04",
                    Description = "Payment Stopped/payment stopped by bank or payee",
                    LookUpKey = "04",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 40,
                    Code = "06",
                    Description = "Account Frozen/no interaction with account possible as it is frozen by the bank",
                    LookUpKey = "06",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 41,
                    Code = "08",
                    Description = "In Sequestration – Private/no debit allowed",
                    LookUpKey = "08",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 42,
                    Code = "10",
                    Description = "In Liquidation – Company/no debit from a company account possible",
                    LookUpKey = "10",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 43,
                    Code = "12",
                    Description = "Account Closed/account closed by payee or bank",
                    LookUpKey = "12",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 44,
                    Code = "14",
                    Description = "Account Transferred within GRP",
                    LookUpKey = "14",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 45,
                    Code = "16",
                    Description = "Account Transferred Other Bank",
                    LookUpKey = "16",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 46,
                    Code = "18",
                    Description = "Account Holder Deceased",
                    LookUpKey = "18",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 47,
                    Code = "22",
                    Description = "Account Effects Not Cleared/settlement still need to take place in account",
                    LookUpKey = "22",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 48,
                    Code = "26",
                    Description = "No Such Account",
                    LookUpKey = "26",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 49,
                    Code = "28",
                    Description = "Recall/Withdrawal",
                    LookUpKey = "28",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 50,
                    Code = "30",
                    Description = "No Authority to Debit/not authorised to debit the account",
                    LookUpKey = "30",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 51,
                    Code = "31",
                    Description = "User Branch is not Numeric",
                    LookUpKey = "31",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 52,
                    Code = "32",
                    Description = "Debit in Contravention",
                    LookUpKey = "32",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 53,
                    Code = "34",
                    Description = "Authorisation Cancelled",
                    LookUpKey = "34",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 54,
                    Code = "36",
                    Description = "Stopped Via Stop Payment Adv",
                    LookUpKey = "36",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 55,
                    Code = "37",
                    Description = "Sequence Number not in Asc Ord",
                    LookUpKey = "37",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 56,
                    Code = "38",
                    Description = "Homing Branch Number Not Num",
                    LookUpKey = "38",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 57,
                    Code = "39",
                    Description = "Homing Branch Number is Zero",
                    LookUpKey = "39",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 58,
                    Code = "40",
                    Description = "Homing Account Number Not Num",
                    LookUpKey = "40",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 59,
                    Code = "41",
                    Description = "Home & Sub Acc Field Con Data",
                    LookUpKey = "41",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 60,
                    Code = "42",
                    Description = "Acc Type is Not Numeric",
                    LookUpKey = "42",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 61,
                    Code = "43",
                    Description = "Amount is not Numeric",
                    LookUpKey = "43",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 62,
                    Code = "44",
                    Description = "Amount is Zeros",
                    LookUpKey = "44",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 63,
                    Code = "45",
                    Description = "Account Failed Validation",
                    LookUpKey = "45",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 64,
                    Code = "46",
                    Description = "Entry Class Not Numeric",
                    LookUpKey = "46",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 65,
                    Code = "47",
                    Description = "Entry Class is Zero",
                    LookUpKey = "47",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 66,
                    Code = "48",
                    Description = "Action Date Not Numeric",
                    LookUpKey = "48",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 67,
                    Code = "49",
                    Description = "Action Date Ne Current Proc Date",
                    LookUpKey = "49",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 68,
                    Code = "50",
                    Description = "Other",
                    LookUpKey = "50",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 69,
                    Code = "51",
                    Description = "Home Acc Name is Spaces",
                    LookUpKey = "51",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 70,
                    Code = "52",
                    Description = "User Ref in Lower Case",
                    LookUpKey = "52",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 71,
                    Code = "53",
                    Description = "Homing Acc Name is Lower Case",
                    LookUpKey = "53",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 72,
                    Code = "54",
                    Description = "Unable to Convert Branch",
                    LookUpKey = "54",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 73,
                    Code = "55",
                    Description = "User Reference is Spaces",
                    LookUpKey = "55",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 74,
                    Code = "56",
                    Description = "Not FICA Compliant",
                    LookUpKey = "56",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 75,
                    Code = "57",
                    Description = "Invalid Entry Class/Record ID",
                    LookUpKey = "57",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 76,
                    Code = "58",
                    Description = "Credit Limit Exceeded",
                    LookUpKey = "58",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 77,
                    Code = "59",
                    Description = "Debit Limit Exceeded",
                    LookUpKey = "59",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 78,
                    Code = "60",
                    Description = "Cust Floor Limit Exc NPS Limit",
                    LookUpKey = "60",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 79,
                    Code = "61",
                    Description = "NPS XBorder Not Allowed/not allowed",
                    LookUpKey = "61",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 80,
                    Code = "62",
                    Description = "NPS Limit Exceeded/not allowed",
                    LookUpKey = "62",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 81,
                    Code = "64",
                    Description = "Entry Class 18 Invalid for USR",
                    LookUpKey = "64",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 82,
                    Code = "65",
                    Description = "Entry Class Not Allowed ACB",
                    LookUpKey = "65",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 83,
                    Code = "67",
                    Description = "ACC Not Found on SMB XRef",
                    LookUpKey = "67",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 84,
                    Code = "68",
                    Description = "Home & Sub Acc Field Both Zero",
                    LookUpKey = "68",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 85,
                    Code = "69",
                    Description = "Namibia Debit Not Allowed/debit order of a Namibian account not allowed",
                    LookUpKey = "69",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 86,
                    Code = "70",
                    Description = "Sub Acc Nbr Not Valid for Bran",
                    LookUpKey = "70",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 87,
                    Code = "71",
                    Description = "Hom/Sub Acc Nbr Combo Invalid",
                    LookUpKey = "71",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 88,
                    Code = "72",
                    Description = "Invalid Account Type Card Acc",
                    LookUpKey = "72",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 89,
                    Code = "73",
                    Description = "User Ref/Home Acc Nme Inv Char",
                    LookUpKey = "73",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 90,
                    Code = "74",
                    Description = "Home Branch is Closed",
                    LookUpKey = "74",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 91,
                    Code = "75",
                    Description = "Home Branch Not EFT Branch",
                    LookUpKey = "75",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 92,
                    Code = "76",
                    Description = "Acc Not Found on SWABOU XRef",
                    LookUpKey = "76",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 93,
                    Code = "77",
                    Description = "Inv Transaction Ref",
                    LookUpKey = "77",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 94,
                    Code = "88",
                    Description = "Possible Stop Payment/payee stopped payment at branch level",
                    LookUpKey = "88",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 95,
                    Code = "89",
                    Description = "Distribution Upfront Rejection",
                    LookUpKey = "89",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 96,
                    Code = "95",
                    Description = "No Cross Border Trxn Allowed",
                    LookUpKey = "95",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 97,
                    Code = "E1",
                    Description = "Payer request to stop presentation",
                    LookUpKey = "E1",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 98,
                    Code = "E4",
                    Description = "Catch All (general rejection code)",
                    LookUpKey = "E4",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 99,
                    Code = "E8",
                    Description = "Successful Recall",
                    LookUpKey = "E8",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 100,
                    Code = "E9",
                    Description = "Unsuccessful Recall",
                    LookUpKey = "E9",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 101,
                    Code = "F0",
                    Description = "Transaction Failed in Validation",
                    LookUpKey = "F0",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 102,
                    Code = "W0",
                    Description = "Waiting to be posted",
                    LookUpKey = "W0",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 103,
                    Code = "F1",
                    Description = "Transaction Duplicated",
                    LookUpKey = "F1",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 104,
                    Code = "F2",
                    Description = "Transactions Disputed by Account Holder",
                    LookUpKey = "F2",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 105,
                    Code = "40",
                    Description = "Item Limit Exceeded",
                    LookUpKey = "40",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 106,
                    Code = "42",
                    Description = "AEDO MAC Verification Failure",
                    LookUpKey = "42",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 107,
                    Code = "44",
                    Description = "Unable to Process",
                    LookUpKey = "44",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 108,
                    Code = "46",
                    Description = "Account in Advance",
                    LookUpKey = "46",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                },
                new ErrorCode
                {
                    ErrorCodeId = 109,
                    Code = "48",
                    Description = "Account Number Fails CDV Routine",
                    LookUpKey = "48",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now
                });

            #endregion
            
            #region ServiceTypes

            context.ServiceTypes.AddOrUpdate(new ServiceType()
            {
                ServiceTypeId = 1,
                Name = "Same Day Settlement",
                Description = "Same Day Settlement",
                LookUpKey = "SAME_DAY_SETTLEMENT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            },
            new ServiceType()
            {
                ServiceTypeId = 2,
                Name = "Next Day Settlement",
                Description = "Next Day Settlement",
                LookUpKey = "NEXT_DAY_SETTLEMENT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            },
            new ServiceType()
            {
                ServiceTypeId = 3,
                Name = "Real Time Settlement",
                Description = "Real Time Settlement",
                LookUpKey = "REAL_DAY_SETTLEMENT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            });
            #endregion
            
            #region Nedbank Service Types

            context.NedbankServiceTypes.AddOrUpdate(new NedbankServiceType
            {
                ServiceType = "01",
                Name = "Same Day Value",
                ShortName = "SDV",
                Description = "Same Day Value",
                LookUpKey = "SDV",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            },
            new NedbankServiceType
            {
                ServiceType = "02",
                Name = "Realtime Line",
                ShortName = "RTL",
                Description = "Realtime Line",
                LookUpKey = "RTL",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            },
            new NedbankServiceType
            {
                ServiceType = "03",
                Name = "One Day",
                ShortName = "One Day",
                Description = "One Day",
                LookUpKey = "ONE_DAY",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            },
            new NedbankServiceType
            {
                ServiceType = "04",
                Name = "Two Day",
                ShortName = "Two Day",
                Description = "Two Day",
                LookUpKey = "TWO_DAY",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            },
            new NedbankServiceType
            {
                ServiceType = "10",
                Name = "Non Authenticated Early Debit Orders",
                ShortName = "NAEDOS",
                Description = "Non Authenticated Early Debit Orders",
                LookUpKey = "NAEDOS",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,

            });
            #endregion
            
            #region Nedbank Client Types

            context.NedbankClientTypes.AddOrUpdate(
            new NedbankClientType
            {
                NedbankClientTypeId = "01",
                Name = "Financial Institution",
                ShortName = "Financial",
                Description = "Financial Institution",
                LookUpKey = "FINANCIAL_INSTITUTION",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankClientType
            {
                NedbankClientTypeId = "02",
                Name = "Private Client",
                ShortName = "Private Client",
                Description = "Private Client",
                LookUpKey = "PRIVATE_CLIENT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankClientType
            {
                NedbankClientTypeId = "03",
                Name = "Private Non-Residential Client",
                ShortName = "Pvt Non-Res Clt",
                Description = "Private Non-Residential Client",
                LookUpKey = "PRIVATE_NON_RESIDENTIAL_CLIENT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            });
            #endregion
            
            #region Nedbank Transaction Types

            context.NedbankTransactionTypes.AddOrUpdate(new NedbankTransactionType
            {
                TransactionType = "0000",
                Name = "Debit",
                Description = "Debit",
                LookUpKey = "DEBIT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankTransactionType
            {
                TransactionType = "9999",
                Name = "Credit",
                Description = "Credit",
                LookUpKey = "CREDIT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            });
            #endregion
            
            #region Nedbank Entry Codes

            context.NedbankEntryCodes.AddOrUpdate(new NedbankEntryCode
            {
                EntryCode = "61",
                LookupKey = "SALARY",
                Description = "Salary",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankEntryCode
            {
                EntryCode = "62",
                LookupKey = "PENSION",
                Description = "Pension",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankEntryCode
            {
                EntryCode = "63",
                LookupKey = "PAYE",
                Description = "PAYE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankEntryCode
            {
                EntryCode = "81",
                LookupKey = "PAYMENT_TO_CREDITOR",
                Description = "Payment to creditor",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            });
            #endregion
            
            #region Nedbank Unpaid Reasons

            context.NedbankUnpaidReasons.AddOrUpdate(
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "02",
                    Reason = "Not provided for (equivalent to R/D on a cheque)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "03",
                    Reason = "Debits not allowed to this account",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "04",
                    Reason = "Payment stopped (by accountholder)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "06",
                    Reason = "Account frozen (as in divorce etc)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "08",
                    Reason = "Account in sequestration (private individual)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "10",
                    Reason = "Account in liquidation (company)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "12",
                    Reason = "Account closed (with no forwarding details)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "14",
                    Reason = "Account transferred within banking group",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "16",
                    Reason = "Account transferred (to another banking group)",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "18",
                    Reason = "Accountholder deceased",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "22",
                    Reason = "Account effects not cleared",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "26",
                    Reason = "No such account",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "28",
                    Reason = "Recall/Withdrawal",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "30",
                    Reason = "No authority to debit",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "32",
                    Reason = "Debit in contravention of payer’s authority",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "34",
                    Reason = "Authorisation cancelled",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "36",
                    Reason = "Previously stopped via stop-payment advice",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "55",
                    Reason = "Reserved for future use",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "56",
                    Reason = "Not FICA compliant",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "57",
                    Reason = "Reserved for future use",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "58",
                    Reason = "Reserved for future use",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "59",
                    Reason = "Reserved for future use",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidReason
                {
                    NedbankUnpaidReasonId = "60",
                    Reason = "Reserved for future use",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                });
            #endregion
            
            #region Nedbank Client Profile

            context.NedbankClientProfiles.AddOrUpdate(new NedbankClientProfile
            {
                ProfileNumber = "1730",
                NedbankClientTypeId = "01",
                Prefix = "C8",
                ClientName = "SBV Services Pvt Ltd",
                LookupKey = "CLIENT_PROFILE_NUMBER",
                NominatedAccountNumber = "1016231946",
                ChargesAccountNumber = "1469155780",
                StatementNarrative = null,
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            });
            #endregion
            
            #region Nedbank Record Types

            context.NedbankUnpaidsRecordTypes.AddOrUpdate(
                new NedbankUnpaidsRecordType
                {
                    NedbankUnpaidsRecordTypeId = "01",
                    Description = "Unpaid",
                    LookupKey = "UNPAID",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidsRecordType
                {
                    NedbankUnpaidsRecordTypeId = "02",
                    Description = "home back",
                    LookupKey = "HOMEBACK",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                },
                new NedbankUnpaidsRecordType
                {
                    NedbankUnpaidsRecordTypeId = "03",
                    Description = "Redirect",
                    LookupKey = "REDIRECT",
                    IsNotDeleted = true,
                    CreatedById = 1,
                    LastChangedById = 1,
                    CreateDate = DateTime.Now,
                    LastChangedDate = DateTime.Now,
                });
            #endregion
            
            #region Nedbank File Types

            context.NedbankInstructionFileTypes.AddOrUpdate(new NedbankInstructionFileType
            {
                FileType = "01",
                FileTypeName = "Transaction Instruction",
                LookupKey = "TRANSACTION_INSTRUCTION",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            },
            new NedbankInstructionFileType
            {
                FileType = "02",
                FileTypeName = "Disallow Instruction",
                LookupKey = "DISALLOW_INSTRUCTION",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now,
            });
            #endregion
            
            #region Fee Types

            context.FeeTypes.AddOrUpdate(new FeeType()
            {
                FeeTypeId = 1,
                Name = "Standard Fee",
                Description = "Standard Fee",
                LookUpKey = "STANDARD_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new FeeType()
            {
                FeeTypeId = 2,
                Name = "Fixed Fee",
                Description = "Fixed Fee",
                LookUpKey = "FIXED_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            });

            #endregion
            
            #region Fees

            FeesMigration.SeedFees(context);

            #endregion
            
            #region VaultTransactionTypes
            
            context.VaultTransactionTypes.AddOrUpdate(new VaultTransactionType()
            {
                VaultTransactionTypeId = 1,
                Code = 02,
                Name = "TR02",
                Description = "CIT",
                LookUpKey = "CIT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },new VaultTransactionType()
            {
                VaultTransactionTypeId = 2,
                Code = 22,
                Name = "TR22",
                Description = "Deposit",
                LookUpKey = "DEPOSIT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },new VaultTransactionType()
            {
                VaultTransactionTypeId = 3,
                Code = 21,
                Name = "TR21",
                Description = "Deposit With Change",
                LookUpKey = "DEPOSIT_CHANGE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            },
            new VaultTransactionType()
            {
                VaultTransactionTypeId = 4,
                Code = 00,
                Name = "TR00",
                Description = "Beneficiary Payment",
                LookUpKey = "PAYMENT",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                CreateDate = DateTime.Now,
                LastChangedDate = DateTime.Now
            });

            #endregion

            #region SQL Commands

            context.Database.ExecuteSqlCommand(DbHelpers.CleanUp);
			context.Database.ExecuteSqlCommand(DbHelpers.DepositSlipHeader);
			context.Database.ExecuteSqlCommand(DbHelpers.CashDepositAuditTrailDetails);
			context.Database.ExecuteSqlCommand(DbHelpers.TransactionSummary);
			context.Database.ExecuteSqlCommand(DbHelpers.DepositsWithContainerTotal);
			context.Database.ExecuteSqlCommand(DbHelpers.DepositSlipDetails);
			context.Database.ExecuteSqlCommand(DbHelpers.DepositSlipSubreport);
			context.Database.ExecuteSqlCommand(DbHelpers.DropSlipSubreport);
			context.Database.ExecuteSqlCommand(DbHelpers.DbCashDepositConstraints);
			context.Database.ExecuteSqlCommand(DbHelpers.DbCashOrderConstraints);
			context.Database.ExecuteSqlCommand(DbHelpers.CashProcessingSlipHeader);
			context.Database.ExecuteSqlCommand(DbHelpers.VarienceDropSlipSubreport);
			context.Database.ExecuteSqlCommand(DbHelpers.CashOrderSlipHeader);
			context.Database.ExecuteSqlCommand(DbHelpers.CashOrderingDetails);
			context.Database.ExecuteSqlCommand(DbHelpers.CashOrderedAndPackedDetails);
			context.Database.ExecuteSqlCommand(DbHelpers.CashSubmittedAndVerifiedDetails);
			context.Database.ExecuteSqlCommand(DbHelpers.VaultReport);
            context.Database.ExecuteSqlCommand(DbHelpers.UserRoleIds);
            context.Database.ExecuteSqlCommand(DbHelpers.VaultPartialPaymentHeader);
            context.Database.ExecuteSqlCommand(DbHelpers.HyphenBatchReport);
            
			#endregion

			#region AuditLog Indexes

			DbHelpers.AddIndexes(context);

			#endregion

        }
    }
}