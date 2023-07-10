using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using Application.Modules.Common;
using Domain.Data.Core;
using Domain.Data.Model;
using Domain.Logging;
using Domain.Repository;
using Domain.Serializer;
using Infrastructure.Logging;
using Utility.Core;
using Vault.Integration.DataContracts;

namespace Vault.Integration.Request.Data
{
    [Export(typeof (IRequest))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class Request : IRequest
    {
        #region Fields

        /// <summary>
        /// </summary>
        private readonly ILookup _lookup;

        /// <summary>
        /// </summary>
        private readonly IRepository _repository;

        private readonly ISerializer _serializer;

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="lookup"></param>
        /// <param name="serializer"></param>
        [ImportingConstructor]
        public Request(IRepository repository, ILogger logger, ILookup lookup, ISerializer serializer)
        {
            _lookup = lookup;
            _serializer = serializer;
            _repository = repository;
        }

        #endregion

        /// <summary>
        ///     dumps Xml request to the database without apply validations
        /// </summary>
        /// <param name="request"></param>
        /// <param name="errorResults"></param>
        public void DumpFailedXmlRequest(RequestMessage request, ValidationError[] errorResults)
        {
            try
            {
                VaultTransactionType citTransactionType = _lookup.GetVaultTransactionType("CIT");
                bool isCit = request.TransactionType.Code == citTransactionType.Code.ToString();

                string statusMessage = (isCit)
                    ? "Successully checked if CIT Transaction Code exists..."
                    : "CIT Transaction Code doesn't exist in the database!";

                this.Log().Info(statusMessage);

                // a deposit with zero value can not be logged since its not carrying
                // any meaning to MySbv. 
                // the deposit is kicked out
                // but only allowed if its CIT
                if (request.Currencies.Denominations.TotalValue <= 0 && isCit == false)
                    return;

                // retrieve 
                // convert request to Xml
                string xmlRequest = request.JsonSerializer();
                this.Log().Info("successfully Serialized to JSON...");
                this.Log().Debug(xmlRequest);


                // get Failed validation status
                int statusId = _lookup.GetStatusId("FAILED_VALIDATION");

                statusMessage = (statusId <= 0) ? "not found" : "found";

                this.Log().Info(string.Format("FAILED_VALIDATION status is {0} in database...", statusMessage));

                // All requests are dumped by SBVAdmin
                User user = _repository.Query<User>(a => a.UserName == "SBVAdmin").FirstOrDefault();

                string serialNumber = request.CollectionUnits.CollectionUnit.FirstOrDefault().Value;

                if (IsAnotherCitRun(serialNumber) == false)
                {
                    VaultTransactionXml vault = GetFailedTransaction(serialNumber, request.TransactionDate);
                    this.Log().Info("Queried failed transactions from VaultTransactionXml table...");

                    if (vault != null)
                    {
                        // update fields data
                        vault.StatusId = statusId;
                        vault.XmlAwaitingApproval = null;
                        vault.BagSerialNumber = serialNumber;
                        vault.ApprovedById = null;
                        vault.ErrorMessages = GetErrors(errorResults);
                        vault.ApprovedDate = null;
                        vault.TransactionDate = request.TransactionDate;
                        vault.TransactionTypeCode = request.TransactionType.Code;
                        vault.XmlMessage = xmlRequest;
                        vault.LastChangedDate = DateTime.Now;
                        vault.EntityState = State.Modified;
                        // run updates
                        _repository.Update(vault);
                    }
                    else
                    {
                        // initialize VaultTransactionXml
                        var depositXml = new VaultTransactionXml
                        {
                            StatusId = statusId,
                            BagSerialNumber = serialNumber,
                            ErrorMessages = GetErrors(errorResults),
                            XmlMessage = xmlRequest,
                            TransactionDate = request.TransactionDate,
                            TransactionTypeCode = request.TransactionType.Code,
                            CreatedById = user.UserId,
                            LastChangedById = user.UserId,
                            EntityState = State.Added
                        };

                        // add request to database

                        this.Log().Info("Try to dump deposit xml to validation table!");
                        _repository.Add(depositXml);
                        this.Log().Info("successfully dumped deposit xml to validation table...");
                    }
                }
            }
            catch (Exception ex)
            {
                // log exception on failure to dump request
                this.Log().Fatal("Failed to dump XML to database : [SaveXmlRequest]", ex);
                throw;
            }
        }


        /// <summary>
        ///     Verify if request once failed validation  and was dumped.
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <param name="transactionDate"></param>
        /// <returns></returns>
        private VaultTransactionXml GetFailedTransaction(string serialNumber, DateTime transactionDate)
        {
            // enumerate entries with same bag number
            IEnumerable<VaultTransactionXml> vaultTransactionXmls =
                _repository.Query<VaultTransactionXml>(a => a.BagSerialNumber == serialNumber);

            return
                vaultTransactionXmls.FirstOrDefault(
                    item => item.TransactionDate.ToString(CultureInfo.InvariantCulture) ==
                            transactionDate.ToString(CultureInfo.InvariantCulture));
        }


        /// <summary>
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        private bool IsAnotherCitRun(string serialNumber)
        {
            VaultTransactionType citRequest =
                _repository.Query<VaultTransactionType>(e => e.LookUpKey == "CIT").FirstOrDefault();

            if (citRequest != null)
            {
                // enumerate entries with same bag number
                bool found = _repository.Any<VaultTransactionXml>(a => a.BagSerialNumber == serialNumber &&
                                                                       a.TransactionTypeCode ==
                                                                       citRequest.Code.ToString());

                return found;
            }
            return false;
        }


        /// <summary>
        ///     List Errors as string
        /// </summary>
        /// <param name="errorResults"></param>
        /// <returns></returns>
        private string GetErrors(IEnumerable<ValidationError> errorResults)
        {
            try
            {
                this.Log().Info("Trying to convert Errors to string...");
                var dataHolderList = new List<DataHolder<int>>();
                errorResults.ToList().ForEach(a => dataHolderList.Add(new DataHolder<int>
                {
                    DataString = a.ErrorMessage,
                    Object = (int) a.ErrorCode
                }));

                var data = new DataHolder<List<DataHolder<int>>>
                {
                    Object = dataHolderList
                };
                string message = _serializer.Serialize(data);

                this.Log().Info("Successfully converted Errors to string...");
                return message;
            }
            catch (Exception ex)
            {
                this.Log().Fatal("FAILED TO CONVERT TO STRING, [REQUESTDATA_GETERRORS()]", ex);
                return string.Empty;
            }
        }
    }
}