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

namespace Application.Modules.CashHandling.Deposit
{
	public class CashDepositValidation : ICashDepositValidation
	{
		#region Fields

		private readonly IMapper _mapper;
		private readonly IRepository _repository;

		#endregion

		#region Constructor

		public CashDepositValidation(IRepository repository, IMapper mapper, ILookup lookup)
		{
			_repository = repository;
			_mapper = mapper;
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

		public MethodResult<CashDeposit> Add(CashDepositDto cashDeposit, User user)
		{
			var date = DateTime.Now;

			cashDeposit.StatusId = GetStatusId(cashDeposit.TransactionStatusName);
			cashDeposit.TransactionReference = GenerateTransactionNumber(cashDeposit.SiteId);
			cashDeposit.ProductTypeId = GetProductTypeId("MYSBV_DEPOSIT");

			foreach (var bag in cashDeposit.Containers)
			{
				if (WasSerialOrSealNumberUsed(bag))
				{
					return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Serial Number Has been used before. Verify and try again.");
				}

				bag.ContainerTypeId = bag.ContainerTypeId == 0 ? 0 : bag.ContainerTypeId;
				bag.ReferenceNumber = GenerateTransactionNumberForContainer(cashDeposit.SiteId);
				bag.ContainerType = null;

				if (bag.ContainerDrops == null) continue;

				foreach (var containerDrop in bag.ContainerDrops)
				{
					if (cashDeposit.DepositTypeName == "Multi Drop")
					{
						if (ContainerDropSerialNumberUsed(containerDrop))
						{
							return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Drop Serial Number Has been used before. Verify and try again.");
						}
					}

					containerDrop.ReferenceNumber = GenerateTransactionNumberForDrop(cashDeposit.SiteId);
					containerDrop.StatusId = GetStatusId(containerDrop.Name);

					foreach (var item in containerDrop.ContainerDropItems.Where(i => i.ValueInCents > 0))
					{
						item.DenominationId = GetDenomination(item.ValueInCents);
						item.DenominationType = GetDenominationType(item.DenominationId);
						item.DenominationName = GetDenominationName(item.DenominationId);
						item.Value = (item.ValueInCents / 100.0M) * item.Count;
					}
				}
			}

			CashDeposit mappedCashDeposit = _mapper.Map<CashDepositDto, CashDeposit>(cashDeposit);
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
				container.CashDeposit = null;
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
						container.ReferenceNumber = GenerateTransactionNumberForContainer(mappedCashDeposit.SiteId);

						foreach (var newdrop in container.ContainerDrops)
						{
							newdrop.ReferenceNumber = GenerateTransactionNumberForDrop(mappedCashDeposit.SiteId);
							newdrop.EntityState = State.Added;

							foreach (var newItem in newdrop.ContainerDropItems)
							{
								newItem.DenominationId = GetDenomination(newItem.ValueInCents);
								newItem.DenominationType = GetDenominationType(newItem.DenominationId);
								newItem.DenominationName = GetDenominationName(newItem.DenominationId);
								newItem.Value = (newItem.ValueInCents/100M)*newItem.Count;
								newItem.EntityState = State.Added;
							}
						}
					}
						//
						// For existing containers
						// Mark as modified
					else
					{
						// get deleted containers
						var containersToDelete = _repository.Query<Container>(o => o.CashDepositId == cashDeposit.CashDepositId,
							o => o.ContainerDrops,
							o => o.ContainerDrops.Select(e => e.ContainerDropItems)
							);


						foreach (var bagDeleted in containersToDelete)
						{
							{
								((IEntity) bagDeleted).WalkObjectGraph(o =>
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
								containerDrop.ReferenceNumber = GenerateTransactionNumberForDrop(mappedCashDeposit.SiteId);
								containerDrop.EntityState = State.Added;

								foreach (var newItem in containerDrop.ContainerDropItems)
								{
									newItem.DenominationId = GetDenomination(newItem.ValueInCents);
									newItem.DenominationType = GetDenominationType(newItem.DenominationId);
									newItem.DenominationName = GetDenominationName(newItem.DenominationId);
									newItem.Value = (newItem.ValueInCents/100M)*newItem.Count;
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
									((IEntity) bagDropDeleted).WalkObjectGraph(o =>
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
									containerDropItem.Value = (containerDropItem.ValueInCents/100M)*containerDropItem.Count;
									containerDropItem.EntityState = containerDropItem.ContainerDropItemId == 0 ? State.Added : State.Modified;
								}
							}
						}

						#endregion

					}
				}

				#endregion

				mappedCashDeposit.EntityState = State.Modified;

				((IEntity) mappedCashDeposit).WalkObjectGraph(o =>
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
					mappedCashDeposit.TransactionReference = GenerateTransactionNumber(mappedCashDeposit.SiteId);
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
			var cashDeposit = _repository.Query<CashDeposit>( o => o.CashDepositId == id,
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
		
		public MethodResult<CashDeposit> Submit(CashDepositDto cashDeposit, User user)
		{
			var roles = Roles.GetRolesForUser(user.UserName);
			var isSupervisor = roles.Contains("RetailSupervisor") || roles.Contains("SBVTellerSupervisor");
			var responseMessage = string.Empty;

			var date = DateTime.Now;
            cashDeposit.LastChangedById = user.UserId;
			cashDeposit.LastChangedDate = date;
			cashDeposit.ProductTypeId = GetProductTypeId("MYSBV_DEPOSIT");

			var site = _repository.Query<Site>(o => o.SiteId == cashDeposit.SiteId).FirstOrDefault();
			
			if (site.ApprovalRequiredFlag && isSupervisor == false)
			{
				cashDeposit.StatusId = GetStatusId("PENDING");
				cashDeposit.TransactionStatusName = "PENDING";
				responseMessage = "Cash deposit successfully sent for approval. Note that this deposit is not yet submitted, will remain pending until the Approval process is done.";
			}
			else
			{
				cashDeposit.StatusId = GetStatusId("SUBMITTED");
				cashDeposit.IsSubmitted = true;
				cashDeposit.SubmitDateTime = date;
				cashDeposit.TransactionStatusName = "SUBMITTED";
				responseMessage = "Cash deposit submitted successfully.";
			}

			foreach (var container in cashDeposit.Containers)
			{
				if (cashDeposit.CashDepositId == 0)
				{
					if (WasSerialOrSealNumberUsed(container))
					{
						return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Serial Number Has been used before. Verify and try again.");
					}

					container.ReferenceNumber = GenerateTransactionNumberForContainer(cashDeposit.SiteId);
				}

				container.ContainerType = null;
				foreach (var containerDrop in container.ContainerDrops)
				{
				    // 
					// Submitting a new Cash Deposit
					if (cashDeposit.CashDepositId == 0)
					{
						if (ContainerDropSerialNumberUsed(containerDrop))
						{
							return new MethodResult<CashDeposit>(MethodStatus.Error, null, "Drop Serial Number Has been used before. Verify and try again.");
						}

						containerDrop.ReferenceNumber = GenerateTransactionNumberForDrop(cashDeposit.SiteId);
					}
					containerDrop.Name = "SUBMITTED";
					containerDrop.StatusId = GetStatusId("SUBMITTED");

					foreach (var item in containerDrop.ContainerDropItems.Where(i => i.ValueInCents > 0))
					{
						item.DenominationId = GetDenomination(item.ValueInCents);
						item.DenominationType = GetDenominationType(item.DenominationId);
						item.DenominationName = GetDenominationName(item.DenominationId);
						item.Value = (item.ValueInCents / 100M) * item.Count;
					}
				}
			}

			if (cashDeposit.CashDepositId == 0)
			{
				cashDeposit.StatusId = GetStatusId(cashDeposit.TransactionStatusName);
				cashDeposit.TransactionReference = GenerateTransactionNumber(cashDeposit.SiteId);
			}

			var mappedCashDeposit = _mapper.Map<CashDepositDto, CashDeposit>(cashDeposit);
			mappedCashDeposit.DepositType = null;
			mappedCashDeposit.Status = null;
			mappedCashDeposit.Site = null;

			RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers);

			((IEntity)mappedCashDeposit).WalkObjectGraph(o =>
			{
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				o.CreateDate = mappedCashDeposit.CreateDate.Value;
				o.CreatedById = mappedCashDeposit.CreatedById;
				o.EntityState = cashDeposit.CashDepositId > 0 ? State.Modified : State.Added;
				return false;
			}, a => { });

            mappedCashDeposit.VaultSource = VaultSource.MYSBV.Name();
		    mappedCashDeposit.IsConfirmed = true;
			var id = _repository.Update(mappedCashDeposit);

			RemoveCyclicCashDepositsInContainers(mappedCashDeposit.Containers);

			return id > 0 ?
				 new MethodResult<CashDeposit>(MethodStatus.Successful, mappedCashDeposit, responseMessage)
				: new MethodResult<CashDeposit>(MethodStatus.Error, null, "Error Updating the cash deposit");
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

		
		public MethodResult<ContainerDrop> SubmitDrop(ContainerDropDto containerDrop, decimal containerAmount, string description, User user)
		{
			var date = DateTime.Now;

			var mappedContainerDrop = _mapper.Map<ContainerDropDto, ContainerDrop>(containerDrop);

			var container = _repository.Query<Container>(x => x.ContainerId == mappedContainerDrop.ContainerId,
			        o => o.CashDeposit,
					o => o.ContainerDrops,
					o => o.ContainerDrops.Select(e => e.ContainerDropItems),
					o => o.ContainerDrops.Select(e => e.ContainerDropItems.Select(d => d.Denomination))
				).FirstOrDefault();

			var cashDeposit = Find(container.CashDepositId);

			mappedContainerDrop.ReferenceNumber = GenerateTransactionNumberForDrop(cashDeposit.SiteId);
			mappedContainerDrop.ContainerId = containerDrop.ContainerId;
			mappedContainerDrop.StatusId = GetStatusId("SUBMITTED");

			if (description.ToLower() == "drop")
			{
				if (ContainerDropSerialNumberUsed(containerDrop))
				{
					return new MethodResult<ContainerDrop>(MethodStatus.Error, null, "Drop Serial Number Has been used before. Verify and try again.");
				}
			}
			
			((IEntity)mappedContainerDrop).WalkObjectGraph(o =>
			{
				o.CreatedById = user.UserId;
				o.CreateDate = date;
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				return false;
			}, a => { });

			if (mappedContainerDrop.ContainerDropId == 0)
			{
				foreach (var item in mappedContainerDrop.ContainerDropItems.Where(e => e.ValueInCents > 0))
				{
					item.DenominationId = GetDenomination(item.ValueInCents);
					item.ContainerDropId = 0;
					item.DenominationType = GetDenominationType(item.DenominationId);
					item.DenominationName = GetDenominationName(item.DenominationId);
					item.EntityState = State.Added;
				}
				mappedContainerDrop.EntityState = State.Added;
				container.ContainerDrops.Add(mappedContainerDrop);

			}
			else
			{
				for (var index = 0; index < container.ContainerDrops.Count; index++)
				{
					if (container.ContainerDrops[index].ContainerDropId == mappedContainerDrop.ContainerDropId)
					{
						container.ContainerDrops[index].EntityState = State.Modified;
						container.ContainerDrops[index].StatusId = GetStatusId("SUBMITTED");

						foreach (var item in container.ContainerDrops[index].ContainerDropItems.Where(e => e.ValueInCents > 0))
						{
							item.DenominationId = GetDenomination(item.ValueInCents);
							item.DenominationType = GetDenominationType(item.DenominationId);
							item.DenominationName = GetDenominationName(item.DenominationId);
							item.EntityState = State.Modified;
						}

						// Added ContainerDropItems to un-submitted drop
						foreach (var item in mappedContainerDrop.ContainerDropItems.Where(e => e.ValueInCents > 0))
						{
							if (item.ContainerDropItemId == 0)
							{
								item.DenominationId = GetDenomination(item.ValueInCents);
								item.DenominationType = GetDenominationType(item.DenominationId);
								item.DenominationName = GetDenominationName(item.DenominationId);
								item.EntityState = State.Added;
								container.ContainerDrops[index].ContainerDropItems.Add(item);
							}
						}

						// Deleted ContainerDropItems from un-submitted drop
						if (mappedContainerDrop.ContainerDropItems.Count != container.ContainerDrops[index].ContainerDropItems.Count)
						{
							foreach (var item in container.ContainerDrops[index].ContainerDropItems.Where(e => e.ValueInCents > 0))
							{
								var found = mappedContainerDrop.ContainerDropItems.Where(e => e.ContainerDropItemId > 0).Any(self => item.ContainerDropItemId == self.ContainerDropItemId);

								if (found == false)
								{
									item.IsNotDeleted = false;
								}
							}
						}
					}
				}
			}



			container.Amount += mappedContainerDrop.Amount;
			container.EntityState = State.Modified;

			container.CashDeposit.DepositedAmount += mappedContainerDrop.Amount;
			container.CashDeposit.EntityState = State.Modified;
			_repository.Update(container);
			var containerDropId = mappedContainerDrop.ContainerDropId;

			mappedContainerDrop.Container = container;

			return containerDropId > 0
				? new MethodResult<ContainerDrop>(MethodStatus.Successful, mappedContainerDrop, description + " Submitted Successfully")
				: new MethodResult<ContainerDrop>(MethodStatus.Error, null, "Failed to submit " + description);
		}
		
		
		public MethodResult<Container> SubmitNewDropInNewContainer(ContainerDto container, decimal containerAmount, string description, User user)
		{
			var date = DateTime.Now;

			if (WasSerialOrSealNumberUsed(container))
			{
				return new MethodResult<Container>(MethodStatus.Error, null, "Serial Number Has been used before. Verify and try again.");
			}

			var mappedContainer = _mapper.Map<ContainerDto, Container>(container);
			mappedContainer.CashDepositId = container.CashDepositId;

			var deposit =Find(container.CashDepositId);
			mappedContainer.ReferenceNumber = GenerateTransactionNumberForContainer(deposit.SiteId);

			foreach (var containerDrop in mappedContainer.ContainerDrops)
			{
				containerDrop.ReferenceNumber = GenerateTransactionNumberForDrop(deposit.SiteId);

				if (description.ToLower() == "drop")
				{
					if (ContainerDropSerialNumberUsed(container.ContainerDrops.FirstOrDefault()))
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
			return _repository.Query<Container>(    o => o.ContainerId == containerId, 
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


		public Site GetSite(int siteId)
		{
			return _repository.Query<Site>(o => o.SiteId == siteId).FirstOrDefault();
		}

		#region Methods

		#region Generate Transaction Number

		private string GetRandomCharacters()
		{
            //var hashSet = new HashSet<string>();
            //var array = new[]
            //{
            //    "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "R", "S", "T", "U", "V",
            //    "W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
            //};
            //while (hashSet.Count < 3)
            //{
            //    hashSet.Add(array[new Random().Next(0, 34)]);
            //}

            //return hashSet.ToHashString();
            return Guid.NewGuid().ToString("N").Substring(0, 3).ToUpper();
		}

		private string Generate(string citCode)
		{
			string depositDate = string.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			string transactionReferenceNumber = string.Concat(GetRandomCharacters(),
				citCode, depositDate);
			return transactionReferenceNumber;
		}

		private string GenerateTransactionNumber(int siteId)
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

		private string GenerateTransactionNumberForContainer(int siteId)
		{
			string citCode =
				_repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault().CitCode;
			string transactionReferenceNumber = Generate(citCode);

			while (true)
			{
				if (!_repository.Any<Container>(a => a.ReferenceNumber == transactionReferenceNumber  && a.IsNotDeleted))
				{
					return transactionReferenceNumber;
				}
				transactionReferenceNumber = Generate(citCode);
			}
		}

		private string GenerateTransactionNumberForDrop(int siteId)
		{
			var citCode = _repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault().CitCode;
			var referenceNumber = Generate(citCode);

			while (true)
			{
				if (!_repository.Any<ContainerDrop>(a => a.ReferenceNumber == referenceNumber  && a.IsNotDeleted))
				{
					return referenceNumber;
				}
				referenceNumber = Generate(citCode);
			}
		}

		#endregion


		private bool ContainerDropSerialNumberUsed(ContainerDropDto drop)
		{
			var containerDrops = _repository.All<ContainerDrop>()
				.Where(c =>
					c.BagSerialNumber == drop.BagSerialNumber &&
					drop.BagSerialNumber != null &&
					c.ContainerDropId != drop.ContainerDropId
				);
			return containerDrops.Any();
		}

		private int GetDenomination(int valueInCents)
		{
			var denomination = _repository.Query<Denomination>(e => e.ValueInCents == valueInCents && valueInCents > 0).FirstOrDefault();
			return denomination != null ? denomination.DenominationId : 0;
		}

		private bool WasSerialOrSealNumberUsed(ContainerDto container)
		{
			var containers = _repository.All<Container>()
				.Where(c => (c.SerialNumber == container.SerialNumber && container.SerialNumber != null) 
				||
				(c.SealNumber == container.SealNumber && container.SealNumber != null));

			var isUsedOnCashDeposit = containers.Any();

			var cashOrdercontainers = _repository.All<CashOrder>()
					.Where(c => (c.ContainerNumberWithCashForExchange == container.SerialNumber && container.SerialNumber != null)
					||
					(c.EmptyContainerOrBagNumber == container.SealNumber && container.SealNumber != null));

			var isUsedOnCashOrder = cashOrdercontainers.Any();

			return isUsedOnCashDeposit || isUsedOnCashOrder;
		}

		public int GetProductTypeId(string productTypeLookupKey)
		{
			var productType = _repository.All<ProductType>()
				.FirstOrDefault(c => c.LookUpKey == productTypeLookupKey);
			return productType != null ? productType.ProductTypeId : 0;
		}

		private int GetStatusId(string lookupKey)
		{
			var status = _repository.Query<Status>(a => a.LookUpKey == lookupKey.ToUpper()).FirstOrDefault();
			return status == null ? 0 : status.StatusId;
		}
        
		private string GetDenominationName(int denominationId)
		{
			return _repository.Query<Denomination>(o => o.DenominationId == denominationId).FirstOrDefault().Description;
		}

		private string GetDenominationType(int denominationId)
		{
			return (from type in _repository.All<DenominationType>()
					join deno in _repository.All<Denomination>() on type.DenominationTypeId equals
						deno.DenominationTypeId
					where deno.DenominationId == denominationId
					select type
				).First().Name;
		}

		#endregion

	}
}