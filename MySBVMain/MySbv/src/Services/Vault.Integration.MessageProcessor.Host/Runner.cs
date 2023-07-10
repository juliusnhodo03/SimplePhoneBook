using System;
using System.ComponentModel.Composition;
using System.Data.Entity.Validation;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Serializer;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;
using Task = System.Threading.Tasks.Task;

namespace Vault.Integration.MessageProcessor.Host
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

        private readonly ILookup _lookup;

        /// <summary>
        ///     Message processor instance
        /// </summary>
        private readonly IMessageProcessor _messageProcessor;

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
        /// <param name="lookup"></param>
        [ImportingConstructor]
        public Runner(ISerializer serializer, IMessageProcessor processor, IMsmqConnector msmqConnector, ILookup lookup)
        {
            _serializer = serializer;
            _messageProcessor = processor;
            _msmqConnector = msmqConnector;
            _lookup = lookup;
        }

        #endregion

        #region IRunner

        /// <summary>
        ///     Pools the GPT and the CASH CONNECT Queues for
        ///     messages and process them
        /// </summary>
        public void Run()
        {
            Task.Factory.StartNew(() =>
            {
                this.Log().Info("Polling the GPT Queue");
                while (true)
                {
                    try
                    {
                        MethodResult<MessageEnvelope<RequestMessage>> message =
                            _msmqConnector.ReadMessage<MessageEnvelope<RequestMessage>>(QueueIdentifier.GptRequest);

                        LogDepositContent(message);

                        MethodResult<bool> result =
                            _messageProcessor.SubmitRequest(GetMessageFromEnvelope(message.EntityResult),
                                VaultSource.GPT);

                        if (result.Status == MethodStatus.Successful)
                        {
                            _msmqConnector.ReceiveMessage<RequestMessage>(QueueIdentifier.GptRequest);
                        }
                        else
                        {
                            this.Log().Error("SubmitRequest Error",
                                new Exception(string.Format("Exception of type [{0}] was raised with message \n[{1}]",
                                    result.Tag, result.Message)));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log().Fatal("Exception On Method : [RUN -> GPT_QUEUE_POOL]", ex);
                    }
                }
            });

            Task.Factory.StartNew(() =>
            {
                this.Log().Info("Polling the Cash Connect Queue");
                while (true)
                {
                    try
                    {
                        MethodResult<MessageEnvelope<RequestMessage>> message =
                            _msmqConnector.ReadMessage<MessageEnvelope<RequestMessage>>(
                                QueueIdentifier.CashConnectRequest);

                        LogDepositContent(message);

                        MethodResult<bool> result =
                            _messageProcessor.SubmitRequest(GetMessageFromEnvelope(message.EntityResult),
                                VaultSource.WEBFLO);

                        if (result.Status == MethodStatus.Successful)
                        {
                            _msmqConnector.ReceiveMessage<RequestMessage>(QueueIdentifier.CashConnectRequest);
                        }
                        else
                        {
                            this.Log().Error("SubmitRequest Error",
                                new Exception(string.Format("Exception of type [{0}] was raised with message \n[{1}]",
                                    result.Tag, result.Message)));
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log().Fatal("Exception On Method : [RUN -> CASH_CONNECT_QUEUE_POOL]", ex);
                    }
                }
            });


            Task.Factory.StartNew(() =>
            {
                this.Log().Info("Polling the Greystone Queue");
                while (true)
                {
                    try
                    {
                        MethodResult<MessageEnvelope<RequestMessage>> message =
                            _msmqConnector.ReadMessage<MessageEnvelope<RequestMessage>>(QueueIdentifier.GreystoneRequest);

                        LogDepositContent(message);

                        MethodResult<bool> result =
                            _messageProcessor.SubmitRequest(GetMessageFromEnvelope(message.EntityResult),
                                VaultSource.GREYSTONE);

                        if (result.Status == MethodStatus.Successful)
                        {
                            _msmqConnector.ReceiveMessage<RequestMessage>(QueueIdentifier.GreystoneRequest);

                            RequestMessage request = message.EntityResult.MessageObject;

                            VaultTransactionType transactionType = _lookup.GetVaultTransactionType("CIT");

                            if (transactionType != null)
                            {
                                bool isCit = request.TransactionType.Code == transactionType.Code.ToString();
                                if (isCit)
                                {
                                    _messageProcessor.SaveCitRequest(request);
                                }
                            }
                        }
                        else
                        {
                            this.Log().Error("SubmitRequest Error",
                                new Exception(string.Format("Exception of type [{0}] was raised with message \n[{1}]",
                                    result.Tag, result.Message)));
                        }
                    }
                    catch (DbEntityValidationException ex)
                    {
                        foreach (DbEntityValidationResult entityValidationError in ex.EntityValidationErrors)
                        {
                            foreach (DbValidationError validationError in entityValidationError.ValidationErrors)
                            {
                                this.Log().Fatal(validationError, ex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log().Fatal("Exception On Method : [RUN -> GREYSTONE_QUEUE_POOL]", ex);
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