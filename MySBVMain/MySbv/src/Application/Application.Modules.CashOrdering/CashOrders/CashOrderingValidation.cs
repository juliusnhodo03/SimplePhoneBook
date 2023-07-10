using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Security;
using Application.Dto.CashOrder;
using Application.Dto.CashOrderTask;
using Application.Dto.Files;
using Application.Mapper;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Security;
using Utility.Core;

namespace Application.Modules.CashOrdering.CashOrders
{
	public class CashOrderingValidation : ICashOrderingValidation
	{
		#region Fields
         
		private readonly IMapper _mapper;
        private readonly IRepository _repository;
        private readonly ISecurity _security;
        private readonly ILookup _lookup;

		#endregion

		#region Constructor

        public CashOrderingValidation(IRepository repository, IMapper mapper, ISecurity security, ILookup lookup)
		{
			_repository = repository;
			_mapper = mapper;
			_security = security;
            _lookup = lookup;
		}

		#endregion

		#region ICashOrderingValidation

		/// <summary>
		///     Return a list of all CashOrders
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Application.Dto.CashOrder.ListCashOrderDto> All(User user)
		{
			List<CashOrder> orders;

			string[] userRole = Roles.GetRolesForUser(user.UserName);
			string userRoleName = userRole[0];

			switch (userRoleName)
			{
				case "RetailSupervisor":
				case "RetailViewer":
					{
						// All Cash Orders per Site
						var siteIds = _repository.Query<UserSite>(a => a.UserId == user.UserId).Select(b => b.SiteId);

						orders = _repository.Query<CashOrder>(a => siteIds.Contains(a.SiteId), 
							b => b.CashOrderType,
							c => c.Site,
							z => z.Site.SiteContainers,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType)))
                            .Where(x => x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING" || x.Status.LookUpKey == "DECLINED")
							.OrderByDescending(o => o.CashOrderId).ToList();

						break;
					}

				case "SBVAdmin":
				case "SBVFinanceReviewer":
					{
						// All Cash Orders
                        orders = _repository.Query<CashOrder>(x => x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING" || x.Status.LookUpKey == "DECLINED",
							b => b.CashOrderType,
							c => c.Site,
							z => z.Site.SiteContainers,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType)))
							.OrderByDescending(o => o.CashOrderId).ToList();
						break;
					}

