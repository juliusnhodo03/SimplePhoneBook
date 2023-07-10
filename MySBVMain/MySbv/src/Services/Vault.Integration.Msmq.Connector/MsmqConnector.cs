using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Messaging;
using Infrastructure.Logging;
using Newtonsoft.Json;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector.Extensions;

namespace Vault.Integration.Msmq.Connector
{
	/// <summary>
	/// </summary>
	[Export(typeof (IMsmqConnector))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MsmqConnector : IMsmqConnector
	{
		#region Request Methods

		/// <summary>
		/// This method Adds message to a MSMQ.
		/// </summary>
		/// <param name="queueIdentifier"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public MethodResult<bool> AddMessage<T>(QueueIdentifier queueIdentifier, T item) where T: IMessageLabel
		{
			try
			{
				string messageQueueUri = GetQueueUri(queueIdentifier);
				using (var messageQueue = GetMessageQueue(messageQueueUri))
				{
					this.Log().Debug("Adding a Request Message to queue");
                    var message = new Message
					{
						Recoverable = true,
						BodyStream = item.ToJsonStream(),
						Label = item.Label
					};

					var scope = new MessageQueueTransaction();
					scope.Begin();
					messageQueue.Send(message, scope);
					scope.Commit();

					this.Log().Debug("message Added to queue");
					this.Log().Debug(string.Format("Message Label \n[{0}]", message.Label));

					return new MethodResult<bool>(MethodStatus.Successful, true);
				}
			}
			catch (Exception ex)
			{
				this.Log().Fatal("Exception On Method : [ADD_MESSAGE_FROM_QUEUE]", ex);
				return new MethodResult<bool>(MethodStatus.Error, false);
			}
		}


		/// <summary>
		/// Receives and Deletes the message from the MSMQ
		/// </summary>
		/// <param name="queueIdentifier"></param>
		/// <returns></returns>
		public MethodResult<T> ReceiveMessage<T>(QueueIdentifier queueIdentifier)
		{
			try
			{
				string messageQueueUri = GetQueueUri(queueIdentifier);

				using (MessageQueue messageQueue = GetMessageQueue(messageQueueUri))
				{
					using (var transaction = new MessageQueueTransaction())
					{
						transaction.Begin();
						Message message = messageQueue.Receive(transaction);
						var reader = new StreamReader(message.BodyStream);
						string jsonBody = reader.ReadToEnd();
						var deposit = JsonConvert.DeserializeObject<T>(jsonBody);
						transaction.Commit();
						return new MethodResult<T>(MethodStatus.Successful, deposit);
					}
				}
			}
			catch (Exception ex)
			{
				this.Log().Fatal("Exception On Method : [RECEIVE_MESSAGE_FROM_QUEUE]", ex);
				return new MethodResult<T>(MethodStatus.Error, default(T), ex.Message);
			}
		}

		
		/// <summary>
		/// This method reads/listens for the first message on the MSMQ.
		/// The message is only read without being deleted
		/// </summary>
		/// <param name="queueIdentifier"></param>
		/// <returns></returns>
		public MethodResult<T> ReadMessage<T>(QueueIdentifier queueIdentifier)
		{
			try
			{
				string messageQueueUri = GetQueueUri(queueIdentifier);

				using (MessageQueue messageQueue = GetMessageQueue(messageQueueUri))
				{
					using (var transaction = new MessageQueueTransaction())
					{
						transaction.Begin();
						Message message = messageQueue.Receive(transaction);
						var reader = new StreamReader(message.BodyStream);
						string jsonBody = reader.ReadToEnd();
						var deposit = JsonConvert.DeserializeObject<T>(jsonBody);
						transaction.Abort();
						return new MethodResult<T>(MethodStatus.Successful, deposit);
					}
				}
			}
			catch (Exception ex)
			{
				this.Log().Fatal("Exception On Method : [READ_MESSAGE_FROM_QUEUE]", ex);
				return new MethodResult<T>(MethodStatus.Error, default(T), ex.Message);
			}
		}

		public bool IsDepositsInQueue<T>(QueueIdentifier queueIdentifier, T item) where T : IMessageLabel
		{
			string messageQueueUri = GetQueueUri(queueIdentifier);

			using (MessageQueue messageQueue = GetMessageQueue(messageQueueUri))
			{
				var messages = messageQueue.GetAllMessages();
				return messages.Any(message => message.Label == item.Label);
			}
		}

		public bool VerifyBagBySerialNumber(QueueIdentifier queueIdentifier, string serialNumber)
		{
			string messageQueueUri = GetQueueUri(queueIdentifier);

			using (MessageQueue messageQueue = GetMessageQueue(messageQueueUri))
			{
				var messages = messageQueue.GetAllMessages();
				var bagExists = false;

				foreach (var message in messages)
				{
					var reader = new StreamReader(message.BodyStream);
					string jsonBody = reader.ReadToEnd();

					var deposit = JsonConvert.DeserializeObject<MessageEnvelope<RequestMessage>>(jsonBody);

					var hasMatch = deposit.MessageObject.CollectionUnits.CollectionUnit.Any(o => o.Value.ToUpper().Equals(serialNumber.ToUpper()));

					if (hasMatch)
					{
						bagExists = true;
					}
				}
				return bagExists;
			}
		}

	    #endregion

		#region Manage Queue

		/// <summary>
		/// </summary>
		/// <param name="msmqPathName"></param>
		/// <returns></returns>
		private MessageQueue GetMessageQueue(string msmqPathName)
		{
			if (MessageQueue.Exists(msmqPathName))
			{
				return new MessageQueue(msmqPathName);
			}
			return MessageQueue.Create(msmqPathName, true);
		}

		/// <summary>
		/// </summary>
		/// <param name="queuIdentifier"></param>
		/// <returns>QueuePath</returns>
		private string GetQueueUri(QueueIdentifier queuIdentifier)
		{
			switch (queuIdentifier)
			{
				case QueueIdentifier.GptRequest:
                    return QueueResourceUrl.GPT_REQUEST;

                case QueueIdentifier.CashConnectRequest:
                    return QueueResourceUrl.CASH_CONNECT_REQUEST;

                case QueueIdentifier.GreystoneRequest:
                    return QueueResourceUrl.GREYSTONE_REQUEST;

				case QueueIdentifier.Response:
					return QueueResourceUrl.RESPONSE;

				case QueueIdentifier.Confirmation:
					return QueueResourceUrl.CONFIRMATION;

				case QueueIdentifier.FailedTransactions:
					return QueueResourceUrl.FAILED_REQUEST;

				default:
					throw new ArgumentOutOfRangeException("queuIdentifier");
			}
		}

		#endregion
	}
}