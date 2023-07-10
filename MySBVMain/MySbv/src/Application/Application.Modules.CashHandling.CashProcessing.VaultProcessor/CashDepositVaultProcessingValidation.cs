using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using Application.Dto.CashDeposit;
using Application.Dto.CashProcessing;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Security;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;

namespace Application.Modules.CashHandling.CashProcessing.VaultProcessor
{
    public class CashDepositVaultProcessingValidation : ICashDepositVaultProcessingValidation
    {
        #region Fields
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly ISecurity _security;
        private readonly IUserAccountValidation _userAccountValidation;
        private readonly IMsmqConnector _queueConnector;
        #endregion

        #region Constructor

        public CashDepositVaultProcessingValidation(IRepository repository, IMapper mapper, ILookup lookup, IUserAccountValidation userAccountValidation, ISecurity security, IMsmqConnector queueConnector)
        {
            _repository = repository;
            _mapper = mapper;
            _lookup = lookup;
            _userAccountValidation = userAccountValidation;
            _security = security;
            _queueConnector = queueConnector;
        }

        #endregion

        #region ICash Deposit Processing
        /// <summary>
        /// Find deposit by Serial number or Seal Number
        /// </summary>
        /// <param name="sealSerialNumber"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<CashProcessingDto> FindBySealSerialNumber(string sealSerialNumber, string username)
        {
            Container container =
                _repository.Query<Container>(
                    a => a.CashDeposit.IsSubmitted.HasValue && a.CashDeposit.IsSubmitted == true
                         && (a.SerialNumber == sealSerialNumber || a.SealNumber == sealSerialNumber),
                    o => o.CashDeposit.Containers,
                    o => o.CashDeposit.DepositType,
                    o => o.CashDeposit.ProductType,
                    o => o.CashDeposit.Site,
                    o => o.CashDeposit.Site.Merchant,
                    o => o.CashDeposit.Containers.Select(a => a.ContainerType),
                    o => o.CashDeposit.Containers.Select(p => p.ContainerDrops),
                    o => o.CashDeposit.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)))
                    .FirstOrDefault();


