using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using Application.Dto.FailedVaultTransaction;
using Application.Dto.Vault;
using Application.Mapper;
using Application.Modules.Common;
using Application.Modules.XmlManipulation;
using Domain.Data.Core;
using Domain.Repository;
using Domain.Serializer;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.Msmq.Connector;
using model = Domain.Data.Model;
using contract = Vault.Integration.DataContracts.Contracts;

namespace Application.Modules.FailedVaultRequest
{
    public class VaultRequestValidation : IVaultRequestValidation
    {
        #region Fields

        private readonly ILookup _lookup;
        private readonly IMapper _mapper;
        private readonly IMsmqConnector _msmqConnector;
        private readonly IRepository _repository;
        private readonly ISerializer _serializer;
        private readonly IXmlFile _xml;

        #endregion

        #region Constructor

        public VaultRequestValidation(IRepository repository, IMapper mapper, IMsmqConnector msmqConnector,
            ILookup lookup, ISerializer serializer, IXmlFile xml)
        {
            _repository = repository;
            _mapper = mapper;
            _msmqConnector = msmqConnector;
            _lookup = lookup;
            _serializer = serializer;
            _xml = xml;
        }

        #endregion

        #region FailedRequestValidation services

        /// <summary>
        ///     Retrieves all Vault Transactions with Failed or Pending Status
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FailedRequestListDto> GetFailedRequests(string serialNumber)
        {
            // get statuses
            model.Status failedStatus = _lookup.GetStatus("FAILED_VALIDATION");
            model.Status pendingStatus = _lookup.GetStatus("PENDING");

            // get all failed vault Transactions
            List<model.VaultTransactionXml> requests = _repository.Query<model.VaultTransactionXml>
                (
                    o => o.BagSerialNumber.ToLower() == serialNumber.ToLower() &&
                         (o.StatusId == failedStatus.StatusId || o.StatusId == pendingStatus.StatusId)
                ).ToList();

            // get a list of failed requests
            List<VaultTransactionXmlDto> mappedFailedRequests =
                requests.Select(request => _mapper.Map<model.VaultTransactionXml, VaultTransactionXmlDto>(request))
                    .ToList();

            var requestsList = new List<FailedRequestListDto>();

            foreach (VaultTransactionXmlDto requestDto in mappedFailedRequests)
            {
                string xml = requestDto.XmlAwaitingApproval ?? requestDto.XmlMessage;
                model.Status status = _lookup.GetStatus(requestDto.StatusId);
                FailedRequestListDto requestMessage = BuildRequest(xml, requestDto, status.Name);
                requestsList.Add(requestMessage);
            }

            return requestsList;
        }

        /// <summary>
        ///     Retrieves all Vault Transactions with Failed or Pending Status
        /// </summary>
        /// <returns></returns>
        public IEnumerable<FailedTransactionHeaderDto> GetFailedHeaders()
        {
            // get statuses
            model.Status failedStatus = _lookup.GetStatus("FAILED_VALIDATION");
            model.Status pendingStatus = _lookup.GetStatus("PENDING");

            // get all failed vault Transactions
            List<model.VaultTransactionXml> requests =
                _repository.Query<model.VaultTransactionXml>(
                    o => o.StatusId == failedStatus.StatusId || o.StatusId == pendingStatus.StatusId).ToList();

            // get a list of failed requests
            List<VaultTransactionXmlDto> mappedFailedRequests =
                requests.Select(request => _mapper.Map<model.VaultTransactionXml, VaultTransactionXmlDto>(request))
                    .ToList();

            var headers = new List<FailedTransactionHeaderDto>();

            foreach (VaultTransactionXmlDto requestDto in mappedFailedRequests)
            {
                string xml = requestDto.XmlAwaitingApproval ?? requestDto.XmlMessage;

                FailedTransactionHeaderDto requestMessage = CreateHeader(xml, requestDto);

                bool hasBeenAdded = headers.Any(o => o.SerialNumber.ToUpper() == requestMessage.SerialNumber.ToUpper());
                if (hasBeenAdded == false)
                {
                    headers.Add(requestMessage);
                }
                else
                {
                    // if it was added before, check if message is cit.
                    // remove the previous one with cit message.
                    // please see that cit stands as the header message.
                    if (!requestMessage.IsCitMessage) continue;

                    FailedTransactionHeaderDto addedMessage =
                        headers.FirstOrDefault(e => e.SerialNumber == requestMessage.SerialNumber);
                    headers.Remove(addedMessage);
                    headers.Add(requestMessage);
                }
            }

            return headers;
        }

