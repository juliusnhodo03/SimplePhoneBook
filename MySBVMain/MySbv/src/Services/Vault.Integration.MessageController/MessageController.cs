using System;
using System.ComponentModel.Composition;
using System.Linq;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;

namespace Vault.Integration.MessageController
{
	[Export(typeof (IMessageController))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MessageController : IMessageController
	{
		#region Fields

		/// <summary>
		/// IRepository Instance
		/// </summary>
		private readonly IRepository _repository;

		/// <summary>
		/// IMsmqConnector Instance
		/// </summary>
		private readonly IMsmqConnector _msmqConnector;

		#endregion

		#region Constructor

		/// <summary>
		/// </summary>
		/// <param name="repository"></param>
		/// <param name="msmqConnector"></param>
		[ImportingConstructor]
		public MessageController(IRepository repository, IMsmqConnector msmqConnector)
		{
			_repository = repository;
			_msmqConnector = msmqConnector;
		}

		#endregion

        #region IMessageController

        /// <summary>
        /// </summary>
        /// <param name="cashDepost"></param>
        /// <returns></returns>
        public MethodResult<bool> RelayMessage(RequestMessage cashDepost)
        {
            try
            {
                var vaultOwner = GetVaultOwner(cashDepost.DeviceSerial);

                if (vaultOwner == "GPT")
                    _msmqConnector.AddMessage(QueueIdentifier.GptRequest, AddMessageToEnvelope(cashDepost));
                else if (vaultOwner == "CASH_CONNECT")
                    _msmqConnector.AddMessage(QueueIdentifier.CashConnectRequest, AddMessageToEnvelope(cashDepost));
                else if (vaultOwner == "GREYSTONE")
                    _msmqConnector.AddMessage(QueueIdentifier.GreystoneRequest, AddMessageToEnvelope(cashDepost));

                return new MethodResult<bool>(MethodStatus.Successful, true);
            }
            catch (Exception ex)
            {
                this.Log().Fatal("Failed to Relay Message", ex);
                return new MethodResult<bool>(MethodStatus.Error, false, ex.Message);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageLabel"></param>
        /// <returns></returns>
        public bool IsDepositsInQueue(RequestMessage message, string messageLabel)
        {
            var queueResult = GetQueue(message);
            if (queueResult == QueueIdentifier.Invalid) return false;

            return _msmqConnector.IsDepositsInQueue(queueResult, new MessageEnvelope<RequestMessage>()
            {
                Label = messageLabel,
                MessageObject = message
            });
        }

	    public bool IsMessageInQueue(RequestMessage request)
	    {
            var queueResult = GetQueue(request);
            if (queueResult == QueueIdentifier.Invalid) return false;
            return _msmqConnector.IsDepositsInQueue(queueResult, AddMessageToValidationEnvelope(request));
	    }

		#endregion

        #region Helper

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private MessageEnvelope<RequestMessage> AddMessageToValidationEnvelope(RequestMessage message)
        {
            // All messages will have a transaction type of 22
            return new MessageEnvelope<RequestMessage>()
            {
                Label = string.Concat(22, "_", message.CollectionUnits.CollectionUnit.FirstOrDefault().Value),
                MessageObject = message
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private MessageEnvelope<RequestMessage> AddMessageToEnvelope(RequestMessage message)
        {
            return new MessageEnvelope<RequestMessage>()
            {
                Label = string.Concat(message.TransactionType.Code, "_", message.CollectionUnits.CollectionUnit.FirstOrDefault().Value),
                MessageObject = message
            };
        }

        /// <summary>
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns>QueuePath</returns>
        private string GetVaultOwner(string serialNumber)
        {
            var result = _repository.Query<Device>(e => e.SerialNumber == serialNumber, e => e.DeviceType, e => e.DeviceType.Supplier).FirstOrDefault();
            return result != null ? result.DeviceType.Supplier.LookUpKey : string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private QueueIdentifier GetQueue(RequestMessage request)
        {
            var vaultOwner = GetVaultOwner(request.DeviceSerial);

            if (vaultOwner == "GPT")
                return QueueIdentifier.GptRequest;
            if (vaultOwner == "CASH_CONNECT")
                return QueueIdentifier.CashConnectRequest;
            if (vaultOwner == "GREYSTONE")
                return QueueIdentifier.GreystoneRequest;

            return QueueIdentifier.Invalid;
        }

        #endregion
        
	}
}