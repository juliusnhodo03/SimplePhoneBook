using System.Collections.Generic;
using Application.Dto.FailedVaultTransaction;
using Domain.Data.Model;
using Utility.Core;
using Vault.Integration.DataContracts;

namespace Application.Modules.FailedVaultRequest
{
    public interface IVaultRequestValidation
    {
        /// <summary>
        ///     Returns a list of all Vault-Failed headers
        /// </summary>
        /// <returns></returns>
        IEnumerable<FailedTransactionHeaderDto> GetFailedHeaders();

        /// <summary>
        ///     Returns a list of all Vault-Failed deposits
        /// </summary>
        /// <returns></returns>
        IEnumerable<FailedRequestListDto> GetFailedRequests(string serialNumber);

        /// <summary>
        ///     Finds a FailedRequest by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MethodResult<FailedRequestDto> Find(int id);

        /// <summary>
        ///     Approve a failed vault request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        MethodResult<bool> Approve(FailedRequestDto request, User user);

        /// <summary>
        ///     Decline approval of failed vault request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        MethodResult<bool> Decline(FailedRequestDto request, User user);

        /// <summary>
        ///     Updates a failed vault request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        MethodResult<bool> Edit(FailedRequestDto request, User user);

        /// <summary>
        ///     Validate updates
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        MethodResult<List<MessageError>> ValidationMessage(RequestMessage requestMessage);
    }
}