        /// <summary>
        ///     Approve Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<bool> Approve(FailedRequestDto request, model.User user)
        {
            try
            {
                // get the Failed Request from database
                var vault = _repository.Find<model.VaultTransactionXml>(request.Id);

                model.Status status = _lookup.GetStatus("VALIDATION_UPDATE");

                using (var scope = new TransactionScope())
                {
                    DateTime date = DateTime.Now;
                    vault.XmlMessage = vault.XmlAwaitingApproval;
                    vault.XmlAwaitingApproval = null;
                    vault.ApprovedById = user.UserId;
                    vault.LastChangedById = user.UserId;
                    vault.ApprovedDate = date;
                    vault.LastChangedDate = date;
                    vault.StatusId = status.StatusId;
                    vault.EntityState = State.Modified;

                    // save changes
                    _repository.Update(vault);

                    // Relay Request to Queue
                    AddVaultTransactionToQueue(request.RequestMessage);

                    // Commit Transaction
                    scope.Complete();
                }
                return new MethodResult<bool>(MethodStatus.Successful, true, UpdateMessages.Approved);
            }
            catch (Exception)
            {
                return new MethodResult<bool>(MethodStatus.Error, false, UpdateMessages.FailedApproval);
            }
        }


        /// <summary>
        ///     Approve Request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<bool> Decline(FailedRequestDto request, model.User user)
        {
            try
            {
                // get the Failed Request from database
                var vault = _repository.Find<model.VaultTransactionXml>(request.Id);

                model.Status status = _lookup.GetStatus("DECLINED");

                using (var scope = new TransactionScope())
                {
                    DateTime date = DateTime.Now;
                    vault.XmlMessage = vault.XmlAwaitingApproval;
                    vault.XmlAwaitingApproval = null;
                    vault.ApprovedById = null;
                    vault.LastChangedById = user.UserId;
                    vault.ApprovedDate = null;
                    vault.LastChangedDate = date;
                    vault.StatusId = status.StatusId;
                    vault.EntityState = State.Modified;

                    // save changes
                    _repository.Update(vault);

                    // Commit Transaction
                    scope.Complete();
                }
                return new MethodResult<bool>(MethodStatus.Successful, true, UpdateMessages.Declined);
            }
            catch (Exception)
            {
                return new MethodResult<bool>(MethodStatus.Error, false, UpdateMessages.Error);
            }
        }

