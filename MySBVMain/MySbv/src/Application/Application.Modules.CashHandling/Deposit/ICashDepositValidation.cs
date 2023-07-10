
using System.Collections.Generic;
using Application.Dto.CashDeposit;
using Domain.Data.Model;
using Utility.Core;

namespace Application.Modules.CashHandling.Deposit
{
	public interface ICashDepositValidation
	{
		/// <summary>
		///     Return a list of all CashDeposits
		/// <param name="user"></param>
		/// </summary>
		/// <returns></returns>
		IEnumerable<ListCashDepositDto> All(User user);

		/// <summary>
		///     Find a CashDeposit by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		CashDepositDto Find(int id);


		/// <summary>
		///     Find a CashDeposit by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		CashDeposit FindUnMappedDeposit(int id);

		/// <summary>
		///     Add A New CashDeposit
		/// </summary>
		/// <param name="cashDeposit"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<CashDeposit> Add(CashDepositDto cashDeposit, User user); 

		/// <summary>
		///     Update a new CashOrder
		/// </summary>
		/// <param name="cashDeposit"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<CashDeposit> Edit(CashDepositDto cashDeposit, User user);

		/// <summary>
		///     Submit CashDeposit for processing
		/// </summary>
		/// <param name="cashDeposit"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<CashDeposit> Submit(CashDepositDto cashDeposit, User user);

		/// <summary>
		///     Delete a CashDeposit
		/// </summary>
		/// <param name="id"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<bool> Delete(int id, User user);
		
		/// <summary>
		/// Submit a ContainerDrop in an existing.
		/// This is appending a submitted drop to a container
		/// </summary>
		/// <param name="containerDrop"></param>
		/// <param name="containerAmount"></param>
		/// <param name="description"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<ContainerDrop> SubmitDrop(ContainerDropDto containerDrop, decimal containerAmount, string description, User user);

		

		/// <summary>
		///     Submit new ContainerDrop in a new Container
		/// </summary>
		/// <param name="container"></param>
		/// <param name="containerAmount"></param>
		/// <param name="description"></param>
		/// <returns></returns>
		MethodResult<Container> SubmitNewDropInNewContainer(ContainerDto container, decimal containerAmount, string description, User user);


		/// <summary>
		///     get deposit Containers
		/// </summary>
		/// <param name="cashDepositId"></param>
		/// <returns></returns>
		MethodResult<IEnumerable<ContainerDto>> GetContainers(int cashDepositId);

		/// <summary>
		///    Find Container
		/// </summary>
		/// <param name="containerId"></param>
		/// <returns></returns>
		Container FindContainer(int containerId);


		/// <summary>
		///     get Container Drops
		/// </summary>
		/// <param name="containerId"></param>
		/// <returns></returns>
		MethodResult<IEnumerable<ContainerDropDto>> GetDrops(int containerId);


		/// <summary>
		///    delete a containr Drop
		/// </summary>
		/// <param name="containerDropId"></param>
		/// <param name="user"></param>
		/// <returns></returns>
		MethodResult<bool> DeleteDrop(int containerDropId, User user);
		

		/// <summary>
		///    initialize deposit
		/// </summary>
		/// <param name="cashDepositId"></param>
		/// <returns></returns>
		CashDepositDto CashDepositInitializer(int cashDepositId);


		/// <summary>
		/// get logged in user
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		User GetLoggedUser(string username);


		/// <summary>
		/// check if Cash Deposit is submitted
		/// </summary>
		/// <param name="cashDepositId"></param>
		/// <returns></returns>
		bool IsSubmitted(int cashDepositId);
		   
		/// <summary>
		/// Get Container By DropId
		/// </summary>
		/// <param name="containerDropId"></param>
		/// <returns></returns>
		Container GetContainerByDropId(int containerDropId);


		/// <summary>
		/// Get Deposit Type
		/// </summary>
		/// <param name="cashDepositId"></param>
		/// <returns></returns>
		string GetDepositType(int cashDepositId);


		/// <summary>
		/// Get ContainerDrops for Kendo List Grid
		/// </summary>
		/// <param name="containerId"></param>
		/// <returns></returns>
		MethodResult<IEnumerable<ContainerDropDto>> GetContainerDrops(int containerId);
		
		
		/// <summary>
		/// Get Site by siteId
		/// </summary>
		/// <param name="siteId"></param>
		/// <returns></returns>		
		Site GetSite(int siteId);
	}
}
