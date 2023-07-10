using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Security;
using Application.Dto.CashDeposit;
using Application.Mapper;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Utility.Core;

namespace Application.Modules.CashHandling.CashDepositManager
{
    public class CashDepositValidation : ICashDepositValidation
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly ILookup _lookup;
        private readonly IRepository _repository;

        #endregion

        #region Constructor

        public CashDepositValidation(IRepository repository, IMapper mapper, ILookup lookup)
        {
            _repository = repository;
            _mapper = mapper;
            _lookup = lookup;
        }

        #endregion

        #region ICashDepositValidation

        public IEnumerable<ListCashDepositDto> All(User user)
        {
            IEnumerable<CashDeposit> cashDeposits;

            // A user can only have one Role on the Mysbv system
            string[] userRole = Roles.GetRolesForUser(user.UserName);
            string userRoleName = userRole[0];

            switch (userRoleName)
            {
                case "RetailSupervisor":
                case "RetailViewer":
                    {
                        // All Cash Deposits per Site
                        var siteIds = _repository.Query<UserSite>(a => a.UserId == user.UserId).Select(b => b.SiteId);

                        cashDeposits = _repository.Query<CashDeposit>(a => siteIds.Contains(a.SiteId),
                                        o => o.Status,
                                        o => o.Containers,
                                        o => o.Account,
                                        o => o.DepositType,
                                        o => o.Device,
                                        o => o.ErrorCode,
                                        o => o.ProductType,
                                        o => o.ProductType,
                                        o => o.Site,
                                        o => o.Site.CashCenter,
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)))
                                        .Where(x => (x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING") && !x.DeviceId.HasValue).ToList();
                        break;
                    }

                case "SBVAdmin":
                case "SBVFinanceReviewer":
                    {
                        // All Cash Deposits
                        cashDeposits = _repository.Query<CashDeposit>(x => (x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING") && !x.DeviceId.HasValue,
                                        o => o.Status,
                                        o => o.Containers,
                                        o => o.Account,
                                        o => o.DepositType,
                                        o => o.Device,
                                        o => o.ErrorCode,
                                        o => o.ProductType,
                                        o => o.ProductType,
                                        o => o.Site,
                                        o => o.Site.CashCenter,
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems))).ToList();
                        break;
                    }

                case "RetailUser":
                case "SBVTeller":
                case "SBVDataCapture":
                    {
                        // Only deposits which were captured by this Teller
                        cashDeposits = _repository.Query<CashDeposit>(a => a.CreatedById == user.UserId,
                                        o => o.Status,
                                        o => o.Containers,
                                        o => o.Account,
                                        o => o.DepositType,
                                        o => o.Device,
                                        o => o.ErrorCode,
                                        o => o.ProductType,
                                        o => o.ProductType,
                                        o => o.Site,
                                        o => o.Site.CashCenter,
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)))
                                        .Where(x => (x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING") && !x.DeviceId.HasValue).ToList();
                        break;
                    }

                case "SBVTellerSupervisor":
                case "SBVRecon":
                case "SBVApprover":
                    {
                        // All Cash Deposits serviced at SBV-User center
                        var cashCenterIds = _repository.Query<Site>(a => a.CashCenterId == user.CashCenterId).Select(e => e.CashCenterId);

                        cashDeposits = _repository.Query<CashDeposit>(a => cashCenterIds.Contains(a.Site.CashCenterId),
                                        o => o.Status,
                                        o => o.Containers,
                                        o => o.Account,
                                        o => o.DepositType,
                                        o => o.Device,
                                        o => o.ErrorCode,
                                        o => o.ProductType,
                                        o => o.ProductType,
                                        o => o.Site,
                                        o => o.Site.CashCenter,
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(e => e.ContainerDropItems.Select(r => r.Denomination))),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)))
                                        .Where(x => (x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING") && !x.DeviceId.HasValue).ToList();
                        break;
                    }

                default:
                    {
                        cashDeposits = _repository.Query<CashDeposit>(x => (x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING") && !x.DeviceId.HasValue,
                                        o => o.Status,
                                        o => o.Containers,
                                        o => o.Account,
                                        o => o.DepositType,
                                        o => o.Device,
                                        o => o.ErrorCode,
                                        o => o.ProductType,
                                        o => o.ProductType,
                                        o => o.Site,
                                        o => o.Site.CashCenter,
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems))).ToList();
                        break;

                    }
            }

            var deposits = cashDeposits.OrderByDescending(o => o.CashDepositId);
            return deposits.Select(cashDeposit => _mapper.Map<CashDeposit, ListCashDepositDto>(cashDeposit));
        }

        /// <summary>
        /// Create new deposit
        /// </summary>
        /// <param name="cashDeposit"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<CashDeposit> Add(CashDepositDto cashDeposit, User user)
        {
            var date = DateTime.Now;

            cashDeposit.StatusId = GetStatusId(cashDeposit.TransactionStatusName);
            cashDeposit.TransactionReference = _lookup.GenerateTransactionNumber(cashDeposit.SiteId);
            cashDeposit.ProductTypeId = GetProductTypeId("MYSBV_DEPOSIT");
            var depositedAmount = 0M;

            foreach (var bag in cashDeposit.Containers)
            {
                var containerAmount = 0m;

                if (WasSerialOrSealNumberUsed(bag.SerialNumber, bag.SealNumber))
                {
                    return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Serial Number Has been used before. Verify and try again.");
                }

                bag.ContainerTypeId = bag.ContainerTypeId == 0 ? 0 : bag.ContainerTypeId;
                bag.ReferenceNumber = _lookup.GenerateTransactionNumberForContainer(cashDeposit.SiteId);
                bag.ContainerType = null;

                if (bag.ContainerDrops == null) continue;

                foreach (var containerDrop in bag.ContainerDrops)
                {
                    if (cashDeposit.DepositTypeName == "Multi Drop")
                    {
                        var drop = _mapper.Map<ContainerDropDto, ContainerDrop>(containerDrop);

                        if (ContainerDropSerialNumberUsed(drop))
                        {
                            return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Drop Serial Number Has been used before. Verify and try again.");
                        }
                    }

                    // calculate container amount
                    containerAmount += containerDrop.Amount;

                    containerDrop.ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(cashDeposit.SiteId);
                    containerDrop.StatusId = GetStatusId(containerDrop.Name);

                    foreach (var item in containerDrop.ContainerDropItems.Where(i => i.ValueInCents > 0))
                    {
                        item.DenominationId = GetDenomination(item.ValueInCents);
                        item.DenominationType = GetDenominationType(item.DenominationId);
                        item.DenominationName = GetDenominationName(item.DenominationId);

                        var valueInCents = (item.ValueInCents/100.0M);
                        item.Value = valueInCents * item.Count;
                    }
                }
                bag.Amount = containerAmount;
                depositedAmount += containerAmount;
            }

            CashDeposit mappedCashDeposit = _mapper.Map<CashDepositDto, CashDeposit>(cashDeposit);
            mappedCashDeposit.DepositedAmount = depositedAmount;

            mappedCashDeposit.ProductType = null;
            mappedCashDeposit.Device = null;
            mappedCashDeposit.Site = null;
            foreach (Container bag in mappedCashDeposit.Containers)
            {
                bag.ContainerType = null;
                bag.CashDeposit = null;
            }

            mappedCashDeposit.VaultSource = VaultSource.MYSBV.Name();

            ((IEntity)mappedCashDeposit).WalkObjectGraph(o =>
            {
                o.CreatedById = user.UserId;
                o.CreateDate = date;
                o.LastChangedById = user.UserId;
                o.LastChangedDate = date;
                o.IsNotDeleted = true;
                o.EntityState = State.Added;
                return false;
            }, a => { });

            RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers); // trim cyclic redundancy before saving
            var cashDepositId = _repository.Add(mappedCashDeposit);
            RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers); // trim cyclic redundancy for JSON serialization

            return (cashDepositId > 0) ?
                new MethodResult<CashDeposit>(MethodStatus.Successful, mappedCashDeposit, "Cash deposit saved successfully.") :
                new MethodResult<CashDeposit>(MethodStatus.Error, null, "Error Saving the Cash Deposit");
        }

        void RemoveCyclicCashDepositsInContainers(IEnumerable<Container> containers)
        {
            foreach (var container in containers)
            {
                container.CashDeposit = null;
                container.ContainerType = null;

                foreach (var containerDrop in container.ContainerDrops)
                {
                    containerDrop.Container = null;

                    foreach (var item in containerDrop.ContainerDropItems)
                    {
                        item.ContainerDrop = null;
                        item.Denomination = null;
                    }
                }
            }
        }

        void RemoveCyclicCashDepositsInContainers(IEnumerable<ContainerDto> containers)
        {
            foreach (var container in containers)
            {
                container.ContainerType = null;
            }
        }

        void RemoveCyclicCashDepositsInContainerDrop(ContainerDrop containerDrop)
        {
            containerDrop.Container = null;

            foreach (var item in containerDrop.ContainerDropItems)
            {
                item.ContainerDrop = null;
                item.Denomination = null;
            }
        }

        void RemoveCyclicCashDepositsInContainer(Container container)
        {
            container.CashDeposit = null;
            container.ContainerType = null;

            foreach (var drop in container.ContainerDrops)
            {
                drop.Container = null;

                foreach (var item in drop.ContainerDropItems)
                {
                    item.ContainerDrop = null;
                    item.Denomination = null;
                }
            }
        }

        public MethodResult<CashDeposit> Edit(CashDepositDto cashDeposit, User user)
        {
            var date = DateTime.Now;
            cashDeposit.StatusId = GetStatusId(cashDeposit.TransactionStatusName);
            cashDeposit.ProductTypeId = GetProductTypeId("MYSBV_DEPOSIT");

            var mappedCashDeposit = _mapper.Map<CashDepositDto, CashDeposit>(cashDeposit);

            var cashDepositId = 0;
            using (var scope = new TransactionScope())
            {
                #region Container Management

                foreach (var container in mappedCashDeposit.Containers)
                {
                    // 
                    // For a new Container mark EntityState as Added.
                    if (container.ContainerId == 0)
                    {
                        container.EntityState = State.Added;
                        container.ReferenceNumber = _lookup.GenerateTransactionNumberForContainer(mappedCashDeposit.SiteId);

                        foreach (var newdrop in container.ContainerDrops)
                        {
                            newdrop.ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(mappedCashDeposit.SiteId);
                            newdrop.EntityState = State.Added;

                            foreach (var newItem in newdrop.ContainerDropItems)
                            {
                                newItem.DenominationId = GetDenomination(newItem.ValueInCents);
                                newItem.DenominationType = GetDenominationType(newItem.DenominationId);
                                newItem.DenominationName = GetDenominationName(newItem.DenominationId);
                                newItem.Value = (newItem.ValueInCents / 100M) * newItem.Count;
                                newItem.EntityState = State.Added;
                            }
                        }
                    }
                    else
                    {
                        // For existing containers, Mark as modified
                        // get deleted containers
                        var containersToDelete = _repository.Query<Container>(o => o.CashDepositId == cashDeposit.CashDepositId,
                            o => o.ContainerDrops,
                            o => o.ContainerDrops.Select(e => e.ContainerDropItems)
                            );


                        foreach (var bagDeleted in containersToDelete)
                        {
                            {
                                ((IEntity)bagDeleted).WalkObjectGraph(o =>
                                {
                                    o.EntityState = State.Deleted;
                                    o.IsNotDeleted = false;
                                    return false;
                                }, a => { });

                                RemoveCyclicCashDepositsInContainer(bagDeleted); // trim cyclic redundancy before saving
                                _repository.Delete<Container>(bagDeleted.ContainerId, user.UserId);
                                RemoveCyclicCashDepositsInContainer(bagDeleted);
                            }
                        }


                        container.EntityState = State.Modified;

                        #region ContainerDrops Management

                        // mark any new drop as Added
                        foreach (var containerDrop in container.ContainerDrops)
                        {
                            containerDrop.EntityState = State.Modified;

                            if (containerDrop.ContainerDropId == 0)
                            {
                                containerDrop.StatusId = GetStatusId("ACTIVE");
                                containerDrop.ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(mappedCashDeposit.SiteId);
                                containerDrop.EntityState = State.Added;

                                foreach (var newItem in containerDrop.ContainerDropItems)
                                {
                                    newItem.DenominationId = GetDenomination(newItem.ValueInCents);
                                    newItem.DenominationType = GetDenominationType(newItem.DenominationId);
                                    newItem.DenominationName = GetDenominationName(newItem.DenominationId);
                                    newItem.Value = (newItem.ValueInCents / 100M) * newItem.Count;
                                    newItem.EntityState = State.Added;
                                }
                            }
                            else
                            {
                                #region Deleted ContainerDrops

                                // get deleted containers Drops
                                var containerDropsToDelete = _repository.Query<ContainerDrop>(o => o.ContainerId == container.ContainerId,
                                    o => o.ContainerDropItems);

                                foreach (var bagDropDeleted in containerDropsToDelete)
                                {
                                    ((IEntity)bagDropDeleted).WalkObjectGraph(o =>
                                    {
                                        o.EntityState = State.Deleted;
                                        o.IsNotDeleted = false;
                                        return false;
                                    }, a => { });

                                    RemoveCyclicCashDepositsInContainerDrop(bagDropDeleted); // trim cyclic redundancy before saving
                                    _repository.Delete<ContainerDrop>(bagDropDeleted.ContainerDropId, user.UserId);
                                }

                                #endregion

                                #region Deleted ContainerDropItems

                                // 
                                // get deleted containers-Drop-Items
                                var containerDropItemsToDelete = _repository.Query<ContainerDropItem>(o => o.ContainerDropId == containerDrop.ContainerDropId);

                                foreach (var bagDropItemDeleted in containerDropItemsToDelete)
                                {
                                    bagDropItemDeleted.EntityState = State.Deleted;
                                    bagDropItemDeleted.IsNotDeleted = false;
                                    _repository.Delete<ContainerDropItem>(bagDropItemDeleted.ContainerDropItemId, user.UserId);
                                }

                                #endregion

                                // mark as Added or Modified
                                foreach (var containerDropItem in containerDrop.ContainerDropItems)
                                {
                                    containerDropItem.DenominationId = GetDenomination(containerDropItem.ValueInCents);
                                    containerDropItem.DenominationType = GetDenominationType(containerDropItem.DenominationId);
                                    containerDropItem.DenominationName = GetDenominationName(containerDropItem.DenominationId);
                                    containerDropItem.Value = (containerDropItem.ValueInCents / 100M) * containerDropItem.Count;
                                    containerDropItem.EntityState = containerDropItem.ContainerDropItemId == 0 ? State.Added : State.Modified;
                                }
                            }
                        }

                        #endregion

                    }
                }

                #endregion

                mappedCashDeposit.EntityState = State.Modified;

                ((IEntity)mappedCashDeposit).WalkObjectGraph(o =>
                {
                    o.CreatedById = cashDeposit.CreatedById;
                    o.CreateDate = cashDeposit.CreateDate;
                    o.LastChangedById = user.UserId;
                    o.LastChangedDate = date;
                    o.IsNotDeleted = true;
                    return false;
                }, a => { });

                if (mappedCashDeposit.TransactionReference == null)
                {
                    mappedCashDeposit.TransactionReference = _lookup.GenerateTransactionNumber(mappedCashDeposit.SiteId);
                }

                mappedCashDeposit.VaultSource = VaultSource.MYSBV.Name();

                RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers); // trim cyclic redundancy before saving
                cashDepositId = _repository.Update(mappedCashDeposit);
                RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers); // trim cyclic redundancy for JSON serialization

                scope.Complete();
            }
            return cashDepositId > 0

                ? new MethodResult<CashDeposit>(MethodStatus.Successful, mappedCashDeposit, "Cash Deposit edited successfully.")
                : new MethodResult<CashDeposit>(MethodStatus.Error, null, "Failed to edit Cash Deposit");
        }

        public User GetLoggedUser(string username)
        {
            var user = _repository.Query<User>(a => a.UserName.ToLower() == username.ToLower(),
                         u => u.UserType,
                         o => o.UserSites,
                         m => m.UserNotifications,
                         k => k.UserSites.Select(e => e.Site),
                         z => z.UserSites.Select(e => e.Site.SiteContainers),
                         v => v.UserSites.Select(p => p.Site.CitCarrier)).FirstOrDefault();

            return user;
        }

        public bool IsSubmitted(int cashDepositId)
        {
            var deposit = Find(cashDepositId);
            var status = _repository.Find<Status>(deposit.StatusId);
            return status.LookUpKey == "SUBMITTED";
        }


        public Container GetContainerByDropId(int containerDropId)
        {
            var containerDrop = _repository.Find<ContainerDrop>(containerDropId);
            return _repository.Find<Container>(containerDrop.ContainerId);
        }

        public string GetDepositType(int cashDepositId)
        {
            var deposit = _repository.Query<CashDeposit>(s => s.CashDepositId == cashDepositId, S => S.DepositType).FirstOrDefault();
            return deposit.DepositType.Name;
        }

        public CashDepositDto Find(int id)
        {
            var cashDeposit = _repository.Query<CashDeposit>(o => o.CashDepositId == id,
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
                        ).FirstOrDefault();

            var mappedCashDeposit = _mapper.Map<CashDeposit, CashDepositDto>(cashDeposit);

            var containers = new List<ContainerDto>();

            foreach (var containerDto in mappedCashDeposit.Containers.Where(e => e.IsNotDeleted))
            {
                var containerDrops = new List<ContainerDropDto>();

                foreach (var containerDrop in containerDto.ContainerDrops.Where(r => r.IsNotDeleted))
                {
                    var items = new List<ContainerDropItemDto>();

                    foreach (var item in containerDrop.ContainerDropItems.Where(o => o.IsNotDeleted))
                    {
                        items.Add(item);
                    }
                    containerDrop.ContainerDropItems = items;
                    containerDrops.Add(containerDrop);
                }
                containerDto.ContainerDrops = containerDrops;
                containers.Add(containerDto);
            }
            mappedCashDeposit.Containers = containers;
            RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers);
            return mappedCashDeposit;
        }

        public CashDeposit FindUnMappedDeposit(int id)
        {
            var cashDeposit = _repository.Query<CashDeposit>(o => o.CashDepositId == id,
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
                                        o => o.Site.Address,
                                        o => o.Site.Address.AddressType,
                                        o => o.Status,
                                        o => o.Site.Merchant,
                                        o => o.Account.Bank,
                                        o => o.Containers.Select(p => p.ContainerDrops),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems)),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.Status)),
                                        o => o.Containers.Select(p => p.ContainerDrops.Select(b => b.ContainerDropItems.Select(f => f.Denomination)))
                        ).FirstOrDefault();

            return cashDeposit;
        }

        /// <summary>
        /// Submit Deposit
        /// </summary>
        /// <param name="cashDeposit"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<CashDeposit> Submit(CashDepositDto cashDeposit, User user)
        {
            try
            {
                var date = DateTime.Now;

                var responseMessage = string.Empty;

                using (var scope = new TransactionScope())
                {

                    // map to Cash Deposit from transfer object.
                    var mappedCashDeposit = _mapper.Map<CashDepositDto, CashDeposit>(cashDeposit);
                    mappedCashDeposit.ProductTypeId = GetProductTypeId("MYSBV_DEPOSIT");
                    mappedCashDeposit.VaultSource = VaultSource.MYSBV.Name();

                    if (mappedCashDeposit.CashDepositId <= 0)
                    {
                        mappedCashDeposit.LastChangedById = user.UserId;
                        mappedCashDeposit.LastChangedDate = date;
                        mappedCashDeposit.CreatedById = user.UserId;
                        mappedCashDeposit.CreateDate = date;
                        mappedCashDeposit.IsNotDeleted = true;
                        mappedCashDeposit.EntityState = State.Added;
                        mappedCashDeposit.TransactionReference = _lookup.GenerateTransactionNumber(mappedCashDeposit.SiteId);
                    }
                    else
                    {
                        mappedCashDeposit.LastChangedById = user.UserId;
                        mappedCashDeposit.LastChangedDate = date;
                        mappedCashDeposit.CreatedById = mappedCashDeposit.CreatedById;
                        mappedCashDeposit.CreateDate = mappedCashDeposit.CreateDate;
                        mappedCashDeposit.IsNotDeleted = true;
                        mappedCashDeposit.EntityState = State.Modified;
                    }

                    // get roles a user belongs to
                    var roles = Roles.GetRolesForUser(user.UserName);
                    // check if user is supervisor
                    var isSupervisor = roles.Contains("RetailSupervisor") || roles.Contains("SBVTellerSupervisor");

                    var isApprovalRequired =
                        _repository.Any<Site>(o => o.SiteId == mappedCashDeposit.SiteId && o.ApprovalRequiredFlag);

                    // check if site is configured to require 
                    // approval for all non supervisor user.
                    if (isApprovalRequired && isSupervisor == false)
                    {
                        // set deposit to pending approval
                        mappedCashDeposit.StatusId = GetStatusId("PENDING");
                        responseMessage =
                            "Cash deposit successfully sent for approval. Note that this deposit is not yet submitted, will remain pending until the Approval process is done.";
                    }
                    else
                    {
                        // submit deposit
                        mappedCashDeposit.StatusId = GetStatusId("SUBMITTED");
                        mappedCashDeposit.IsSubmitted = true;
                        mappedCashDeposit.SubmitDateTime = date;
                        responseMessage = "Cash deposit submitted successfully.";
                    }

                    foreach (var container in mappedCashDeposit.Containers)
                    {
                        container.ContainerType = null;
                        container.CashDeposit = null;

                        // In case a new container got appended but not saved
                        // when submit fires, it has to be created and added to deposit
                        if (container.ContainerId == 0)
                        {
                            // check if container wasnt used before.
                            if (WasSerialOrSealNumberUsed(container.SerialNumber, container.SealNumber))
                            {
                                return new MethodResult<CashDeposit>(MethodStatus.Error, null,
                                    "Serial Number Has been used before. Verify and try again.");
                            }
                            container.LastChangedById = user.UserId;
                            container.LastChangedDate = date;
                            container.CreatedById = user.UserId;
                            container.CreateDate = date;
                            container.IsNotDeleted = true;
                            container.EntityState = State.Added;

                            container.ReferenceNumber = _lookup.GenerateTransactionNumberForContainer(mappedCashDeposit.SiteId);

                            foreach (var containerDrop in container.ContainerDrops)
                            {
                                containerDrop.ReferenceNumber =
                                    _lookup.GenerateTransactionNumberForDrop(mappedCashDeposit.SiteId);

                                containerDrop.StatusId = GetStatusId("SUBMITTED");
                                containerDrop.LastChangedById = user.UserId;
                                containerDrop.LastChangedDate = date;
                                containerDrop.CreatedById = user.UserId;
                                containerDrop.CreateDate = date;
                                containerDrop.IsNotDeleted = true;
                                containerDrop.EntityState = State.Added;

                                foreach (var newItem in containerDrop.ContainerDropItems)
                                {
                                    newItem.DenominationId = GetDenomination(newItem.ValueInCents);
                                    newItem.DenominationType = GetDenominationType(newItem.DenominationId);
                                    newItem.DenominationName = GetDenominationName(newItem.DenominationId);
                                    newItem.Value = (newItem.ValueInCents/100M)*newItem.Count;

                                    newItem.EntityState = State.Added;
                                    newItem.LastChangedById = user.UserId;
                                    newItem.LastChangedDate = date;
                                    newItem.CreatedById = user.UserId;
                                    newItem.CreateDate = date;
                                    newItem.IsNotDeleted = true;
                                }
                            }
                        }
                        else
                        {
                            // check if a bag serial number is being updated to an
                            // existing bag number
                            var tryingToUpdateToExistingBag = IsTryingToUpdateToExistingBag(container);

                            if (tryingToUpdateToExistingBag)
                            {
                                if (WasSerialOrSealNumberUsed(container.SerialNumber, container.SealNumber))
                                {
                                    return new MethodResult<CashDeposit>(MethodStatus.Error, null,
                                        "Serial Number Has been used before. Verify and try again.");
                                }
                            }

                            // For existing containers, Mark as modified
                            container.LastChangedById = user.UserId;
                            container.LastChangedDate = date;
                            container.CreatedById = mappedCashDeposit.CreatedById;
                            container.CreateDate = mappedCashDeposit.CreateDate;
                            container.IsNotDeleted = true;
                            container.EntityState = State.Modified;

                            // get deleted containers
                            var containersToDelete =
                                _repository.Query<Container>(o => o.CashDepositId == cashDeposit.CashDepositId,
                                    o => o.ContainerDrops,
                                    o => o.ContainerDrops.Select(e => e.ContainerDropItems));

                            // mark all containers as deleted first
                            foreach (var bagDeleted in containersToDelete)
                            {
                                ((IEntity) bagDeleted).WalkObjectGraph(o =>
                                {
                                    o.EntityState = State.Deleted;
                                    o.IsNotDeleted = false;
                                    return false;
                                }, a => { });
                                _repository.Delete<Container>(bagDeleted.ContainerId, user.UserId);
                            }
                        }

                        #region ContainerDrops Management

                        // mark any new drop as Added
                        foreach (var containerDrop in container.ContainerDrops)
                        {
                            containerDrop.StatusId = GetStatusId("SUBMITTED");
                            containerDrop.IsNotDeleted = true;
                            containerDrop.LastChangedById = user.UserId;
                            containerDrop.LastChangedDate = date;

                            if (containerDrop.ContainerDropId == 0)
                            {
                                containerDrop.ReferenceNumber =
                                    _lookup.GenerateTransactionNumberForDrop(mappedCashDeposit.SiteId);
                                containerDrop.EntityState = State.Added;
                                containerDrop.CreatedById = user.UserId;
                                containerDrop.CreateDate = date;

                                foreach (var newItem in containerDrop.ContainerDropItems)
                                {
                                    newItem.DenominationId = GetDenomination(newItem.ValueInCents);
                                    newItem.DenominationType = GetDenominationType(newItem.DenominationId);
                                    newItem.DenominationName = GetDenominationName(newItem.DenominationId);
                                    newItem.Value = (newItem.ValueInCents/100M)*newItem.Count;
                                    newItem.EntityState = State.Added;
                                    newItem.LastChangedById = user.UserId;
                                    newItem.LastChangedDate = date;
                                    newItem.CreatedById = mappedCashDeposit.CreatedById;
                                    newItem.CreateDate = mappedCashDeposit.CreateDate;
                                    newItem.IsNotDeleted = true;
                                }
                            }
                            else
                            {
                                containerDrop.EntityState = State.Modified;
                                containerDrop.CreatedById = mappedCashDeposit.CreatedById;
                                containerDrop.CreateDate = mappedCashDeposit.CreateDate;

                                #region Deleted ContainerDrops

                                // get deleted containers Drops
                                var containerDropsToDelete =
                                    _repository.Query<ContainerDrop>(o => o.ContainerId == container.ContainerId,
                                        o => o.ContainerDropItems);

                                foreach (var bagDropDeleted in containerDropsToDelete)
                                {
                                    ((IEntity) bagDropDeleted).WalkObjectGraph(o =>
                                    {
                                        o.EntityState = State.Deleted;
                                        o.IsNotDeleted = false;
                                        return false;
                                    }, a => { });
                                    _repository.Delete<ContainerDrop>(bagDropDeleted.ContainerDropId, user.UserId);
                                }

                                #endregion

                                #region Deleted ContainerDropItems

                                // get deleted containers-Drop-Items
                                var containerDropItemsToDelete =
                                    _repository.Query<ContainerDropItem>(
                                        o => o.ContainerDropId == containerDrop.ContainerDropId);

                                foreach (var bagDropItemDeleted in containerDropItemsToDelete)
                                {
                                    bagDropItemDeleted.EntityState = State.Deleted;
                                    bagDropItemDeleted.IsNotDeleted = false;
                                    _repository.Delete<ContainerDropItem>(bagDropItemDeleted.ContainerDropItemId,
                                        user.UserId);
                                }

                                #endregion

                                // mark as Added or Modified
                                foreach (var containerDropItem in containerDrop.ContainerDropItems)
                                {
                                    containerDropItem.DenominationId = GetDenomination(containerDropItem.ValueInCents);
                                    containerDropItem.DenominationType =
                                        GetDenominationType(containerDropItem.DenominationId);
                                    containerDropItem.DenominationName =
                                        GetDenominationName(containerDropItem.DenominationId);
                                    containerDropItem.Value = (containerDropItem.ValueInCents/100M)*
                                                              containerDropItem.Count;
                                    containerDropItem.EntityState = containerDropItem.ContainerDropItemId == 0
                                        ? State.Added
                                        : State.Modified;

                                    containerDropItem.LastChangedById = user.UserId;
                                    containerDropItem.LastChangedDate = date;
                                    containerDropItem.CreatedById = mappedCashDeposit.CreatedById;
                                    containerDropItem.CreateDate = mappedCashDeposit.CreateDate;
                                    containerDropItem.IsNotDeleted = true;
                                }
                            }
                        }

                        #endregion
                    }

                    mappedCashDeposit.StatusId = GetStatusId("SUBMITTED");
                    mappedCashDeposit.Status = null;

                    var isSubmitted = _repository.Update(mappedCashDeposit) > 0;
                    // update Cash deposit
                    scope.Complete();

                    RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers);

                    return isSubmitted
                        ? new MethodResult<CashDeposit>(MethodStatus.Successful, mappedCashDeposit, responseMessage)
                        : new MethodResult<CashDeposit>(MethodStatus.Error, null, "Error Updating the cash deposit");
                }
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Error Updating the cash deposit");
            }
        }


        public MethodResult<bool> Delete(int id, User user)
        {
            var deposit = Find(id);

            if (deposit != null)
            {
                if (deposit.Containers.Any(container => container.ContainerDrops.Any(containerDrop => containerDrop.StatusId == GetStatusId("SUBMITTED"))))
                {
                    return new MethodResult<bool>(MethodStatus.Warning, false, "Cannot delete a deposit that already has submitted drops.");
                }

                if (_repository.Delete<CashDeposit>(id, user.UserId) > 0)
                {
                    return new MethodResult<bool>(MethodStatus.Successful, true, "Cash Deposit Was Deleted Successfully.");
                }
            }
            return new MethodResult<bool>(MethodStatus.Error, false, "Cash Deposit Was Not Deleted.");
        }


        /// <summary>
        /// Submit appended drop
        /// </summary>
        /// <param name="containerDrop"></param>
        /// <param name="containerAmount"></param>
        /// <param name="description"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<ContainerDrop> SubmitDrop(ContainerDropDto containerDrop, decimal containerAmount, string description, User user)
        {
            var drop = _mapper.Map<ContainerDropDto, ContainerDrop>(containerDrop);

            // check On DROP check if drop serial number was used before.
            if (description.ToLower() == "drop")
            {
                if (ContainerDropSerialNumberUsed(drop))
                {
                    return new MethodResult<ContainerDrop>(MethodStatus.Error, null, "Drop Serial Number Has been used before. Verify and try again.");
                }
            }

            try
            {
                var date = DateTime.Now;

                var mappedContainerDrop = _mapper.Map<ContainerDropDto, ContainerDrop>(containerDrop);

                // get the container to which the drop/deposit belongs
                var container = _repository.Query<Container>(x => x.ContainerId == mappedContainerDrop.ContainerId,
                        o => o.CashDeposit,
                        o => o.CashDeposit.Account,
                        o => o.CashDeposit.DepositType,
                        o => o.CashDeposit.Device,
                        o => o.CashDeposit.ErrorCode,
                        o => o.CashDeposit.ProductType,
                        o => o.CashDeposit.Site,
                        o => o.CashDeposit.Site.SiteContainers,
                        o => o.CashDeposit.Site.Address,
                        o => o.CashDeposit.Site.Address.AddressType,
                        o => o.CashDeposit.Status,
                        o => o.CashDeposit.Site.Merchant,
                        o => o.CashDeposit.Site.Accounts,
                        o => o.CashDeposit.Site.Accounts.Select(b => b.Bank),
                        o => o.CashDeposit.Site.Accounts.Select(b => b.TransactionType),
                        o => o.CashDeposit.Site.Accounts.Select(b => b.AccountType),
                        o => o.CashDeposit.Account.Bank,
                        o => o.CashDeposit.VaultBeneficiaries,
                        o => o.CashDeposit.Containers,
                        o => o.CashDeposit.Containers.Select(d => d.ContainerType),
                        o => o.CashDeposit.Containers.Select(d => d.ContainerType.ContainerTypeAttributes),
                        o => o.ContainerDrops,
                        o => o.ContainerDrops.Select(e => e.ContainerDropItems),
                        o => o.ContainerDrops.Select(e => e.ContainerDropItems.Select(d => d.Denomination))
                    ).FirstOrDefault();

                // get Site from cash deposit
                // this is used to generate Drop transaction reference number.
                var siteId = container.CashDeposit.SiteId;

                // check if drop is new or its an update
                if (mappedContainerDrop.ContainerDropId > 0)
                {
                    // get the drop to update from container.
                    // in some instances drop items are deleted or added to the drop
                    // check for such a scenario
                    var oldDrop = container.ContainerDrops.FirstOrDefault(
                            e => e.ContainerDropId == mappedContainerDrop.ContainerDropId);

                    // subtract the dropAmount from container
                    // in case there were items removed/appended from/to the drop 
                    // this will be added with the actual/current drop amount from "mappedContainerDrop" below.
                    container.Amount -= oldDrop.Amount;
                    container.CashDeposit.DepositedAmount -= oldDrop.Amount;

                    // mark deleted/Append items
                    mappedContainerDrop = AppendDeleteDropItems(mappedContainerDrop, oldDrop);

                    // update container amount
                    container.Amount += mappedContainerDrop.Amount;
                    container.CashDeposit.DepositedAmount += mappedContainerDrop.Amount;

                    // greater than zero show its an update
                    mappedContainerDrop.EntityState = State.Modified;
                }
                else
                {
                    // this is a new drop
                    // mark it as new by assigning "Modified State".
                    mappedContainerDrop = MarkContainerDropItems(mappedContainerDrop);

                    mappedContainerDrop.EntityState = State.Added;
                    mappedContainerDrop.ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(siteId);

                    // increment container amount
                    container.Amount += mappedContainerDrop.Amount;
                    // append drop to container
                    container.ContainerDrops.Add(mappedContainerDrop);
                    // increment deposit amount
                    container.CashDeposit.DepositedAmount += mappedContainerDrop.Amount;
                }

                mappedContainerDrop.StatusId = GetStatusId("SUBMITTED");
                // since there is changes to container, also mark it as updated.
                container.EntityState = State.Modified;

                // mark deposit as modified since deposit amount is changing
                container.CashDeposit.EntityState = State.Modified;

                ((IEntity)mappedContainerDrop).WalkObjectGraph(o =>
                {
                    o.CreatedById = user.UserId;
                    o.CreateDate = date;
                    o.LastChangedById = user.UserId;
                    o.LastChangedDate = date;
                    return false;
                }, a => { });

                mappedContainerDrop.Status = null;

                // update deposit
                _repository.Update(container.CashDeposit);

                return mappedContainerDrop.ContainerDropId > 0
                    ? new MethodResult<ContainerDrop>(MethodStatus.Successful, mappedContainerDrop, description + " Submitted Successfully")
                    : new MethodResult<ContainerDrop>(MethodStatus.Error, null, "Failed to submit " + description);
            }
            catch (Exception ex)
            {
                var message = ex.Message;
                return new MethodResult<ContainerDrop>(MethodStatus.Error, null, "Internal error occurred, failed to submit " + description);
            }
        }


        /// <summary>
        /// Submit New Drop In New Container
        /// </summary>
        /// <param name="container"></param>
        /// <param name="containerAmount"></param>
        /// <param name="description"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<Container> SubmitNewDropInNewContainer(ContainerDto container, decimal containerAmount, string description, User user)
        {
            var date = DateTime.Now;

            if (WasSerialOrSealNumberUsed(container.SerialNumber, container.SealNumber))
            {
                return new MethodResult<Container>(MethodStatus.Error, null, "Serial Number Has been used before. Verify and try again.");
            }

            var mappedContainer = _mapper.Map<ContainerDto, Container>(container);
            mappedContainer.CashDepositId = container.CashDepositId;

            var deposit = Find(container.CashDepositId);
            mappedContainer.ReferenceNumber = _lookup.GenerateTransactionNumberForContainer(deposit.SiteId);

            // calculate container total
            var containerTotal = 0m;
            foreach (var containerDrop in mappedContainer.ContainerDrops)
            {
                containerTotal += containerDrop.Amount;

                containerDrop.ReferenceNumber = _lookup.GenerateTransactionNumberForDrop(deposit.SiteId);

                if (description.ToLower() == "drop")
                {
                    var toMap = container.ContainerDrops.FirstOrDefault();
                    var drop = _mapper.Map<ContainerDropDto, ContainerDrop>(toMap);

                    if (ContainerDropSerialNumberUsed(drop))
                    {
                        return new MethodResult<Container>(MethodStatus.Error, null, "Drop Serial Number Has been used before. Verify and try again.");
                    }
                }
                containerDrop.StatusId = GetStatusId("SUBMITTED");

                foreach (var item in containerDrop.ContainerDropItems.Where(e => e.ValueInCents > 0))
                {
                    item.DenominationId = GetDenomination(item.ValueInCents);
                    item.DenominationType = GetDenominationType(item.DenominationId);
                    item.DenominationName = GetDenominationName(item.DenominationId);
                }
            }
            mappedContainer.Amount = containerTotal;

            ((IEntity)mappedContainer).WalkObjectGraph(o =>
            {
                o.CreatedById = user.UserId;
                o.CreateDate = date;
                o.LastChangedById = user.UserId;
                o.LastChangedDate = date;
                o.IsNotDeleted = true;
                o.EntityState = State.Added;
                return false;
            }, a => { });

            var containerId = 0;

            using (var scope = new TransactionScope())
            {
                RemoveCyclicCashDepositsInContainer(mappedContainer); // trim cyclic redundancy before saving
                containerId = _repository.Add(mappedContainer);
                RemoveCyclicCashDepositsInContainer(mappedContainer); // trim cyclic redundancy for JSON serialization

                var cashDeposit = _repository.Query<CashDeposit>(o => o.CashDepositId == mappedContainer.CashDepositId).FirstOrDefault();
                cashDeposit.EntityState = State.Modified;
                cashDeposit.DepositedAmount += mappedContainer.Amount;
                _repository.Update(cashDeposit);

                scope.Complete();
            }

            return containerId > 0
                ? new MethodResult<Container>(MethodStatus.Successful, mappedContainer, description + " Submitted Successfully")
                : new MethodResult<Container>(MethodStatus.Error, null, "Failed to submit " + description);
        }


        public MethodResult<IEnumerable<ContainerDto>> GetContainers(int cashDepositId)
        {
            var cashDeposit = Find(cashDepositId);
            var containers = cashDeposit.Containers;
            return new MethodResult<IEnumerable<ContainerDto>>(MethodStatus.Successful, containers);
        }



        public Container FindContainer(int containerId)
        {
            return _repository.Query<Container>(o => o.ContainerId == containerId,
                                                    o => o.ContainerDrops
                                               ).FirstOrDefault();
        }


        public MethodResult<IEnumerable<ContainerDropDto>> GetDrops(int containerId)
        {
            var containerDrops = _repository.Query<ContainerDrop>(a => a.ContainerId == containerId, b => b.ContainerDropItems, c => c.Status);
            var containerDropsDto = containerDrops.Select(ContainerDrop => _mapper.Map<ContainerDrop, ContainerDropDto>(ContainerDrop)).ToList();

            foreach (var containerDrop in containerDrops)
            {
                string depositTypeName = containerDrop.Container.CashDeposit.DepositType.Name;

                foreach (var containerDropDto in containerDropsDto)
                {
                    containerDropDto.Name = depositTypeName;
                }
            }

            return new MethodResult<IEnumerable<ContainerDropDto>>(MethodStatus.Successful, containerDropsDto);
        }


        public MethodResult<bool> DeleteDrop(int containerDropId, User user)
        {
            var drop = _repository.Query<ContainerDrop>(a => a.ContainerDropId == containerDropId).FirstOrDefault();

            if (drop != null)
            {
                if (drop.StatusId == GetStatusId("SUBMITTED"))
                {
                    return new MethodResult<bool>(MethodStatus.Warning, false, "Cannot Delete An Already Submitted Drop");
                }

                using (var scope = new TransactionScope())
                {
                    var isDeleted = _repository.Delete<ContainerDrop>(containerDropId, user.UserId) > 0;

                    if (isDeleted)
                    {
                        // when containerDrop is successfully deleted
                        // update cashDeposit DepositedAmount & Container Amount
                        var containerDrop = _repository.Find<ContainerDrop>(containerDropId);

                        var bag = _repository.Find<Container>(containerDrop.ContainerId);

                        var cashDeposit = _repository.Query<CashDeposit>(e => e.CashDepositId == bag.CashDepositId,
                            o => o.Containers,
                            o => o.Containers.Select(e => e.ContainerDrops)).FirstOrDefault();


                        // get Deposit to which the ContainerDrop in question is attached

                        var depositSum = 0M;
                        foreach (var container in cashDeposit.Containers)
                        {
                            var containerSum = container.ContainerDrops
                                .Where(item => item.ContainerDropId != containerDropId && item.IsNotDeleted)
                                .Sum(item => item.Amount);

                            container.Amount = containerSum;
                            container.EntityState = State.Modified;
                            depositSum += container.Amount;
                        }
                        cashDeposit.DepositedAmount = depositSum;
                        cashDeposit.EntityState = State.Modified;
                        _repository.Update(cashDeposit);
                    }
                    scope.Complete();
                }
                if (_repository.Delete<ContainerDrop>(containerDropId, user.UserId) > 0)
                {
                    return new MethodResult<bool>(MethodStatus.Successful, true, "Drop Deleted Successfully");
                }

                return new MethodResult<bool>(MethodStatus.Error, false, "Error Deleting Drop");
            }

            return new MethodResult<bool>(MethodStatus.Error, false, "Drop Does Not Exist.");
        }


        public CashDepositDto CashDepositInitializer(int cashDepositId)
        {
            var cashDeposit = _repository.Query<CashDeposit>(a => a.CashDepositId == cashDepositId).FirstOrDefault();
            var mappedCashDeposit = _mapper.Map<CashDeposit, CashDepositDto>(cashDeposit);

            if (mappedCashDeposit != null) mappedCashDeposit.CashDepositId = 0;

            foreach (var drop in mappedCashDeposit.Containers.SelectMany(container => container.ContainerDrops))
            {
                drop.StatusId = GetStatusId("ACTIVE");
                drop.ContainerDropId = 0;
            }

            var containers = mappedCashDeposit.Containers.OrderBy(e => e.ContainerId).ToList();

            foreach (var container in containers)
            {
                container.ContainerId = 0;
                var drops = container.ContainerDrops.OrderBy(e => e.Number);
                var dropsList = drops.ToList();
                container.ContainerDrops = dropsList;
            }
            mappedCashDeposit.Containers = containers;

            return mappedCashDeposit;
        }

        #endregion


        public MethodResult<IEnumerable<ContainerDropDto>> GetContainerDrops(int containerId)
        {
            var containerDrops = _repository.Query<ContainerDrop>(a => a.ContainerId == containerId,
                                                                    a => a.ContainerDropItems,
                                                                    a => a.ContainerDropItems.Select(e => e.Denomination),
                                                                    a => a.Status,
                                                                    a => a.Container,
                                                                    a => a.Container.CashDeposit
                                                                 );
            var containerDropsDto = containerDrops.Select(ContainerDrop => _mapper.Map<ContainerDrop, ContainerDropDto>(ContainerDrop))
                    .ToList();

            return new MethodResult<IEnumerable<ContainerDropDto>>(MethodStatus.Successful, containerDropsDto);
        }

        /// <summary>
        /// get site by siteId
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        public Site GetSite(int siteId)
        {
            return _repository.Query<Site>(o => o.SiteId == siteId).FirstOrDefault();
        }


        /// <summary>
        /// Append or Delete drop items
        /// </summary>
        /// <param name="mapped"></param>
        /// <param name="old"></param>
        /// <returns></returns>
        private ContainerDrop AppendDeleteDropItems(ContainerDrop mapped, ContainerDrop old)
        {
            // update drop amount
            old.Amount = mapped.Amount;
            old.EntityState = State.Modified;

            // this enumeration checks for new items & updates to the existing ContainerDrop
            // in database
            foreach (var item in mapped.ContainerDropItems)
            {
                item.DenominationId = GetDenomination(item.ValueInCents);
                item.DenominationType = GetDenominationType(item.DenominationId);
                item.DenominationName = GetDenominationName(item.DenominationId);
                item.IsNotDeleted = true;

                // check if item is new 
                if (item.ContainerDropItemId == 0)
                {
                    // mark item as new
                    item.EntityState = State.Added;
                    // add item to old/ existing container drop
                    old.ContainerDropItems.Add(item);
                }
                else
                {
                    // simply mark the item as modified to enable update.
                    var oldItem =
                        old.ContainerDropItems.FirstOrDefault(e => e.ContainerDropItemId == item.ContainerDropItemId);

                    if (oldItem != null)
                    {
                        oldItem.EntityState = State.Modified;
                        item.DenominationId = GetDenomination(item.ValueInCents);
                        oldItem.DenominationType = GetDenominationType(item.DenominationId);
                        oldItem.DenominationName = GetDenominationName(item.DenominationId);
                        oldItem.Count = item.Count;
                        oldItem.Value = item.Value;
                    }
                }
            }

            // Check for deleted items in the Existing containerDrop
            foreach (var dropItem in old.ContainerDropItems)
            {
                var found = mapped.ContainerDropItems.Where(e => e.ContainerDropItemId > 0).Any(o => o.ContainerDropItemId == dropItem.ContainerDropItemId);

                if (found == false)
                {
                    dropItem.IsNotDeleted = dropItem.ContainerDropItemId == 0 ? true : false;
                    dropItem.EntityState = dropItem.ContainerDropItemId == 0 ? State.Added : State.Modified;
                }
            }
            return old;
        }

        /// <summary>
        /// Initialize all drop items
        /// </summary>
        /// <param name="containerDrop"></param>
        /// <returns></returns>
        private ContainerDrop MarkContainerDropItems(ContainerDrop containerDrop)
        {
            foreach (var item in containerDrop.ContainerDropItems.Where(e => e.ValueInCents > 0))
            {
                item.DenominationId = GetDenomination(item.ValueInCents);
                item.DenominationType = GetDenominationType(item.DenominationId);
                item.DenominationName = GetDenominationName(item.DenominationId);
                item.IsNotDeleted = true;
                item.EntityState = State.Added;
            }
            return containerDrop;
        }

        #region Methods

        /// <summary>
        /// check if drop serial number was used before
        /// </summary>
        /// <param name="drop"></param>
        private bool ContainerDropSerialNumberUsed(ContainerDrop drop)
        {
            var containerDrops = _repository.All<ContainerDrop>()
                .Where(c =>
                    c.BagSerialNumber == drop.BagSerialNumber &&
                    drop.BagSerialNumber != null &&
                    c.ContainerDropId != drop.ContainerDropId
                );
            return containerDrops.Any();
        }

        /// <summary>
        /// get denomination
        /// </summary>
        /// <param name="valueInCents"></param>
        private int GetDenomination(int valueInCents)
        {
            var denomination = _repository.Query<Denomination>(e => e.ValueInCents == valueInCents && valueInCents > 0).FirstOrDefault();
            return denomination != null ? denomination.DenominationId : 0;
        }

        /// <summary>
        /// get product type
        /// </summary>
        /// <param name="productTypeLookupKey"></param>
        public int GetProductTypeId(string productTypeLookupKey)
        {
            var productType = _repository.All<ProductType>()
                .FirstOrDefault(c => c.LookUpKey == productTypeLookupKey);
            return productType != null ? productType.ProductTypeId : 0;
        }

        /// <summary>
        /// get status Id
        /// </summary>
        /// <param name="lookupKey"></param>
        private int GetStatusId(string lookupKey)
        {
            var status = _repository.Query<Status>(a => a.LookUpKey == lookupKey.ToUpper()).FirstOrDefault();
            return status == null ? 0 : status.StatusId;
        }

        /// <summary>
        /// get denomination name
        /// </summary>
        /// <param name="denominationId"></param>
        private string GetDenominationName(int denominationId)
        {
            var denomination = _repository.Query<Denomination>(o =>
                o.DenominationId == denominationId).FirstOrDefault();

            // return denomination name
            return denomination != null ? denomination.Description : string.Empty;
        }

        /// <summary>
        ///  get denomination type
        /// </summary>
        /// <param name="denominationId"></param>
        private string GetDenominationType(int denominationId)
        {
            return (from type in _repository.All<DenominationType>()
                    join deno in _repository.All<Denomination>() on type.DenominationTypeId equals
                        deno.DenominationTypeId
                    where deno.DenominationId == denominationId
                    select type
                ).First().Name;
        }


        /// <summary>
        /// Check if Trying to update bag to an existing one.
        /// </summary>
        /// <param name="bag"></param>
        private bool IsTryingToUpdateToExistingBag(Container bag)
        {
            // check deposit bags
            var containers = _repository.All<Container>()
                .Where(c =>
                        ((c.SerialNumber == bag.SerialNumber && !string.IsNullOrWhiteSpace(bag.SerialNumber)) ||
                        (c.SealNumber == bag.SealNumber && !string.IsNullOrWhiteSpace(bag.SealNumber))) &&
                        (bag.ContainerId != c.ContainerId)
                      );

            var isUsedOnCashDeposit = containers.Any();

            // check cash order bags
            var cashOrdercontainers = _repository.All<CashOrder>()
                    .Where(c =>
                            ((c.ContainerNumberWithCashForExchange == bag.SerialNumber && !string.IsNullOrWhiteSpace(bag.SerialNumber)) ||
                            (c.EmptyContainerOrBagNumber == bag.SealNumber && !string.IsNullOrWhiteSpace(bag.SealNumber))) &&
                            (bag.ContainerId != c.CashOrderContainerId)
                        );

            var isUsedOnCashOrder = cashOrdercontainers.Any();

            // result
            return isUsedOnCashDeposit || isUsedOnCashOrder;
        }


        /// <summary>
        /// Check if Container was used before
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="sealNumber"></param>
        private bool WasSerialOrSealNumberUsed(string serialNumber, string sealNumber)
        {
            // check deposit bags
            var containers = _repository.All<Container>()
                .Where(c => (c.SerialNumber == serialNumber && !string.IsNullOrWhiteSpace(serialNumber)) ||
                      (c.SealNumber == sealNumber && !string.IsNullOrWhiteSpace(sealNumber)));

            var isUsedOnCashDeposit = containers.Any();

            // check cash order bags
            var cashOrdercontainers = _repository.All<CashOrder>()
                    .Where(c => (c.ContainerNumberWithCashForExchange == serialNumber && !string.IsNullOrWhiteSpace(serialNumber))
                    ||
                    (c.EmptyContainerOrBagNumber == sealNumber && !string.IsNullOrWhiteSpace(sealNumber)));

            var isUsedOnCashOrder = cashOrdercontainers.Any();

            // result
            return isUsedOnCashDeposit || isUsedOnCashOrder;
        }
        #endregion

    }
}