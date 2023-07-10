using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Transactions;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Repository;
using Infrastructure.Logging;
using Vault.Integration.DataContracts;
using Vault.Integration.DataContracts.Contracts;
using Vault.Integration.MessageController;
using Vault.Integration.MessageValidator;
using Vault.Integration.Request.Data;

namespace Vault.Integration.FailedTransactions.MessageProcessor
{
    /// <summary>
    /// </summary>
    [Export(typeof (IFailedMessageProcessorValidation))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class FailedMessageProcessorValidation : IFailedMessageProcessorValidation
    {
        #region Fields

        private readonly IRequest _forceFailureController;
        private readonly ILookup _lookup;
        private readonly IMessageController _messageController;
        private readonly IRepository _repository;
        private readonly IMessageValidator _validator;

        #endregion

        #region Constructor

        /// <summary>
        /// </summary>
        /// <param name="messageController"></param>
        /// <param name="forceFailureController"></param>
        /// <param name="lookup"></param>
        /// <param name="repository"></param>
        /// <param name="validator"></param>
        [ImportingConstructor]
        public FailedMessageProcessorValidation(
            IMessageController messageController,
            IRequest forceFailureController,
            ILookup lookup,
            IRepository repository,
            IMessageValidator validator)
        {
            _messageController = messageController;
            _forceFailureController = forceFailureController;
            _lookup = lookup;
            _repository = repository;
            _validator = validator;
        }

        #endregion

        #region Implementation

        /// <summary>
        ///     Process a failed transaction to the database
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool IsProcessed(RequestMessage message)
        {
            try
            {
                using (var scope = new TransactionScope())
                {
                    var validationResults = new ValidationError[] {};
                    this.Log()
                        .Debug(() => { validationResults = GetValidationResults(message).ToArray(); }, "Running Validations");

                    RequestMessage requestMessage = StructureRequestMessage(message);

                    if (validationResults.Any())
                    {
                        // reject request
                        RejectRequest(requestMessage, validationResults);
                        // remove from queue the message has been logged back to databasae
                        //if (isRejected)
                        //{
                        //    _msmqConnector.ReceiveMessage<MessageEnvelope<RequestMessage>>(
                        //        QueueIdentifier.FailedTransactions);
                        //}
                    }
                    else
                    {
                        // The message has no errors
                        // Proceed log it to the deposits queue

                        // Relay Message to MSMQ
                        this.Log()
                            .Debug(() => { _messageController.RelayMessage(requestMessage); }, "Relaying deposit message");
                    }

                    // commit changes
                    scope.Complete();

                    return true;
                }
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal(string.Format("Exception On Method : [PROCESS_FAILED_TRANSACTION]\n[{0}]\nStacktrace\n", ex.Message), ex);

                return false;
            }
        }

        #endregion

        #region Helpers

        /// <summary>
        ///     Forces a message to fail on failing validation
        /// </summary>
        /// <param name="message"></param>
        /// <param name="validationResults"></param>
        /// <returns></returns>
        private void RejectRequest(RequestMessage message, ValidationError[] validationResults)
        {
            // Dumping Failed XML Request to Database
            this.Log().Debug(() => { _forceFailureController.DumpFailedXmlRequest(message, validationResults); },
                "Dumping Failed XML Request to Database");
        }


        /// <summary>
        ///     Validate the incoming deposit XML message
        /// </summary>
        /// <param name="cashDeposit"></param>
        /// <returns></returns>
        private IEnumerable<ValidationError> GetValidationResults(RequestMessage cashDeposit)
        {
            // check if Beneficiary Code is Empty.
            // if its GPT device and not a Payment message,
            // allow it to pass validation.
            if (string.IsNullOrWhiteSpace(cashDeposit.BeneficiaryCode))
            {
                cashDeposit = ResolveGptEmptyBeneficiaryCode(cashDeposit);
            }

            var results = new List<ValidationError>();

            this.Log().Debug(() =>
            {
                ValidationResult validation = _validator.ValidateCitCode(cashDeposit.CitCode);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating Site CIT Code");

            this.Log().Debug(() =>
            {
                ValidationResult validation =
                    _validator.ValidateBagNumber(cashDeposit.CollectionUnits.CollectionUnit.FirstOrDefault().Value);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating Bag Number");


            this.Log().Debug(() =>
            {
                ValidationResult validation = _validator.ValidateCurrencyCode(cashDeposit);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating CurrencyCode");

            this.Log().Debug(() =>
            {
                ValidationResult validation = _validator.ValidateGptPartialPayment(cashDeposit);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating GPT Partial Payments");

            VaultTransactionType paymenTransactionType = _lookup.GetVaultTransactionType("PAYMENT");

            if (cashDeposit.TransactionType.Code == paymenTransactionType.Code.ToString(CultureInfo.InvariantCulture))
            {
                this.Log().Debug(() =>
                {
                    ValidationResult validation = _validator.ValidatePayment(cashDeposit);
                    if (!validation.Result)
                        results.Add(new ValidationError
                        {
                            ErrorCode = validation.ValidationError.ErrorCode,
                            ErrorMessage = validation.ValidationError.ErrorMessage
                        });
                }, "Validating Payment");
            }

            this.Log().Debug(() =>
            {
                ValidationResult validation = _validator.ValidateIsBagOpened(cashDeposit);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating if CIT Message was sent on the BagNumber before");

            VaultTransactionType depositTransactionType = _lookup.GetVaultTransactionType("DEPOSIT");

            if (cashDeposit.TransactionType.Code == depositTransactionType.Code.ToString(CultureInfo.InvariantCulture))
            {
                this.Log().Debug(() =>
                {
                    ValidationResult validation = _validator.ValidateBeneficiary(cashDeposit.BeneficiaryCode);
                    if (!validation.Result)
                        results.Add(new ValidationError
                        {
                            ErrorCode = validation.ValidationError.ErrorCode,
                            ErrorMessage = validation.ValidationError.ErrorMessage
                        });
                }, "Validating Beneficiary");
            }

            // repository
            bool isValidTransactionCode = true;

            // validate transaction type code
            this.Log().Debug(() =>
            {
                short code = Convert.ToInt16(cashDeposit.TransactionType.Code);

                if (!_repository.Any<VaultTransactionType>(a => a.Code == code))
                {
                    isValidTransactionCode = false;

                    results.Add(new ValidationError
                    {
                        ErrorCode = HttpStatusCode.NotFound,
                        ErrorMessage =
                            string.Format("Transaction type [{0}] does not exist on MySbv.",
                                cashDeposit.TransactionType.Code)
                    });
                }
            }, "Validating Transaction type code");


            // check if is valid transaction code, and not a deposit
            // and check if CIT was run
            if (isValidTransactionCode &&
                (cashDeposit.TransactionType.Code !=
                 depositTransactionType.Code.ToString(CultureInfo.InstalledUICulture)))
            {
                this.Log().Debug(() =>
                {
                    foreach (CollectionUnit collectionUnit in cashDeposit.CollectionUnits.CollectionUnit)
                    {
                        // First check if the deposit is in the DB
                        if (!_repository.Any<Container>(a => a.SerialNumber.ToLower() == collectionUnit.Value.ToLower()))
                        {
                            // If deposit is not in the DB, check if its in the queue
                            string label = string.Concat(depositTransactionType.Code, "_", collectionUnit.Value);

                            if (!_messageController.IsDepositsInQueue(cashDeposit, label))
                            {
                                // If the deposit is not in the queue then it does not exist in our environment

                                results.Add(new ValidationError
                                {
                                    ErrorCode = HttpStatusCode.NotFound,
                                    ErrorMessage =
                                        string.Format(
                                            "A Bag with serial Number [{0}] has to exist on MySbv before you run CIT Operation",
                                            collectionUnit.Value)
                                });
                            }
                        }
                    }
                }, "Validate Payment & CIT");
            }


            this.Log().Debug(() =>
            {
                ValidationResult validation = _validator.ValidateDevice(cashDeposit.DeviceSerial);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating Device Serial Numbers");


            this.Log().Debug(() =>
            {
                List<ValidationResult> validations = _validator.ValidateXml(cashDeposit);
                validations.ForEach(validation =>
                {
                    if (!validation.Result)
                        results.Add(new ValidationError
                        {
                            ErrorCode = validation.ValidationError.ErrorCode,
                            ErrorMessage = validation.ValidationError.ErrorMessage
                        });
                });
            }, "Validating/Checking XML Totals");


            this.Log().Debug(() =>
            {
                ValidationResult validation = _validator.ValidateTotals(cashDeposit);
                if (!validation.Result)
                    results.Add(new ValidationError
                    {
                        ErrorCode = validation.ValidationError.ErrorCode,
                        ErrorMessage = validation.ValidationError.ErrorMessage
                    });
            }, "Validating Actual Totals");


            VaultTransactionType citTransactionType = _lookup.GetVaultTransactionType("CIT");

            if (cashDeposit.TransactionType.Code == citTransactionType.Code.ToString(CultureInfo.InvariantCulture))
            {
                bool isCitAlreadyReceived = false;
                Status failedStatus =
                    _repository.Query<Status>(e => e.LookUpKey == "FAILED_VALIDATION").FirstOrDefault();
                Status pendingStatus = _repository.Query<Status>(e => e.LookUpKey == "PENDING").FirstOrDefault();

                this.Log().Debug(() =>
                {
                    foreach (CollectionUnit collectionUnit in cashDeposit.CollectionUnits.CollectionUnit)
                    {
                        bool hasFailedXmls = _repository.Any<VaultTransactionXml>(
                            a => a.BagSerialNumber == collectionUnit.Value &&
                                 (a.StatusId == failedStatus.StatusId || a.StatusId == pendingStatus.StatusId) &&
                                 a.TransactionTypeCode == citTransactionType.Code.ToString());

                        if (hasFailedXmls)
                        {
                            isCitAlreadyReceived = true;
                            results.Add(new ValidationError
                            {
                                ErrorCode = HttpStatusCode.Conflict,
                                ErrorMessage =
                                    string.Format("CIT already received for bag with serial number [{0}].",
                                        collectionUnit.Value)
                            });
                        }
                    }
                }, "Validate CIT For Rejected Deposits");

                if (!isCitAlreadyReceived)
                {
                    this.Log().Debug(() =>
                    {
                        foreach (CollectionUnit collectionUnit in cashDeposit.CollectionUnits.CollectionUnit)
                        {
                            // Cit cannot run when both FAILED and PENDING failed transactions
                            // are not fixed.
                            if (_repository.Any<VaultTransactionXml>(a => a.BagSerialNumber == collectionUnit.Value &&
                                                                          (a.StatusId == failedStatus.StatusId ||
                                                                           a.StatusId == pendingStatus.StatusId)))
                            {
                                results.Add(new ValidationError
                                {
                                    ErrorCode = HttpStatusCode.Moved,
                                    ErrorMessage =
                                        string.Format(
                                            "CIT cannot run, fix failed transactions with bag serial number [{0}] first!",
                                            collectionUnit.Value)
                                });
                            }
                        }
                    }, "Validate CIT For Rejected Deposits");
                }
            }

            return results;
        }

        /// <summary>
        ///     Resolve GPT Empty Beneficiary
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private RequestMessage ResolveGptEmptyBeneficiaryCode(RequestMessage request)
        {
            if (IsGpt(request))
            {
                if (string.IsNullOrWhiteSpace(request.BeneficiaryCode))
                {
                    Site site =
                        _repository.Query<Site>(o => o.CitCode.ToUpper() == request.CitCode.ToUpper()).FirstOrDefault();
                    if (site != null)
                    {
                        VaultTransactionType cit = _lookup.GetVaultTransactionType("CIT");
                        VaultTransactionType deposit = _lookup.GetVaultTransactionType("DEPOSIT");

                        Account account =
                            _repository.Query<Account>(o => o.SiteId == site.SiteId && o.DefaultAccount)
                                .FirstOrDefault();

                        if (request.TransactionType.Code == cit.Code.ToString() ||
                            request.TransactionType.Code == deposit.Code.ToString())
                        {
                            request.BeneficiaryCode = account.BeneficiaryCode;
                        }
                    }
                }
            }
            return request;
        }

        /// <summary>
        ///     Check if Gpt deposit
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private bool IsGpt(RequestMessage request)
        {
            Device device =
                _repository.Query<Device>(a => a.SerialNumber.Trim().ToLower() == request.DeviceSerial.Trim().ToLower(),
                    o => o.DeviceType, o => o.DeviceType.Supplier).FirstOrDefault();

            if (device != null)
            {
                Supplier supplier = device.DeviceType.Supplier;
                return supplier.LookUpKey == "GPT" || supplier.LookUpKey == "GREYSTONE";
            }
            return false;
        }


        /// <summary>
        ///     Map to a fresh xml request to avoid failure.
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private RequestMessage StructureRequestMessage(RequestMessage message)
        {
            var requestMessage = new RequestMessage
            {
                UserId = message.UserId,
                BeneficiaryCode = message.BeneficiaryCode,
                CitCode = message.CitCode,
                DeviceSerial = message.DeviceSerial,
                TransactionDate = message.TransactionDate,
                ErrorCode = message.ErrorCode,
                ItramsReference = message.ItramsReference,
                UserReferance = message.UserReferance,
                TransactionType = message.TransactionType,
                Users = message.Users,
                CollectionUnits = message.CollectionUnits,
                Currencies = message.Currencies
            };

            return requestMessage;
        }

        #endregion
    }
}