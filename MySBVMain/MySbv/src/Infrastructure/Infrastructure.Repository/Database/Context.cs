using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using Domain.Data.Core;
using Domain.Data.Hyphen.Model;
using Domain.Data.Model;
using Domain.Data.Nedbank.Model;

namespace Infrastructure.Repository.Database
{
    public class Context : DbContext
    {
        #region Constructor

        public Context(string connectionString)
            : base(connectionString)
        {
            ((IObjectContextAdapter)this).ObjectContext
                .ObjectMaterialized += (sender, args) =>
                {
                    var entity = args.Entity as IEntity;
                    if (entity != null)
                    {
                        entity.EntityState = State.Unchanged;
                    }
                };
            ((IObjectContextAdapter)this).ObjectContext.SavingChanges += OnSavingChanges;
        }


        /// <summary>
        ///     NOTE : This constructor is need for enabling migrations.
        /// </summary>
        public Context()
            : base("DefaultConnection")
        {
        }

        #endregion

        #region Methods

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            //Remove cascading deletes from these entities 

            modelBuilder.Entity<User>()
                .HasOptional(a => a.UserType)
                .WithMany()
                .HasForeignKey(u => u.UserTypeId).WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(a => a.Title)
                .WithMany()
                .HasForeignKey(u => u.TitleId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Site)
                .WithMany()
                .HasForeignKey(a => a.SiteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(a => a.Merchant)
                .WithMany()
                .HasForeignKey(u => u.MerchantId).WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(a => a.UserSites)
                .WithRequired(b => b.User)
                .HasForeignKey(z => z.UserId);

            modelBuilder.Entity<User>()
                .HasMany(a => a.UserNotifications)
                .WithRequired(b => b.User)
                .HasForeignKey(z => z.UserId);

            modelBuilder.Entity<CashCenter>()
                .HasMany(a => a.Users)
                .WithOptional(b => b.CashCenter)
                .HasForeignKey(z => z.CashCenterId);

            modelBuilder.Entity<CashCenter>()
                .HasRequired(a => a.City)
                .WithMany()
                .HasForeignKey(f => f.CityId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
                .HasRequired(a => a.Address)
                .WithMany()
                .HasForeignKey(u => u.AddressId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Site>()
                .HasRequired(a => a.Merchant)
                .WithMany(a => a.Sites)
                .HasForeignKey(p => p.MerchantId).WillCascadeOnDelete(false);

            modelBuilder.Entity<CashOrder>()
                .HasRequired(a => a.CashOrderContainer)
                .WithMany()
                .HasForeignKey(p => p.CashOrderContainerId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.TransactionType)
                .WithMany(b => b.Accounts)
                .HasForeignKey(p => p.TransactionTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Status)
                .WithMany()
                .HasForeignKey(b => b.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Site)
                .WithMany(b => b.Accounts)
                .HasForeignKey(p => p.SiteId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CashDeposit>()
                .HasOptional(a => a.ErrorCode)
                .WithMany()
                .HasForeignKey(b => b.ErrorCodeId)
                .WillCascadeOnDelete(false);

			modelBuilder.Entity<CashDeposit>()
				.HasRequired(a => a.Status)
				.WithMany()
				.HasForeignKey(p => p.StatusId).WillCascadeOnDelete(false);

			modelBuilder.Entity<CashOrder>()
				.HasRequired(a => a.Status)
				.WithMany()
				.HasForeignKey(p => p.StatusId).WillCascadeOnDelete(false);

            modelBuilder.Entity<CashOrderTask>()
                .HasRequired(a => a.Status)
                .WithMany()
                .HasForeignKey(p => p.StatusId).WillCascadeOnDelete(false);

            modelBuilder.Entity<CashOrderTask>()
                .HasRequired(a => a.Site)
                .WithMany()
                .HasForeignKey(p => p.SiteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<CashOrderTask>()
                .HasRequired(a => a.User)
                .WithMany()
                .HasForeignKey(p => p.UserId).WillCascadeOnDelete(false);

            modelBuilder.Entity<ContainerDrop>()
                .HasRequired(a => a.Status)
                .WithMany()
                .HasForeignKey(b => b.StatusId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ContainerDrop>()
                .HasOptional(a => a.ErrorCode)
                .WithMany()
                .HasForeignKey(b => b.ErrorCodeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasRequired(a => a.Status)
                .WithMany()
                .HasForeignKey(a => a.StatusId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>()
                .HasRequired(a => a.ProductType)
                .WithMany()
                .HasForeignKey(a => a.ProductTypeId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasRequired(a => a.Site)
                .WithMany()
                .HasForeignKey(a => a.SiteId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasRequired(a => a.Merchant)
                .WithMany()
                .HasForeignKey(a => a.MerchantId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasRequired(a => a.User)
                .WithMany()
                .HasForeignKey(a => a.UserId).WillCascadeOnDelete(false);

            modelBuilder.Entity<Task>()
                .HasRequired(a => a.Account)
                .WithMany()
                .HasForeignKey(a => a.AccountId).WillCascadeOnDelete(false);

            modelBuilder.Entity<DeviceType>()
                .HasRequired(a => a.Supplier)
                .WithMany()
				.HasForeignKey(a => a.SupplierId).WillCascadeOnDelete(false);
            
			modelBuilder.Entity<RoleReport>()
				.HasRequired(a => a.Report)
				.WithMany()
                .HasForeignKey(u => u.ReportId).WillCascadeOnDelete(false);

            modelBuilder.Entity<VaultBeneficiary>()
                .HasRequired(a => a.ContainerDrop)
                .WithMany()
                .HasForeignKey(u => u.ContainerDropId).WillCascadeOnDelete(false);

            modelBuilder.Entity<SettlementStatusDescription>()
                .HasRequired(a => a.SettlementStatus)
                .WithMany()
                .HasForeignKey(u => u.StatusCode).WillCascadeOnDelete(false);

            modelBuilder.Entity<Recon>()
                .HasRequired(a => a.SettlementStatus)
                .WithMany()
                .HasForeignKey(u => u.StatusCode).WillCascadeOnDelete(false);

            modelBuilder.Entity<Recon>()
                .HasRequired(a => a.AccountType)
                .WithMany()
                .HasForeignKey(u => u.AccountTypeId).WillCascadeOnDelete(false);

            #region Nedbank Models

            modelBuilder.Entity<NedbankBatchFile>()
                .Property(e => e.BatchNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankBatchFile>()
                .Property(e => e.BatchTotal)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankBatchFile>()
                .Property(e => e.BatchCount)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankBatchFile>()
                .Property(e => e.BatchDate)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankBatchFile>()
                .Property(e => e.FileName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankBatchFile>()
                .HasMany(e => e.NedbankFileItems)
                .WithRequired(e => e.NedbankBatchFile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankBatchFile>()
                .HasMany(e => e.NedbankSchedulers)
                .WithRequired(e => e.NedbankBatchFile)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankClientProfile>()
                .Property(e => e.ClientName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientProfile>()
                .Property(e => e.ProfileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientProfile>()
                .Property(e => e.LookupKey)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientProfile>()
                .Property(e => e.ChargesAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientProfile>()
                .Property(e => e.NominatedAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientProfile>()
                .Property(e => e.StatementNarrative)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientType>()
                .Property(e => e.NedbankClientTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientType>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientType>()
                .Property(e => e.LookUpKey)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankClientType>()
                .HasMany(e => e.NedbankFileItems)
                .WithRequired(e => e.NedbankClientType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankConfiguration>()
                .Property(e => e.ConfigName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankConfiguration>()
                .Property(e => e.DocumentType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankConfiguration>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankConfiguration>()
                .Property(e => e.DailyCutoffTime)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.RecordIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.NominatedAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.PaymentReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.DestinationBranchCode)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.DestinationAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.Amount)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.ActionDate)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.Reference)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.DestinationAccountHoldersName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.NedbankClientTypeId)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.ChargesAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.OriginalPaymentReferenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.EntryClass)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.NominatedAccountReference)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.BDFIndicator)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankFileItem>()
                .Property(e => e.SettlementIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankInstructionFileType>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankInstructionFileType>()
                .Property(e => e.FileTypeName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankInstructionFileType>()
                .Property(e => e.LookupKey)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankInstructionFileType>()
                .HasMany(e => e.NedbankHeaderRecords)
                .WithRequired(e => e.NedbankFileType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.RecordIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.ClientProfileNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.FileSequenceNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.FileType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.NominatedAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.ChargesAccountNumber)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .Property(e => e.StatementNarrative)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankHeaderRecord>()
                .HasMany(e => e.NedbankBatchFiles)
                .WithRequired(e => e.NedbankHeaderRecord)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankScheduler>()
                .Property(e => e.NumberOfDepositsSent)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankScheduler>()
                .Property(e => e.LastRan)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankServiceType>()
                .Property(e => e.ServiceType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankServiceType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankServiceType>()
                .Property(e => e.ShortName)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankServiceType>()
                .Property(e => e.LookUpKey)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankServiceType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankServiceType>()
                .HasMany(e => e.NedbankFileItems)
                .WithRequired(e => e.NedbankServiceType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankTrailerRecord>()
                .Property(e => e.RecordIdentifier)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTrailerRecord>()
                .Property(e => e.TotalNumberOfTransactions)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTrailerRecord>()
                .Property(e => e.TotalValue)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTrailerRecord>()
                .HasMany(e => e.NedbankBatchFiles)
                .WithRequired(e => e.NedbankTrailerRecord)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankTransactionType>()
                .Property(e => e.TransactionType)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTransactionType>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTransactionType>()
                .Property(e => e.LookUpKey)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTransactionType>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankTransactionType>()
                .HasMany(e => e.NedbankFileItems)
                .WithRequired(e => e.NedbankTransactionType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NedbankEntryCode>()
                .Property(e => e.EntryCode)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankEntryCode>()
                .Property(e => e.LookupKey)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankEntryCode>()
                .Property(e => e.Description)
                .IsUnicode(false);

            modelBuilder.Entity<NedbankEntryCode>()
                .HasMany(e => e.NedbankFileItems)
                .WithOptional(e => e.NedbankEntryCode)
                .HasForeignKey(e => e.EntryClass)
                .WillCascadeOnDelete(false);
            #endregion
        }

        private void OnSavingChanges(object sender, EventArgs e)
        {
            List<DbEntityEntry> changeEntries = ChangeTracker.Entries().Where(a => a.State == EntityState.Added
                                                                                   || a.State == EntityState.Deleted
                                                                                   || a.State == EntityState.Modified)
                .ToList();

            if (changeEntries.Any())
            {
                foreach (DbEntityEntry dbEntityEntry in changeEntries)
                {
                    foreach (Audit audit in CreateAuditRecordsForChanges(dbEntityEntry))
                    {
                        Audits.Add(audit);
                    }
                }
            }
        }

        private IEnumerable<Audit> CreateAuditRecordsForChanges(DbEntityEntry dbEntityEntry)
        {
            var result = new List<Audit>();

            #region Generate Audit

            if (dbEntityEntry != null)
            {
                // determine Audit Time
                DateTime auditTime = DateTime.Now;

                // Get the Table name by attribute
                //TableAttribute tableAttr = dbEntityEntry.Entity.GetType().GetCustomAttributes(typeof(TableAttribute), false).SingleOrDefault() as TableAttribute;
                //string tableName = tableAttr != null ? tableAttr.Name : dbEntityEntry.Entity.GetType().Name;

                // Get the Table name 
                string tableName = GetTableName(dbEntityEntry);

                if (dbEntityEntry.State == EntityState.Added)
                {
                    result.Add(new Audit
                    {
                        AuditState = "Added",
                        TableName = tableName,
                        RecordId = (dbEntityEntry.Entity as IEntity).Key,
                        IsNotDeleted = true,
                        CreateDate = auditTime,
                        LastChangedDate = auditTime,
                        CreatedById = (dbEntityEntry.Entity as IEntity).CreatedById,
                        LastChangedById = (dbEntityEntry.Entity as IEntity).LastChangedById
                    });
                }
                else if (dbEntityEntry.State == EntityState.Deleted)
                {
                    result.Add(new Audit
                    {
                        AuditState = "Deleted",
                        TableName = tableName,
                        RecordId = (dbEntityEntry.Entity as IEntity).Key,
                        IsNotDeleted = true,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now,
                        CreatedById = (dbEntityEntry.Entity as IEntity).CreatedById,
                        LastChangedById = (dbEntityEntry.Entity as IEntity).LastChangedById
                    });
                }
                else if (dbEntityEntry.State == EntityState.Modified)
                {
                    foreach (string propertyName in dbEntityEntry.OriginalValues.PropertyNames)
                    {
                        if (
                            !Equals(dbEntityEntry.OriginalValues.GetValue<object>(propertyName),
                                dbEntityEntry.CurrentValues.GetValue<object>(propertyName)))
                        {
                            result.Add(new Audit
                            {
                                AuditState = "Modified",
                                TableName = tableName,
                                RecordId = (dbEntityEntry.Entity as IEntity).Key,
                                ColumnName = propertyName,
                                OriginalValue = dbEntityEntry.OriginalValues.GetValue<object>(propertyName) == null
                                    ? null
                                    : dbEntityEntry.OriginalValues.GetValue<object>(propertyName).ToString(),
                                NewValue = dbEntityEntry.CurrentValues.GetValue<object>(propertyName) == null
                                    ? null
                                    : dbEntityEntry.CurrentValues.GetValue<object>(propertyName).ToString(),
                                IsNotDeleted = true,
                                CreateDate = DateTime.Now,
                                LastChangedDate = DateTime.Now,
                                CreatedById = (dbEntityEntry.Entity as IEntity).CreatedById,
                                LastChangedById = (dbEntityEntry.Entity as IEntity).LastChangedById
                            });
                        }
                    }
                }
            }
            return result;

            #endregion
        }

        private string GetTableName(DbEntityEntry entry)
        {
            string[] names = entry.Entity.ToString().Split('.');
            int length = names.Length;

            string tableName = names[length - 1];

            if (tableName.Contains("_"))
            {
                // get the proper table name . remove the last 64 characters including the underscore
                // eg. Address_D1996DDCAE2286E6B87F9DD1E2F537C1265D1D3914AAEBF0E3C14A7FAE59693D
                // becomes Address

                names = tableName.Split('_');
                length = names.Length;
                if (names[length - 1].Length == 64)
                {
                    tableName = names[0];
                }
            }

            return tableName;
        }

        #endregion

        #region Properties
        public DbSet<VaultContainer> VaultContainers { get; set; }
        public DbSet<VaultContainerDrop> VaultContainerDrops { get; set; }
        public DbSet<AccountType> AccountTypes { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<CitCarrier> CitCarriers { get; set; }
        public DbSet<SalesArea> SalesAreas { get; set; }
        public DbSet<CashOrder> CashOrders { get; set; }
        public DbSet<CashOrderType> CashOrderTypes { get; set; }
        public DbSet<CashOrderTask> CashOrderTasks { get; set; }
        public DbSet<CashOrderContainerDropItem> CashOrderContainerDropItems { get; set; }
        public DbSet<CashOrderContainerDrop> CashOrderContainerDrops { get; set; }
        public DbSet<CashOrderContainer> CashOrderContainers { get; set; }
        public DbSet<EasterGoldenNumber> EasterGoldenNumbers { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }
        public DbSet<CashCenter> CashCenters { get; set; }
        public DbSet<CashDeposit> CashDeposits { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<CompanyType> CompanyTypes { get; set; }
        public DbSet<Container> Containers { get; set; }
        public DbSet<ContainerDrop> ContainerDrops { get; set; }
        public DbSet<ContainerDropItem> ContainerDropItems { get; set; }
        public DbSet<ContainerType> ContainerTypes { get; set; }
        public DbSet<ContainerTypeAttribute> ContainerTypeAttributes { get; set; }
        public DbSet<Continent> Continents { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Denomination> Denominations { get; set; }
        public DbSet<DenominationType> DenominationTypes { get; set; }
        public DbSet<DepositType> DepositTypes { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<ProductType> ProductType { get; set; }
        public DbSet<Geography> Geographies { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Merchant> Merchants { get; set; }
        public DbSet<MerchantDescription> MerchantDescriptions { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<Province> Provinces { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<SiteContainer> SiteContainers { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<FeeType> FeeTypes { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Cluster> Cluster { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        public DbSet<ErrorLogging> ErrorLogging { get; set; }
        public DbSet<UserSite> UserSites { get; set; }
        public DbSet<SettlementType> SettlementTypes { get; set; }
        public DbSet<SystemConfiguration> SystemConfigurations { get; set; }
        public DbSet<DiscrepancyReason> DiscrepancyReasons { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<ApprovalObjects> ApprovalObjectses { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<ErrorCode> ErrorCodes { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<ProductFee> ProductFees { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
		public DbSet<Supplier> Suppliers { get; set; }
		public DbSet<RoleReport> RoleReports { get; set; }
		public DbSet<Report> Reports { get; set; }
        public DbSet<VaultPartialPayment> VaultPartialPayments { get; set; }
        public DbSet<VaultBeneficiary> VaultBeneficiaries { get; set; }
        public DbSet<VaultTransactionType> VaultTransactionTypes { get; set; }
        public DbSet<VaultTransactionXml> VaultTransactionXmls { get; set; }
		public DbSet<VaultAuditLog> VaultAuditLogs { get; set; }
        public DbSet<CitRequestDetail> CitRequestDetails { get; set; }
        public DbSet<Recon> Recon { get; set; }
        public DbSet<SettlementStatusDescription> SettlementStatusDescription { get; set; } 
        public DbSet<SettlementStatus> SettlementStatus { get; set; } 
        
        #endregion
        
        #region Hyphen

        public DbSet<BatchFile> HyphenBatchFiles { get; set; }
        public DbSet<HeaderRecord> HyphenHeaderRecords { get; set; }
        public DbSet<TrailerRecord> HyphenTrailerRecords { get; set; }
        public DbSet<TransactionDetailRecord> HyphenTransactionDetailRecords { get; set; }
        public DbSet<LoadReport> HyphenLoadReports { get; set; }
        public DbSet<Configuration> HyphenConfiguration { get; set; }
        public DbSet<HyphenScheduler> HyphenScheduler { get; set; }
        
        #endregion

        #region Nedbank
        public virtual DbSet<NedbankBatchFile> NedbankBatchFiles { get; set; }
        public virtual DbSet<NedbankClientProfile> NedbankClientProfiles { get; set; }
        public virtual DbSet<NedbankClientType> NedbankClientTypes { get; set; }
        public virtual DbSet<NedbankConfiguration> NedbankConfigurations { get; set; }
        public virtual DbSet<NedbankFileItem> NedbankFileItems { get; set; }
        public virtual DbSet<NedbankInstructionFileType> NedbankInstructionFileTypes { get; set; }
        public virtual DbSet<NedbankHeaderRecord> NedbankHeaderRecords { get; set; }
        public virtual DbSet<NedbankScheduler> NedbankSchedulers { get; set; }
        public virtual DbSet<NedbankServiceType> NedbankServiceTypes { get; set; }
        public virtual DbSet<NedbankTrailerRecord> NedbankTrailerRecords { get; set; }
        public virtual DbSet<NedbankTransactionType> NedbankTransactionTypes { get; set; }
        public virtual DbSet<NedbankEntryCode> NedbankEntryCodes { get; set; }
        public virtual DbSet<NedbankResponseDetail> NedbankResponseDetails { get; set; }
        public virtual DbSet<NedbankUnpaidReason> NedbankUnpaidReasons { get; set; }
        public virtual DbSet<NedbankUnpaidsRecordType> NedbankUnpaidsRecordTypes { get; set; }
        public virtual DbSet<NedbankUnpaidOrNaedo> NedbankUnpaidOrNaedos { get; set; }
        public virtual DbSet<NedbankDuplicate> NedbankDuplicates { get; set; } 
        
        #endregion
    }
}