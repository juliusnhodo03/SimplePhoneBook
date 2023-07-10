using System;
using System.Collections.Generic;
using Application.Dto.CashOrder;
using Application.Dto.CashOrderTask;
using Application.Dto.Files;
using Domain.Data.Model;
using Utility.Core;

namespace Application.Modules.CashOrdering.CashOrders
{
	public interface ICashOrderingValidation
	{
		/// <summary>
		///     Return a list of all CashOrders
		/// </summary>
		/// <returns></returns>
		IEnumerable<ListCashOrderDto> All(User user);

		/// <summary>
		///     Find a CashOrder by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		MethodResult<CashOrderDto> Find(int id);

		/// <summary>
		///     Add A New CashOrder
		/// </summary>
		/// <param name="cashOrder"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<CashOrder> Add(CashOrderDto cashOrder, User user);

		/// <summary>
		///     Update a new CashOrder
		/// </summary>
		/// <param name="cashOrder"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<int> Edit(CashOrderDto cashOrder, User user);

		/// <summary>
		///     Process CashOrder
		/// </summary>
		/// <param name="cashOrderContainer"></param>
		/// <param name="exchangeBagNumber"></param>
		/// <param name="emptyBagNumber"></param>
		/// <returns></returns>
		MethodResult<int> Process(CashOrderContainerDto cashOrderContainer, string exchangeBagNumber, string emptyBagNumber, User user);

		/// <summary>
		///     Submit CashOrder for processing
		/// </summary>
		/// <param name="cashOrder"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<CashOrder> Submit(CashOrderDto cashOrder, User user);

        /// <summary>
        /// Approve a Cash Order and update a Cash Order Task to APPROVED STATUS
        /// </summary>
        /// <param name="cashOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
	    MethodResult<CashOrder> Approve(CashOrderDto cashOrder, User user);

        /// <summary>
        /// Decline a Cash Order and update a Cash Order Task to DECLINED STATUS
        /// </summary>
        /// <param name="cashOrder"></param>
        /// <param name="user"></param>
        /// <returns></returns>
	    MethodResult<CashOrder> Reject(CashOrderDto cashOrder, User user);

		/// <summary>
		///     Delete a CashOrder
		/// </summary>
		/// <param name="id"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<bool> Delete(int id, User user);

		/// <summary>
		///     Find CashOrder by Sealnumber or SerialNumber
		/// </summary>
		/// <param name="searchText"></param>
		/// <returns></returns>
		MethodResult<CashOrderDto> FindBySealSerialNumber(string searchText, User user);
		
		/// <summary>
		///     get a cashOrder types
		/// </summary>
		/// <param name="cashOrderTypeId"></param>
		/// <returns></returns>
		string GetCashOrderType(int cashOrderTypeId);

		/// <summary>
		///     Cash denominations forwarded for Exchange by client
		/// </summary>
		/// <param name="cashOrderId"></param>
		/// <returns></returns>
		IEnumerable<ItemDenominationDto> CashForwardedForExchange(int cashOrderId);

		/// <summary>
		///     Cash denominations required by client
		/// </summary>
		/// <param name="cashOrderId"></param>
		/// <returns></returns>
		IEnumerable<ItemDenominationDto> CashRequiredByClient(int cashOrderId);

		/// <summary>
		///     Gets the next DeliveryDate
		///     This depends on the 11h00 Cutoff time on cash orders, weekends, and public holidays.
		/// </summary>
		/// <returns></returns>
		DateTime GetNextDeliveryDate();


		/// <summary> 
		/// get logged in user
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns> 
		User GetLoggedUser(string username);


        /// <summary>
        /// Find Order by Reference Number
        /// </summary>
        /// <param name="referenceNumber"></param>
        /// <param name="user"></param>
	    MethodResult<CashOrderDto> FindByRefenceNumber(string referenceNumber, User user);


	    /// <summary>
	    /// Add Cash Order Approval Task
	    /// </summary>
	    /// <param name="cashOrderId"></param>
	    /// <param name="user"></param>
	    /// <param name="action"></param>
        void CRUD_ApprovalTask(int cashOrderId, User user, string action);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <returns></returns>
	    string GetCashOrderTaskRefNum(int cashOrderId);


        /// <summary>
        /// CHECK IF CASH ORDER IN A PENDING STATE
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
	    bool IsCashOrderPending(int id);


        /// <summary>
        /// Find Cash Order Task information
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
	    MethodResult<CashOrderTaskDto> FindCashOrderTask(int id);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cashOrderId"></param>
        /// <returns></returns>
	    List<FileDto> GetFiles(int cashOrderId);

	    /// <summary>
	    ///     Find a CashOrder by id
	    /// </summary>
	    /// <param name="id"></param>
	    MethodResult<CashOrder> GetOrder(int id);

	}
}