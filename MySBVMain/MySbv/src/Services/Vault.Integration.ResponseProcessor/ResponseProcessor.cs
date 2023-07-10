using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Xml;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Repository;
using Domain.Serializer;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;

namespace Vault.Integration.ResponseProcessor
{
    public class ResponseProcessor : IResponseProcessor
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly ILookup _lookup;

        /// <summary>
        /// </summary>
        private readonly IRepository _repository;

        /// <summary>
        /// </summary>
        private readonly ISerializer _serializer;

        /// <summary>
        /// </summary>
        private readonly IMsmqConnector _msmqConnector;

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="lookup"></param>
        /// <param name="serializer"></param>
        /// <param name="msmqConnector"></param>
        [ImportingConstructor]
        public ResponseProcessor(IRepository repository, ILookup lookup, ISerializer serializer, IMsmqConnector msmqConnector)
        {
            _repository = repository;
            _lookup = lookup;
            _serializer = serializer;
            _msmqConnector = msmqConnector;
        }

        #endregion

        #region IResponse Processor Implementation

        /// <summary>
        /// Prepare XML Document and return a Serialized Object
        /// </summary>
        /// <returns></returns>
        public MethodResult<ConfirmationMessage> SubmitResponse(ConfirmationMessage confirmationMessage)
        {
            try
            {
                if (confirmationMessage != null && confirmationMessage.SafeId != null)
                {
                    if (RequiresResponse(confirmationMessage.SafeId))
                    {
                        //SAVE XML FILE
                        var doc = new XmlDocument();
                        var xmlReturnString = XmlProcessDataConverter(confirmationMessage);
                        doc.LoadXml(xmlReturnString.Substring(xmlReturnString.IndexOf(Environment.NewLine, System.StringComparison.Ordinal)));

                        doc.PreserveWhitespace = true;
                        //SAVE THE XML INTO PATH - FileName will be the Serial Number
                        doc.Save(_lookup.GetResponseMessageXMLPath() + confirmationMessage.SafeId + ".xml");

                        return new MethodResult<ConfirmationMessage>(MethodStatus.Successful, confirmationMessage);
                    }

                    return new MethodResult<ConfirmationMessage>(MethodStatus.Error, null);
                }
            }

            catch (Exception ex)
            {
                this.Log().Fatal("Exception On Method : [SUBMIT_CONFIRMATION] --> Generate Response XML Message", ex);
                throw;
            }

            return new MethodResult<ConfirmationMessage>(MethodStatus.Error, null);
        }


        /// <summary>
        ///     Serialize Response into XML
        /// </summary>
        /// <param name="deposits"></param>
        /// <returns></returns>
        public string XmlProcessDataConverter(ConfirmationMessage deposits)
        {
            return _serializer.Serialize(deposits);
        }


        /// <summary>
        ///     Check the Supplier that needs Response Messages - (Cash Connect)
        /// </summary>
        /// <param name="safeId"></param>
        /// <returns></returns>
        public bool RequiresResponse(string safeId)
        {
            int supplierId = _lookup.GetSupplierId("CASH_CONNECT");

            return
                _repository.Any<Device>(
                    a => a.SerialNumber == safeId && a.SupplierId == supplierId);
        }

        #endregion
    }
}