        /// <summary>
        ///     Updates a Vault Transaction
        /// </summary>
        /// <param name="request"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public MethodResult<bool> Edit(FailedRequestDto request, model.User user)
        {
            try
            {
                // get the Failed Request from database
                var vaultTransactionXml = _repository.Find<model.VaultTransactionXml>(request.Id);

                // XML strings to compare
                string dbXml = ConvertToRequestMessage(vaultTransactionXml.XmlMessage).ConvertToXml();
                string editedXml = request.RequestMessage.ConvertToXml();

                // check if any updates were done
                List<XmlComparisonResult> diffences = _xml.Compare(dbXml, editedXml);

                var requestMessage = _serializer.Deserialize<RequestMessage>(editedXml);

                short code = Convert.ToInt16(requestMessage.TransactionType.Code);
                model.VaultTransactionType transactionType =
                    _repository.Query<model.VaultTransactionType>(e => e.Code == code).FirstOrDefault();

                int linkedTransactionsCount = 0;
                model.Status updatedStatus = _lookup.GetStatus("VALIDATION_UPDATE");

                foreach (contract.CollectionUnit container in requestMessage.CollectionUnits.CollectionUnit)
                {
                    bool hasMany = _repository.Any<model.VaultTransactionXml>
                        (
                            e => e.BagSerialNumber == container.Value &&
                                 e.StatusId != updatedStatus.StatusId &&
                                 e.TransactionTypeCode != requestMessage.TransactionType.Code
                        );

                    if (hasMany)
                    {
                        linkedTransactionsCount++;
                    }
                }

                if (transactionType.LookUpKey == "CIT" && linkedTransactionsCount > 0)
                {
                    return new MethodResult<bool>(MethodStatus.Error, false,
                        UpdateMessages.CantReleaseCitWithTransactions);
                }

                // check if Message is Cit and has no errors on it.
                // if no deposits/payments are linked to it allow it to be queued.
                if ((transactionType.LookUpKey == "CIT" && diffences.Any() == false && linkedTransactionsCount == 0) ||
                    diffences.Any())
                {
                    // set values
                    model.Status status = _lookup.GetStatus("PENDING");
                    vaultTransactionXml.XmlAwaitingApproval = requestMessage.JsonSerializer();
                    vaultTransactionXml.LastChangedById = user.UserId;
                    vaultTransactionXml.EntityState = State.Modified;
                    vaultTransactionXml.StatusId = status.StatusId;
                    vaultTransactionXml.LastChangedDate = DateTime.Now;

                    using (var scope = new TransactionScope())
                    {
                        // save changes
                        _repository.Update(vaultTransactionXml);

                        // Run Audits
                        SaveAudit(diffences, request.Id, user);

                        // Commit Transaction
                        scope.Complete();
                    }
                    return new MethodResult<bool>(MethodStatus.Successful, true, UpdateMessages.Success);
                }

                // No updates 
                return new MethodResult<bool>(MethodStatus.Error, false, UpdateMessages.NoChanges);
            }
            catch (Exception e)
            {
                return new MethodResult<bool>(MethodStatus.Error, false, UpdateMessages.Error + e.Message);
            }
        }

        /// <summary>
        ///     Finds a single Vault Transaction by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MethodResult<FailedRequestDto> Find(int id)
        {
            // get statuses
            model.Status failedStatus = _lookup.GetStatus("FAILED_VALIDATION");
            model.Status pendingStatus = _lookup.GetStatus("PENDING");

            // get the Failed Request from database
            var request = _repository.Find<model.VaultTransactionXml>(id);

            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (request == null)
            {
                return new MethodResult<FailedRequestDto>(MethodStatus.NotFound, null,
                    string.Format(
                        "The record identifer [id = {0}] is not valid. Please do not manually enter values on the Url!",
                        id));
            }

            bool canBeProcessed = (request.StatusId == failedStatus.StatusId ||
                                   request.StatusId == pendingStatus.StatusId);
            if (canBeProcessed == false)
            {
                model.Status statusName = _lookup.GetStatus(request.StatusId);
                return new MethodResult<FailedRequestDto>(MethodStatus.Warning, null,
                    string.Format("A record with [status = '{0}'] cannot be Edited!", statusName.Name));
            }

            // map request response objects
            VaultTransactionXmlDto mapped = _mapper.Map<model.VaultTransactionXml, VaultTransactionXmlDto>(request);
            model.Status status = _lookup.GetStatus(mapped.StatusId);

            string json = mapped.XmlAwaitingApproval ?? mapped.XmlMessage;

            var messagesCollection = _serializer.Deserialize<DataHolder<List<DataHolder<int>>>>(mapped.ErrorMessages);
            var requestMessage = json.JsonDeserializer<RequestMessage>();

            // build RequestMeassage from XML
            var failedRequest = new FailedRequestDto
            {
                Id = mapped.VaultTransactionXmlId,
                UserId = mapped.LastChangedById,
                IsPending = status.LookUpKey == "PENDING",
                RequestMessage = requestMessage,
                Currencies = GetCurrencies(),
                SelectedVaultTransactionType = "",
                VaultTransactionTypes = GetVaultTransactionTypes(),
                ErrorMessages = messagesCollection.Object.Select(a => a.DataString).ToList(),
                IsGptRequest = IsGpt(requestMessage)
            };

            AssignErrorsEncounter(failedRequest);

            short code = Convert.ToInt16(failedRequest.RequestMessage.TransactionType.Code);
            model.VaultTransactionType type =
                _repository.Query<model.VaultTransactionType>(o => o.Code == code).FirstOrDefault();
            if (type != null)
            {
                failedRequest.SelectedVaultTransactionType = type.Description;
                failedRequest.IsCitRequest = type.LookUpKey == "CIT";
            }

            return new MethodResult<FailedRequestDto>(MethodStatus.Successful, failedRequest);
        }

