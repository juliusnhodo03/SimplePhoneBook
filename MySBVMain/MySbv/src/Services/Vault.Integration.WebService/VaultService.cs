using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.UI;
using Application.Modules.Common;
using Domain.Data.Model;
using Domain.Data.Core;
using Domain.Repository;
using Domain.Security;
using Infrastructure.Logging;
using Domain.Notifications;
using Domain.Repository;
using Infrastructure.Logging;
using Ninject;
using Utility.Core;
using Vault.Integration.DataContracts;
using Vault.Integration.DataContracts.Contracts;
using Vault.Integration.MessageController;
using Vault.Integration.MessageValidator;
using Vault.Integration.Msmq.Connector;
using Vault.Integration.Request.Data;
using Task = System.Threading.Tasks.Task;

namespace Vault.Integration.WebService
{
    /// <summary>
    ///     The is used by devices to create deposits and submit
    ///     payment requests on Mysbv
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class VaultService : IVaultService
    {
        #region Fields

        private ILookup _lookup;

        private static INotification _notification;
         
        //private CompositionContainer _container;
        /// <summary>
        ///     MEF Composition Container
        /// </summary>
        /// <summary>
        ///     IMessage Controller Instance. Used to relay a message to the correct processor implementation
        /// </summary>
        private IMessageController _messageController;

        /// <summary>
        ///     IMessage Connector Instance.
        /// </summary>
        private IMsmqConnector _msmqConnector;

        /// <summary>
        ///     IMessage Validator instance. Used to validate a message.
        /// </summary>
        private IRequest _request;

        /// <summary>
        ///     IMessage Validator instance. Used to validate a message.
        /// </summary>
        private IMessageValidator _validator;

        private IRepository repository;
        private IEmailManager _emailManager;

        #endregion

        #region Constructor

        /// <summary>
        ///     IVault Service implementation
        /// </summary>
        public VaultService()
        {
            //this.Log().Debug(() =>
            //{
            //    var pathConfig = new PathConfiguration();
            //    pathConfig = new PathConfiguration();
            //    pathConfig.Initialize();

            //    this.Log().Debug(pathConfig.Configuration.Path);

            //    var catalog = new AggregateCatalog(new DirectoryCatalog(pathConfig.Configuration.Path),
            //        new AssemblyCatalog(Assembly.GetExecutingAssembly()));

            //    this.Log().Debug(string.Format("Number of discovered parts [{0}]", catalog.Parts.Count()));

            //    _container = new CompositionContainer(catalog);
            //    _container.SatisfyImportsOnce(this);

            //    foreach (ComposablePartDefinition part in catalog.Parts)
            //    {
            //        foreach (ExportDefinition exportDefinition in part.ExportDefinitions)
            //        {
            //            this.Log().Debug(exportDefinition.ContractName);
            //        }
            //    }
            //}, "Initializing MEF Catalog");


            this.Log().Debug(() =>
            {
                IKernel kernel = new StandardKernel(new Bindings());

                _validator = kernel.Get<IMessageValidator>();
                _messageController = kernel.Get<IMessageController>();
                _msmqConnector = kernel.Get<IMsmqConnector>();
                _request = kernel.Get<IRequest>();
                _lookup = kernel.Get<ILookup>();
                repository = kernel.Get<IRepository>();
                _notification = kernel.Get<INotification>();
                _emailManager = kernel.Get<IEmailManager>();
                
            }, "Initializing NINJECT Catalog");


            //this.Log().Debug(() =>
            //{
            //    _validator = _container.GetExportedValue<IMessageValidator>();
            //    _messageController = _container.GetExportedValue<IMessageController>();
            //    _msmqConnector = _container.GetExportedValue<IMsmqConnector>();
            //    _request = _container.GetExportedValue<IRequest>();
            //}, "Resolve all exported items.");

            //GetUpdatedVaultTransaction();
        }

        #endregion

        #region IVault Service

        /// <summary>
        ///     Returns a list of beneficiaries that are linked to a site
        /// </summary>
        /// <param name="citCode">The CIT Code of the Site</param>
        /// <returns>List of beneficiaries</returns>
        public IEnumerable<Beneficiary> GetBeneficiaries(string citCode)
        {
            try
            {
                var beneficiaries = new List<Beneficiary>();
                int status = _lookup.GetStatusId("ACTIVE");
                this.Log().Debug(() =>
                {
                    //var repository = _container.GetExportedValue<IRepository>();

                    Site result = repository.Query<Site>(
                        a => a.CitCode == citCode && a.IsNotDeleted && a.StatusId == status,
                        a => a.Accounts,
                        a => a.Accounts.Select(x => x.Bank),
                        a => a.Accounts.Select(v => v.AccountType))
                        .FirstOrDefault()
                        .RemoveDeletedInactiveAccounts(_lookup);

                    if (result != null)
                    {
                        if (result.Accounts != null)
                        {
                            beneficiaries = result.Accounts.Select(acc => new Beneficiary
                            {
                                BeneficiaryCode = acc.BeneficiaryCode,
                                BankName = acc.Bank.Name,
                                AccountHolder = acc.AccountHolderName,
                                AccountType = acc.AccountType.Name,
                                IsDefault = acc.DefaultAccount
                            }).ToList();
                        }
                        else
                        {
                            throw new WebFaultException<string>("A site does not have any accounts associated with it.",
                                HttpStatusCode.NotFound);
                        }
                    }
                    else
                    {
                        throw new WebFaultException<string>(
                            string.Format("A site with CIT Code = {0} is not found.", citCode),
                            HttpStatusCode.NotFound);
                    }
                }, string.Format("Retrieving beneficiaries for CIT Code  [{0}]", citCode));

                return beneficiaries;
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal(string.Format("Exception On Method : [GET BENEFICIARIES]\n[{0}]\nStacktrace\n", ex.Message),
                        ex);
                throw new WebFaultException<string>(
                    string.Format("Internal Server Error"),
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="cashDepost"></param>
        /// <returns></returns>
        public List<ValidationError> SubmitRequest(RequestMessage cashDepost)
        {
            try
            {
                WebOperationContext webContext = WebOperationContext.Current;

                var validationResults = new ValidationError[] {};
                this.Log()
                    .Debug(() => { validationResults = RunValidations(cashDepost).ToArray(); }, "Running Validations");

                if (validationResults.Any())
                {
                    // Dumping Failed XML Request to Database
                    this.Log()
                        .Debug(() => { _request.DumpFailedXmlRequest(cashDepost, validationResults); },
                            "Dumping Failed XML Request to Database");

                    if (cashDepost.TransactionType.Code == _lookup
                        .GetVaultTransactionType("CIT")
                        .Code.ToString(CultureInfo.InvariantCulture) &&
                        validationResults.ToList().All(a => a.ErrorCode != HttpStatusCode.Conflict))
                    {
                        webContext.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                        return null;
                    }                   

                    // return error code
                    webContext.OutgoingResponse.StatusCode = (HttpStatusCode)422;

                    var recepients = GetEmailRecepients().ToArray();
                    var body = CreateEmailBody();

                    // send emails on error happening
                    Task.Factory.StartNew(() => _emailManager.SendEmail(recepients.ToList(), new MailMessage(), "Rejected mySBV.vault Transactions", body)); 

                    return validationResults.ToList();
                }

                // Relay Message to MSMQ
                this.Log().Debug(() => { _messageController.RelayMessage(cashDepost); }, "Relaying deposit message");

                webContext.OutgoingResponse.StatusCode = HttpStatusCode.Created;
                return null;
            }
            catch (WebFaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal(string.Format("Exception On Method : [SUBMIT REQUEST]\n[{0}]\nStacktrace\n", ex.Message), ex);
                throw new WebFaultException<string>(
                    string.Format("Internal Server Error"),
                    HttpStatusCode.InternalServerError);
            }
        }
        


        /// <summary>
        /// 
        /// </summary>
        private List<string> GetEmailRecepients()
        {
            const string query = @"SELECT DISTINCT U.EmailAddress
                    FROM [User] U INNER JOIN webpages_UsersInRoles UIR ON U.UserId = UIR.UserId
	                     INNER JOIN webpages_Roles R ON R.RoleId=UIR.RoleId
                    WHERE R.RoleName IN ('SBVAdmin', 'SBVDataCapture', 'SBVApprover') AND (U.EmailAddress IS NOT NULL) AND (u.IsNotDeleted = 1)";

            var recepients = repository.ExecuteQueryCommand<string>(query);
            return recepients;
        }

        

        /// <summary>
        ///     Create email body
        /// </summary>
        private string CreateEmailBody()
        {
            var writer = new StringWriter();
            var html = new HtmlTextWriter(writer);

            html.RenderBeginTag(HtmlTextWriterTag.P);
            html.WriteEncodedText("Rejected mySBV.vault Transactions");

            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText("You are receiving this email because mySBV.vault transactions have been rejected made on MYSBV :");

            html.WriteBreak();
            html.WriteEncodedText("Please login to MYSBV and review under financial management.");

            html.WriteBreak();
            html.WriteBreak();
            html.WriteEncodedText(string.Format("©SBV {0}", DateTime.Now.Year));
            html.RenderEndTag();

            html.Flush();
            return writer.ToString();
        }


        /// <summary>
        ///     Verify bag number existence on mySBV system
        /// </summary>
        /// <param name="serialNumber"></param>
        /// <returns></returns>
        public ValidationError VerifyBag(string serialNumber)
        {
            bool bagExists = false;
            try
            {
                var validationResult = new ValidationError
                {
                    ErrorCode = HttpStatusCode.NotFound,
                    ErrorMessage = string.Format("Bag Number {0} Was never used.", serialNumber)
                };

                //var repository = _container.GetExportedValue<IRepository>();
                //var msmqConnector = _container.GetExportedValue<IMsmqConnector>();

                this.Log().Debug(() =>
                {
                    //  Check the bag in Deposits
                    bagExists = repository.Any<Container>(a => a.SerialNumber.ToUpper().Equals(serialNumber.ToUpper()));

                    if (bagExists)
                    {
                        validationResult = new ValidationError
                        {
                            ErrorCode = HttpStatusCode.Found,
                            ErrorMessage = string.Format("Bag Number {0} Exists on MySbv.", serialNumber)
                        };
                    }
                }, string.Format("Checking deposit Containers for Serial [{0}]", serialNumber));

                this.Log().Debug(() =>
                {
                    // Check the bag in CashOrders
                    bagExists =
                        repository.Any<CashOrderContainer>(a => a.SerialNumber.ToUpper().Equals(serialNumber.ToUpper()));

                    if (bagExists)
                    {
                        validationResult = new ValidationError
                        {
                            ErrorCode = HttpStatusCode.Found,
                            ErrorMessage = string.Format("Bag Number {0} Exists on MySbv.", serialNumber)
                        };
                    }
                }, string.Format("Checking orders Containers for Serial [{0}]", serialNumber));

                this.Log().Debug(() =>
                {
                    // Check the bag in FailedValidations
                    bagExists =
                        repository.Any<VaultTransactionXml>(
                            a => a.BagSerialNumber.ToUpper().Equals(serialNumber.ToUpper()));

                    if (bagExists)
                    {
                        validationResult = new ValidationError
                        {
                            ErrorCode = HttpStatusCode.Found,
                            ErrorMessage = string.Format("Bag Number {0} Exists on MySbv.", serialNumber)
                        };
                    }
                }, string.Format("Checking orders Containers for Serial [{0}]", serialNumber));

                this.Log().Debug(() =>
                {
                    // Check the bag in QUEUES
                    bool inGptQueue = _msmqConnector.VerifyBagBySerialNumber(QueueIdentifier.GptRequest, serialNumber);
                    bool inCashConnectQueue = _msmqConnector.VerifyBagBySerialNumber(QueueIdentifier.CashConnectRequest, serialNumber);
                    bool ingreystoneQueue = _msmqConnector.VerifyBagBySerialNumber(QueueIdentifier.GreystoneRequest, serialNumber);

                    if (inCashConnectQueue || inGptQueue || ingreystoneQueue)
                    {
                        validationResult = new ValidationError
                        {
                            ErrorCode = HttpStatusCode.Found,
                            ErrorMessage = string.Format("Bag Number {0} Exists on MySbv.", serialNumber)
                        };
                    }
                }, string.Format("Checking Queues for Serial number [{0}]", serialNumber));

                return validationResult;
            }
            catch (WebFaultException<string>)
            {
                throw;
            }
            catch (Exception ex)
            {
                this.Log()
                    .Fatal(string.Format("Exception On Method : [GET VERIFYBAG]\n[{0}]\nStacktrace\n", ex.Message),
                        ex);
                throw new WebFaultException<string>(
                    string.Format("Internal Server Error"),
                    HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        ///     Validate the incoming deposit XML message
        /// </summary>
        /// <param name="cashDeposit"></param>
        /// <returns></returns>
        private IEnumerable<ValidationError> RunValidations(RequestMessage cashDeposit)
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
            //var repository = _container.GetExportedValue<IRepository>();
            bool isValidTransactionCode = true;

            // validate transaction type code
            this.Log().Debug(() =>
            {
                short code = Convert.ToInt16(cashDeposit.TransactionType.Code);

                if (!repository.Any<VaultTransactionType>(a => a.Code == code))
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
                        if (!repository.Any<Container>(a => a.SerialNumber.ToLower() == collectionUnit.Value.ToLower()))
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
                //var repository = _container.GetExportedValue<IRepository>();
                Status failedStatus = repository.Query<Status>(e => e.LookUpKey == "FAILED_VALIDATION").FirstOrDefault();
                Status pendingStatus = repository.Query<Status>(e => e.LookUpKey == "PENDING").FirstOrDefault();

                this.Log().Debug(() =>
                {
                    foreach (CollectionUnit collectionUnit in cashDeposit.CollectionUnits.CollectionUnit)
                    {
                        bool hasFailedXmls = repository.Any<VaultTransactionXml>(
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
                            if (repository.Any<VaultTransactionXml>(a => a.BagSerialNumber == collectionUnit.Value &&
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
            //var repository = _container.GetExportedValue<IRepository>();

            if (IsGpt(request))
            {
                if (string.IsNullOrWhiteSpace(request.BeneficiaryCode))
                {
                    Site site =
                        repository.Query<Site>(o => o.CitCode.ToUpper() == request.CitCode.ToUpper()).FirstOrDefault();
                    if (site != null)
                    {
                        VaultTransactionType cit = _lookup.GetVaultTransactionType("CIT");
                        VaultTransactionType deposit = _lookup.GetVaultTransactionType("DEPOSIT");

                        Account account =
                            repository.Query<Account>(o => o.SiteId == site.SiteId && o.DefaultAccount).FirstOrDefault();

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
            //var repository = _container.GetExportedValue<IRepository>();

            Device device =
                repository.Query<Device>(a => a.SerialNumber.Trim().ToLower() == request.DeviceSerial.Trim().ToLower(),
                    o => o.DeviceType, o => o.DeviceType.Supplier).FirstOrDefault();
            if (device != null)
            {
                Supplier supplier = device.DeviceType.Supplier;
                return supplier.LookUpKey == "GPT" || supplier.LookUpKey == "GREYSTONE";
            }
            return false;
        }

        /// <summary>
        ///     Get updated vault deposits
        /// </summary>
        private void GetUpdatedVaultTransaction()
        {
            Task.Factory.StartNew(() =>
            {
                this.Log().Info("Pooling the failed Transactions Queue");
                while (true)
                {
                    try
                    {
                        MethodResult<MessageEnvelope<RequestMessage>> envelope =
                            _msmqConnector.ReceiveMessage<MessageEnvelope<RequestMessage>>(
                                QueueIdentifier.FailedTransactions);

                        this.Log()
                            .Debug(string.Format("Received message with label [{0}]", envelope.EntityResult.Label));

                        if (envelope != null)
                        {
                            RequestMessage message = envelope.EntityResult.MessageObject;

                            this.Log().Debug("Calling Submit Request");
                            SubmitRequest(message);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.Log().Fatal("Exception On Method : [GET_UPDATED_VAULT_TRANSACTIONS]", ex);
                    }
                }
            });
        }

        #endregion
    }
}