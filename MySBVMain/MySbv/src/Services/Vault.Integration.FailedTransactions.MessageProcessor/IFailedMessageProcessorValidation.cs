using Vault.Integration.DataContracts;

namespace Vault.Integration.FailedTransactions.MessageProcessor
{
    public interface IFailedMessageProcessorValidation
    {
        /// <summary>
        ///  Process a failed transaction to the database
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        bool IsProcessed(RequestMessage message);
    }
}