        /// <summary>
        ///     Validate updates
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public MethodResult<List<MessageError>> ValidationMessage(RequestMessage request)
        {
            // rerturn messages;
            var messages = new List<MessageError>();

            bool beneficiaryIsValid = IsValidationBeneficiaryCode(request);
            bool citCodeIsValid = IsValidationCitCode(request.CitCode);
            bool deviceNumberIsValid = IsValidationDeviceSerial(request.DeviceSerial);

            if (beneficiaryIsValid == false)
            {
                messages.Add(new MessageError {Error = "Beneficiary Code was not found."});
            }

            if (citCodeIsValid == false)
            {
                messages.Add(new MessageError {Error = "Cit Code was not found."});
            }

            if (deviceNumberIsValid == false)
            {
                messages.Add(new MessageError {Error = "Device Serial was not found."});
            }

            return beneficiaryIsValid && citCodeIsValid && deviceNumberIsValid
                ? new MethodResult<List<MessageError>>(MethodStatus.Successful, messages)
                : new MethodResult<List<MessageError>>(MethodStatus.Error, messages);
        }

        /// <summary>
        ///     creates a Cit Header List
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        private FailedTransactionHeaderDto CreateHeader(string jsonString, VaultTransactionXmlDto request)
        {
            var requestMessage = jsonString.JsonDeserializer<RequestMessage>();

            if (requestMessage == null) return null;

            short code = Convert.ToInt16(requestMessage.TransactionType.Code);
            model.VaultTransactionType cit =
                _repository.Query<model.VaultTransactionType>(e => e.Code == code).FirstOrDefault();

            bool isCitMessage = (cit != null) && cit.LookUpKey == "CIT";

            return new FailedTransactionHeaderDto
            {
                Id = request.VaultTransactionXmlId,
                BeneficiaryCode = requestMessage.BeneficiaryCode,
                CitCode = requestMessage.CitCode,
                IsCitMessage = isCitMessage,
                SerialNumber = request.BagSerialNumber,
                CitReceivedStatus = isCitMessage ? "Received" : "Not Received",
                Supplier = GetDeviceSupplier(requestMessage.DeviceSerial)
            };
        }


        /// <summary>
        ///     Check if Gpt deposit
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsGpt(RequestMessage request)
        {
            model.Device device =
                _repository.Query<model.Device>(
                    a => a.SerialNumber.Trim().ToLower() == request.DeviceSerial.Trim().ToLower(),
                    o => o.DeviceType, o => o.DeviceType.Supplier).FirstOrDefault();
            if (device != null)
            {
                model.Supplier supplier = device.DeviceType.Supplier;
                return supplier.LookUpKey == "GPT";
            }
            return false;
        }


        /// <summary>
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        private RequestMessage ConvertToRequestMessage(string json)
        {
            // To convert JSON text contained in string json into an XML node
            return json.JsonDeserializer<RequestMessage>();
            //var doc = JsonConvert.DeserializeXmlNode(json);
        }

