using Application.Modules.Common;
using Utility.Core;
using Vault.Integration.DataContracts;

namespace Vault.Integration.MessageProcessor
{
    /// <summary>
    /// </summary>
    public interface IMessageProcessor
    {
        /// <summary>
        /// </summary>
        /// <param name="deposit"></param>
        /// <param name="vaultSource"></param>
        MethodResult<bool> SubmitRequest(RequestMessage deposit, VaultSource vaultSource);

        /// <summary>
        ///     Saves the Cit Request details
        /// </summary>
        /// <param name="request"></param>
        void SaveCitRequest(RequestMessage request);
    }
}