using System.Collections.Generic;
using Application.Dto.Merchant;
using Domain.Data.Model;
using Utility.Core;

namespace Application.Modules.Maintanance.Merchant
{
    public interface IMerchantValidation
    {
        /// <summary>
        /// Return a list of all Merchants
        /// </summary>
        /// <returns></returns>
        IEnumerable<ListMerchantDto> All();

        /// <summary>
        /// Find a merchant by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<MerchantDto> Find(int id);

        /// <summary>
        /// Add A New Merchant
        /// </summary>
        /// <param name="merchant"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<int> Add(MerchantDto merchant, string username);

        /// <summary>
        /// Update a new Merchant
        /// </summary>
        /// <param name="merchant"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(MerchantDto merchant, string username);

        /// <summary>
        /// Delete a Merchant
        /// </summary>
        /// <param name="id"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<Task> Delete(int id, string username);

        /// <summary>
        /// Submit a Merchant for approval
        /// </summary>
        /// <param name="merchantDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<int> Submit(MerchantDto merchantDto, string username);

        /// <summary>
        /// Check if Merchant if rejected
        /// </summary>
        /// <param name="rejectParameters"></param>
        /// <param name="merchant"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> IsRejected(RejectMerchantArgumentsDto rejectParameters, MerchantDto merchant, string username);

        /// <summary>
        /// Check if the Merchant Name has been used
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool IsNameUsed(string name);


        /// <summary>
        /// Check if the Merchant Name has been used another Merchant
        /// </summary>
        /// <param name="name"></param>
        /// /// <param name="merchnatId"></param>
        /// <returns></returns>
        bool IsNameUsedByAnother(string name, int merchnatId);

        /// <summary>
        /// Check if the contract number has been used
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <returns></returns>
        bool IsContractNumberUsed(string contractNumber);
        
        /// <summary>
        /// Check if the contract number has been used by another Merchant
        /// </summary>
        /// <param name="contractNumber"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        bool IsContractNumberUsedByAnother(string contractNumber, int merchantId);
        
        /// <summary>
        /// Check if the contract number has been used
        /// </summary>
        /// <param name="registrationNumber"></param>
        /// <returns></returns>
        bool IsRegistrationNumberUsed(string registrationNumber);
        
        /// <summary>
        /// Check if the contract number has been used by another Merchant
        /// </summary>
        /// <param name="registrationNumber"></param>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        bool IsRegistrationNumberUsedByAnother(string registrationNumber, int merchantId);

    }
}