        private void AssignErrorsEncounter(FailedRequestDto failedRequest)
        {
            if (IsValidationBeneficiaryCode(failedRequest.RequestMessage) == false)
            {
                failedRequest.HasErrorOnBeneficiaryCode = true;
            }

            if (IsValidationCitCode(failedRequest.RequestMessage.CitCode) == false)
            {
                failedRequest.HasErrorOnCitCode = true;
            }

            if (IsValidationDeviceSerial(failedRequest.RequestMessage.DeviceSerial) == false)
            {
                failedRequest.HasErrorOnDeviceSerialNumber = true;
            }

            if (IsValidationBagNumber(failedRequest.RequestMessage.CollectionUnits) == false)
            {
                failedRequest.HasErrorOnBagSerialNumber = true;
            }

            if (string.IsNullOrWhiteSpace(failedRequest.RequestMessage.UserReferance))
            {
                failedRequest.HasErrorOnUserReferance = true;
            }

            if (IsValidCurrencyCode(failedRequest.RequestMessage.Currencies.Denominations.CurrencyCode) == false)
            {
                failedRequest.HasErrorOnCurrencyCode = true;
            }

            if (IsValidTransactionType(failedRequest.RequestMessage.TransactionType.Code) == false)
            {
                failedRequest.HasErrorOnTransactionType = true;
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Log all updates to the Audit table
        /// </summary>
        /// <param name="comparisons"></param>
        /// <param name="xmlId"></param>
        /// <param name="user"></param>
        private void SaveAudit(IEnumerable<XmlComparisonResult> comparisons, int xmlId, model.User user)
        {
            try
            {
                DateTime date = DateTime.Now;
                foreach (XmlComparisonResult comparisonResult in comparisons)
                {
                    var vaultAuditLog = new model.VaultAuditLog
                    {
                        OldValue = comparisonResult.OldValue,
                        NewValue = comparisonResult.NewValue,
                        TableName = typeof (model.VaultAuditLog).Name,
                        ColumnName = comparisonResult.NodeName,
                        VaultTransactionXmlId = xmlId,
                        LastChangedById = user.UserId,
                        CreatedById = user.UserId,
                        CreateDate = date,
                        LastChangedDate = date,
                        EntityState = State.Added
                    };
                    _repository.Add(vaultAuditLog);
                }
            }
            catch (Exception)
            {
                throw new Exception("Failed on Vault Tarnsaction Auditing!");
            }
        }


        /// <summary>
        ///     creates a FailedRequestList item
        /// </summary>
        /// <param name="xmlString"></param>
        /// <param name="request"></param>
        /// <param name="statusName"></param>
        /// <returns></returns>
        private FailedRequestListDto BuildRequest(string xmlString, VaultTransactionXmlDto request, string statusName)
        {
            var requestMessage = xmlString.JsonDeserializer<RequestMessage>();
            ;
            if (requestMessage == null) return null;

            short code = Convert.ToInt16(requestMessage.TransactionType.Code);

            model.VaultTransactionType transactionType =
                _repository.Query<model.VaultTransactionType>(o => o.Code == code).FirstOrDefault();


            return new FailedRequestListDto
            {
                FailedRequestId = request.VaultTransactionXmlId,
                Id = request.VaultTransactionXmlId,
                CitCode = requestMessage.CitCode,
                BeneficiaryCode = requestMessage.BeneficiaryCode,
                BagSerialNumber = request.BagSerialNumber,
                StatusName = statusName,
                TransactionType = transactionType != null ? transactionType.Description : "Unknown",
                Errors = request.ErrorMessages,
                TotalDepositAmount = string.Format("{0:0.00}", requestMessage.Currencies.Denominations.TotalValue),
                Supplier = GetDeviceSupplier(requestMessage.DeviceSerial)
            };
        }


        /// <summary>
        ///     Enqueues the vault transaction to normal flow of events
        /// </summary>
        /// <param name="request"></param>
        private void AddVaultTransactionToQueue(RequestMessage request)
        {
            try
            {
                // submit request to the vault service
                // if validation passes, relay to queue
                string label = request.CollectionUnits.CollectionUnit.FirstOrDefault().Value;
                _msmqConnector.AddMessage(QueueIdentifier.FailedTransactions,
                    new MessageEnvelope<RequestMessage> {MessageObject = request, Label = label});
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to add a Transaction to the Queue.", ex);
            }
        }


        /// <summary>
        ///     Validate Beneficiary Code
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        private bool IsValidationBeneficiaryCode(RequestMessage requestMessage)
        {
            short code = Convert.ToInt16(requestMessage.TransactionType.Code);
            model.VaultTransactionType cit =
                _repository.Query<model.VaultTransactionType>(e => e.Code == code).FirstOrDefault();

            bool isCitMessage = (cit != null) && cit.LookUpKey == "CIT";
            // get all sites with CitCode
            IEnumerable<model.Account> accounts =
                _repository.Query<model.Account>(
                    o => o.BeneficiaryCode.ToLower() == requestMessage.BeneficiaryCode.ToLower());

            // return if any
            return (accounts.Any() || (string.IsNullOrWhiteSpace(requestMessage.BeneficiaryCode) && isCitMessage));
        }


        /// <summary>
        /// </summary>
        /// <param name="transactionTypeCode"></param>
        /// <returns></returns>
        private bool IsValidTransactionType(string transactionTypeCode)
        {
            short code = Convert.ToInt16(transactionTypeCode);
            return _repository.Any<model.VaultTransactionType>(e => e.Code == code);
        }

        /// <summary>
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns></returns>
        private bool IsValidCurrencyCode(string currencyCode)
        {
            return _repository.Any<model.Currency>(e => e.Code == currencyCode);
        }

        /// <summary>
        ///     Validate CitCode
        /// </summary>
        /// <param name="citCode"></param>
        /// <returns></returns>
        private bool IsValidationCitCode(string citCode)
        {
            // get all sites with CitCode
            return _repository.Query<model.Site>(o => o.CitCode.ToLower() == citCode.ToLower()).Any();
        }

        /// <summary>
        ///     Validate Device Serial Number
        /// </summary>
        /// <param name="deviceSerial"></param>
        /// <returns></returns>
        private bool IsValidationDeviceSerial(string deviceSerial)
        {
            // get all devices with deviceSerial
            return _repository.Query<model.Device>(o => o.SerialNumber.ToLower() == deviceSerial.ToLower()).Any();
        }

        /// <summary>
        ///     Validate bag number
        /// </summary>
        /// <param name="bagNumbers"></param>
        /// <returns></returns>
        private bool IsValidationBagNumber(contract.CollectionUnits bagNumbers)
        {
            const bool isValid = true;

            if (bagNumbers.CollectionUnit.Any
                (bagNumber => bagNumber.Value.Length != 14 && IsNumeric(bagNumber.Value)))
            {
                return false;
            }
            return isValid;
        }

        /// <summary>
        ///     check numberic
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumeric(string text)
        {
            try
            {
                double numeric = double.Parse(text);
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        ///     get devive owner/ supplier e.g. GPT or Cas Connect
        /// </summary>
        /// <param name="deviceSerial"></param>
        /// <returns></returns>
        private string GetDeviceSupplier(string deviceSerial)
        {
            model.Device result =
                _repository.Query<model.Device>(e => e.SerialNumber == deviceSerial, e => e.DeviceType,
                    e => e.DeviceType.Supplier).FirstOrDefault();

            return result != null ? result.DeviceType.Supplier.Name : "Unknown";
        }


        /// <summary>
        ///     List Vault Transaction types
        /// </summary>
        /// <returns></returns>
        private List<string> GetVaultTransactionTypes()
        {
            List<model.VaultTransactionType> transactions = _repository.All<model.VaultTransactionType>().ToList();

            IEnumerable<string> listOfVaultTypes =
                transactions.Select(
                    currency => _mapper.Map<model.VaultTransactionType, VaultTransactionTypeDto>(currency).Description);
            return listOfVaultTypes.Distinct().ToList();
        }

        /// <summary>
        ///     list Currencies
        /// </summary>
        /// <returns></returns>
        private List<string> GetCurrencies()
        {
            List<model.Currency> currencies = _repository.All<model.Currency>().ToList();

            IEnumerable<string> listOfCurrencies =
                currencies.Select(currency => _mapper.Map<model.Currency, CurrencyDto>(currency).Code);
            return listOfCurrencies.Distinct().ToList();
        }

        #endregion
    }
}