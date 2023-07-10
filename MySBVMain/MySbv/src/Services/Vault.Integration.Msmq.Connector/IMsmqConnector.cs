using Utility.Core;
using Vault.Integration.DataContracts;

namespace Vault.Integration.Msmq.Connector
{
    /// <summary>
    ///     Message Queue Identifier
    /// </summary>
    public enum QueueIdentifier
    {
        GptRequest,
        CashConnectRequest,
        GreystoneRequest,
        Response,
        Confirmation,
        FailedTransactions,
        Invalid
    }

    /// <summary>
    /// </summary>
    public interface IMsmqConnector
    {
        /// <summary>
        ///     This method Adds message to a MSMQ.
        /// </summary>
        /// <param name="queueIdentifier"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        MethodResult<bool> AddMessage<T>(QueueIdentifier queueIdentifier, T item) where T : IMessageLabel;


        /// <summary>
        ///     Receives and Deletes the message from the MSMQ
        /// </summary>
        /// <param name="queueIdentifier"></param>
        /// <returns></returns>
        MethodResult<T> ReceiveMessage<T>(QueueIdentifier queueIdentifier);


        /// <summary>
        ///     This method reads/listens for the first message on the MSMQ.
        ///     The message is only read without being deleted
        /// </summary>
        /// <param name="queueIdentifier"></param>
        /// <returns></returns>
        MethodResult<T> ReadMessage<T>(QueueIdentifier queueIdentifier);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queueIdentifier"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        bool IsDepositsInQueue<T>(QueueIdentifier queueIdentifier, T item) where T : IMessageLabel;

	    /// <summary>
	    /// 
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="queueIdentifier"></param>
	    /// <param name="serialNumber"></param>
	    /// <returns></returns>
	    bool VerifyBagBySerialNumber(QueueIdentifier queueIdentifier, string serialNumber);

    }
}