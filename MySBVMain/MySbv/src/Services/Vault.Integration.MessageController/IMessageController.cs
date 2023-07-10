using Utility.Core;
using Vault.Integration.DataContracts;

namespace Vault.Integration.MessageController
{
    public interface IMessageController
    {
        /// <summary>
        /// </summary>
        /// <param name="cashDepost"></param>
        /// <returns></returns>
        MethodResult<bool> RelayMessage(RequestMessage cashDepost);

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageLabel"></param>
        /// <returns></returns>
        bool IsDepositsInQueue(RequestMessage message, string messageLabel);

        /// <summary>
        /// </summary>
        /// <returns></returns>
        bool IsMessageInQueue(RequestMessage message);
    }
}