            if (container != null)
            {
                // Get the logged in user details
                User loggedOnUser = _userAccountValidation.UserByName(username);

                // A deposit cannot be processed more than once and
                // the current logged in user can only processed deposits that were captured by
                // other users in his/her cash centre and not deposits that were
                // captured by (him/her)self.
                //
                // NOTE : Head Office User with (ADMINISTRATOR) role can process all deposits for all cash centers 
                //
                if (container.CashDeposit.IsProcessed.HasValue && container.CashDeposit.IsProcessed.Value ||
                    container.CashDeposit.LastChangedById == loggedOnUser.UserId)
                    return new MethodResult<CashProcessingDto>(MethodStatus.Error, null,
                        "A deposit cannot be processed more than once and you cannot process deposits that were captured by yourself.");

                // If deposit is of type vault, make
                // sure that CIT has taken place otherwise
                // reject processing.
                ProductType productType =
                    _repository.Query<ProductType>(a => a.LookUpKey == "MYSBV_VAULT").FirstOrDefault();
                int status = _lookup.GetStatusId("PAY_UNCONFIRMED");

                if (container.CashDeposit.ProductTypeId == productType.ProductTypeId &&
                    container.CashDeposit.StatusId != status && container.CashDeposit.CitDateTime == null)
                    return new MethodResult<CashProcessingDto>(MethodStatus.Error, null,
                        "You can only process a vault deposit after CIT is complete.");


                List<ContainerDrop> drops = container.ContainerDrops.Where(a => a.IsNotDeleted).ToList();

                for (int index = 0; index < drops.Count; index++)
                {
                    drops[index].ContainerDropItems = OrderDescendingContainerDropItems(drops[index].ContainerDropItems);
                }
                container.ContainerDrops = drops.ToCollection();

                CashProcessingDto cashDepositDto = _mapper.Map<CashDeposit, CashProcessingDto>(container.CashDeposit);
                cashDepositDto.DeviceId = container.CashDeposit.DeviceId;

                IEnumerable<ProcessingContainerDto> containers = cashDepositDto.Containers.Where(e => e.IsNotDeleted);
                cashDepositDto.Containers = containers.OrderBy(o => o.ContainerId).ToList();

                switch (loggedOnUser.UserType.LookUpKey)
                {
                    case "SBV_USER": // Cash Center User
                        return RetrieveForSbvUser(cashDepositDto, loggedOnUser);
                    case "HEAD_OFFICE_USER":
                        return (Roles.IsUserInRole(loggedOnUser.UserName, "SBVAdmin"))
                            ? new MethodResult<CashProcessingDto>(MethodStatus.Successful, cashDepositDto)
                            : new MethodResult<CashProcessingDto>(MethodStatus.Error, null,
                                "A User at Head Office must be in the ADMIN Role to process cash Deposits.");
                    default:
                        return new MethodResult<CashProcessingDto>(MethodStatus.Error, null,
                            "Retail users cannot process deposits.");
                }
            }
            return new MethodResult<CashProcessingDto>(MethodStatus.Error, null, "Cash Deposit Not Found.");
        }


        /// <summary>
        /// Process Vault deposit
        /// </summary>
        /// <param name="vaultDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<CashProcessingDto> ProcessVault(VaultContainerDto vaultDto, string username)
        {
            // Get the logged in user details.
            User user = _userAccountValidation.UserByName(username);
            var date = DateTime.Now;

            var vaultDeposit = _mapper.Map<VaultContainerDto, VaultContainer>(vaultDto);
            var cashDeposit = _repository.Find<CashDeposit>(vaultDeposit.CashDepositId);

            var cashProcessingDto = FindBySealSerialNumber(vaultDeposit.SerialNumber, username);
            var processedDeposit = cashProcessingDto.EntityResult;
            //  _mapper.Map<CashDeposit, CashProcessingDto>(cashDeposit);

            // vault deposit values
            vaultDeposit.SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Cashdeposit, _repository);
            vaultDeposit.ProcessedById = user.UserId;
            vaultDeposit.ProcessedDateTime = date;
            vaultDeposit.EntityState = State.Added;
            vaultDeposit.CreatedById = user.UserId;
            vaultDeposit.CreateDate = date;
            vaultDeposit.LastChangedById = user.UserId;
            vaultDeposit.LastChangedDate = date;
            vaultDeposit.IsNotDeleted = true;

            var denominations = _repository.All<Denomination>();

            foreach (var containerDrop in vaultDeposit.VaultContainerDrops)
            {
                if (containerDrop.DenominationId == 0)
                {
                    containerDrop.DenominationId =
                        denominations.FirstOrDefault(e => e.ValueInCents == containerDrop.ValueInCents).DenominationId;
                }
                containerDrop.EntityState = State.Added;
                containerDrop.CreatedById = user.UserId;
                containerDrop.CreateDate = date;
                containerDrop.LastChangedById = user.UserId;
                containerDrop.LastChangedDate = date;
                containerDrop.IsNotDeleted = true;
            }

            // cashdeposit values.
            cashDeposit.DepositedAmount = vaultDeposit.Amount;
            cashDeposit.ActualAmount = vaultDeposit.ActualAmount;
            cashDeposit.DiscrepancyAmount = vaultDeposit.DiscrepancyAmount;
            cashDeposit.HasDescripancy = vaultDeposit.HasDiscrepancy;
            cashDeposit.SupervisorId = vaultDeposit.SupervisorId;

            cashDeposit.IsProcessed = true;
            cashDeposit.IsSubmitted = true;
            cashDeposit.ProcessedDateTime = vaultDeposit.ProcessedDateTime;
            cashDeposit.ProcessedById = user.UserId;
            cashDeposit.HasDescripancy = vaultDto.HasDiscrepancy;
            cashDeposit.SettlementIdentifier = vaultDeposit.SettlementIdentifier;
            cashDeposit.EntityState = State.Modified;

            var pendingSettlementStatus = _lookup.GetStatusId("PENDING_SETTLEMENT");
            var settledStatus = _lookup.GetStatusId("SETTLED");

            if (cashDeposit.StatusId != settledStatus || cashDeposit.StatusId == pendingSettlementStatus)
            {
                cashDeposit.StatusId = _lookup.GetStatusId("CONFIRMED");
            }

            bool added;

            using (var scope = new TransactionScope())
            {
                _repository.Update(cashDeposit);
                added = _repository.Add(vaultDeposit) > 0;
                scope.Complete();
            }

            if (added)
            {
                // wrap in a separate thread for better performance
                // send confirmation message to Cash connect
                System.Threading.Tasks.Task.Factory.StartNew(() =>
                    AddWebFlowConfirmationToQueue(vaultDeposit));

                return new MethodResult<CashProcessingDto>(MethodStatus.Successful, processedDeposit, "Vault deposit was successfully processed.");
            }

            return new MethodResult<CashProcessingDto>(MethodStatus.Error, null, "Failed to process vault deposit.");
        }


        /// <summary>
        /// This method Formats a deposit to a VaultDeposit
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        public VaultContainerDto FormatToVaultDeposit(CashProcessingDto deposit)
        {
            VaultContainerDto vaultContainer = null;
            var container = deposit.Containers.FirstOrDefault();

            if (container != null)
            {
                var containerDrops = new List<VaultContainerDropDto>();

                foreach (var item in container.ContainerDrops.SelectMany(e => e.ContainerDropItems))
                {
                    var containerDrop = new VaultContainerDropDto
                    {
                        DenominationId = item.DenominationId,
                        VaultContainerId = container.ContainerId,
                        ValueInCents = item.ValueInCents,
                        DenominationType = item.DenominationType.ToUpper()
                    };

                    foreach (var vaultItem in container.ContainerDrops.SelectMany(e => e.ContainerDropItems))
                    {
                        if (item.DenominationId == vaultItem.DenominationId)
                        {
                            containerDrop.Value += vaultItem.Value;
                            var count = vaultItem.Count.HasValue ? vaultItem.Count : 0;
                            containerDrop.Count += count;
                        }
                    }
                    containerDrops.Add(containerDrop);
                }

                var drops = RemoveDuplicateVaultItems(containerDrops);

                var site = _repository.Query<Site>(e => e.SiteId == deposit.SiteId, o => o.Merchant).FirstOrDefault();
                var device = _repository.Find<Device>(deposit.DeviceId.Value);

                vaultContainer = new VaultContainerDto
                {
                    ContainerId = container.ContainerId,
                    CashDepositId = deposit.CashDepositId,
                    DeviceId = deposit.DeviceId != null ? deposit.DeviceId.Value : 0,
                    ProductTypeId = deposit.ProductTypeId,
                    ProductTypeName = deposit.ProductType,
                    CitCode = site.CitCode,
                    SiteId = site.SiteId,
                    VaultContainerId = container.ContainerId,
                    DeviceName = device.Name,
                    MerchantName = site.Merchant.Name,
                    ContractNumber = site.ContractNumber,
                    TransactionNumber = deposit.TransactionReference,
                    DepositReference = site.DepositReference,
                    Amount = deposit.DepositedAmount,
                    ActualAmount = deposit.ActualAmount,
                    DiscrepancyAmount = deposit.DiscrepancyAmount,
                    SerialNumber = container.SerialNumber,
                    VaultContainerDrops = drops
                };
            }

            return vaultContainer;
        }


        /// <summary>
        /// Remove duplicate Vault Drops
        /// </summary>
        /// <param name="drops"></param>
        /// <returns></returns>
        private IEnumerable<VaultContainerDropDto> RemoveDuplicateVaultItems(IEnumerable<VaultContainerDropDto> drops)
        {
            var collection = new List<VaultContainerDropDto>();
            foreach (var drop in drops)
            {
                if (collection.All(e => e.DenominationId != drop.DenominationId))
                {
                    collection.Add(drop);
                }
            }
            return collection;
        }

        /// <summary>
        /// Add WebFlow ConfirmationToQueue
        /// </summary>
        /// <param name="cashDeposit"></param>
        private void AddWebFlowConfirmationToQueue(VaultContainer cashDeposit)
        {
            var confirmation = ConvertToConfirmationMessage(cashDeposit);
            if (confirmation != null)
            {
                _queueConnector.AddMessage(QueueIdentifier.Confirmation,
                    new MessageEnvelope<ConfirmationMessage>()
                    {
                        MessageObject = confirmation,
                        Label = "Cash_Connect_Message"
                    });
            }
        }

        #endregion

        #region Helpers

        private MethodResult<CashProcessingDto> RetrieveForSbvUser(CashProcessingDto deposit, User user)
        {
            // Check if cash deposit
            // can be processed by the 
            // current logged in user's cash centre.
            Site depositSite =
                _repository.Query<Site>(a => a.SiteId == deposit.SiteId, c => c.CashCenter).FirstOrDefault();

            return depositSite != null && depositSite.CashCenterId == user.CashCenterId
                ? new MethodResult<CashProcessingDto>(MethodStatus.Successful, deposit)
                : new MethodResult<CashProcessingDto>(MethodStatus.Error, null,
                    "Deposit cannot be processed by the current logged in user(s) cash Centre");
        }

        private Collection<ContainerDropItem> OrderDescendingContainerDropItems(IEnumerable<ContainerDropItem> items)
        {
            var collection = new Collection<ContainerDropItem>();
            IOrderedEnumerable<ContainerDropItem> entire = items.OrderByDescending(e => e.ValueInCents);

            foreach (ContainerDropItem item in entire)
            {
                collection.Add(item);
            }
            return collection;
        }
        
        private ConfirmationMessage ConvertToConfirmationMessage(VaultContainer cashDeposit)
        {
            try
            {
                if (!IsDepositFromCashConnectDevice(cashDeposit)) return null;

                var actualAmount = cashDeposit.ActualAmount.HasValue ? cashDeposit.ActualAmount : 0;

                var confirmationMessage = new ConfirmationMessage
                {
                    CountedValue = float.Parse(actualAmount.ToString()),
                    DeclaredValue = float.Parse(cashDeposit.Amount.ToString(CultureInfo.InvariantCulture)),
                    Date = cashDeposit.LastChangedDate,
                    SafeId = GetSafeId(cashDeposit.DeviceId),
                    BagBarcode = cashDeposit.SerialNumber
                };

                foreach (var containerDropItem in cashDeposit.VaultContainerDrops)
                {
                    var countedValue = containerDropItem.HasDiscrepancy ? containerDropItem.DiscrepancyCount.Value : containerDropItem.ActualCount.Value;                    

                    switch (containerDropItem.ValueInCents)
                    {
                        case 1000:
                            confirmationMessage.Declared10 = containerDropItem.Count;
                            confirmationMessage.Counted10 = countedValue;
                            break;

                        case 2000:
                            confirmationMessage.Declared20 = containerDropItem.Count;
                            confirmationMessage.Counted20 = countedValue;
                            break;

                        case 5000:
                            confirmationMessage.Declared50 = containerDropItem.Count;
                            confirmationMessage.Counted50 = countedValue;
                            break;

                        case 10000:
                            confirmationMessage.Declared100 = containerDropItem.Count;
                            confirmationMessage.Counted100 = countedValue;
                            break;

                        case 20000:
                            confirmationMessage.Declared200 = containerDropItem.Count;
                            confirmationMessage.Counted200 = countedValue;
                            break;
                    }
                }

                return confirmationMessage;

            }
            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [ConvertToConfirmationMessage]", ex);
                throw;
            }
        }

        // Verify if deposit is from Cash-Connect
        private bool IsDepositFromCashConnectDevice(VaultContainer cashDeposit)
        {
            var supplierId = _lookup.GetSupplierId("CASH_CONNECT");
            var device = _repository.Query<Device>(a => a.DeviceId == cashDeposit.DeviceId, a => a.DeviceType).FirstOrDefault();

            return _repository.Any<DeviceType>(a => a.SupplierId == supplierId && a.DeviceTypeId == device.DeviceTypeId);
        }

        private string GetSafeId(int deviceId)
        {
            var device = _lookup.Devices().FirstOrDefault(a => a.DeviceId == deviceId);
            if (device != null) return device.SerialNumber;
            return string.Empty;
        }
        #endregion
    }
}
