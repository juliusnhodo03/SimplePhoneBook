using System;
using System.ComponentModel.Composition;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Configuration;
using System.Transactions;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Serializer;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;
using Vault.Integration.ResponseClient.CashConnect;
using Task = System.Threading.Tasks.Task;

namespace Vault.Integration.ResponseClient
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
        ///     web service client
        /// </summary>
        private readonly CashConnectClient _client;

        /// <summary>
        ///     Msmq Connector Instance
        /// </summary>
        private readonly IMsmqConnector _msmqConnector;

        private readonly IRepository _repository;

        /// <summary>
        ///     Object to xml serializer
        /// </summary>
        private readonly ISerializer _serializer;

        #endregion

        #region Constructor

        /// <summary>
        ///     Runner constructor
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="msmqConnector"></param>
        /// <param name="repository"></param>
        [ImportingConstructor]
        public Runner(ISerializer serializer, IMsmqConnector msmqConnector, IRepository repository)
        {
            _serializer = serializer;
            _msmqConnector = msmqConnector;
            _repository = repository;
            _client = new CashConnectClient();
            _client.Open();
        }

        #endregion

        #region IRunner

        /// <summary>
        ///     Pools the CASH CONNECT Response Queues for
        ///     messages and process them
        /// </summary>
        public void Run()
        {
            var client = new CashConnectClient();
            client.Open();

            Task.Factory.StartNew(() =>
            {
                this.Log().Info("Pooling the CASH-CONNECT Count Confirmation Queue");

                while (true)
                    try
                    {
                        this.Log().Info("Checking to see if there is any Count Information on the Queue...");
                        MethodResult<MessageEnvelope<ConfirmationMessage>> countData =
                            _msmqConnector.ReadMessage<MessageEnvelope<ConfirmationMessage>>(
                                QueueIdentifier.Confirmation);

                        this.Log()
                            .Info(string.Format("Peeked Information for Bag Serial Number [{0}] on Queue...",
                                countData.EntityResult.MessageObject.BagBarcode));

                        this.Log().Debug(countData.EntityResult.MessageObject);

                        string url = GetEndPointUrl();

                        if (IsConnectionAvailable(url))
                        {
                            this.Log().Info("Connection to Cash Connect established...");

                            this.Log().Info("Trying to send count information to Cash Connect...");

                            SendMessageToWebFlow(countData.EntityResult.MessageObject);

                            this.Log().Info("Serialize And Save Message Xml...");

                            SerializeAndSaveMessageXml(countData.EntityResult.MessageObject);

                            this.Log().Info("removing message from QUEUE...");

                            _msmqConnector.ReceiveMessage<MessageEnvelope<ConfirmationMessage>>(
                                QueueIdentifier.Confirmation);

                            this.Log().Info("removed message from QUEUE...");

                            this.Log().Info("Successfully sent Count Information to CashConnect...");
                        }
                        else
                        {
                            var error =
                                string.Format(
                                    "Failed to send Count Information to '{0}', Failed to establish network connection!",
                                    url);

                            this.Log().Info(error);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log().Fatal("Fatal Exception On Method : [RUN -> CASH_CONNECT_QUEUE_POOL]", ex);
                    }
            });
        }


        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private void SendMessageToWebFlow(ConfirmationMessage message)
        {
            this.Log().Info(string.Format("Sending message for device [{0}] to WebFlow", message.SafeId));
           var response =  _client.CountInfoReceiveAsync(message.SafeId, message.BagBarcode, message.Date, message.Declared10,
                message.Declared20, message.Declared50, message.Declared100, message.Declared200,
                message.DeclaredValue,
                message.Counted10, message.Counted20, message.Declared50, message.Counted100, message.Counted200,
                message.CountedValue);
        }

        #endregion

        #region Internal

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        private void SerializeAndSaveMessageXml(ConfirmationMessage message)
        {
            string messageXml = _serializer.Serialize(message);
            SystemConfiguration pathConfiguration =
                _repository.Query<SystemConfiguration>(a => a.LookUpKey == "RESPONSE_MESSAGE_XML_PATH")
                    .FirstOrDefault();

            SaveMessageToDisk(message.SafeId, messageXml, pathConfiguration);
        }


        /// <summary>
        ///     Save the xml message to dist
        /// </summary>
        /// <param name="deviceSerial"></param>
        /// <param name="messageXml"></param>
        /// <param name="pathConfiguration"></param>
        private void SaveMessageToDisk(string deviceSerial, string messageXml, SystemConfiguration pathConfiguration)
        {
            string archivePath = pathConfiguration.Value;

            if (!Directory.Exists(archivePath)) Directory.CreateDirectory(archivePath);

            string filePath = Path.Combine(archivePath,
                string.Concat(DateTime.Now.Day, " ", DateTime.Now.ToString("MMMM"), " ", DateTime.Now.Year));
            if (!Directory.Exists(filePath)) Directory.CreateDirectory(filePath);

            string newFileName = string.Concat(deviceSerial, "___",
                string.Concat(DateTime.Now.Year, DateTime.Now.Month.ToString("00"),
                    DateTime.Now.Day.ToString("00"),
                    DateTime.Now.Hour.ToString("00"), DateTime.Now.Minute.ToString("00"),
                    DateTime.Now.Second.ToString("00")));
            string newFilePath = Path.Combine(filePath, newFileName);
            File.WriteAllText(
                newFilePath + "___" + string.Concat("VP", Guid.NewGuid().ToString("N").Substring(0, 5)) + ".xml",
                messageXml);
        }


        /// <summary>
        ///     Check if connection to the service is available before
        ///     sending a response
        /// </summary>
        /// <returns></returns>
        private bool IsConnectionAvailable(string url)
        {
            try
            {
                using (var client = new WebClient())
                using (Stream stream = client.OpenRead(url))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///     get base address from config file
        /// </summary>
        /// <returns></returns>
        private string GetEndPointUrl()
        {
            var clientSettings = ConfigurationManager.GetSection("system.serviceModel/client") as ClientSection;

            if (clientSettings != null)
            {
                foreach (ChannelEndpointElement endpoint in clientSettings.Endpoints)
                {
                    if (endpoint.Name == "BasicHttpBinding_ICashConnect")
                    {
                        return endpoint.Address.ToString();
                    }
                }
            }
            return string.Empty;
        }

        #endregion
    }
}