using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Security;
using Application.Dto.CashProcessing;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.UserAccountValidation;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.CashHandling.CashProcessing.WebProcessor
{
    public class CashDepositWebProcessingValidation : ICashDepositWebProcessingValidation
    {
        #region Fields
        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly IUserAccountValidation _userAccountValidation;
        #endregion

        #region Constructor

        public CashDepositWebProcessingValidation(IRepository repository, IMapper mapper, ILookup lookup, IUserAccountValidation userAccountValidation)
        {
            _repository = repository;
            _mapper = mapper;
            _lookup = lookup;
            _userAccountValidation = userAccountValidation;
        }

        #endregion

        #region ICash Deposit Processing

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
        ///     Process cash deposit
        /// </summary>
        /// <param name="processingDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public MethodResult<bool> Process(CashProcessingDto processingDto, string username)
        {
            // Get the logged in user details
            User user = _userAccountValidation.UserByName(username);
            CashDeposit cashDeposit = _mapper.Map<CashProcessingDto, CashDeposit>(processingDto);
            int productTypeId = _lookup.GetProductTypeId("MYSBV_DEPOSIT");

            if (cashDeposit.ProductTypeId == productTypeId &&
                cashDeposit.DepositTypeId != _lookup.GetDepositType("MULTI_DEPOSIT").DepositTypeId)
            {
                cashDeposit.SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(DbTable.Cashdeposit,
                    _repository);
            }

            cashDeposit.IsProcessed = true;
            cashDeposit.IsSubmitted = true;
            cashDeposit.ProcessedDateTime = DateTime.Now;
            cashDeposit.ProcessedById = user.UserId;
            cashDeposit.HasDescripancy = processingDto.IsDescripency;

            int pendingSettlementStatus = _lookup.GetStatusId("PENDING_SETTLEMENT");
            int settledStatus = _lookup.GetStatusId("SETTLED");
            var depo = _repository.Find<CashDeposit>(cashDeposit.CashDepositId);

            if (depo.StatusId != settledStatus || depo.StatusId == pendingSettlementStatus)
                cashDeposit.StatusId = _lookup.GetStatusId("CONFIRMED");

            cashDeposit.EntityState = State.Modified;

            foreach (Container container in cashDeposit.Containers)
            {
                container.EntityState = State.Modified;

                foreach (ContainerDrop drop in container.ContainerDrops)
                {
                    drop.EntityState = State.Modified;
                    drop.DiscrepancyReasonId = drop.DiscrepancyReasonId == 0 ? null : drop.DiscrepancyReasonId;
                    drop.StatusId = _lookup.GetStatusId("CONFIRMED");

                    if (cashDeposit.ProductTypeId == productTypeId &&
                        cashDeposit.DepositTypeId == _lookup.GetDepositType("MULTI_DEPOSIT").DepositTypeId)
                    {
                        drop.SettlementIdentifier = ApplicationHelpers.GenerateSettlementIdentifier(
                            DbTable.MultiDeposit, _repository);
                    }

                    foreach (ContainerDropItem item in drop.ContainerDropItems)
                    {
                        if (item.ContainerDropItemId == 0)
                        {
                            item.EntityState = State.Added;
                            item.DenominationId = GetDenomination(item.ValueInCents);
                        }
                        else
                        {
                            item.EntityState = State.Modified;
                        }
                    }
                }
            }

            ((IEntity) cashDeposit).WalkObjectGraph(o =>
            {
                o.IsNotDeleted = true;
                o.CreateDate = cashDeposit.CreateDate;
                o.CreatedById = cashDeposit.CreatedById;
                o.LastChangedDate = DateTime.Now;
                o.LastChangedById = user.UserId;
                return false;
            }, a => { });

            bool result = _repository.Update(cashDeposit) > 0;

            return new MethodResult<bool>(MethodStatus.Successful, result, "Deposit was successfully processed.");
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

        private int GetDenomination(int valueInCents)
        {
            Denomination denomination =
                _repository.Query<Denomination>(e => e.ValueInCents == valueInCents && valueInCents > 0)
                    .FirstOrDefault();
            return denomination != null ? denomination.DenominationId : 0;
        }

        #endregion
    }
}