				case "RetailUser":
				case "SBVTeller":
				case "SBVDataCapture":
					{
						// Only Orders which were captured by this Teller
						orders = _repository.Query<CashOrder>(a => a.CreatedById == user.UserId,
							b => b.CashOrderType,
							c => c.Site,
							z => z.Site.SiteContainers,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType)))
                            .Where(x => x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING" || x.Status.LookUpKey == "DECLINED")
							.OrderByDescending(o => o.CashOrderId).ToList();
						break;
					}

				case "SBVTellerSupervisor":
				case "SBVRecon":
				case "SBVApprover":
					{
						// All Cash Orders serviced at SBV-User center
						
						var cashCenterIds = _repository.Query<Site>(a => a.CashCenterId == user.CashCenterId).Select(e => e.CashCenterId);

						orders = _repository.Query<CashOrder>(a => cashCenterIds.Contains(a.Site.CashCenterId),
							b => b.CashOrderType,
							c => c.Site,
							z => z.Site.SiteContainers,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType)))
                            .Where(x => x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING" || x.Status.LookUpKey == "DECLINED")
							.OrderByDescending(o => o.CashOrderId).ToList();
						break;
					}

				default:
					{
                        orders = _repository.Query<CashOrder>(x => x.Status.LookUpKey == "ACTIVE" || x.Status.LookUpKey == "PENDING" || x.Status.LookUpKey == "DECLINED",
							b => b.CashOrderType,
							c => c.Site,
							z => z.Site.SiteContainers,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType)))
							.OrderByDescending(o => o.CashOrderId).ToList();
						break;
					}
			}

			var cashOrders = new List<ListCashOrderDto>();

			foreach (var order in orders)
			{
				var cashOrder = _mapper.Map<CashOrder, ListCashOrderDto>(order);
				cashOrder.CashOrderAmount = string.Format("{0:C}", order.CashOrderAmount);
			    cashOrder.StatusName = order.Status.Name;
				cashOrders.Add(cashOrder);
			}
			return cashOrders;
		}
		


		/// <summary>
		/// get logged in user
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
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



        /// <summary>
        /// Find Order by Reference Number
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="user"></param>
        public MethodResult<CashOrderDto> FindByRefenceNumber(string searchText, User user)
        {
            var cashOrder = _repository.Query<CashOrder>(a => a.ReferenceNumber.Trim().ToUpper() == searchText.Trim().ToUpper(),
                            b => b.CashOrderType,
                            c => c.Site,
                            d => d.Site.Address,
                            e => e.Status,
                            f => f.Site.Address.AddressType,
                            g => g.Site.Merchant,
                            h => h.CashOrderContainer,
                            o => o.CashOrderContainer.CashOrderContainerDrops,
                            r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
                            q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
                            q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType))
                        ).OrderByDescending(o => o.CashOrderId).FirstOrDefault();
            
            if (cashOrder != null)
            {
                if (cashOrder.IsSubmitted == false)
                {
                    return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "You cannot process a Cash Order awaiting submission!");
                }

                // If EFT Order, check if its approved before processing.
                if (_lookup.IsEft(cashOrder.CashOrderTypeId))
                {
                    var pendingStatus = _repository.Query<Status>(e => e.LookUpKey == "PENDING").FirstOrDefault();

                    if (cashOrder.StatusId == pendingStatus.StatusId)
                    {
                        return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "This Cash Order has not yet been approved for processing.");
                    }

                    var activeStatus = _repository.Query<Status>(e => e.LookUpKey == "ACTIVE").FirstOrDefault();

                    if (cashOrder.StatusId == activeStatus.StatusId)
                    {
                        return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "This Cash Order has not yet been approved for processing.");
                    }
                }

                // A Cash Order cannot be processed more than once and
                // the current logged in user can only process orders that were captured by
                // other users in his/her cash centre and not orders that were
                // captured by (him/her)self.
                //
                // NOTE : Head Office User with (ADMINISTRATOR) role can process all orders for all cash centres 
                //
                if (cashOrder.CreatedById == user.UserId)
                {
                    return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "A Cash Order cannot be processed by the person who captured it!");
                }

                if (cashOrder.IsProcessed)
                {
                    return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "A Cash Order cannot be processed more than once!");
                }

                if (user != null)
                {
                    var mappedCashOrder = _mapper.Map<CashOrder, CashOrderDto>(cashOrder);

                    mappedCashOrder.SealSerialNumber = searchText;

                    switch (user.UserTypeId)
                    {
                        case 1: // Cash Center User
                            return ForSbvUserProcessing(mappedCashOrder, user);
                        case 2:
                            // Merchant User (NOTE : Merchant User cannot process a deposit) Line of code below must not execute.
                            return ForMerchantUserProcessing(mappedCashOrder, user);
                        case 3: // Head Office User
                            return (Roles.IsUserInRole(user.UserName, "SBVAdmin"))
                                ? new MethodResult<CashOrderDto>(MethodStatus.Successful, mappedCashOrder)
                                : new MethodResult<CashOrderDto>(MethodStatus.Error, null, "Only Administrators on Head Office level can process cash orders.");
                    }
                }
            }
            return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "The Order Reference entered does not match any record on the mySBV system, please re-enter the Order Reference.");
	    }


	    /// <summary>
		///     Find a CashOrder by id
		/// </summary>
		/// <param name="id"></param>
		public MethodResult<CashOrderDto> Find(int id)
		{
			var order = _repository.Query<CashOrder>(a => a.CashOrderId == id,
							b => b.CashOrderType,
							c => c.Site,
							z => z.Site.SiteContainers,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType))
						).OrderByDescending(o => o.CashOrderId).FirstOrDefault();

			var cashOrder = _mapper.Map<CashOrder, CashOrderDto>(order);
	        if (order != null) cashOrder.StatusName = order.Status.Name;

	        if (cashOrder == null)
			{
				return new MethodResult<CashOrderDto>(MethodStatus.NotFound, null, "Cash Order not found");
			}

			var site = _repository.Query<Site>(a => a.SiteId == cashOrder.SiteId, o => o.CitCarrier).FirstOrDefault();
			cashOrder.InitialCitSerialNumber = site.CitCarrier.SerialStartNumber;

			return new MethodResult<CashOrderDto>(MethodStatus.Found, cashOrder, "Cash Order found");
		}


        /// <summary>
        ///     Find a CashOrder by id
        /// </summary>
        /// <param name="id"></param>
        public MethodResult<CashOrder> GetOrder(int id)
        {
            var cashOrder = _repository.Query<CashOrder>(a => a.CashOrderId == id,
                            b => b.CashOrderType,
                            c => c.Site,
                            z => z.Site.SiteContainers,
                            d => d.Site.Address,
                            e => e.Status,
                            f => f.Site.Address.AddressType,
                            g => g.Site.Merchant,
                            h => h.CashOrderContainer,
                            o => o.CashOrderContainer.CashOrderContainerDrops,
                            r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
                            q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
                            q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType))
                        ).OrderByDescending(o => o.CashOrderId).FirstOrDefault();
            
            if (cashOrder == null)
            {
                return new MethodResult<CashOrder>(MethodStatus.NotFound, null, "Cash Order not found");
            }
            
            return new MethodResult<CashOrder>(MethodStatus.Successful, cashOrder, "Cash Order found");
        }


        /// <summary>
        ///     Find a CashOrder Task by CashOrderID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MethodResult<CashOrderTaskDto> FindCashOrderTask(int id)
        {
            var pendingStatus = _lookup.GetStatusId("PENDING");
            var declinedStatus = _lookup.GetStatusId("DECLINED");

            var order = _repository.Query<CashOrderTask>(a => a.CashOrderId == id && a.StatusId == pendingStatus ||
                                                            a.CashOrderId == id && a.StatusId == declinedStatus,
                            c => c.Site,
                            z => z.Site.SiteContainers,
                            d => d.Site.Address,
                            i => i.Site.Accounts,
                            r => r.Site.Accounts.Select(z => z.Bank),
                            e => e.Status,
                            f => f.Site.Address.AddressType,
                            g => g.Site.Merchant,
                            h => h.User
                        ).OrderByDescending(o => o.CashOrderId).FirstOrDefault();

            var cashOrderTask = _mapper.Map<CashOrderTask, CashOrderTaskDto>(order);

            if (cashOrderTask == null)
            {
                return new MethodResult<CashOrderTaskDto>(MethodStatus.NotFound, null, "Cash Order Task not found");
            }

            return new MethodResult<CashOrderTaskDto>(MethodStatus.Found, cashOrderTask, "Cash Order Task found");
        }


		/// <summary>
		///     Add A New CashOrder
		/// </summary>
		/// <param name="cashOrder"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public MethodResult<CashOrder> Add(CashOrderDto cashOrder, User user)		
		{
			var date = DateTime.Now;

            var isEft = GetCashOrderType(cashOrder.CashOrderTypeId).ToUpper() == "EFT" ;
		    var wasUsed = WasContainerSerialUsed(cashOrder.ContainerNumberWithCashForExchange, cashOrder.EmptyContainerOrBagNumber);

			// Check if the container was used before
            if (wasUsed && isEft == false)
			{
				return new MethodResult<CashOrder>(MethodStatus.Warning, null, "Serial Number Has been used before. Verify and try again.");
			}
            
			cashOrder.CapturedDateTime = date;
			cashOrder.StatusId = GetStatusId(cashOrder.StatusName);
			cashOrder.DeliveryDate = cashOrder.DeliveryDateDto.ToShortDateString();
			cashOrder.ReferenceNumber = GenerateTransactionNumber(cashOrder.SiteId);
			cashOrder.CashOrderContainer.CashOrderContainerDrops = cashOrder.CashOrderContainer.CashOrderContainerDrops.Where(o => o.CashOrderContainerDropItems != null).ToList();

			foreach (var item in cashOrder.CashOrderContainer.CashOrderContainerDrops.Where(o => o.CashOrderContainerDropItems != null)
						.SelectMany(containerDrop => containerDrop.CashOrderContainerDropItems.Where(i => i.ValueInCents > 0)))
			{
				item.DenominationId = GetDenomination(item.ValueInCents);
				item.DenominationType = GetDenominationType(item.DenominationId);
				item.Value = (item.ValueInCents/100.0M)*item.Count;
			}

			var mappedCashOrder = _mapper.Map<CashOrderDto, CashOrder>(cashOrder);
			mappedCashOrder.CreatedById = user.UserId;
			mappedCashOrder.CreateDate = date;
			mappedCashOrder.LastChangedById = user.UserId;
			mappedCashOrder.LastChangedDate = date;
			mappedCashOrder.IsNotDeleted = true;

			((IEntity) mappedCashOrder.CashOrderContainer).WalkObjectGraph(o =>
			{
				o.CreatedById = user.UserId;
				o.CreateDate = date;
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				return false;
			}, a => { });

		    var results = _repository.Add(mappedCashOrder);
		    var pendingStatus = _lookup.GetStatusId("PENDING");

		    if (results > 0 && mappedCashOrder.StatusId == pendingStatus)
		    {
                return new MethodResult<CashOrder>(MethodStatus.Successful, mappedCashOrder, "Cash Order submitted successfully.");
		    }
            
            return results > 0 
                    ? new MethodResult<CashOrder>(MethodStatus.Successful, mappedCashOrder, "Cash Order saved successfully.") 
                    : new MethodResult<CashOrder>(MethodStatus.Warning, null, "Failed to save Cash Order!");
		}
        

		/// <summary>
		///     Update a new CashOrder
		/// </summary>
		/// <param name="cashOrder"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public MethodResult<int> Edit(CashOrderDto cashOrder, User user)
		{
			#region CashOrder "Added & Modified" EntityState Marking on Drop Items

			var valueInCents = new ArrayList();
            var declinedStatus = _lookup.GetStatusId("DECLINED");
            var pendingStatus = _lookup.GetStatusId("PENDING");

			DateTime date = DateTime.Now;
			cashOrder.DeliveryDate = cashOrder.DeliveryDateDto.ToShortDateString();
            cashOrder.CashOrderContainer.CashOrderContainerDrops = cashOrder.CashOrderContainer.CashOrderContainerDrops.Where(o => o.CashOrderContainerDropItems != null).ToList();

            if (_lookup.IsEft(cashOrder.CashOrderTypeId))
            {
                var files = GetFiles(cashOrder.CashOrderId);

                if (files.Count == 0)
                {
                    return new MethodResult<int>(MethodStatus.Warning, -1, "No Attachment has been added, Please attach Proof of Payment!");
                }
            }

            //CHECK IF EFT CASH ORDER TYPE
            if (cashOrder.CashOrderTypeId == GetCashOrderTypeId("EFT"))
            {
                var cashOrderBeforeUpdate = _repository.Find<CashOrder>(cashOrder.CashOrderId);
                
                //IF ORDER IS FROM A DECLINED STATUS THEN SET IT BACK TO PENDING AFTER UPDATES
                if (cashOrderBeforeUpdate.StatusId == declinedStatus)
                {
                    cashOrder.StatusId = GetStatusId("PENDING");
                }

                if (!string.IsNullOrWhiteSpace(cashOrder.Comments) && cashOrderBeforeUpdate.StatusId == declinedStatus)
                {
                    cashOrder.Comments += Environment.NewLine + "CashOrder: Submission";
                }
            }
            
			foreach (var item in cashOrder.CashOrderContainer.CashOrderContainerDrops.Where(o => o.CashOrderContainerDropItems != null)
						.SelectMany(containerDrop => containerDrop.CashOrderContainerDropItems.Where(i => i.ValueInCents > 0)))
			{
				item.DenominationId = GetDenomination(item.ValueInCents);
				item.DenominationType = GetDenominationType(item.DenominationId);
				item.Value = (item.ValueInCents/100)*item.Count;
			}

			var allItems = new List<CashOrderContainerDropItem>();
			var mappedCashOrder = _mapper.Map<CashOrderDto, CashOrder>(cashOrder);

			//
			// Mark EntityState of Drop-Items from Frontend as either Added or Modified.  
			foreach (var containerDrop in mappedCashOrder.CashOrderContainer.CashOrderContainerDrops)
			{
				if (containerDrop.CashOrderContainerDropId == 0)
				{
					containerDrop.EntityState = State.Added;
					containerDrop.CreateDate = date;
					containerDrop.CreatedById = user.UserId;
				}

				foreach (var item in containerDrop.CashOrderContainerDropItems.Where(i => i.ValueInCents > 0))
				{
					var dropItem = IsEqualDropItem(item.ValueInCents, cashOrder);

					item.DenominationId = GetDenomination(item.ValueInCents);
					item.DenominationType = GetDenominationType(item.DenominationId);
					item.Value = (dropItem.ValueInCents/100M)*dropItem.Count;
					valueInCents.Add(item.ValueInCents);

					if (item.CashOrderContainerDropItemId == 0)
					{
						item.EntityState = State.Added;
					}
					else if (dropItem.IsEqual)
					{
						item.EntityState = State.Modified;
					}
					allItems.Add(item);
				}
			}

			//
			// Mark EntityState of Drop-Items not from Frontend as deleted.           
			var cashForwardedDropId = 0;
			var cashRequiredDropId = 0;

			if (cashOrder.CashOrderContainer.CashOrderContainerDrops.Count() > 1)
			{
				cashForwardedDropId = cashOrder.CashOrderContainer.CashOrderContainerDrops[0].CashOrderContainerDropId;
				cashRequiredDropId = cashOrder.CashOrderContainer.CashOrderContainerDrops[1].CashOrderContainerDropId;
			}
			else
			{
				cashRequiredDropId = cashOrder.CashOrderContainer.CashOrderContainerDrops[0].CashOrderContainerDropId;
			}

			var deletionMarkedItems = _repository.Query<CashOrderContainerDropItem>(a => a.CashOrderContainerDropId == cashForwardedDropId || a.CashOrderContainerDropId == cashRequiredDropId);

			foreach (var item in deletionMarkedItems.Distinct())
			{
				if (!valueInCents.Contains(item.ValueInCents))
				{
					var mappedItem = _mapper.Map<CashOrderContainerDropItem, CashOrderContainerDropItem>(item);
					mappedItem.EntityState = State.Deleted;
					mappedItem.CashOrderContainerDrop = null;
					mappedItem.Denomination = null;
					allItems.Add(mappedItem);
				}
			}


			if (cashOrder.CashOrderContainer.CashOrderContainerDrops.Count() > 1)
			{
				mappedCashOrder.CashOrderContainer.CashOrderContainerDrops[0].CashOrderContainerDropItems = allItems.Where(e => e.CashOrderContainerDropId == cashForwardedDropId).ToList();

				mappedCashOrder.CashOrderContainer.CashOrderContainerDrops[1].CashOrderContainerDropItems = allItems.Where(e => e.CashOrderContainerDropId == cashRequiredDropId).ToList();
			}
			else
			{
				mappedCashOrder.CashOrderContainer.CashOrderContainerDrops[0].CashOrderContainerDropItems = allItems.Where(e => e.CashOrderContainerDropId == cashRequiredDropId).ToList();

				var drop = _repository.Query<CashOrderContainerDrop>(a => a.CashOrderContainerId == cashOrder.CashOrderContainerId && a.CashOrderContainerDropId != cashRequiredDropId).FirstOrDefault();

				if (drop != null)
				{
					mappedCashOrder.CashOrderContainer.CashOrderContainerDrops.Add(new CashOrderContainerDrop());

					var mappedContainerDropDto = _mapper.Map<CashOrderContainerDrop, CashOrderContainerDropDto>(drop);

					var mappedContainerDrop = _mapper.Map<CashOrderContainerDropDto, CashOrderContainerDrop>(mappedContainerDropDto);

					mappedContainerDrop.LastChangedDate = date;
					mappedContainerDrop.LastChangedById = user.UserId;
					mappedContainerDrop.IsNotDeleted = false;
					mappedContainerDrop.CashOrderContainer = null;

					var containerDropId = mappedContainerDrop.CashOrderContainerDropId;
					var containerDrops = mappedCashOrder.CashOrderContainer.CashOrderContainerDrops.Where(e => e.CashOrderContainerDropId == containerDropId).Distinct();

					foreach (var d in containerDrops)
					{
						d.EntityState = State.Deleted;
						d.IsNotDeleted = false;

						foreach (var item in d.CashOrderContainerDropItems)
						{
							item.EntityState = State.Deleted;
							item.Denomination = null;
							item.IsNotDeleted = false;
						}
					}
				}
			}

			#endregion

			((IEntity) mappedCashOrder.CashOrderContainer).WalkObjectGraph(o =>
			{
				o.CreatedById = cashOrder.CreatedById;
				o.CreateDate = cashOrder.CreateDate;
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				return false;
			}, a => { });

			((IEntity) mappedCashOrder).WalkObjectGraph(o =>
			{
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				o.EntityState = State.Modified;
				return false;
			}, a => { });

			mappedCashOrder.CashOrderContainer.EntityState = State.Modified;
			var id = _repository.Update(mappedCashOrder);

            var results = mappedCashOrder.StatusId == pendingStatus ? "Submitted" : "edited";
		    
			return id > 0
                ? new MethodResult<int>(MethodStatus.Successful, mappedCashOrder.CashOrderId, "Cash Order " + results + " successfully.")
				: new MethodResult<int>(MethodStatus.Error, -1, "Failed to edit Cash Order");
		}


		/// <summary>
		///     Process CashOrder
		/// </summary>
		/// <param name="cashOrderContainer"></param>
		/// <param name="exchangeBagNumber"></param>
		/// <param name="emptyBagNumber"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public MethodResult<int> Process(CashOrderContainerDto cashOrderContainer, string exchangeBagNumber,string emptyBagNumber, User user)
		{
			DateTime date = DateTime.Now;
			CashOrderContainer container = _mapper.Map<CashOrderContainerDto, CashOrderContainer>(cashOrderContainer);

			//
			// Mark EntityState of Drop-Items from Frontend as either Added or Modified.

			foreach (CashOrderContainerDrop containerDrop in container.CashOrderContainerDrops)
			{
				containerDrop.EntityState = State.Modified;

				foreach (CashOrderContainerDropItem item in containerDrop.CashOrderContainerDropItems)
				{
					item.DenominationId = GetDenomination(item.ValueInCents);
					item.DenominationType = GetDenominationType(item.DenominationId);

					if (item.CashOrderContainerDropItemId == 0)
					{
						item.EntityState = State.Added;
						item.CreateDate = date;
						item.CreatedById = user.UserId;
					}
					else
					{
						item.EntityState = State.Modified;
						item.CreateDate = cashOrderContainer.CreateDate;
						item.CreatedById = cashOrderContainer.CreatedById;
					}
				}

				containerDrop.CreateDate = cashOrderContainer.CreateDate;
				containerDrop.CreatedById = cashOrderContainer.CreatedById;
			}
			container.EntityState = State.Modified;

			var cashOrder = _repository.Query<CashOrder>(a => a.CashOrderContainerId == container.CashOrderContainerId)
							.FirstOrDefault();

			var mappedCashOrder = _mapper.Map<CashOrder, CashOrder>(cashOrder);

			mappedCashOrder.CashOrderContainer = container;
			mappedCashOrder.ContainerNumberWithCashForExchange = exchangeBagNumber;
			mappedCashOrder.EmptyContainerOrBagNumber = emptyBagNumber;
			mappedCashOrder.EntityState = State.Modified;
			mappedCashOrder.IsProcessed = true;
			mappedCashOrder.DateProcessed = date;
			mappedCashOrder.StatusId = GetStatusId("PROCESSED");

			((IEntity) container).WalkObjectGraph(o =>
			{
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				return false;
			}, a => { });

			((IEntity) mappedCashOrder).WalkObjectGraph(o =>
			{
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				return false;
			}, a => { });

			var id = _repository.Update(mappedCashOrder);

			return id > 0
				? new MethodResult<int>(MethodStatus.Successful, container.CashOrderContainerId, "Cash Order processed successfully.")
				: new MethodResult<int>(MethodStatus.Error, -1, "Failed to process Cash Order!");
		}
        

		/// <summary>
		/// Submit CashOrder for processing
		/// </summary>
		/// <param name="cashOrder"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public MethodResult<CashOrder> Submit(CashOrderDto cashOrder, User user)
		{
			DateTime date = DateTime.Now;
			cashOrder.OrderDate = date;
			cashOrder.DateSubmitted = date;

			// Check if cash order is submitted
			if (IsSubmitted(cashOrder.CashOrderId))
			{
				return new MethodResult<CashOrder>(MethodStatus.Warning, null, "You cannot Submit an already submitted Cash Order!");
			}

            if (GetCashOrderType(cashOrder.CashOrderTypeId) == "EFT")
		    {
                cashOrder.StatusId = GetStatusId("PENDING");
                cashOrder.IsSubmitted = false;

		        if (!string.IsNullOrWhiteSpace(cashOrder.Comments))
		        {
                    cashOrder.Comments += Environment.NewLine + "CashOrder: Submission";
		        }
		        else
		        {
                    cashOrder.Comments = "CashOrder: Submission";
		        }


		        //UPDATE OR ADD CASH ORDER TASK, BASED ON STATUS
                CRUD_ApprovalTask(cashOrder.CashOrderId, user,
		            DeclinedCashOrderTaskExist(cashOrder.CashOrderId) ? "UPDATE" : "CREATE");

		    }
		    else
		    {
                cashOrder.StatusId = GetStatusId("SUBMITTED");
                cashOrder.IsSubmitted = true;
		    }
            
			CashOrder mappedCashOrder = _mapper.Map<CashOrderDto, CashOrder>(cashOrder);

			((IEntity) mappedCashOrder).WalkObjectGraph(o =>
			{
				o.LastChangedById = user.UserId;
				o.LastChangedDate = date;
				o.IsNotDeleted = true;
				o.EntityState = State.Modified;
				return false;
			}, a => { });

			return _repository.Update(mappedCashOrder) > 0
				? new MethodResult<CashOrder>(MethodStatus.Successful, mappedCashOrder, "Cash Order submitted successfully.")
				: new MethodResult<CashOrder>(MethodStatus.Error, null, "Failed to submitted Cash Order!");
		}


        /// <summary>
        /// Submit CashOrder for processing
        /// </summary>
        /// <param name="cashOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<CashOrder> Approve(CashOrderDto cashOrder, User user)
        {
            DateTime date = DateTime.Now;
            cashOrder.OrderDate = date;
            cashOrder.DateSubmitted = date;

            // Check if cash order is submitted
            if (IsSubmitted(cashOrder.CashOrderId))
            {
                return new MethodResult<CashOrder>(MethodStatus.Warning, null, "You cannot Submit an already submitted Cash Order!");
            }

            var declineStatus = GetStatusId("DECLINED");
            if (cashOrder.StatusId == declineStatus)
            {
                return new MethodResult<CashOrder>(MethodStatus.Error, null, "You cannot approve a Cash Order that has been declined!");
            }

            var pendingStatusId = GetStatusId("PENDING");
            if (UserAutherizingOwnTask(user.UserId, cashOrder.CashOrderId, pendingStatusId))
            {
                return new MethodResult<CashOrder>(MethodStatus.Error, null, "You cannot approve a task that you have submitted for approval!");
            }

            cashOrder.StatusId = GetStatusId("SUBMITTED");
            cashOrder.IsSubmitted = true;

            //UPDATE OR ADD CASH ORDER TASK, BASED ON STATUS
            CRUD_ApprovalTask(cashOrder.CashOrderId, user, "APPROVE");

            CashOrder mappedCashOrder = _mapper.Map<CashOrderDto, CashOrder>(cashOrder);

            ((IEntity)mappedCashOrder).WalkObjectGraph(o =>
            {
                o.LastChangedById = user.UserId;
                o.LastChangedDate = date;
                o.IsNotDeleted = true;
                o.EntityState = State.Modified;
                return false;
            }, a => { });

            return _repository.Update(mappedCashOrder) > 0
                ? new MethodResult<CashOrder>(MethodStatus.Successful, mappedCashOrder, "Cash Order approved successfully.")
                : new MethodResult<CashOrder>(MethodStatus.Error, null, "Failed to approve the Cash Order!");
        }


        /// <summary>
        /// Submit CashOrder for processing
        /// </summary>
        /// <param name="cashOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<CashOrder> Reject(CashOrderDto cashOrder, User user)
        {
            var declineStatus = GetStatusId("DECLINED");
            if (cashOrder.StatusId == declineStatus)
            {
                return new MethodResult<CashOrder>(MethodStatus.Error, null, "You cannot decline a Cash Order that has already been Declined!");
            }

            var pendingStatusId = GetStatusId("PENDING");
            if (UserAutherizingOwnTask(user.UserId, cashOrder.CashOrderId, pendingStatusId))
            {
                return new MethodResult<CashOrder>(MethodStatus.Error, null, "You cannot decline a task that you have submitted for approval!");
            }

            cashOrder.StatusId = declineStatus;
            cashOrder.IsSubmitted = false;

            //UPDATE OR ADD CASH ORDER TASK, BASED ON STATUS
            CRUD_ApprovalTask(cashOrder.CashOrderId, user, "DECLINE");

            CashOrder mappedCashOrder = _mapper.Map<CashOrderDto, CashOrder>(cashOrder);

            ((IEntity)mappedCashOrder).WalkObjectGraph(o =>
            {
                o.LastChangedById = user.UserId;
                o.LastChangedDate = DateTime.Now;
                o.IsNotDeleted = true;
                o.EntityState = State.Modified;
                return false;
            }, a => { });

            return _repository.Update(mappedCashOrder) > 0
                ? new MethodResult<CashOrder>(MethodStatus.Successful, mappedCashOrder, "Cash Order decline successfully.")
                : new MethodResult<CashOrder>(MethodStatus.Error, null, "Failed to decline the Cash Order!");
        }



		/// <summary>
		///     Delete a CashOrder
		/// </summary>
		/// <param name="id"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public MethodResult<bool> Delete(int id, User user)
		{
			var cashOrder = _repository.Find<CashOrder>(id);

			if (cashOrder == null) return new MethodResult<bool>(MethodStatus.Error, false, "Cash Order Was Not Deleted.");

			if (cashOrder.StatusId == GetStatusId("SUBMITTED"))
			{
				return new MethodResult<bool>(MethodStatus.Warning, false,
					"Cannot delete an Order that has already been approved.");
			}

            //Remove Task if Pending OR Declined Cash Order
            if (cashOrder.StatusId == GetStatusId("PENDING") || cashOrder.StatusId == GetStatusId("DECLINED"))
            {
                DeleteCashOrderTask(id, user.UserId);
            }
            
			if (_repository.Delete<CashOrder>(id, user.UserId) > 0)
			{
				return new MethodResult<bool>(MethodStatus.Successful, true, "Cash Order Was Deleted Successfully.");
			}
			return new MethodResult<bool>(MethodStatus.Error, false, "Cash Order Was Not Deleted.");
		}




	    /// <summary>
		///     Find CashOrder by Sealnumber or SerialNumber
		/// </summary>
		/// <param name="searchText"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		public MethodResult<CashOrderDto> FindBySealSerialNumber(string searchText, User user)
		{
			var cashOrder = _repository.Query<CashOrder>(a => a.IsSubmitted && (a.ContainerNumberWithCashForExchange == searchText ||
							a.EmptyContainerOrBagNumber == searchText),
							b => b.CashOrderType,
							c => c.Site,
							d => d.Site.Address,
							e => e.Status,
							f => f.Site.Address.AddressType,
							g => g.Site.Merchant,
							h => h.CashOrderContainer,
							o => o.CashOrderContainer.CashOrderContainerDrops,
							r => r.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination)),
							q => q.CashOrderContainer.CashOrderContainerDrops.Select(z => z.CashOrderContainerDropItems.Select(u => u.Denomination.DenominationType))
						).OrderByDescending(o => o.CashOrderId).FirstOrDefault();

			if (cashOrder != null)
			{
				// A Cash Order cannot be processed more than once and
				// the current logged in user can only process orders that were captured by
				// other users in his/her cash centre and not orders that were
				// captured by (him/her)self.
				//
				// NOTE : Head Office User with (ADMINISTRATOR) role can process all orders for all cash centres 
				//
				if (cashOrder.CreatedById == user.UserId)
				{
					return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "A Cash Order cannot be processed by the person who captured it!");
				}

				if (cashOrder.IsSubmitted == false)
				{
					return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "You cannot process a Cash Order awaiting submission!");
				}

				if (cashOrder.IsProcessed)
				{
					return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "A Cash Order cannot be processed more than once!");
				}

				if (user != null)
				{
					var mappedCashOrder = _mapper.Map<CashOrder, CashOrderDto>(cashOrder);

					mappedCashOrder.SealSerialNumber = searchText;

					switch (user.UserTypeId)
					{
						case 1: // Cash Center User
							return ForSbvUserProcessing(mappedCashOrder, user);
						case 2:
							// Merchant User (NOTE : Merchant User cannot process a deposit) Line of code below must not execute.
							return ForMerchantUserProcessing(mappedCashOrder, user);
						case 3: // Head Office User
							return (Roles.IsUserInRole(user.UserName, "SBVAdmin"))
								? new MethodResult<CashOrderDto>(MethodStatus.Successful, mappedCashOrder)
								: new MethodResult<CashOrderDto>(MethodStatus.Error, null, "Only Administrators on Head Office level can process cash orders.");
					}
				}
			}
			return new MethodResult<CashOrderDto>(MethodStatus.Error, null, "Cash Order was Not Found.");
		}




		/// <summary>
		///     Cash denominations forwarded for Exchange by client
		/// </summary>
		/// <param name="cashOrderId"></param>
		/// <returns></returns>
		public IEnumerable<ItemDenominationDto> CashForwardedForExchange(int cashOrderId)
		{
			var cashOrder = _repository.Query<CashOrder>(o => o.CashOrderId == cashOrderId,
														 o => o.CashOrderContainer,
														 o => o.Status,
														 o => o.Site,
														 o => o.Site.Merchant,
														 o => o.CashOrderContainer.CashOrderContainerDrops,
														 o => o.CashOrderContainer.CashOrderContainerDrops.Select(e => e.CashOrderContainerDropItems),
														 o => o.CashOrderContainer.CashOrderContainerDrops.Select(e => e.CashOrderContainerDropItems.Select(p => p.Denomination))
														).FirstOrDefault();

			var containerDrop = cashOrder.CashOrderContainer.CashOrderContainerDrops.FirstOrDefault(e => e.IsCashForwardedForExchange);

			if (containerDrop == null)
			{
				return null;
			}
			var containerDropItems = containerDrop.CashOrderContainerDropItems;

			var leftOuterJoinQuery = from denomination in _repository.All<Denomination>()
									 join denominationType in _repository.All<DenominationType>() on denomination.DenominationTypeId equals denominationType.DenominationTypeId
									 join containerDropItem in containerDropItems
										on denomination.DenominationId equals containerDropItem.DenominationId into prodGroup
									 from item in prodGroup.DefaultIfEmpty(new CashOrderContainerDropItem())
									 select new ItemDenominationDto
									 {
										 CashOrderContainerDropItemId = item.CashOrderContainerDropItemId,
										 DenominationId = denomination.DenominationId,
										 Value = item.Value,
										 Count = item.Count,
										 ValueInCents = denomination.ValueInCents,
										 DenominationType = denominationType.Name,
										 DenominationName = denomination.Description
									 };

			return leftOuterJoinQuery.ToList();
		}


		/// <summary>
		///     Cash denominations required by client
		/// </summary>
		/// <param name="cashOrderId"></param>
		/// <returns></returns>
		public	IEnumerable<ItemDenominationDto> CashRequiredByClient(int cashOrderId)
		{
			var cashOrder = _repository.Query<CashOrder>(o => o.CashOrderId == cashOrderId,
														 o => o.CashOrderContainer,
														 o => o.Status,
														 o => o.Site,
														 o => o.Site.Merchant,
														 o => o.CashOrderContainer.CashOrderContainerDrops,
														 o => o.CashOrderContainer.CashOrderContainerDrops.Select(e => e.CashOrderContainerDropItems),
														 o => o.CashOrderContainer.CashOrderContainerDrops.Select(e => e.CashOrderContainerDropItems.Select(p => p.Denomination))
														).FirstOrDefault();

			var containerDropItems = cashOrder.CashOrderContainer.CashOrderContainerDrops.FirstOrDefault(e => e.IsCashRequiredInExchange).CashOrderContainerDropItems;

			var leftOuterJoinQuery = from denomination in _repository.All<Denomination>()
									 join denominationType in _repository.All<DenominationType>() on denomination.DenominationTypeId equals denominationType.DenominationTypeId
									 join containerDropItem in containerDropItems
										on denomination.DenominationId equals containerDropItem.DenominationId into prodGroup
									 from item in prodGroup.DefaultIfEmpty(new CashOrderContainerDropItem())
									 select new ItemDenominationDto
									 {
										 CashOrderContainerDropItemId = item.CashOrderContainerDropItemId,
										 DenominationId = denomination.DenominationId,
										 Value = item.Value,
										 Count = item.Count,
										 ValueInCents = denomination.ValueInCents,
										 DenominationType = denominationType.Name,
										 DenominationName = denomination.Description
									 };

			return leftOuterJoinQuery.ToList();
		}

		
		/// <summary>
		///     get a cashOrder types
		/// </summary>
		/// <param name="cashOrderTypeId"></param>
		/// <returns></returns>
		public string GetCashOrderType(int cashOrderTypeId)
		{
			var cashOrderType = _repository.Query<CashOrderType>(o => o.CashOrderTypeId == cashOrderTypeId).FirstOrDefault();
			return cashOrderType != null ? cashOrderType.Name : string.Empty;
		}


	    public int GetCashOrderTypeId(string cashOrderTypeName)
	    {
	        var cashOrderType = _repository.Query<CashOrderType>(a => a.LookUpKey == cashOrderTypeName).FirstOrDefault();
	        return cashOrderType != null ? cashOrderType.CashOrderTypeId : 0;
	    }

        public bool UserAutherizingOwnTask(int userId, int cashOrderId, int statusId)
	    {
            return _repository.Any<CashOrder>(a => a.CashOrderId == cashOrderId && a.LastChangedById == userId && a.StatusId == statusId);
	    }


		#endregion
		
		#region Methods


        /// <summary>
        /// Get cash order EFT attachments 
        /// </summary>
        /// <param name="cashOrderId"></param>
        public List<FileDto> GetFiles(int cashOrderId)
        {
            var root = _lookup.GetConfigurationValue("CASH_ORDER_EFT_ATTACHMENTS_URL");
            var physicalPath = Path.Combine(root, cashOrderId.ToString());

            var collection = new List<FileDto>();

            if (Directory.Exists(physicalPath))
            {
                var files = Directory.GetFiles(physicalPath, "*");

                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file);

                    var iconClass = GetExtensionClass(extension);

                    collection.Add(new FileDto
                    {
                        name = Path.GetFileName(file),
                        extension = iconClass,
                        size = file.Length
                    });
                }
            }

            return collection;
        }
        

        /// <summary>
        /// Get Extension Class
        /// </summary>
        /// <param name="extension"></param>
        string GetExtensionClass(string extension)
        {
            switch (extension.ToLower())
            {
                case ".jpg":
                case ".img":
                case ".png":
                case ".gif":
                    return "img-file";
                case ".doc":
                case ".docx":
                    return "doc-file";
                case ".xls":
                case ".xlsx":
                    return "xls-file";
                case ".pdf":
                    return "pdf-file";
                case ".zip":
                case ".rar":
                    return "zip-file";
                default:
                    return "default-file";
            }
        }


		private MethodResult<CashOrderDto> ForSbvUserProcessing(CashOrderDto cashOrder, User user)
		{
			// Check if cash order
			// can be processed by the 
			// current logged in user's cash centre.
			var depositSite = _repository.Query<Site>(a => a.SiteId == cashOrder.SiteId, b => b.CashCenter).FirstOrDefault();

			return depositSite != null && depositSite.CashCenterId == user.CashCenterId
				? new MethodResult<CashOrderDto>(MethodStatus.Successful, cashOrder)
				: new MethodResult<CashOrderDto>(MethodStatus.Error, null, "Orders cannot be processed by the current logged in user(s) cash Centre");
		}


		private MethodResult<CashOrderDto> ForMerchantUserProcessing(CashOrderDto cashOrder, User user)
		{
			// Check if cash order
			// belongs to one of the
			// current logged in user's sites.
			UserSite userSite = user.UserSites.FirstOrDefault(a => a.SiteId == cashOrder.SiteId);
			return userSite != null
				? new MethodResult<CashOrderDto>(MethodStatus.Successful, cashOrder)
				: new MethodResult<CashOrderDto>(MethodStatus.Error, null, "Cash Order was Not Found.");
		}


		private int GetDenomination(int valueInCents)
		{
			var denomination = _repository.Query<Denomination>(e => e.ValueInCents == valueInCents).FirstOrDefault();
			return denomination != null ? denomination.DenominationId : 0;
		}

		private CashOrderContainerDropItemDto IsEqualDropItem(int valueInCents, CashOrderDto cashOrderDto)
		{
			var dropItem = new CashOrderContainerDropItemDto {IsEqual = false};

			foreach (var containerDrop in cashOrderDto.CashOrderContainer.CashOrderContainerDrops.Where(o => o.CashOrderContainerDropItems != null))
			{
				foreach (var item in
						containerDrop.CashOrderContainerDropItems.Where(i => i.ValueInCents > 0))
				{
					if (item.ValueInCents == valueInCents)
					{
						dropItem = item;
						dropItem.IsEqual = true;
						return dropItem;
					}
				}
			}
			return dropItem;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serialNumber"></param>
		/// <param name="emptyBagNumber"></param>
		/// <returns></returns>
		private bool WasContainerSerialUsed(string serialNumber, string emptyBagNumber)
		{
			var containerSerials = _repository.All<CashOrder>()
										.Where(c => c.ContainerNumberWithCashForExchange == serialNumber ||
													c.EmptyContainerOrBagNumber == emptyBagNumber ||
													c.ContainerNumberWithCashForExchange == emptyBagNumber ||
													c.EmptyContainerOrBagNumber == serialNumber);
													
			var containers = _repository.All<Container>()
				.Where(c => (c.SerialNumber == serialNumber) || (c.SealNumber == emptyBagNumber));
				
			return (containerSerials.Any() || containers.Any());
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cashOrderId"></param>
		/// <returns></returns>
		private bool IsSubmitted(int cashOrderId)
		{
			var cashOrder = _repository.All<CashOrder>().Where(c => c.CashOrderId == cashOrderId && c.StatusId == GetStatusId("SUBMITTED"));
			return cashOrder.Any();
		}


		private int GetStatusId(string lookUpKey)
		{
			return _repository.All<Status>().FirstOrDefault(a => a.LookUpKey == lookUpKey).StatusId;
		}


		private string GetDenominationType(int denominationId)
		{
			return (from type in _repository.All<DenominationType>()
				join deno in _repository.All<Denomination>() on type.DenominationTypeId equals deno.DenominationTypeId
				where deno.DenominationId == denominationId
				select type ).First().Name;
		}

		#region Holidays

		/// <summary>
		///     Gets the next DeliveryDate.
		///     This depends on the 11h00 Cutoff time on cash orders, weekends, and public holidays.
		/// </summary>
		/// <returns></returns>
		public DateTime GetNextDeliveryDate()
		{
			DateTime currentDate = DateTime.Now;
			int orderHour = DateTime.Now.Hour;
			int dayOfWeek = Convert.ToInt32(currentDate.Date.DayOfWeek);

			// Non holiday days
			if (IsPublicHoliday(currentDate) == false)
			{
				if (dayOfWeek == 5)
				{
					int days = (orderHour < 11) ? 1 : 4;
					return currentDate.AddDays(days);
				}
				if (dayOfWeek == 0 || dayOfWeek == 6)
				{
					int days = GetNumberOfDaysOnWeekends(currentDate, dayOfWeek) + 1;
					return currentDate.AddDays(days);
				}
				if (dayOfWeek > 0 && dayOfWeek <= 4)
				{
					int days = (orderHour < 11) ? 1 : 2;
					return currentDate.AddDays(days);
				}
			}
				// Current day is a holiday
			else
			{
				if (dayOfWeek == 5)
				{
					int days = 3;
					return NextBusinessDate(currentDate, days);
				}
				if (dayOfWeek == 0 || dayOfWeek == 6)
				{
					int days = GetNumberOfDaysOnWeekends(currentDate, dayOfWeek);
					return NextBusinessDate(currentDate, days);
				}
				if (dayOfWeek > 0 && dayOfWeek <= 4)
				{
					int days = 1;
					return NextBusinessDate(currentDate, days);
				}
			}

			return currentDate.AddDays(1);
		}

		private DateTime GetEasterSundayDate()
		{
			var year = DateTime.Today.Year;

			decimal d = Math.Floor(year*1M/19*1M);
			decimal m = d*19;

			decimal goldenNumber = year - m + 1;

			var easter = _repository.Query<EasterGoldenNumber>(a => a.GoldenNumber == goldenNumber).FirstOrDefault();

			var dateBeforeNextEasterSunday = DateTime.Now;

			if (easter != null)
			{
				dateBeforeNextEasterSunday = new DateTime(year, easter.Month, easter.Day);
			}

			DayOfWeek dayOfWeek = dateBeforeNextEasterSunday.Date.DayOfWeek;
			DateTime easterDate = dateBeforeNextEasterSunday;

			while (dayOfWeek != DayOfWeek.Sunday)
			{
				easterDate = easterDate.AddDays(1);
				dayOfWeek = easterDate.Date.DayOfWeek;
			}

			return easterDate;
		}


		private bool HasFoundPublicHolidayOnWeekend(DateTime currentDate, int dayOfWeek)
		{
			bool isSaturdayPublicHoliday = false;
			bool isSundayPublicHoliday = false;

			if (dayOfWeek == 6)
			{
				isSaturdayPublicHoliday = IsPublicHoliday(currentDate);
				DateTime sunday = currentDate.AddDays(1);
				isSundayPublicHoliday = IsPublicHoliday(sunday);
			}
			else if (dayOfWeek == 0)
			{
				isSundayPublicHoliday = IsPublicHoliday(currentDate);
				DateTime saturday = currentDate.AddDays(-1);
				isSaturdayPublicHoliday = IsPublicHoliday(saturday);
			}
			return isSaturdayPublicHoliday || isSundayPublicHoliday;
		}


		public DateTime NextBusinessDate(DateTime currentDate, int days)
		{
			DateTime nextBusinessDate = currentDate.AddDays(days);

			int holidayDays = 0;

			while (IsPublicHoliday(nextBusinessDate))
			{
				nextBusinessDate = nextBusinessDate.AddDays(holidayDays);
				holidayDays = 1;
			}
			days = 1;
			return nextBusinessDate.AddDays(days);
		}


		private int GetNumberOfDaysOnWeekends(DateTime currentDate, int dayOfWeek)
		{
			int daysToAdd = 0;
			bool isHoliday = HasFoundPublicHolidayOnWeekend(currentDate, dayOfWeek);

			switch (dayOfWeek)
			{
				case 6:
				{
					daysToAdd = isHoliday ? 3 : 1;
					break;
				}
				case 0:
				{
					daysToAdd = isHoliday ? 2 : 0;
					break;
				}
				default:
				{
					daysToAdd = 0;
					break;
				}
			}
			return daysToAdd;
		}


		private bool IsPublicHoliday(DateTime currentDate)
		{
			DateTime easterSunday = GetEasterSundayDate();

			DateTime goodFidayDate = easterSunday.AddDays(-2);
			DateTime easterMondayDate = easterSunday.AddDays(1);

			if (currentDate.ToShortDateString() == goodFidayDate.ToShortDateString() ||
			    currentDate.ToShortDateString() == easterMondayDate.ToShortDateString())
			{
				return true;
			}
			return _repository.Query<PublicHoliday>(a => a.Day == currentDate.Day && a.Month == currentDate.Month).Any();
		}

		#endregion

		#region Generate Transaction Number

		private string GetRandomCharacters()
		{
			var hashSet = new HashSet<string>();
			var array = new[]
			{
				"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "P", "R", "S", "T", "U", "V",
				"W", "X", "Y", "Z", "1", "2", "3", "4", "5", "6", "7", "8", "9", "0"
			};
			while (hashSet.Count < 3)
			{
				hashSet.Add(array[new Random().Next(0, 34)]);
			}

			return hashSet.ToHashString();
		}

		private string Generate(string citCode)
		{
			var depositDate = string.Concat(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
			var transactionReferenceNumber = string.Concat(GetRandomCharacters(), citCode, depositDate);
			return transactionReferenceNumber;
		}

		private string GenerateTransactionNumber(int siteId)
		{
			var endDate = DateTime.Now.AddDays(1);
			var depositDate = string.Concat(DateTime.Now.Year, "-", DateTime.Now.Month, "-", DateTime.Now.Day);

			var date = Convert.ToDateTime(depositDate);

			var citCode = _repository.Query<Site>(a => a.SiteId == siteId).FirstOrDefault().CitCode;
			var transactionReferenceNumber = Generate(citCode);

			while (true)
			{
				var anyOrders = _repository.Any<CashOrder>(a => a.ReferenceNumber == transactionReferenceNumber
				                                                && a.CreateDate.HasValue
				                                                && a.CreateDate >= date && a.CreateDate < endDate);
				if (!anyOrders)
				{
					return transactionReferenceNumber;
				}
				transactionReferenceNumber = Generate(citCode);
			}
		}
        
        public void CRUD_ApprovalTask(int cashOrderId, User user, string action)
        {
            var cashOrder = _repository.Query<CashOrder>(a => a.CashOrderId == cashOrderId).FirstOrDefault();

            var pendingStatus = _lookup.GetStatusId("PENDING");
            
            if (cashOrder == null) return;
            
            switch (action)
            {
                case "CREATE":
                    var task = new CashOrderTask
                    {
                        CashOrderId = cashOrder.CashOrderId,
                        ReferenceNumber = cashOrder.ReferenceNumber,
                        Date = DateTime.Now,
                        SiteId = cashOrder.SiteId,
                        UserId = user.UserId,
                        StatusId = _lookup.GetStatusId("PENDING"),
                        //RequestUrl = url,
                        CreatedById = user.UserId,
                        LastChangedById = user.UserId,
                        CreateDate = DateTime.Now,
                        LastChangedDate = DateTime.Now,
                        IsNotDeleted = true
                    };
                    _repository.Add(task);
                    break;

                case "UPDATE":

                    var declinedStatus = _lookup.GetStatusId("DECLINED");

                    var updateTask = _repository.Query<CashOrderTask>(a => a.CashOrderId == cashOrder.CashOrderId &&
                                                                    a.StatusId == declinedStatus).FirstOrDefault();

                    if (updateTask != null)
                    {
                        var taskDto = new CashOrderTaskDto
                        {
                            CashOrderTaskId = updateTask.CashOrderTaskId,
                            CashOrderId = cashOrder.CashOrderId,
                            ReferenceNumber = updateTask.ReferenceNumber,     //KEEPING THE OLD TASK REF NUMBER
                            Date = DateTime.Now,
                            SiteId = cashOrder.SiteId,
                            UserId = user.UserId,
                            //RequestUrl = url,
                        };

                        var mappedTask = _mapper.Map<CashOrderTaskDto, CashOrderTask>(taskDto);
                        mappedTask.EntityState = State.Modified;
                        mappedTask.LastChangedById = user.UserId;
                        mappedTask.LastChangedDate = DateTime.Now;
                        mappedTask.CreateDate = updateTask.CreateDate;
                        mappedTask.CreatedById = updateTask.CreatedById;
                        mappedTask.IsNotDeleted = true;
                        mappedTask.StatusId = _lookup.GetStatusId("PENDING");

                        _repository.Update(mappedTask);
                    }
                    break;

                case "APPROVE":

                    var approveTask = _repository.Query<CashOrderTask>(a => a.CashOrderId == cashOrder.CashOrderId &&
                                                                     a.StatusId == pendingStatus).FirstOrDefault();
                    if (approveTask != null)
                    {
                        var taskDto = new CashOrderTaskDto
                        {
                            CashOrderTaskId = approveTask.CashOrderTaskId,
                            CashOrderId = cashOrder.CashOrderId,
                            ReferenceNumber = approveTask.ReferenceNumber,     //KEEPING THE OLD TASK REF NUMBER
                            Date = DateTime.Now,
                            SiteId = cashOrder.SiteId,
                            UserId = user.UserId,
                            //RequestUrl = url,
                        };

                        var mappedTask = _mapper.Map<CashOrderTaskDto, CashOrderTask>(taskDto);
                        mappedTask.EntityState = State.Modified;
                        mappedTask.LastChangedById = user.UserId;
                        mappedTask.LastChangedDate = DateTime.Now;
                        mappedTask.CreateDate = approveTask.CreateDate;
                        mappedTask.CreatedById = approveTask.CreatedById;
                        mappedTask.IsNotDeleted = true;
                        mappedTask.StatusId = _lookup.GetStatusId("APPROVED");

                        _repository.Update(mappedTask);
                    }
                    break;

                case "DECLINE":

                    var declineTask = _repository.Query<CashOrderTask>(a => a.CashOrderId == cashOrder.CashOrderId &&
                                                                     a.StatusId == pendingStatus).FirstOrDefault();
                    if (declineTask != null)
                    {
                        var taskDto = new CashOrderTaskDto
                        {
                            CashOrderTaskId = declineTask.CashOrderTaskId,
                            CashOrderId = cashOrder.CashOrderId,
                            ReferenceNumber = declineTask.ReferenceNumber,     //KEEPING THE OLD TASK REF NUMBER
                            Date = DateTime.Now,
                            SiteId = cashOrder.SiteId,
                            UserId = user.UserId,
                            //RequestUrl = url,
                        };

                        var mappedTask = _mapper.Map<CashOrderTaskDto, CashOrderTask>(taskDto);
                        mappedTask.EntityState = State.Modified;
                        mappedTask.LastChangedById = user.UserId;
                        mappedTask.LastChangedDate = DateTime.Now;
                        mappedTask.CreateDate = declineTask.CreateDate;
                        mappedTask.CreatedById = declineTask.CreatedById;
                        mappedTask.IsNotDeleted = true;
                        mappedTask.StatusId = _lookup.GetStatusId("DECLINED");

                        _repository.Update(mappedTask);
                    }

                    break;

                default:
                    return;
            }
        }
        

	    public string GetCashOrderTaskRefNum(int cashOrderId)
	    {
	        var cashOrderTask = _repository.Query<CashOrderTask>(a => a.CashOrderId == cashOrderId && a.StatusId == GetStatusId("PENDING")).FirstOrDefault();
	        return cashOrderTask != null ? cashOrderTask.ReferenceNumber : null;
	    }

	    public bool IsCashOrderPending(int id)
	    {
	        var pendingStatus = GetStatusId("PENDING");
	        return _repository.Any<CashOrder>(a => a.CashOrderId == id && a.StatusId == pendingStatus);
	    }

	    public bool DeclinedCashOrderTaskExist(int cashOrderId)
	    {
            var declinedStatus = GetStatusId("DECLINED");
	        return _repository.Any<CashOrderTask>(a => a.CashOrderId == cashOrderId && a.StatusId == declinedStatus);
	    }

        public void DeleteCashOrderTask(int id, int userId)
        {
            var cashOrderTask = _repository.Query<CashOrderTask>(a => a.CashOrderId == id).FirstOrDefault();
            if (cashOrderTask != null) _repository.Delete<CashOrderTask>(cashOrderTask.CashOrderTaskId, userId);
        }

	    #endregion

		#endregion
	}
}