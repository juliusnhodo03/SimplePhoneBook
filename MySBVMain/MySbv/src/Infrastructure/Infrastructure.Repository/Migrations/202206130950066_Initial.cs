namespace Infrastructure.Repository.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Account",
                c => new
                    {
                        AccountId = c.Int(nullable: false, identity: true),
                        SiteId = c.Int(nullable: false),
                        AccountTypeId = c.Int(nullable: false),
                        BankId = c.Int(nullable: false),
                        TransactionTypeId = c.Int(nullable: false),
                        AccountNumber = c.String(),
                        AccountHolderName = c.String(),
                        IfscCode = c.String(),
                        SwiftCode = c.String(),
                        BeneficiaryCode = c.String(),
                        DefaultAccount = c.Boolean(nullable: false),
                        ToBeDeleted = c.Boolean(nullable: false),
                        StatusId = c.Int(nullable: false),
                        IsApproved = c.Boolean(nullable: false),
                        Comments = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccountId)
                .ForeignKey("dbo.AccountType", t => t.AccountTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Bank", t => t.BankId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.TransactionType", t => t.TransactionTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.AccountTypeId)
                .Index(t => t.BankId)
                .Index(t => t.TransactionTypeId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.AccountType",
                c => new
                    {
                        AccountTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AccountTypeId);
            
            CreateTable(
                "dbo.Bank",
                c => new
                    {
                        BankId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        BranchCode = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BankId);
            
            CreateTable(
                "dbo.Site",
                c => new
                    {
                        SiteId = c.Int(nullable: false, identity: true),
                        MerchantId = c.Int(nullable: false),
                        CitCarrierId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        CityId = c.Int(nullable: false),
                        CashCenterId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        CitCode = c.String(),
                        SysproNumber = c.String(),
                        PostalAddress = c.String(),
                        DepositReference = c.String(),
                        DepositReferenceIsEditable = c.Boolean(nullable: false),
                        ContactPersonName1 = c.String(),
                        ContactPersonEmailAddress1 = c.String(),
                        ContactPersonNumber1 = c.String(),
                        ContactPersonDesignation1 = c.String(),
                        ContactPersonName2 = c.String(),
                        ContactPersonEmailAddress2 = c.String(),
                        ContactPersonNumber2 = c.String(),
                        ContactPersonDesignation2 = c.String(),
                        ApprovalRequiredFlag = c.Boolean(nullable: false),
                        IsCashCentreAllowedDepositCapturing = c.Boolean(nullable: false),
                        Comments = c.String(),
                        ImplementationDate = c.DateTime(),
                        TerminationDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiteId)
                .ForeignKey("dbo.Address", t => t.AddressId)
                .ForeignKey("dbo.City", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.CashCenter", t => t.CashCenterId, cascadeDelete: true)
                .ForeignKey("dbo.CitCarrier", t => t.CitCarrierId, cascadeDelete: true)
                .ForeignKey("dbo.Merchant", t => t.MerchantId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.MerchantId)
                .Index(t => t.CitCarrierId)
                .Index(t => t.StatusId)
                .Index(t => t.CityId)
                .Index(t => t.CashCenterId)
                .Index(t => t.AddressId);
            
            CreateTable(
                "dbo.Address",
                c => new
                    {
                        AddressId = c.Int(nullable: false, identity: true),
                        AddressTypeId = c.Int(nullable: false),
                        AddressLine1 = c.String(),
                        AddressLine2 = c.String(),
                        AddressLine3 = c.String(),
                        PostalCode = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AddressId)
                .ForeignKey("dbo.AddressType", t => t.AddressTypeId, cascadeDelete: true)
                .Index(t => t.AddressTypeId);
            
            CreateTable(
                "dbo.AddressType",
                c => new
                    {
                        AddressTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AddressTypeId);
            
            CreateTable(
                "dbo.CashCenter",
                c => new
                    {
                        CashCenterId = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        AddressId = c.Int(nullable: false),
                        ClusterId = c.Int(nullable: false),
                        Number = c.String(),
                        TelephoneNumber = c.String(),
                        ContactPerson = c.String(),
                        EmailAddress1 = c.String(),
                        EmailAddress2 = c.String(),
                        EmailAddress3 = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                        City_CityId = c.Int(),
                    })
                .PrimaryKey(t => t.CashCenterId)
                .ForeignKey("dbo.Address", t => t.AddressId, cascadeDelete: true)
                .ForeignKey("dbo.City", t => t.City_CityId)
                .ForeignKey("dbo.City", t => t.CityId)
                .ForeignKey("dbo.Cluster", t => t.ClusterId, cascadeDelete: true)
                .Index(t => t.CityId)
                .Index(t => t.AddressId)
                .Index(t => t.ClusterId)
                .Index(t => t.City_CityId);
            
            CreateTable(
                "dbo.City",
                c => new
                    {
                        CityId = c.Int(nullable: false, identity: true),
                        ProvinceId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CityId)
                .ForeignKey("dbo.Province", t => t.ProvinceId, cascadeDelete: true)
                .Index(t => t.ProvinceId);
            
            CreateTable(
                "dbo.Province",
                c => new
                    {
                        ProvinceId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProvinceId)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Country",
                c => new
                    {
                        CountryId = c.Int(nullable: false, identity: true),
                        ContinentId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CountryId)
                .ForeignKey("dbo.Continent", t => t.ContinentId, cascadeDelete: true)
                .Index(t => t.ContinentId);
            
            CreateTable(
                "dbo.Continent",
                c => new
                    {
                        ContinentId = c.Int(nullable: false, identity: true),
                        GeographyId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContinentId)
                .ForeignKey("dbo.Geography", t => t.GeographyId, cascadeDelete: true)
                .Index(t => t.GeographyId);
            
            CreateTable(
                "dbo.Geography",
                c => new
                    {
                        GeographyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.GeographyId);
            
            CreateTable(
                "dbo.Currency",
                c => new
                    {
                        CurrencyId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        Code = c.String(),
                        Symbol = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CurrencyId)
                .ForeignKey("dbo.Country", t => t.CountryId, cascadeDelete: true)
                .Index(t => t.CountryId);
            
            CreateTable(
                "dbo.Cluster",
                c => new
                    {
                        ClusterId = c.Int(nullable: false, identity: true),
                        RegionManagerId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ClusterId)
                .ForeignKey("dbo.User", t => t.RegionManagerId)
                .Index(t => t.RegionManagerId);
            
            CreateTable(
                "dbo.User",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        TitleId = c.Int(),
                        CashCenterId = c.Int(),
                        UserTypeId = c.Int(),
                        MerchantId = c.Int(),
                        UserName = c.String(),
                        FirstName = c.String(),
                        LastName = c.String(),
                        IdNumber = c.String(),
                        PassportNumber = c.String(),
                        EmailAddress = c.String(),
                        CellNumber = c.String(),
                        OfficeNumber = c.String(),
                        FaxNumber = c.String(),
                        LockedStatus = c.Boolean(nullable: false),
                        CanMakeVaultPayment = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Merchant", t => t.MerchantId)
                .ForeignKey("dbo.Title", t => t.TitleId)
                .ForeignKey("dbo.UserType", t => t.UserTypeId)
                .ForeignKey("dbo.CashCenter", t => t.CashCenterId)
                .Index(t => t.TitleId)
                .Index(t => t.CashCenterId)
                .Index(t => t.UserTypeId)
                .Index(t => t.MerchantId);
            
            CreateTable(
                "dbo.Merchant",
                c => new
                    {
                        MerchantId = c.Int(nullable: false, identity: true),
                        MerchantDescriptionId = c.Int(),
                        CompanyTypeId = c.Int(nullable: false),
                        StatusId = c.Int(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        Number = c.String(),
                        TradingName = c.String(),
                        CompanyGroupName = c.String(),
                        FranchiseName = c.String(),
                        RegisteredName = c.String(),
                        RegistrationNumber = c.String(),
                        VATNumber = c.String(),
                        ContractNumber = c.String(),
                        EmailAddress = c.String(),
                        PhoneNumber = c.String(),
                        ContactPerson1 = c.String(),
                        ContactPerson2 = c.String(),
                        ContactPerson1Phone = c.String(),
                        ContactPerson2Phone = c.String(),
                        ContactPerson1Designation = c.String(),
                        ContactPerson2Designation = c.String(),
                        ContactPerson1EmailAddress = c.String(),
                        ContactPerson2EmailAddress = c.String(),
                        ContractDocumentUrl = c.String(),
                        WebSiteUrl = c.String(),
                        FinancialAccountant = c.String(),
                        FinancialAccountantEmailAddress = c.String(),
                        DepositSlipEmailIndicator = c.Boolean(nullable: false),
                        Comments = c.String(),
                        ImplementationDate = c.DateTime(),
                        TerminationDate = c.DateTime(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MerchantId)
                .ForeignKey("dbo.CompanyType", t => t.CompanyTypeId, cascadeDelete: true)
                .ForeignKey("dbo.MerchantDescription", t => t.MerchantDescriptionId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.MerchantDescriptionId)
                .Index(t => t.CompanyTypeId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.CompanyType",
                c => new
                    {
                        CompanyTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CompanyTypeId);
            
            CreateTable(
                "dbo.MerchantDescription",
                c => new
                    {
                        MerchantDescriptionId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.MerchantDescriptionId);
            
            CreateTable(
                "dbo.Status",
                c => new
                    {
                        StatusId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StatusId);
            
            CreateTable(
                "dbo.Title",
                c => new
                    {
                        TitleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TitleId);
            
            CreateTable(
                "dbo.UserNotification",
                c => new
                    {
                        UserNotificationId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        NotificationTypeId = c.Int(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserNotificationId)
                .ForeignKey("dbo.NotificationType", t => t.NotificationTypeId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.NotificationTypeId);
            
            CreateTable(
                "dbo.NotificationType",
                c => new
                    {
                        NotificationTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NotificationTypeId);
            
            CreateTable(
                "dbo.UserSite",
                c => new
                    {
                        UserSiteId = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserSiteId)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.SiteId);
            
            CreateTable(
                "dbo.UserType",
                c => new
                    {
                        UserTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.UserTypeId);
            
            CreateTable(
                "dbo.CashDeposit",
                c => new
                    {
                        CashDepositId = c.Int(nullable: false, identity: true),
                        DepositTypeId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        AccountId = c.Int(),
                        ProductTypeId = c.Int(nullable: false),
                        Narrative = c.String(),
                        TransactionReference = c.String(nullable: false, maxLength: 50),
                        DepositedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualAmount = c.Decimal(precision: 18, scale: 2),
                        DiscrepancyAmount = c.Decimal(precision: 18, scale: 2),
                        VaultAmount = c.Decimal(precision: 18, scale: 2),
                        HasDescripancy = c.Boolean(),
                        IsProcessed = c.Boolean(),
                        IsConfirmed = c.Boolean(),
                        ProcessedById = c.Int(),
                        IsSubmitted = c.Boolean(),
                        IsSettled = c.Boolean(),
                        DeviceId = c.Int(),
                        SupervisorId = c.Int(),
                        StatusId = c.Int(nullable: false),
                        ErrorCodeId = c.Int(),
                        iTramsUserName = c.String(),
                        VaultSource = c.String(),
                        SettlementIdentifier = c.String(),
                        CitDateTime = c.DateTime(),
                        ProcessedDateTime = c.DateTime(),
                        SettledDateTime = c.DateTime(),
                        SendDateTime = c.DateTime(),
                        SubmitDateTime = c.DateTime(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                        DeviceType_DeviceTypeId = c.Int(),
                    })
                .PrimaryKey(t => t.CashDepositId)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.DepositType", t => t.DepositTypeId, cascadeDelete: true)
                .ForeignKey("dbo.DeviceType", t => t.DeviceType_DeviceTypeId)
                .ForeignKey("dbo.Device", t => t.DeviceId)
                .ForeignKey("dbo.ErrorCode", t => t.ErrorCodeId)
                .ForeignKey("dbo.ProductType", t => t.ProductTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.DepositTypeId)
                .Index(t => t.SiteId)
                .Index(t => t.AccountId)
                .Index(t => t.ProductTypeId)
                .Index(t => t.DeviceId)
                .Index(t => t.StatusId)
                .Index(t => t.ErrorCodeId)
                .Index(t => t.DeviceType_DeviceTypeId);
            
            CreateTable(
                "dbo.Container",
                c => new
                    {
                        ContainerId = c.Int(nullable: false, identity: true),
                        CashDepositId = c.Int(nullable: false),
                        ContainerTypeId = c.Int(nullable: false),
                        ReferenceNumber = c.String(),
                        SerialNumber = c.String(),
                        SealNumber = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscrepancyAmount = c.Decimal(precision: 18, scale: 2),
                        ActualAmount = c.Decimal(precision: 18, scale: 2),
                        IsPrimaryContainer = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContainerId)
                .ForeignKey("dbo.CashDeposit", t => t.CashDepositId, cascadeDelete: true)
                .ForeignKey("dbo.ContainerType", t => t.ContainerTypeId, cascadeDelete: true)
                .Index(t => t.CashDepositId)
                .Index(t => t.ContainerTypeId);
            
            CreateTable(
                "dbo.ContainerDrop",
                c => new
                    {
                        ContainerDropId = c.Int(nullable: false, identity: true),
                        ContainerId = c.Int(nullable: false),
                        DiscrepancyReasonId = c.Int(),
                        StatusId = c.Int(nullable: false),
                        Narrative = c.String(),
                        ReferenceNumber = c.String(),
                        BagSerialNumber = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        HasDiscrepancy = c.Boolean(nullable: false),
                        DiscrepancyAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comment = c.String(),
                        Number = c.Int(nullable: false),
                        ErrorCodeId = c.Int(),
                        SettlementIdentifier = c.String(),
                        DuplicateChecksum = c.String(),
                        SettlementDateTime = c.DateTime(),
                        SendDateTime = c.DateTime(),
                        TransactionDateTime = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContainerDropId)
                .ForeignKey("dbo.Container", t => t.ContainerId, cascadeDelete: true)
                .ForeignKey("dbo.DiscrepancyReason", t => t.DiscrepancyReasonId)
                .ForeignKey("dbo.ErrorCode", t => t.ErrorCodeId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.ContainerId)
                .Index(t => t.DiscrepancyReasonId)
                .Index(t => t.StatusId)
                .Index(t => t.ErrorCodeId);
            
            CreateTable(
                "dbo.ContainerDropItem",
                c => new
                    {
                        ContainerDropItemId = c.Int(nullable: false, identity: true),
                        ContainerDropId = c.Int(nullable: false),
                        DenominationId = c.Int(nullable: false),
                        ValueInCents = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscrepancyCount = c.Int(),
                        DiscrepancyValue = c.Decimal(precision: 18, scale: 2),
                        ActualCount = c.Int(),
                        ActualValue = c.Decimal(precision: 18, scale: 2),
                        DenominationType = c.String(),
                        DenominationName = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContainerDropItemId)
                .ForeignKey("dbo.ContainerDrop", t => t.ContainerDropId, cascadeDelete: true)
                .ForeignKey("dbo.Denomination", t => t.DenominationId, cascadeDelete: true)
                .Index(t => t.ContainerDropId)
                .Index(t => t.DenominationId);
            
            CreateTable(
                "dbo.Denomination",
                c => new
                    {
                        DenominationId = c.Int(nullable: false, identity: true),
                        CountryId = c.Int(nullable: false),
                        DenominationTypeId = c.Int(nullable: false),
                        ValueInCents = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DenominationId)
                .ForeignKey("dbo.DenominationType", t => t.DenominationTypeId, cascadeDelete: true)
                .Index(t => t.DenominationTypeId);
            
            CreateTable(
                "dbo.DenominationType",
                c => new
                    {
                        DenominationTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DenominationTypeId);
            
            CreateTable(
                "dbo.DiscrepancyReason",
                c => new
                    {
                        DiscrepancyReasonId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DiscrepancyReasonId);
            
            CreateTable(
                "dbo.ErrorCode",
                c => new
                    {
                        ErrorCodeId = c.Int(nullable: false, identity: true),
                        Code = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ErrorCodeId);
            
            CreateTable(
                "dbo.ContainerType",
                c => new
                    {
                        ContainerTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContainerTypeId);
            
            CreateTable(
                "dbo.ContainerTypeAttribute",
                c => new
                    {
                        ContainerTypeAttributeId = c.Int(nullable: false, identity: true),
                        ContainerTypeId = c.Int(nullable: false),
                        Attribute1 = c.String(),
                        Attribute1MinLength = c.Int(nullable: false),
                        Attribute1MaxLength = c.Int(nullable: false),
                        Attribute2 = c.String(),
                        Attribute2MaxLength = c.Int(),
                        Attribute2MinLength = c.Int(),
                        Attribute3 = c.String(),
                        Attribute3MinLength = c.Int(),
                        Attribute3MaxLength = c.Int(),
                        Attribute4 = c.String(),
                        Attribute4MinLength = c.Int(),
                        Attribute4MaxLength = c.Int(),
                        Attribute5 = c.String(),
                        Attribute5MaxLength = c.Int(),
                        Attribute5MinLength = c.Int(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ContainerTypeAttributeId)
                .ForeignKey("dbo.ContainerType", t => t.ContainerTypeId, cascadeDelete: true)
                .Index(t => t.ContainerTypeId);
            
            CreateTable(
                "dbo.DepositType",
                c => new
                    {
                        DepositTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DepositTypeId);
            
            CreateTable(
                "dbo.Device",
                c => new
                    {
                        DeviceId = c.Int(nullable: false, identity: true),
                        DeviceTypeId = c.Int(),
                        SerialNumber = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                        Supplier_SupplierId = c.Int(),
                    })
                .PrimaryKey(t => t.DeviceId)
                .ForeignKey("dbo.Supplier", t => t.Supplier_SupplierId)
                .ForeignKey("dbo.DeviceType", t => t.DeviceTypeId)
                .Index(t => t.DeviceTypeId)
                .Index(t => t.Supplier_SupplierId);
            
            CreateTable(
                "dbo.DeviceType",
                c => new
                    {
                        DeviceTypeId = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        ManufacturerId = c.Int(nullable: false),
                        Model = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.DeviceTypeId)
                .ForeignKey("dbo.Manufacturer", t => t.ManufacturerId, cascadeDelete: true)
                .ForeignKey("dbo.Supplier", t => t.SupplierId)
                .Index(t => t.SupplierId)
                .Index(t => t.ManufacturerId);
            
            CreateTable(
                "dbo.Manufacturer",
                c => new
                    {
                        ManufacturerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ManufacturerId);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        SupplierId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SupplierId);
            
            CreateTable(
                "dbo.ProductType",
                c => new
                    {
                        ProductTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProductTypeId);
            
            CreateTable(
                "dbo.VaultBeneficiary",
                c => new
                    {
                        VaultBeneficiaryId = c.Int(nullable: false, identity: true),
                        CashDepositId = c.Int(nullable: false),
                        ContainerDropId = c.Int(nullable: false),
                        AccountId = c.Int(),
                        DeviceUserName = c.String(),
                        DeviceUserRole = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultBeneficiaryId)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.CashDeposit", t => t.CashDepositId, cascadeDelete: true)
                .ForeignKey("dbo.ContainerDrop", t => t.ContainerDropId)
                .Index(t => t.CashDepositId)
                .Index(t => t.ContainerDropId)
                .Index(t => t.AccountId);
            
            CreateTable(
                "dbo.CitCarrier",
                c => new
                    {
                        CitCarrierId = c.Int(nullable: false, identity: true),
                        SerialStartNumber = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CitCarrierId);
            
            CreateTable(
                "dbo.SiteContainer",
                c => new
                    {
                        SiteId = c.Int(nullable: false),
                        ContainerTypeId = c.Int(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.SiteId, t.ContainerTypeId })
                .ForeignKey("dbo.ContainerType", t => t.ContainerTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .Index(t => t.SiteId)
                .Index(t => t.ContainerTypeId);
            
            CreateTable(
                "dbo.TransactionType",
                c => new
                    {
                        TransactionTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransactionTypeId);
            
            CreateTable(
                "dbo.ApprovalObjects",
                c => new
                    {
                        ApprovalObjectsId = c.Int(nullable: false, identity: true),
                        NewObject = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ApprovalObjectsId);
            
            CreateTable(
                "dbo.AuditLog",
                c => new
                    {
                        AuditId = c.Int(nullable: false, identity: true),
                        AuditState = c.String(),
                        TableName = c.String(),
                        RecordId = c.Int(nullable: false),
                        ColumnName = c.String(),
                        OriginalValue = c.String(),
                        NewValue = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.AuditId);
            
            CreateTable(
                "dbo.CashOrderContainerDropItem",
                c => new
                    {
                        CashOrderContainerDropItemId = c.Int(nullable: false, identity: true),
                        CashOrderContainerDropId = c.Int(nullable: false),
                        DenominationId = c.Int(nullable: false),
                        ValueInCents = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VerifiedCount = c.Int(),
                        VerifiedValue = c.Decimal(precision: 18, scale: 2),
                        PackedCount = c.Int(),
                        PackedValue = c.Decimal(precision: 18, scale: 2),
                        DenominationType = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CashOrderContainerDropItemId)
                .ForeignKey("dbo.CashOrderContainerDrop", t => t.CashOrderContainerDropId, cascadeDelete: true)
                .ForeignKey("dbo.Denomination", t => t.DenominationId, cascadeDelete: true)
                .Index(t => t.CashOrderContainerDropId)
                .Index(t => t.DenominationId);
            
            CreateTable(
                "dbo.CashOrderContainerDrop",
                c => new
                    {
                        CashOrderContainerDropId = c.Int(nullable: false, identity: true),
                        CashOrderContainerId = c.Int(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VerifiedAmount = c.Decimal(precision: 18, scale: 2),
                        PackedAmount = c.Decimal(precision: 18, scale: 2),
                        IsCashRequiredInExchange = c.Boolean(nullable: false),
                        IsCashForwardedForExchange = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CashOrderContainerDropId)
                .ForeignKey("dbo.CashOrderContainer", t => t.CashOrderContainerId, cascadeDelete: true)
                .Index(t => t.CashOrderContainerId);
            
            CreateTable(
                "dbo.CashOrderContainer",
                c => new
                    {
                        CashOrderContainerId = c.Int(nullable: false, identity: true),
                        SerialNumber = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VerifiedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PackedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CashOrderContainerId);
            
            CreateTable(
                "dbo.CashOrder",
                c => new
                    {
                        CashOrderId = c.Int(nullable: false, identity: true),
                        CashOrderTypeId = c.Int(nullable: false),
                        CashOrderContainerId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        IsSubmitted = c.Boolean(nullable: false),
                        IsProcessed = c.Boolean(nullable: false),
                        ReferenceNumber = c.String(nullable: false, maxLength: 50),
                        ContainerNumberWithCashForExchange = c.String(),
                        EmptyContainerOrBagNumber = c.String(),
                        DeliveryDate = c.String(),
                        CashOrderAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DateSubmitted = c.DateTime(),
                        DateProcessed = c.DateTime(),
                        OrderDate = c.DateTime(),
                        Comments = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CashOrderId)
                .ForeignKey("dbo.CashOrderContainer", t => t.CashOrderContainerId)
                .ForeignKey("dbo.CashOrderType", t => t.CashOrderTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.CashOrderTypeId)
                .Index(t => t.CashOrderContainerId)
                .Index(t => t.SiteId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.CashOrderType",
                c => new
                    {
                        CashOrderTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CashOrderTypeId);
            
            CreateTable(
                "dbo.CashOrderTask",
                c => new
                    {
                        CashOrderTaskId = c.Int(nullable: false, identity: true),
                        CashOrderId = c.Int(nullable: false),
                        ReferenceNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        SiteId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        RequestUrl = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.CashOrderTaskId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.SiteId)
                .Index(t => t.UserId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.CitRequestDetail",
                c => new
                    {
                        CitRequestDetailId = c.Int(nullable: false, identity: true),
                        CashDepositId = c.Long(nullable: false),
                        BeneficiaryCode = c.String(nullable: false, maxLength: 20),
                        CitCode = c.String(nullable: false, maxLength: 20),
                        DeviceSerialNumber = c.String(nullable: false, maxLength: 100),
                        BagSerialNumber = c.String(nullable: false, maxLength: 100),
                        TransactionDate = c.DateTime(nullable: false),
                        ItramsReference = c.String(maxLength: 100),
                        UserReferance = c.String(maxLength: 100),
                        IsReceiptPrinted = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                        CashDeposit_CashDepositId = c.Int(),
                    })
                .PrimaryKey(t => t.CitRequestDetailId)
                .ForeignKey("dbo.CashDeposit", t => t.CashDeposit_CashDepositId)
                .Index(t => t.CashDeposit_CashDepositId);
            
            CreateTable(
                "dbo.EasterGoldenNumber",
                c => new
                    {
                        EasterGoldenNumberId = c.Int(nullable: false, identity: true),
                        GoldenNumber = c.Int(nullable: false),
                        Day = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EasterGoldenNumberId);
            
            CreateTable(
                "dbo.ErrorLogging",
                c => new
                    {
                        ErrorLoggingId = c.Int(nullable: false, identity: true),
                        Host = c.String(maxLength: 20),
                        Version = c.String(maxLength: 20),
                        Exception = c.String(),
                        Date = c.DateTime(nullable: false),
                        Message = c.String(),
                        Level = c.String(maxLength: 10),
                        Logger = c.String(maxLength: 150),
                    })
                .PrimaryKey(t => t.ErrorLoggingId);
            
            CreateTable(
                "dbo.Fee",
                c => new
                    {
                        FeeId = c.Int(nullable: false, identity: true),
                        FeeTypeId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Code = c.String(),
                        Value = c.Double(nullable: false),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FeeId)
                .ForeignKey("dbo.FeeType", t => t.FeeTypeId, cascadeDelete: true)
                .Index(t => t.FeeTypeId);
            
            CreateTable(
                "dbo.FeeType",
                c => new
                    {
                        FeeTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FeeTypeId);
            
            CreateTable(
                "dbo.BatchFile",
                c => new
                    {
                        BatchFileId = c.Int(nullable: false, identity: true),
                        BatchCount = c.Int(nullable: false),
                        DateTimeCreated = c.DateTime(nullable: false),
                        HeaderRecordId = c.Int(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                        TrailerRecordId = c.Int(nullable: false),
                        FileName = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.BatchFileId)
                .ForeignKey("dbo.HeaderRecord", t => t.HeaderRecordId, cascadeDelete: true)
                .ForeignKey("dbo.TrailerRecord", t => t.TrailerRecordId, cascadeDelete: true)
                .Index(t => t.HeaderRecordId)
                .Index(t => t.TrailerRecordId);
            
            CreateTable(
                "dbo.HeaderRecord",
                c => new
                    {
                        HeaderRecordId = c.Int(nullable: false, identity: true),
                        BatchNumber = c.String(),
                        MessageType = c.String(),
                        TransmissionDate = c.String(),
                        TransmissionTime = c.String(),
                        Blank1 = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HeaderRecordId);
            
            CreateTable(
                "dbo.LoadReport",
                c => new
                    {
                        LoadReportId = c.Int(nullable: false, identity: true),
                        BatchFileId = c.Int(nullable: false),
                        BatchNumber = c.String(),
                        TotalReceivedCount = c.Int(nullable: false),
                        TotalReceivedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalRejectedAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalRejectedCount = c.Int(nullable: false),
                        TransmissionDateTime = c.DateTime(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.LoadReportId)
                .ForeignKey("dbo.BatchFile", t => t.BatchFileId, cascadeDelete: true)
                .Index(t => t.BatchFileId);
            
            CreateTable(
                "dbo.TrailerRecord",
                c => new
                    {
                        TrailerRecordId = c.Int(nullable: false, identity: true),
                        BatchNumber = c.String(),
                        Checksum = c.String(),
                        MessageType = c.String(),
                        NumberOfTransactions = c.String(),
                        TotalValue = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TrailerRecordId);
            
            CreateTable(
                "dbo.TransactionDetailRecordResponse",
                c => new
                    {
                        TransactionDetailRecordResponseId = c.Int(nullable: false, identity: true),
                        SettlementIdentifier = c.String(),
                        AccountType = c.String(),
                        AgencyNumber = c.String(),
                        AgencyPrefix = c.String(),
                        BankAccountNumber = c.String(),
                        BatchNumber = c.String(),
                        Blank2 = c.String(),
                        BranchCode = c.String(),
                        DocumentNumber = c.String(),
                        DocumentType = c.String(),
                        ErrorCode = c.String(),
                        MessageType = c.String(),
                        Payee = c.String(),
                        ProcessingOption1 = c.String(),
                        ProcessingOption2 = c.String(),
                        RequisitionNumber = c.String(),
                        TransactionAmount = c.String(),
                        TransactionType = c.String(),
                        UserReference1 = c.String(),
                        UserReference2 = c.String(),
                        ActionDate = c.String(),
                        CashBookBankAccountNumber = c.String(),
                        ChequeClearanceCode = c.String(),
                        ClientChequeNumber = c.String(),
                        Code1 = c.String(),
                        Code2 = c.String(),
                        HashTotal = c.String(),
                        INDF = c.String(),
                        ProgramNameCreated = c.String(),
                        ThirdParty = c.String(),
                        UniqueUserCode = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                        BatchFile_BatchFileId = c.Int(),
                    })
                .PrimaryKey(t => t.TransactionDetailRecordResponseId)
                .ForeignKey("dbo.BatchFile", t => t.BatchFile_BatchFileId)
                .Index(t => t.BatchFile_BatchFileId);
            
            CreateTable(
                "dbo.TransactionDetailRecord",
                c => new
                    {
                        TransactionDetailRecordId = c.Int(nullable: false, identity: true),
                        AccountType = c.String(),
                        AgencyNumber = c.String(),
                        AgencyPrefix = c.String(),
                        BankAccountNumber = c.String(),
                        BatchFileId = c.Int(nullable: false),
                        BatchNumber = c.String(),
                        Blank2 = c.String(),
                        BranchCode = c.String(),
                        DocumentNumber = c.String(),
                        DocumentType = c.String(),
                        ErrorCode = c.String(),
                        MessageType = c.String(),
                        Payee = c.String(),
                        ProcessingOption1 = c.String(),
                        ProcessingOption2 = c.String(),
                        RequisitionNumber = c.String(),
                        TransactionAmount = c.String(),
                        TransactionType = c.String(),
                        UserReference1 = c.String(),
                        UserReference2 = c.String(),
                        ActionDate = c.String(),
                        CashBookBankAccountNumber = c.String(),
                        ChequeClearanceCode = c.String(),
                        ClientChequeNumber = c.String(),
                        Code1 = c.String(),
                        Code2 = c.String(),
                        HashTotal = c.String(),
                        INDF = c.String(),
                        ProgramNameCreated = c.String(),
                        ThirdParty = c.String(),
                        UniqueUserCode = c.String(),
                        Value = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransactionDetailRecordId)
                .ForeignKey("dbo.BatchFile", t => t.BatchFileId, cascadeDelete: true)
                .Index(t => t.BatchFileId);
            
            CreateTable(
                "dbo.Configuration",
                c => new
                    {
                        ConfigurationId = c.Int(nullable: false, identity: true),
                        ConfigName = c.String(),
                        DocumentType = c.String(),
                        TransactionType = c.String(),
                        DailyCutoffTime = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ConfigurationId);
            
            CreateTable(
                "dbo.HyphenScheduler",
                c => new
                    {
                        HyphenSchedulerId = c.Int(nullable: false, identity: true),
                        BatchNumber = c.Int(nullable: false),
                        NumberOfDepositsSent = c.String(),
                        LastRan = c.DateTime(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.HyphenSchedulerId);
            
            CreateTable(
                "Nedbank.BatchFeed",
                c => new
                    {
                        NedbankBatchFileId = c.Int(nullable: false, identity: true),
                        NedbankHeaderRecordId = c.Int(nullable: false),
                        NedbankTrailerRecordId = c.Int(nullable: false),
                        BatchNumber = c.String(nullable: false, maxLength: 15, unicode: false),
                        BatchTotal = c.String(nullable: false, maxLength: 18, unicode: false),
                        BatchCount = c.String(nullable: false, maxLength: 3, unicode: false),
                        BatchDate = c.String(nullable: false, maxLength: 8, unicode: false),
                        IsSent = c.Boolean(nullable: false),
                        FileName = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankBatchFileId)
                .ForeignKey("Nedbank.HeaderItem", t => t.NedbankHeaderRecordId)
                .ForeignKey("Nedbank.TrailerItem", t => t.NedbankTrailerRecordId)
                .Index(t => t.NedbankHeaderRecordId)
                .Index(t => t.NedbankTrailerRecordId);
            
            CreateTable(
                "Nedbank.FileItem",
                c => new
                    {
                        NedbankFileItemId = c.Int(nullable: false, identity: true),
                        NedbankBatchFileId = c.Int(nullable: false),
                        NedbankClientTypeId = c.String(nullable: false, maxLength: 2, unicode: false),
                        AccountId = c.Int(nullable: false),
                        RecordIdentifier = c.String(nullable: false, maxLength: 2, unicode: false),
                        NominatedAccountNumber = c.String(nullable: false, maxLength: 16, unicode: false),
                        PaymentReferenceNumber = c.String(nullable: false, maxLength: 34, unicode: false),
                        DestinationBranchCode = c.String(nullable: false, maxLength: 6, unicode: false),
                        DestinationAccountNumber = c.String(nullable: false, maxLength: 16, unicode: false),
                        Amount = c.String(nullable: false, maxLength: 12, unicode: false),
                        ActionDate = c.String(nullable: false, maxLength: 8, unicode: false),
                        Reference = c.String(nullable: false, maxLength: 30, unicode: false),
                        DestinationAccountHoldersName = c.String(nullable: false, maxLength: 30, unicode: false),
                        TransactionType = c.String(nullable: false, maxLength: 4, unicode: false),
                        ChargesAccountNumber = c.String(nullable: false, maxLength: 16, unicode: false),
                        ServiceType = c.String(nullable: false, maxLength: 2, unicode: false),
                        OriginalPaymentReferenceNumber = c.String(maxLength: 34, unicode: false),
                        EntryClass = c.String(maxLength: 2, unicode: false),
                        NominatedAccountReference = c.String(nullable: false, maxLength: 30, unicode: false),
                        BDFIndicator = c.String(maxLength: 1, unicode: false),
                        SettlementIdentifier = c.String(nullable: false, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankFileItemId)
                .ForeignKey("dbo.Account", t => t.AccountId, cascadeDelete: true)
                .ForeignKey("Nedbank.ClientType", t => t.NedbankClientTypeId)
                .ForeignKey("Nedbank.EntryCode", t => t.EntryClass)
                .ForeignKey("Nedbank.ServiceTypes", t => t.ServiceType)
                .ForeignKey("Nedbank.TransactionTypes", t => t.TransactionType)
                .ForeignKey("Nedbank.BatchFeed", t => t.NedbankBatchFileId)
                .Index(t => t.NedbankBatchFileId)
                .Index(t => t.NedbankClientTypeId)
                .Index(t => t.AccountId)
                .Index(t => t.TransactionType)
                .Index(t => t.ServiceType)
                .Index(t => t.EntryClass);
            
            CreateTable(
                "Nedbank.ClientType",
                c => new
                    {
                        NedbankClientTypeId = c.String(nullable: false, maxLength: 2, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        ShortName = c.String(nullable: false, maxLength: 17, unicode: false),
                        LookUpKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankClientTypeId);
            
            CreateTable(
                "Nedbank.EntryCode",
                c => new
                    {
                        EntryCode = c.String(nullable: false, maxLength: 2, unicode: false),
                        LookupKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.EntryCode);
            
            CreateTable(
                "Nedbank.ServiceTypes",
                c => new
                    {
                        ServiceType = c.String(nullable: false, maxLength: 2, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        ShortName = c.String(nullable: false, maxLength: 17, unicode: false),
                        LookUpKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ServiceType);
            
            CreateTable(
                "Nedbank.TransactionTypes",
                c => new
                    {
                        TransactionType = c.String(nullable: false, maxLength: 4, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50, unicode: false),
                        LookUpKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        Description = c.String(nullable: false, maxLength: 100, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TransactionType);
            
            CreateTable(
                "Nedbank.HeaderItem",
                c => new
                    {
                        NedbankHeaderRecordId = c.Int(nullable: false, identity: true),
                        RecordIdentifier = c.String(nullable: false, maxLength: 2, unicode: false),
                        ClientProfileNumber = c.String(nullable: false, maxLength: 10, unicode: false),
                        FileSequenceNumber = c.String(nullable: false, maxLength: 24, unicode: false),
                        FileType = c.String(nullable: false, maxLength: 2, unicode: false),
                        NominatedAccountNumber = c.String(maxLength: 16, unicode: false),
                        ChargesAccountNumber = c.String(maxLength: 16, unicode: false),
                        StatementNarrative = c.String(maxLength: 30, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankHeaderRecordId)
                .ForeignKey("Nedbank.InstructionFileType", t => t.FileType)
                .Index(t => t.FileType);
            
            CreateTable(
                "Nedbank.InstructionFileType",
                c => new
                    {
                        FileType = c.String(nullable: false, maxLength: 2, unicode: false),
                        FileTypeName = c.String(nullable: false, maxLength: 50, unicode: false),
                        LookupKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.FileType);
            
            CreateTable(
                "Nedbank.Scheduler",
                c => new
                    {
                        NedbankSchedulerId = c.Int(nullable: false, identity: true),
                        NedbankBatchFileId = c.Int(nullable: false),
                        NumberOfDepositsSent = c.String(nullable: false, maxLength: 8, unicode: false),
                        LastRan = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankSchedulerId)
                .ForeignKey("Nedbank.BatchFeed", t => t.NedbankBatchFileId)
                .Index(t => t.NedbankBatchFileId);
            
            CreateTable(
                "Nedbank.TrailerItem",
                c => new
                    {
                        NedbankTrailerRecordId = c.Int(nullable: false, identity: true),
                        RecordIdentifier = c.String(nullable: false, maxLength: 2, unicode: false),
                        TotalNumberOfTransactions = c.String(nullable: false, maxLength: 8, unicode: false),
                        TotalValue = c.String(nullable: false, maxLength: 18, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankTrailerRecordId);
            
            CreateTable(
                "Nedbank.ClientProfile",
                c => new
                    {
                        ProfileNumber = c.String(nullable: false, maxLength: 10, unicode: false),
                        NedbankClientTypeId = c.String(nullable: false, maxLength: 2, unicode: false),
                        ClientName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Prefix = c.String(nullable: false, maxLength: 2),
                        LookupKey = c.String(nullable: false, maxLength: 50, unicode: false),
                        ChargesAccountNumber = c.String(nullable: false, maxLength: 16, unicode: false),
                        NominatedAccountNumber = c.String(nullable: false, maxLength: 16, unicode: false),
                        StatementNarrative = c.String(maxLength: 30, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProfileNumber)
                .ForeignKey("Nedbank.ClientType", t => t.NedbankClientTypeId, cascadeDelete: true)
                .Index(t => t.NedbankClientTypeId);
            
            CreateTable(
                "Nedbank.Configurations",
                c => new
                    {
                        NedbankConfigurationId = c.Int(nullable: false, identity: true),
                        ConfigName = c.String(nullable: false, maxLength: 50, unicode: false),
                        DocumentType = c.String(nullable: false, maxLength: 50, unicode: false),
                        TransactionType = c.String(nullable: false, maxLength: 50, unicode: false),
                        DailyCutoffTime = c.String(nullable: false, maxLength: 50, unicode: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankConfigurationId);
            
            CreateTable(
                "Nedbank.Duplicate",
                c => new
                    {
                        NedbankDuplicateId = c.Int(nullable: false, identity: true),
                        ClientProfileNumber = c.String(maxLength: 10),
                        FileSequenceNumber = c.String(maxLength: 24),
                        FileType = c.String(maxLength: 2),
                        NominatedAccountNumber = c.String(maxLength: 16),
                        ChargesAccountNumber = c.String(maxLength: 16),
                        TotalNumberOfTransactions = c.String(maxLength: 8),
                        TotalValue = c.String(maxLength: 18),
                        FileStatus = c.String(maxLength: 8),
                        Reason = c.String(maxLength: 30),
                        HashTotal = c.String(maxLength: 256),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankDuplicateId);
            
            CreateTable(
                "Nedbank.ResponseDetail",
                c => new
                    {
                        NedbankResponseDetailId = c.Int(nullable: false, identity: true),
                        RecordIdentifier = c.String(nullable: false, maxLength: 2),
                        NominatedAccountNumber = c.String(maxLength: 16),
                        PaymentReferenceNumber = c.String(nullable: false, maxLength: 34),
                        DestinationBranchCode = c.String(nullable: false, maxLength: 6),
                        DestinationAccountNumber = c.String(nullable: false, maxLength: 16),
                        Amount = c.String(nullable: false, maxLength: 12),
                        ActionDate = c.String(nullable: false, maxLength: 8),
                        Reference = c.String(nullable: false, maxLength: 30),
                        DestinationAccountHoldersName = c.String(nullable: false, maxLength: 30),
                        TransactionType = c.String(nullable: false, maxLength: 4),
                        NedbankClientTypeId = c.String(nullable: false, maxLength: 2, unicode: false),
                        ChargesAccountNumber = c.String(maxLength: 16),
                        ServiceType = c.String(nullable: false, maxLength: 2),
                        OriginalPaymentReferenceNumber = c.String(maxLength: 34),
                        TransactionStatus = c.String(nullable: false, maxLength: 8),
                        ResponseFilename = c.String(nullable: false, maxLength: 50),
                        Reason = c.String(maxLength: 98),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankResponseDetailId)
                .ForeignKey("Nedbank.ClientType", t => t.NedbankClientTypeId, cascadeDelete: true)
                .Index(t => t.NedbankClientTypeId);
            
            CreateTable(
                "Nedbank.UnpaidOrNaedo",
                c => new
                    {
                        NedbankUnpaidOrNaedoId = c.Int(nullable: false, identity: true),
                        RecordIdentifier = c.String(nullable: false, maxLength: 2),
                        RecordType = c.String(nullable: false, maxLength: 2),
                        NedbankUnpaidReasonId = c.String(maxLength: 2),
                        PaymentReferenceNumber = c.String(maxLength: 34),
                        NedbankReferenceNumber = c.String(maxLength: 8),
                        RejectingBankCode = c.String(maxLength: 3),
                        RejectingBankBranchCode = c.String(maxLength: 6),
                        NewDestinationBranchCode = c.String(maxLength: 6),
                        NewDestinationAccountNumber = c.String(maxLength: 16),
                        NewDestinationAccountType = c.String(maxLength: 1),
                        Status = c.String(maxLength: 8),
                        Reason = c.String(maxLength: 100),
                        UnpaidsUserReference = c.String(maxLength: 30),
                        NaedosUserReference = c.String(maxLength: 30),
                        OriginalHomingAccountNumber = c.String(maxLength: 11),
                        OriginalAccountType = c.String(maxLength: 1),
                        Amount = c.String(maxLength: 12),
                        OriginalActionDate = c.String(maxLength: 6),
                        Class = c.String(maxLength: 2),
                        TaxCode = c.String(maxLength: 1),
                        ReasonCode = c.String(maxLength: 2),
                        OriginalHomingAccountName = c.String(maxLength: 30),
                        NewSequenceNumber = c.String(maxLength: 6),
                        NumberOfTimesRedirected = c.String(maxLength: 2),
                        NewActionDate = c.String(maxLength: 6),
                        ResponseFilename = c.String(maxLength: 50),
                        SettlementIdentifier = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankUnpaidOrNaedoId);
            
            CreateTable(
                "Nedbank.UnpaidReason",
                c => new
                    {
                        NedbankUnpaidReasonId = c.String(nullable: false, maxLength: 2),
                        Reason = c.String(nullable: false, maxLength: 150),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankUnpaidReasonId);
            
            CreateTable(
                "Nedbank.UnpaidsRecordType",
                c => new
                    {
                        NedbankUnpaidsRecordTypeId = c.String(nullable: false, maxLength: 2),
                        LookupKey = c.String(nullable: false, maxLength: 50),
                        Description = c.String(nullable: false, maxLength: 50),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.NedbankUnpaidsRecordTypeId);
            
            CreateTable(
                "dbo.ProductFee",
                c => new
                    {
                        ProductId = c.Int(nullable: false),
                        FeeId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => new { t.ProductId, t.FeeId })
                .ForeignKey("dbo.Fee", t => t.FeeId, cascadeDelete: true)
                .ForeignKey("dbo.Product", t => t.ProductId, cascadeDelete: true)
                .Index(t => t.ProductId)
                .Index(t => t.FeeId);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        ProductId = c.Int(nullable: false, identity: true),
                        ProductTypeId = c.Int(nullable: false),
                        DeviceTypeId = c.Int(),
                        DeviceId = c.Int(),
                        SiteId = c.Int(nullable: false),
                        ServiceTypeId = c.Int(nullable: false),
                        SettlementTypeId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        PublicHolidayInclInFeeFlag = c.Boolean(nullable: false),
                        ImplementationDate = c.DateTime(nullable: false),
                        TerminationDate = c.DateTime(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ProductId)
                .ForeignKey("dbo.Device", t => t.DeviceId)
                .ForeignKey("dbo.DeviceType", t => t.DeviceTypeId)
                .ForeignKey("dbo.ProductType", t => t.ProductTypeId)
                .ForeignKey("dbo.ServiceType", t => t.ServiceTypeId, cascadeDelete: true)
                .ForeignKey("dbo.SettlementType", t => t.SettlementTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Site", t => t.SiteId, cascadeDelete: true)
                .ForeignKey("dbo.Status", t => t.StatusId)
                .Index(t => t.ProductTypeId)
                .Index(t => t.DeviceTypeId)
                .Index(t => t.DeviceId)
                .Index(t => t.SiteId)
                .Index(t => t.ServiceTypeId)
                .Index(t => t.SettlementTypeId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.ServiceType",
                c => new
                    {
                        ServiceTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ServiceTypeId);
            
            CreateTable(
                "dbo.SettlementType",
                c => new
                    {
                        SettlementTypeId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SettlementTypeId);
            
            CreateTable(
                "dbo.PublicHoliday",
                c => new
                    {
                        PublicHolidayId = c.Int(nullable: false, identity: true),
                        Day = c.Int(nullable: false),
                        Month = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PublicHolidayId);
            
            CreateTable(
                "dbo.Recon",
                c => new
                    {
                        ReconId = c.Int(nullable: false, identity: true),
                        BankType = c.String(nullable: false, maxLength: 50),
                        ClientReference = c.String(nullable: false, maxLength: 150),
                        Amount = c.String(nullable: false, maxLength: 100),
                        DateActioned = c.DateTime(nullable: false),
                        ClientSite = c.String(nullable: false, maxLength: 100),
                        MySbvReference = c.String(maxLength: 100),
                        StatusCode = c.String(nullable: false, maxLength: 1),
                        UniqueUserCode = c.String(maxLength: 100),
                        BatchNumber = c.String(nullable: false, maxLength: 100),
                        DateSent = c.DateTime(nullable: false),
                        AccountNumber = c.String(nullable: false, maxLength: 100),
                        BranchCode = c.String(nullable: false, maxLength: 50),
                        AccountTypeId = c.Int(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReconId)
                .ForeignKey("dbo.AccountType", t => t.AccountTypeId)
                .ForeignKey("dbo.SettlementStatus", t => t.StatusCode)
                .Index(t => t.StatusCode)
                .Index(t => t.AccountTypeId);
            
            CreateTable(
                "dbo.SettlementStatus",
                c => new
                    {
                        StatusCode = c.String(nullable: false, maxLength: 1),
                        Status = c.String(maxLength: 50),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.StatusCode);
            
            CreateTable(
                "dbo.Report",
                c => new
                    {
                        ReportId = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.ReportId);
            
            CreateTable(
                "dbo.RoleReport",
                c => new
                    {
                        RoleReportId = c.Int(nullable: false, identity: true),
                        RoleId = c.Int(nullable: false),
                        ReportId = c.Int(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.RoleReportId)
                .ForeignKey("dbo.Report", t => t.ReportId)
                .Index(t => t.ReportId);
            
            CreateTable(
                "dbo.SalesArea",
                c => new
                    {
                        SalesAreaId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SalesAreaId);
            
            CreateTable(
                "dbo.SettlementStatusDescription",
                c => new
                    {
                        SettlementStatusDescriptionId = c.Int(nullable: false, identity: true),
                        Description = c.String(maxLength: 50),
                        StatusCode = c.String(nullable: false, maxLength: 1),
                        LookupKey = c.String(maxLength: 50),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SettlementStatusDescriptionId)
                .ForeignKey("dbo.SettlementStatus", t => t.StatusCode)
                .Index(t => t.StatusCode);
            
            CreateTable(
                "dbo.SystemConfiguration",
                c => new
                    {
                        SystemConfigurationId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        LookUpKey = c.String(),
                        Value = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.SystemConfigurationId);
            
            CreateTable(
                "dbo.Task",
                c => new
                    {
                        TaskId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        ReferenceNumber = c.String(),
                        Date = c.DateTime(nullable: false),
                        ApprovalObjectsId = c.Int(),
                        MerchantId = c.Int(nullable: false),
                        SiteId = c.Int(nullable: false),
                        AccountId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        StatusId = c.Int(nullable: false),
                        Module = c.String(),
                        Link = c.String(),
                        IsExecuted = c.Boolean(nullable: false),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.TaskId)
                .ForeignKey("dbo.Account", t => t.AccountId)
                .ForeignKey("dbo.ApprovalObjects", t => t.ApprovalObjectsId)
                .ForeignKey("dbo.Merchant", t => t.MerchantId)
                .ForeignKey("dbo.Site", t => t.SiteId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .ForeignKey("dbo.User", t => t.UserId)
                .Index(t => t.ApprovalObjectsId)
                .Index(t => t.MerchantId)
                .Index(t => t.SiteId)
                .Index(t => t.AccountId)
                .Index(t => t.UserId)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.VaultAuditLog",
                c => new
                    {
                        VaultAuditLogId = c.Int(nullable: false, identity: true),
                        VaultTransactionXmlId = c.Int(),
                        OldValue = c.String(),
                        NewValue = c.String(),
                        TableName = c.String(),
                        ColumnName = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultAuditLogId)
                .ForeignKey("dbo.VaultTransactionXml", t => t.VaultTransactionXmlId)
                .Index(t => t.VaultTransactionXmlId);
            
            CreateTable(
                "dbo.VaultTransactionXml",
                c => new
                    {
                        VaultTransactionXmlId = c.Int(nullable: false, identity: true),
                        StatusId = c.Int(nullable: false),
                        BagSerialNumber = c.String(),
                        TransactionDate = c.DateTime(nullable: false),
                        TransactionTypeCode = c.String(),
                        ErrorMessages = c.String(),
                        XmlMessage = c.String(),
                        XmlAwaitingApproval = c.String(),
                        ApprovedById = c.Int(),
                        ApprovedDate = c.DateTime(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultTransactionXmlId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.StatusId);
            
            CreateTable(
                "dbo.VaultContainerDrop",
                c => new
                    {
                        VaultContainerDropId = c.Int(nullable: false, identity: true),
                        VaultContainerId = c.Int(nullable: false),
                        DenominationId = c.Int(nullable: false),
                        ValueInCents = c.Int(nullable: false),
                        Count = c.Int(nullable: false),
                        Value = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DiscrepancyCount = c.Int(),
                        DiscrepancyValue = c.Decimal(precision: 18, scale: 2),
                        HasDiscrepancy = c.Boolean(nullable: false),
                        ActualCount = c.Int(),
                        ActualValue = c.String(),
                        DenominationType = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultContainerDropId)
                .ForeignKey("dbo.Denomination", t => t.DenominationId, cascadeDelete: true)
                .ForeignKey("dbo.VaultContainer", t => t.VaultContainerId, cascadeDelete: true)
                .Index(t => t.VaultContainerId)
                .Index(t => t.DenominationId);
            
            CreateTable(
                "dbo.VaultContainer",
                c => new
                    {
                        VaultContainerId = c.Int(nullable: false, identity: true),
                        ContainerId = c.Int(nullable: false),
                        CashDepositId = c.Int(nullable: false),
                        SettlementIdentifier = c.String(),
                        SiteId = c.Int(nullable: false),
                        SupervisorId = c.Int(nullable: false),
                        DeviceId = c.Int(nullable: false),
                        DiscrepancyReasonId = c.Int(),
                        CitCode = c.String(),
                        Comment = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ActualAmount = c.Decimal(precision: 18, scale: 2),
                        DiscrepancyAmount = c.Decimal(precision: 18, scale: 2),
                        HasDiscrepancy = c.Boolean(nullable: false),
                        ProcessedById = c.Int(),
                        ProcessedDateTime = c.DateTime(),
                        SerialNumber = c.String(maxLength: 18),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultContainerId)
                .ForeignKey("dbo.Device", t => t.DeviceId, cascadeDelete: true)
                .Index(t => t.DeviceId);
            
            CreateTable(
                "dbo.VaultPartialPayment",
                c => new
                    {
                        VaultPartialPaymentId = c.Int(nullable: false, identity: true),
                        StatusId = c.Int(nullable: false),
                        ErrorCodeId = c.Int(),
                        PaymentReference = c.String(),
                        DeviceSerialNumber = c.String(),
                        BagSerialNumber = c.String(),
                        CitCode = c.String(),
                        BeneficiaryCode = c.String(),
                        TotalToBePaid = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DeviceUserName = c.String(),
                        DeviceUserRole = c.String(),
                        SettlementIdentifier = c.String(),
                        SendDateTime = c.DateTime(),
                        SettlementDate = c.DateTime(),
                        TransactionDate = c.DateTime(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultPartialPaymentId)
                .ForeignKey("dbo.ErrorCode", t => t.ErrorCodeId)
                .ForeignKey("dbo.Status", t => t.StatusId, cascadeDelete: true)
                .Index(t => t.StatusId)
                .Index(t => t.ErrorCodeId);
            
            CreateTable(
                "dbo.VaultTransactionType",
                c => new
                    {
                        VaultTransactionTypeId = c.Int(nullable: false, identity: true),
                        Code = c.Int(nullable: false),
                        Name = c.String(),
                        LookUpKey = c.String(),
                        Description = c.String(),
                        IsNotDeleted = c.Boolean(nullable: false),
                        LastChangedById = c.Int(),
                        LastChangedDate = c.DateTime(nullable: false),
                        CreatedById = c.Int(),
                        CreateDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.VaultTransactionTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.VaultPartialPayment", "StatusId", "dbo.Status");
            DropForeignKey("dbo.VaultPartialPayment", "ErrorCodeId", "dbo.ErrorCode");
            DropForeignKey("dbo.VaultContainerDrop", "VaultContainerId", "dbo.VaultContainer");
            DropForeignKey("dbo.VaultContainer", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.VaultContainerDrop", "DenominationId", "dbo.Denomination");
            DropForeignKey("dbo.VaultAuditLog", "VaultTransactionXmlId", "dbo.VaultTransactionXml");
            DropForeignKey("dbo.VaultTransactionXml", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Task", "UserId", "dbo.User");
            DropForeignKey("dbo.Task", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Task", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Task", "MerchantId", "dbo.Merchant");
            DropForeignKey("dbo.Task", "ApprovalObjectsId", "dbo.ApprovalObjects");
            DropForeignKey("dbo.Task", "AccountId", "dbo.Account");
            DropForeignKey("dbo.SettlementStatusDescription", "StatusCode", "dbo.SettlementStatus");
            DropForeignKey("dbo.RoleReport", "ReportId", "dbo.Report");
            DropForeignKey("dbo.Recon", "StatusCode", "dbo.SettlementStatus");
            DropForeignKey("dbo.Recon", "AccountTypeId", "dbo.AccountType");
            DropForeignKey("dbo.ProductFee", "ProductId", "dbo.Product");
            DropForeignKey("dbo.Product", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Product", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Product", "SettlementTypeId", "dbo.SettlementType");
            DropForeignKey("dbo.Product", "ServiceTypeId", "dbo.ServiceType");
            DropForeignKey("dbo.Product", "ProductTypeId", "dbo.ProductType");
            DropForeignKey("dbo.Product", "DeviceTypeId", "dbo.DeviceType");
            DropForeignKey("dbo.Product", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.ProductFee", "FeeId", "dbo.Fee");
            DropForeignKey("Nedbank.ResponseDetail", "NedbankClientTypeId", "Nedbank.ClientType");
            DropForeignKey("Nedbank.ClientProfile", "NedbankClientTypeId", "Nedbank.ClientType");
            DropForeignKey("Nedbank.BatchFeed", "NedbankTrailerRecordId", "Nedbank.TrailerItem");
            DropForeignKey("Nedbank.Scheduler", "NedbankBatchFileId", "Nedbank.BatchFeed");
            DropForeignKey("Nedbank.HeaderItem", "FileType", "Nedbank.InstructionFileType");
            DropForeignKey("Nedbank.BatchFeed", "NedbankHeaderRecordId", "Nedbank.HeaderItem");
            DropForeignKey("Nedbank.FileItem", "NedbankBatchFileId", "Nedbank.BatchFeed");
            DropForeignKey("Nedbank.FileItem", "TransactionType", "Nedbank.TransactionTypes");
            DropForeignKey("Nedbank.FileItem", "ServiceType", "Nedbank.ServiceTypes");
            DropForeignKey("Nedbank.FileItem", "EntryClass", "Nedbank.EntryCode");
            DropForeignKey("Nedbank.FileItem", "NedbankClientTypeId", "Nedbank.ClientType");
            DropForeignKey("Nedbank.FileItem", "AccountId", "dbo.Account");
            DropForeignKey("dbo.TransactionDetailRecord", "BatchFileId", "dbo.BatchFile");
            DropForeignKey("dbo.TransactionDetailRecordResponse", "BatchFile_BatchFileId", "dbo.BatchFile");
            DropForeignKey("dbo.BatchFile", "TrailerRecordId", "dbo.TrailerRecord");
            DropForeignKey("dbo.LoadReport", "BatchFileId", "dbo.BatchFile");
            DropForeignKey("dbo.BatchFile", "HeaderRecordId", "dbo.HeaderRecord");
            DropForeignKey("dbo.Fee", "FeeTypeId", "dbo.FeeType");
            DropForeignKey("dbo.CitRequestDetail", "CashDeposit_CashDepositId", "dbo.CashDeposit");
            DropForeignKey("dbo.CashOrderTask", "UserId", "dbo.User");
            DropForeignKey("dbo.CashOrderTask", "StatusId", "dbo.Status");
            DropForeignKey("dbo.CashOrderTask", "SiteId", "dbo.Site");
            DropForeignKey("dbo.CashOrder", "StatusId", "dbo.Status");
            DropForeignKey("dbo.CashOrder", "SiteId", "dbo.Site");
            DropForeignKey("dbo.CashOrder", "CashOrderTypeId", "dbo.CashOrderType");
            DropForeignKey("dbo.CashOrder", "CashOrderContainerId", "dbo.CashOrderContainer");
            DropForeignKey("dbo.CashOrderContainerDropItem", "DenominationId", "dbo.Denomination");
            DropForeignKey("dbo.CashOrderContainerDropItem", "CashOrderContainerDropId", "dbo.CashOrderContainerDrop");
            DropForeignKey("dbo.CashOrderContainerDrop", "CashOrderContainerId", "dbo.CashOrderContainer");
            DropForeignKey("dbo.Account", "TransactionTypeId", "dbo.TransactionType");
            DropForeignKey("dbo.Account", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Account", "SiteId", "dbo.Site");
            DropForeignKey("dbo.Site", "StatusId", "dbo.Status");
            DropForeignKey("dbo.SiteContainer", "SiteId", "dbo.Site");
            DropForeignKey("dbo.SiteContainer", "ContainerTypeId", "dbo.ContainerType");
            DropForeignKey("dbo.Site", "MerchantId", "dbo.Merchant");
            DropForeignKey("dbo.Site", "CitCarrierId", "dbo.CitCarrier");
            DropForeignKey("dbo.VaultBeneficiary", "ContainerDropId", "dbo.ContainerDrop");
            DropForeignKey("dbo.VaultBeneficiary", "CashDepositId", "dbo.CashDeposit");
            DropForeignKey("dbo.VaultBeneficiary", "AccountId", "dbo.Account");
            DropForeignKey("dbo.CashDeposit", "StatusId", "dbo.Status");
            DropForeignKey("dbo.CashDeposit", "SiteId", "dbo.Site");
            DropForeignKey("dbo.CashDeposit", "ProductTypeId", "dbo.ProductType");
            DropForeignKey("dbo.CashDeposit", "ErrorCodeId", "dbo.ErrorCode");
            DropForeignKey("dbo.CashDeposit", "DeviceId", "dbo.Device");
            DropForeignKey("dbo.Device", "DeviceTypeId", "dbo.DeviceType");
            DropForeignKey("dbo.DeviceType", "SupplierId", "dbo.Supplier");
            DropForeignKey("dbo.Device", "Supplier_SupplierId", "dbo.Supplier");
            DropForeignKey("dbo.DeviceType", "ManufacturerId", "dbo.Manufacturer");
            DropForeignKey("dbo.CashDeposit", "DeviceType_DeviceTypeId", "dbo.DeviceType");
            DropForeignKey("dbo.CashDeposit", "DepositTypeId", "dbo.DepositType");
            DropForeignKey("dbo.ContainerTypeAttribute", "ContainerTypeId", "dbo.ContainerType");
            DropForeignKey("dbo.Container", "ContainerTypeId", "dbo.ContainerType");
            DropForeignKey("dbo.ContainerDrop", "StatusId", "dbo.Status");
            DropForeignKey("dbo.ContainerDrop", "ErrorCodeId", "dbo.ErrorCode");
            DropForeignKey("dbo.ContainerDrop", "DiscrepancyReasonId", "dbo.DiscrepancyReason");
            DropForeignKey("dbo.Denomination", "DenominationTypeId", "dbo.DenominationType");
            DropForeignKey("dbo.ContainerDropItem", "DenominationId", "dbo.Denomination");
            DropForeignKey("dbo.ContainerDropItem", "ContainerDropId", "dbo.ContainerDrop");
            DropForeignKey("dbo.ContainerDrop", "ContainerId", "dbo.Container");
            DropForeignKey("dbo.Container", "CashDepositId", "dbo.CashDeposit");
            DropForeignKey("dbo.CashDeposit", "AccountId", "dbo.Account");
            DropForeignKey("dbo.Site", "CashCenterId", "dbo.CashCenter");
            DropForeignKey("dbo.User", "CashCenterId", "dbo.CashCenter");
            DropForeignKey("dbo.Cluster", "RegionManagerId", "dbo.User");
            DropForeignKey("dbo.User", "UserTypeId", "dbo.UserType");
            DropForeignKey("dbo.UserSite", "UserId", "dbo.User");
            DropForeignKey("dbo.UserSite", "SiteId", "dbo.Site");
            DropForeignKey("dbo.UserNotification", "UserId", "dbo.User");
            DropForeignKey("dbo.UserNotification", "NotificationTypeId", "dbo.NotificationType");
            DropForeignKey("dbo.User", "TitleId", "dbo.Title");
            DropForeignKey("dbo.User", "MerchantId", "dbo.Merchant");
            DropForeignKey("dbo.Merchant", "StatusId", "dbo.Status");
            DropForeignKey("dbo.Merchant", "MerchantDescriptionId", "dbo.MerchantDescription");
            DropForeignKey("dbo.Merchant", "CompanyTypeId", "dbo.CompanyType");
            DropForeignKey("dbo.CashCenter", "ClusterId", "dbo.Cluster");
            DropForeignKey("dbo.CashCenter", "CityId", "dbo.City");
            DropForeignKey("dbo.Site", "CityId", "dbo.City");
            DropForeignKey("dbo.Province", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Currency", "CountryId", "dbo.Country");
            DropForeignKey("dbo.Continent", "GeographyId", "dbo.Geography");
            DropForeignKey("dbo.Country", "ContinentId", "dbo.Continent");
            DropForeignKey("dbo.City", "ProvinceId", "dbo.Province");
            DropForeignKey("dbo.CashCenter", "City_CityId", "dbo.City");
            DropForeignKey("dbo.CashCenter", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Site", "AddressId", "dbo.Address");
            DropForeignKey("dbo.Address", "AddressTypeId", "dbo.AddressType");
            DropForeignKey("dbo.Account", "BankId", "dbo.Bank");
            DropForeignKey("dbo.Account", "AccountTypeId", "dbo.AccountType");
            DropIndex("dbo.VaultPartialPayment", new[] { "ErrorCodeId" });
            DropIndex("dbo.VaultPartialPayment", new[] { "StatusId" });
            DropIndex("dbo.VaultContainer", new[] { "DeviceId" });
            DropIndex("dbo.VaultContainerDrop", new[] { "DenominationId" });
            DropIndex("dbo.VaultContainerDrop", new[] { "VaultContainerId" });
            DropIndex("dbo.VaultTransactionXml", new[] { "StatusId" });
            DropIndex("dbo.VaultAuditLog", new[] { "VaultTransactionXmlId" });
            DropIndex("dbo.Task", new[] { "StatusId" });
            DropIndex("dbo.Task", new[] { "UserId" });
            DropIndex("dbo.Task", new[] { "AccountId" });
            DropIndex("dbo.Task", new[] { "SiteId" });
            DropIndex("dbo.Task", new[] { "MerchantId" });
            DropIndex("dbo.Task", new[] { "ApprovalObjectsId" });
            DropIndex("dbo.SettlementStatusDescription", new[] { "StatusCode" });
            DropIndex("dbo.RoleReport", new[] { "ReportId" });
            DropIndex("dbo.Recon", new[] { "AccountTypeId" });
            DropIndex("dbo.Recon", new[] { "StatusCode" });
            DropIndex("dbo.Product", new[] { "StatusId" });
            DropIndex("dbo.Product", new[] { "SettlementTypeId" });
            DropIndex("dbo.Product", new[] { "ServiceTypeId" });
            DropIndex("dbo.Product", new[] { "SiteId" });
            DropIndex("dbo.Product", new[] { "DeviceId" });
            DropIndex("dbo.Product", new[] { "DeviceTypeId" });
            DropIndex("dbo.Product", new[] { "ProductTypeId" });
            DropIndex("dbo.ProductFee", new[] { "FeeId" });
            DropIndex("dbo.ProductFee", new[] { "ProductId" });
            DropIndex("Nedbank.ResponseDetail", new[] { "NedbankClientTypeId" });
            DropIndex("Nedbank.ClientProfile", new[] { "NedbankClientTypeId" });
            DropIndex("Nedbank.Scheduler", new[] { "NedbankBatchFileId" });
            DropIndex("Nedbank.HeaderItem", new[] { "FileType" });
            DropIndex("Nedbank.FileItem", new[] { "EntryClass" });
            DropIndex("Nedbank.FileItem", new[] { "ServiceType" });
            DropIndex("Nedbank.FileItem", new[] { "TransactionType" });
            DropIndex("Nedbank.FileItem", new[] { "AccountId" });
            DropIndex("Nedbank.FileItem", new[] { "NedbankClientTypeId" });
            DropIndex("Nedbank.FileItem", new[] { "NedbankBatchFileId" });
            DropIndex("Nedbank.BatchFeed", new[] { "NedbankTrailerRecordId" });
            DropIndex("Nedbank.BatchFeed", new[] { "NedbankHeaderRecordId" });
            DropIndex("dbo.TransactionDetailRecord", new[] { "BatchFileId" });
            DropIndex("dbo.TransactionDetailRecordResponse", new[] { "BatchFile_BatchFileId" });
            DropIndex("dbo.LoadReport", new[] { "BatchFileId" });
            DropIndex("dbo.BatchFile", new[] { "TrailerRecordId" });
            DropIndex("dbo.BatchFile", new[] { "HeaderRecordId" });
            DropIndex("dbo.Fee", new[] { "FeeTypeId" });
            DropIndex("dbo.CitRequestDetail", new[] { "CashDeposit_CashDepositId" });
            DropIndex("dbo.CashOrderTask", new[] { "StatusId" });
            DropIndex("dbo.CashOrderTask", new[] { "UserId" });
            DropIndex("dbo.CashOrderTask", new[] { "SiteId" });
            DropIndex("dbo.CashOrder", new[] { "StatusId" });
            DropIndex("dbo.CashOrder", new[] { "SiteId" });
            DropIndex("dbo.CashOrder", new[] { "CashOrderContainerId" });
            DropIndex("dbo.CashOrder", new[] { "CashOrderTypeId" });
            DropIndex("dbo.CashOrderContainerDrop", new[] { "CashOrderContainerId" });
            DropIndex("dbo.CashOrderContainerDropItem", new[] { "DenominationId" });
            DropIndex("dbo.CashOrderContainerDropItem", new[] { "CashOrderContainerDropId" });
            DropIndex("dbo.SiteContainer", new[] { "ContainerTypeId" });
            DropIndex("dbo.SiteContainer", new[] { "SiteId" });
            DropIndex("dbo.VaultBeneficiary", new[] { "AccountId" });
            DropIndex("dbo.VaultBeneficiary", new[] { "ContainerDropId" });
            DropIndex("dbo.VaultBeneficiary", new[] { "CashDepositId" });
            DropIndex("dbo.DeviceType", new[] { "ManufacturerId" });
            DropIndex("dbo.DeviceType", new[] { "SupplierId" });
            DropIndex("dbo.Device", new[] { "Supplier_SupplierId" });
            DropIndex("dbo.Device", new[] { "DeviceTypeId" });
            DropIndex("dbo.ContainerTypeAttribute", new[] { "ContainerTypeId" });
            DropIndex("dbo.Denomination", new[] { "DenominationTypeId" });
            DropIndex("dbo.ContainerDropItem", new[] { "DenominationId" });
            DropIndex("dbo.ContainerDropItem", new[] { "ContainerDropId" });
            DropIndex("dbo.ContainerDrop", new[] { "ErrorCodeId" });
            DropIndex("dbo.ContainerDrop", new[] { "StatusId" });
            DropIndex("dbo.ContainerDrop", new[] { "DiscrepancyReasonId" });
            DropIndex("dbo.ContainerDrop", new[] { "ContainerId" });
            DropIndex("dbo.Container", new[] { "ContainerTypeId" });
            DropIndex("dbo.Container", new[] { "CashDepositId" });
            DropIndex("dbo.CashDeposit", new[] { "DeviceType_DeviceTypeId" });
            DropIndex("dbo.CashDeposit", new[] { "ErrorCodeId" });
            DropIndex("dbo.CashDeposit", new[] { "StatusId" });
            DropIndex("dbo.CashDeposit", new[] { "DeviceId" });
            DropIndex("dbo.CashDeposit", new[] { "ProductTypeId" });
            DropIndex("dbo.CashDeposit", new[] { "AccountId" });
            DropIndex("dbo.CashDeposit", new[] { "SiteId" });
            DropIndex("dbo.CashDeposit", new[] { "DepositTypeId" });
            DropIndex("dbo.UserSite", new[] { "SiteId" });
            DropIndex("dbo.UserSite", new[] { "UserId" });
            DropIndex("dbo.UserNotification", new[] { "NotificationTypeId" });
            DropIndex("dbo.UserNotification", new[] { "UserId" });
            DropIndex("dbo.Merchant", new[] { "StatusId" });
            DropIndex("dbo.Merchant", new[] { "CompanyTypeId" });
            DropIndex("dbo.Merchant", new[] { "MerchantDescriptionId" });
            DropIndex("dbo.User", new[] { "MerchantId" });
            DropIndex("dbo.User", new[] { "UserTypeId" });
            DropIndex("dbo.User", new[] { "CashCenterId" });
            DropIndex("dbo.User", new[] { "TitleId" });
            DropIndex("dbo.Cluster", new[] { "RegionManagerId" });
            DropIndex("dbo.Currency", new[] { "CountryId" });
            DropIndex("dbo.Continent", new[] { "GeographyId" });
            DropIndex("dbo.Country", new[] { "ContinentId" });
            DropIndex("dbo.Province", new[] { "CountryId" });
            DropIndex("dbo.City", new[] { "ProvinceId" });
            DropIndex("dbo.CashCenter", new[] { "City_CityId" });
            DropIndex("dbo.CashCenter", new[] { "ClusterId" });
            DropIndex("dbo.CashCenter", new[] { "AddressId" });
            DropIndex("dbo.CashCenter", new[] { "CityId" });
            DropIndex("dbo.Address", new[] { "AddressTypeId" });
            DropIndex("dbo.Site", new[] { "AddressId" });
            DropIndex("dbo.Site", new[] { "CashCenterId" });
            DropIndex("dbo.Site", new[] { "CityId" });
            DropIndex("dbo.Site", new[] { "StatusId" });
            DropIndex("dbo.Site", new[] { "CitCarrierId" });
            DropIndex("dbo.Site", new[] { "MerchantId" });
            DropIndex("dbo.Account", new[] { "StatusId" });
            DropIndex("dbo.Account", new[] { "TransactionTypeId" });
            DropIndex("dbo.Account", new[] { "BankId" });
            DropIndex("dbo.Account", new[] { "AccountTypeId" });
            DropIndex("dbo.Account", new[] { "SiteId" });
            DropTable("dbo.VaultTransactionType");
            DropTable("dbo.VaultPartialPayment");
            DropTable("dbo.VaultContainer");
            DropTable("dbo.VaultContainerDrop");
            DropTable("dbo.VaultTransactionXml");
            DropTable("dbo.VaultAuditLog");
            DropTable("dbo.Task");
            DropTable("dbo.SystemConfiguration");
            DropTable("dbo.SettlementStatusDescription");
            DropTable("dbo.SalesArea");
            DropTable("dbo.RoleReport");
            DropTable("dbo.Report");
            DropTable("dbo.SettlementStatus");
            DropTable("dbo.Recon");
            DropTable("dbo.PublicHoliday");
            DropTable("dbo.SettlementType");
            DropTable("dbo.ServiceType");
            DropTable("dbo.Product");
            DropTable("dbo.ProductFee");
            DropTable("Nedbank.UnpaidsRecordType");
            DropTable("Nedbank.UnpaidReason");
            DropTable("Nedbank.UnpaidOrNaedo");
            DropTable("Nedbank.ResponseDetail");
            DropTable("Nedbank.Duplicate");
            DropTable("Nedbank.Configurations");
            DropTable("Nedbank.ClientProfile");
            DropTable("Nedbank.TrailerItem");
            DropTable("Nedbank.Scheduler");
            DropTable("Nedbank.InstructionFileType");
            DropTable("Nedbank.HeaderItem");
            DropTable("Nedbank.TransactionTypes");
            DropTable("Nedbank.ServiceTypes");
            DropTable("Nedbank.EntryCode");
            DropTable("Nedbank.ClientType");
            DropTable("Nedbank.FileItem");
            DropTable("Nedbank.BatchFeed");
            DropTable("dbo.HyphenScheduler");
            DropTable("dbo.Configuration");
            DropTable("dbo.TransactionDetailRecord");
            DropTable("dbo.TransactionDetailRecordResponse");
            DropTable("dbo.TrailerRecord");
            DropTable("dbo.LoadReport");
            DropTable("dbo.HeaderRecord");
            DropTable("dbo.BatchFile");
            DropTable("dbo.FeeType");
            DropTable("dbo.Fee");
            DropTable("dbo.ErrorLogging");
            DropTable("dbo.EasterGoldenNumber");
            DropTable("dbo.CitRequestDetail");
            DropTable("dbo.CashOrderTask");
            DropTable("dbo.CashOrderType");
            DropTable("dbo.CashOrder");
            DropTable("dbo.CashOrderContainer");
            DropTable("dbo.CashOrderContainerDrop");
            DropTable("dbo.CashOrderContainerDropItem");
            DropTable("dbo.AuditLog");
            DropTable("dbo.ApprovalObjects");
            DropTable("dbo.TransactionType");
            DropTable("dbo.SiteContainer");
            DropTable("dbo.CitCarrier");
            DropTable("dbo.VaultBeneficiary");
            DropTable("dbo.ProductType");
            DropTable("dbo.Supplier");
            DropTable("dbo.Manufacturer");
            DropTable("dbo.DeviceType");
            DropTable("dbo.Device");
            DropTable("dbo.DepositType");
            DropTable("dbo.ContainerTypeAttribute");
            DropTable("dbo.ContainerType");
            DropTable("dbo.ErrorCode");
            DropTable("dbo.DiscrepancyReason");
            DropTable("dbo.DenominationType");
            DropTable("dbo.Denomination");
            DropTable("dbo.ContainerDropItem");
            DropTable("dbo.ContainerDrop");
            DropTable("dbo.Container");
            DropTable("dbo.CashDeposit");
            DropTable("dbo.UserType");
            DropTable("dbo.UserSite");
            DropTable("dbo.NotificationType");
            DropTable("dbo.UserNotification");
            DropTable("dbo.Title");
            DropTable("dbo.Status");
            DropTable("dbo.MerchantDescription");
            DropTable("dbo.CompanyType");
            DropTable("dbo.Merchant");
            DropTable("dbo.User");
            DropTable("dbo.Cluster");
            DropTable("dbo.Currency");
            DropTable("dbo.Geography");
            DropTable("dbo.Continent");
            DropTable("dbo.Country");
            DropTable("dbo.Province");
            DropTable("dbo.City");
            DropTable("dbo.CashCenter");
            DropTable("dbo.AddressType");
            DropTable("dbo.Address");
            DropTable("dbo.Site");
            DropTable("dbo.Bank");
            DropTable("dbo.AccountType");
            DropTable("dbo.Account");
        }
    }
}
