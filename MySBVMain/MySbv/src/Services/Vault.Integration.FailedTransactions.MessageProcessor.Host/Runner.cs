using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Domain.Serializer;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;

namespace Vault.Integration.FailedTransactions.MessageProcessor.Host
{
    /// <summary>
    ///     Queries MSMQ's for messages and pass the messages
    ///     to th message processor for processing.
    /// </summary>
    [Export(typeof (IRunner))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Runner : IRunner
    {
        #region Fields

        /// <summary>
        ///     Message processor instance
        /// </summary>
        private readonly IFailedMessageProcessorValidation _messageProcessor;

        /// <summary>
        ///     Msmq Connector Instance
        /// </summary>
        private readonly IMsmqConnector _msmqConnector;

        /// <summary>
        ///     XML Serializer instance
        /// </summary>
        private readonly ISerializer _serializer;

        #endregion

        #region Constructor

        /// <summary>
        ///     Runner constructor
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="processor"></param>
        /// <param name="msmqConnector"></param>
        [ImportingConstructor]
        public Runner(ISerializer serializer, IFailedMessageProcessorValidation processor, IMsmqConnector msmqConnector)
        {
            _serializer = serializer;
            _messageProcessor = processor;
            _msmqConnector = msmqConnector;
        }

        #endregion

        #region IRunner

        /// <summary>
        ///     Pools the FailedTransactions Queue for
        ///     messages and process them
        /// </summary>
        public void Run()
        {
            Task.Factory.StartNew(() =>
            {
                this.Log().Info("Pooling the FailedTransactions Queue");
                while (true)
                {
                    try
                    {
                        MethodResult<MessageEnvelope<RequestMessage>> message =
                            _msmqConnector.ReadMessage<MessageEnvelope<RequestMessage>>(
                                QueueIdentifier.FailedTransactions);

                        LogDepositContent(message);

                        // convert to request message
                        RequestMessage requestMessage = GetMessageFromEnvelope(message.EntityResult);

                        bool wasProcessed = _messageProcessor.IsProcessed(requestMessage);

                        if (wasProcessed)
                        {
                            _msmqConnector.ReceiveMessage<RequestMessage>(QueueIdentifier.FailedTransactions);
                        }
                        else
                        {
                            this.Log()
                                .Error("SubmitRequest Error", new Exception("Failed to process a failed Transaction"));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log().Fatal("Exception On Method : [RUN -> FAILED_TRANSACTION_QUEUE_POOL]", ex);
                    }
                }
            });
        }

        #endregion

        #region Internal

        /// <summary>
        ///     Log the entire message in XML format to file
        /// </summary>
        /// <param name="message"></param>
        private void LogDepositContent(MethodResult<MessageEnvelope<RequestMessage>> message)
        {
            if (this.Log().IsDebugEnabled)
            {
                this.Log().Debug(_serializer.Serialize(message.EntityResult));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="messageEnvelope"></param>
        /// <returns></returns>
        private RequestMessage GetMessageFromEnvelope(MessageEnvelope<RequestMessage> messageEnvelope)
        {
            return messageEnvelope.MessageObject;
        }

        #endregion
    }
}