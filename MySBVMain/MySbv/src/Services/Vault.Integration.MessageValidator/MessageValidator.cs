using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.Linq;
using System.Net;
using Domain.Data.Model;
using Domain.Logging;
using Domain.Repository;
using Application.Modules.Common;
using Vault.Integration.DataContracts;
using Vault.Integration.MessageController;
using Denomination = Vault.Integration.DataContracts.Contracts.Denomination;

namespace Vault.Integration.MessageValidator
{
	[Export(typeof (IMessageValidator))]
	[PartCreationPolicy(CreationPolicy.Shared)]
	public class MessageValidator : IMessageValidator
	{
		#region Fields

		/// <summary>
		/// </summary>
		private readonly ILookup _lookup;

	    private readonly IMessageController _messageController;

	    /// <summary>
		/// </summary>
		private readonly IRepository _repository;

		/// <summary>
		/// </summary>
		private ILogger _logger;

		#endregion

		#region Constructor

	    /// <summary>
	    /// </summary>
	    /// <param name="repository"></param>
	    /// <param name="logger"></param>
	    /// <param name="messageController"></param>
	    [ImportingConstructor]
		public MessageValidator(IRepository repository, ILogger logger, ILookup lookup, IMessageController messageController)
		{
			_logger = logger;
			_lookup = lookup;
		    _repository = repository;
            _messageController = messageController;
            
		}

		#endregion

		#region IMessage Validator Implementation

		/// <summary>
		/// </summary>
		/// <param name="citCode"></param>
		/// <returns></returns>
		public ValidationResult ValidateCitCode(string citCode)
		{
			if (string.IsNullOrWhiteSpace(citCode))
			{
				return new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError 
                        { 
                            ErrorCode = HttpStatusCode.ExpectationFailed, 
                            ErrorMessage = "CIT Code Is Required" 
                        }
				};
			}

            bool result = _repository.Any<Site>(a => a.CitCode.Trim().ToLower() == citCode.Trim().ToLower() && a.IsNotDeleted);
			return result
				? new ValidationResult { Result = true, ValidationError = null }
				: new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
						    ErrorCode = HttpStatusCode.NotFound, 
                            ErrorMessage = string.Format("CIT Code [{0}] Not found", citCode)
						}
				};
		}

		/// <summary>
		/// VAlidates a bag number
		/// </summary>
		/// <param name="bagNumber"></param>
		/// <returns></returns>
		public ValidationResult ValidateBagNumber(string bagNumber)
		{
			var isValid = bagNumber.Trim().Length == 14;

			if (!isValid)
			{
				return new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
						    ErrorCode = HttpStatusCode.ExpectationFailed, 
                            ErrorMessage = string.Format("Bag Number '{0}' must be 14 Characters long.", bagNumber)
						}
				};
			}

			double number;
			if (Double.TryParse(bagNumber, out number) == false)
			{
				return new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
						    ErrorCode = HttpStatusCode.ExpectationFailed, 
                            ErrorMessage = "Bag Number must be Numeric only."
						}
				};
			}

			return new ValidationResult {Result = true, ValidationError = null};
		}

		/// <summary>
		/// Checks if Cit Message was sent to finalize the deposit
		/// </summary>
		/// <param name="depositMessage"></param>
		/// <returns></returns>
		public ValidationResult ValidateIsBagOpened(RequestMessage depositMessage)
		{
			var cashDeposits = new List<CashDeposit>();
		    Container container = null;

			foreach (var bag in depositMessage.CollectionUnits.CollectionUnit)
			{
				container = _repository.Query<Container>(o => o.SerialNumber == bag.Value, o => o.CashDeposit).FirstOrDefault();
				if (container != null)
				{
					cashDeposits.Add(container.CashDeposit);
				}
			}

			if (!cashDeposits.Any())
			{
				return new ValidationResult {Result = true, ValidationError = null};
			}

			var wasCitRun = cashDeposits.Any(a => a.CitDateTime == null);
			return wasCitRun
				? new ValidationResult {Result = true, ValidationError = null}
				: new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
						    ErrorCode = HttpStatusCode.ExpectationFailed, 
                            ErrorMessage = "You cannot make a [DEPOSIT, CIT, PAYMENT] on a bag that is already Closed."
						}
				};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="depositMessage"></param>
		/// <returns></returns>
		public ValidationResult ValidatePayment(RequestMessage depositMessage)
		{
            var result = ValidateBeneficiary(depositMessage.BeneficiaryCode);

            if (!result.Result)
                return result;
		    
			if (depositMessage.Currencies.Denominations.TotalValue < 0)
			{
				return new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
							ErrorCode = HttpStatusCode.ExpectationFailed,
							ErrorMessage = string.Format("No negative totals are allowed on Amount or TotalValue")
						}
				};
			}

            var code = Convert.ToInt16(depositMessage.TransactionType.Code);
            var codeType = _repository.Query<VaultTransactionType>(o => o.Code == code).SingleOrDefault();

			if (codeType == null)
			{
				return new ValidationResult { Result = true, ValidationError = null };
			}

			if (codeType.LookUpKey != "PAYMENT")
			{
				return new ValidationResult {Result = true, ValidationError = null};
			}

			// PAYMENT message
			var containers = new List<Container>();

			foreach (var a in depositMessage.CollectionUnits.CollectionUnit)
			{
				var container = _repository.Query<Container>(c => c.SerialNumber == a.Value, c => c.CashDeposit).FirstOrDefault();

				if (container != null)
				{
					containers.Add(container);
				}
			}

		    if (containers.Any())
		    {
		        // container to pay from was found
		        var container = containers.FirstOrDefault();

		        var depositAmount = container.CashDeposit.DepositedAmount;

		        if (container.CashDeposit.VaultAmount.HasValue == false)
		        {
		            if (depositMessage.Currencies.Denominations.TotalValue > depositAmount)
		            {
		                return new ValidationResult
		                {
		                    Result = false,
		                    ValidationError =
		                        new ValidationError
		                        {
		                            ErrorCode = HttpStatusCode.ExpectationFailed,
		                            ErrorMessage =
		                                string.Format(
		                                    "The amount deposited is less than the amount to be paid. Amount deposited = [{0}]and Amount to be paid [{1}]",
		                                    container.CashDeposit.DepositedAmount,
		                                    depositMessage.Currencies.Denominations.TotalValue)
		                        }
		                };
		            }
		            return new ValidationResult {Result = true, ValidationError = null};
		        }
		        else
		        {
		            if (container.CashDeposit.VaultAmount.Value < depositMessage.Currencies.Denominations.TotalValue)
		            {
		                return new ValidationResult
		                {
		                    Result = false,
		                    ValidationError =
		                        new ValidationError
		                        {
		                            ErrorCode = HttpStatusCode.ExpectationFailed,
		                            ErrorMessage =
		                                string.Format(
		                                    "You do not have enough funds to make a payment. Available funds = [{0}] and requested amount to pay = [{1}]",
		                                    container.CashDeposit.VaultAmount.Value,
		                                    depositMessage.Currencies.Denominations.TotalValue)
		                        }
		                };
		            }
		            return new ValidationResult {Result = true, ValidationError = null};
		        }
		    }
		    else // There are no deposits in the database with the specified bag number. Check the Queue instead
		    {
		        if (!_messageController.IsMessageInQueue(depositMessage))
		        {
                    return new ValidationResult
                    {
                        Result = false,
                        ValidationError =
                            new ValidationError
                            {
                                ErrorCode = HttpStatusCode.ExpectationFailed,
                                ErrorMessage =
                                    string.Format(
                                        "A deposit from bag '{0}' has to be made before you can make a payment.",
                                        depositMessage.CollectionUnits.CollectionUnit.FirstOrDefault().Value)
                            }
                    };        
		        }
		    }
		    return new ValidationResult {Result = true, ValidationError = null};

		}

		/// <summary>
		/// </summary>
		/// <param name="beneficiaryCode"></param>
		/// <returns></returns>
		public ValidationResult ValidateBeneficiary(string beneficiaryCode)
		{
            var status = _lookup.GetStatusId("ACTIVE");
            bool result = _repository.Query<Account>(a => a.BeneficiaryCode.Trim().ToLower() == beneficiaryCode.Trim().ToLower() && a.IsNotDeleted && a.StatusId == status).Any();
			return result
				? new ValidationResult {Result = true, ValidationError = null}
				: new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
							ErrorCode = HttpStatusCode.NotFound,
							ErrorMessage = string.Format("Beneficiary Code with code [{0}] Not found Or Beneficiary's account could be deleted or inactive.", beneficiaryCode)
						}
				};
		}

		/// <summary>
		/// </summary>
		/// <param name="deviceSerialNumber"></param>
		/// <returns></returns>
		public ValidationResult ValidateDevice(string deviceSerialNumber)
		{
			if (string.IsNullOrWhiteSpace(deviceSerialNumber))
			{
				return new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
							ErrorCode = HttpStatusCode.ExpectationFailed,
							ErrorMessage = "Device Serial Number is Required"
						}
				};
			}
            bool result = _repository.Any<Device>(a => a.SerialNumber.Trim().ToLower() == deviceSerialNumber.Trim().ToLower());
			return result
				? new ValidationResult { Result = true, ValidationError = null }
				: new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
							ErrorCode = HttpStatusCode.NotFound,
							ErrorMessage = string.Format("Device Serial Number [{0}]  Not found", deviceSerialNumber)
						}
				};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="depositMessage"></param>
		/// <returns></returns>
		public ValidationResult ValidateCurrencyCode(RequestMessage depositMessage)
		{
			var currencyCode = depositMessage.Currencies.Denominations.CurrencyCode; 
			bool result = _repository.Any<Currency>(a => a.Code == currencyCode);
			
			return result
				? new ValidationResult { Result = true, ValidationError = null }
				: new ValidationResult
				{
					Result = false,
					ValidationError =
						new ValidationError
						{
							ErrorCode = HttpStatusCode.NotFound,
							ErrorMessage = string.Format("Currency with Code [{0}] Not Found",currencyCode)
						}
				};
		}
     
		/// <summary>
		/// </summary>
		/// <param name="depositMessage"></param>
		/// <returns></returns>
		public ValidationResult ValidateTotals(RequestMessage depositMessage)
		{
            var citTransactionType = _lookup.GetVaultTransactionType("CIT");
            var paymentTransactionType = _lookup.GetVaultTransactionType("PAYMENT");

            // Ignore the total calculations when the message received
            // is for payment or CIT
            if (depositMessage.TransactionType.Code == citTransactionType.Code.ToString(CultureInfo.InvariantCulture) ||
                depositMessage.TransactionType.Code == paymentTransactionType.Code.ToString(CultureInfo.InvariantCulture))
                return new ValidationResult { Result = true, ValidationError = null }; 

			decimal total = 0;

		    try
		    {
		        if (depositMessage.Currencies.Denominations.Fit.Denomination != null)
		        {
                    depositMessage.Currencies.Denominations.Fit.Denomination.ForEach(
                    denomination => { total += CalculateDenominationValue(denomination); });    
		        }

		        if (depositMessage.Currencies.Denominations.UnFit.Denomination != null)
		        {
                    depositMessage.Currencies.Denominations.UnFit.Denomination.ForEach(
                    denomination => { total += CalculateDenominationValue(denomination); });
		        }
		    }
		    catch (Exception ex)
		    {
                return new ValidationResult
                {
                    Result = false,
                    ValidationError = new ValidationError
                    {
                        ErrorCode = HttpStatusCode.ExpectationFailed,
                        ErrorMessage = string.Format("Invalid Denomination Count must can only be Numeric [{0}]", ex.Message)
                    }
                };
		    }
			
			//Check if the Total of the denominations amount to the Denominations of the current Deposit
			if (total != Convert.ToDecimal(depositMessage.Currencies.Denominations.TotalValue))
				return new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.ExpectationFailed,
						ErrorMessage = "The Total Computation Of The Supplied Denomination Count Does Not Amount To The Total Amount."
					}
				};


			if (depositMessage.Currencies.Denominations.TotalValue <= 0)
				return new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.ExpectationFailed,
						ErrorMessage =
							string.Format("The total Deposit amount [{0}] Cannot be less or equal to Zero", depositMessage.Currencies.Denominations.TotalValue)
					}
				};


			//Check if the Total Fit Notes equal to the total Value
			if (depositMessage.Currencies.Denominations.Fit.Value != depositMessage.Currencies.Denominations.TotalValue)
				return new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.ExpectationFailed,
						ErrorMessage = "The Total Computation Of The Supplied Fit Denomination Count Does Not Amount To The Total Amount."
					}
				};


			decimal totalFitUnfit = depositMessage.Currencies.Denominations.Fit.Value +
									depositMessage.Currencies.Denominations.UnFit.Value;

			//Check Total Value for Fit and Unfit Denominations
			if (totalFitUnfit != depositMessage.Currencies.Denominations.TotalValue)
				return new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.ExpectationFailed,
						ErrorMessage = "The Total Computation Of The Supplied Fit AND Unfit Denomination Count Does Not Amount To The Total Amount."
					}
				};

			return new ValidationResult {Result = true, ValidationError = null};
		}



		private bool IsCashConnect(RequestMessage message)
		{
			var supplier = _repository.Query<Device>(e => e.SerialNumber.ToLower().Trim() == message.DeviceSerial.ToLower().Trim(), 
			e => e.DeviceType, e => e.DeviceType.Supplier).FirstOrDefault();
            
            var result = supplier != null ? supplier.DeviceType.Supplier.LookUpKey : string.Empty;

			return result == "CASH_CONNECT";
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="deposit"></param>
		/// <returns></returns>
		public List<ValidationResult> ValidateXml(RequestMessage deposit)
		{
			var validationResultList = new List<ValidationResult>();
			deposit.CollectionUnits.CollectionUnit.ForEach(collectionUnit =>
			{
				if (string.IsNullOrEmpty(collectionUnit.Value) || string.IsNullOrWhiteSpace(collectionUnit.Value))
					validationResultList.Add(new ValidationResult
					{
						Result = false,
						ValidationError = new ValidationError
						{
							ErrorCode = HttpStatusCode.ExpectationFailed,
							ErrorMessage = "Bag Serial Number Is Required"
						}
					});
			});

			
			//try
			//{
			//	if (IsCashConnect(deposit))
			//	{
			//		deposit.TransactionDate = deposit.TransactionDate.Replace("-", "/");
			//		var dateResult = DateTime.ParseExact(deposit.TransactionDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
			//		deposit.TransactionDate = dateResult.ToString("yyyy/MM/dd HH:mm:ss");
			//	}
			//	else
			//	{
			//		var date = Convert.ToDateTime(deposit.TransactionDate).ToString("yyyy/MM/dd HH:mm:ss");
			//		deposit.TransactionDate = date;
			//	}
			//}
			//catch (Exception)
			//{
			//	validationResultList.Add(new ValidationResult
			//	{
			//		Result = false,
			//		ValidationError = new ValidationError
			//		{
			//			ErrorCode = HttpStatusCode.NotAcceptable,
			//			ErrorMessage = string.Format("Invalid Date [{0}]",
			//				CheckValue(deposit.TransactionDate.ToString(CultureInfo.InvariantCulture)))
			//		}
			//	});
			//}
		


			if (deposit.Currencies.Denominations.Fit.Denomination != null)
			{
				deposit.Currencies.Denominations.Fit.Denomination.ForEach(denomination =>
				{
					int count;
					if (!int.TryParse(denomination.Count.ToString(CultureInfo.InvariantCulture), out count))
						validationResultList.Add(new ValidationResult
						{
							Result = false,
							ValidationError = new ValidationError
							{
								ErrorCode = HttpStatusCode.ExpectationFailed,
								ErrorMessage = string.Format("Invalid denomination [{0}]",
									CheckValue(denomination.ToString()))
							}
						});
				});
			}

			if (deposit.Currencies.Denominations.UnFit.Denomination != null)
			{
				deposit.Currencies.Denominations.UnFit.Denomination.ForEach(denomination =>
				{
					int count;
					if (!int.TryParse(denomination.Count.ToString(CultureInfo.InvariantCulture), out count))
						validationResultList.Add(new ValidationResult
						{
							Result = false,
							ValidationError = new ValidationError
							{
                                ErrorCode = HttpStatusCode.ExpectationFailed,
								ErrorMessage = string.Format("Invalid denomination [{0}]",
									CheckValue(denomination.ToString()))
							}
						});
				});
			}


			if (deposit.Currencies.Denominations.Fit.Denomination != null)
			{
				deposit.Currencies.Denominations.Fit.Denomination.ForEach(denomination =>
				{
					int count;
					if (!int.TryParse(denomination.Value.ToString(CultureInfo.InvariantCulture), out count))
						validationResultList.Add(new ValidationResult
						{
							Result = false,
							ValidationError = new ValidationError
							{
								ErrorCode = HttpStatusCode.NotAcceptable,
								ErrorMessage = string.Format("Invalid Value [{0}]",
									CheckValue(denomination.Value.ToString(CultureInfo.InvariantCulture)))
							}
						});
				});
			}


			if (deposit.Currencies.Denominations.UnFit.Denomination != null)
			{
				deposit.Currencies.Denominations.UnFit.Denomination.ForEach(denomination =>
				{
					int count;
					if (!int.TryParse(denomination.Value.ToString(CultureInfo.InvariantCulture), out count))
						validationResultList.Add(new ValidationResult
						{
							Result = false,
							ValidationError = new ValidationError
							{
								ErrorCode = HttpStatusCode.NotAcceptable,
								ErrorMessage = string.Format("Invalid Value [{0}]",
									CheckValue(denomination.Value.ToString(CultureInfo.InvariantCulture)))
							}
						});
				});
			}


			if (deposit.Currencies.Denominations.Fit.Denomination != null)
			{
				deposit.Currencies.Denominations.Fit.Denomination.ForEach(denomination =>
				{
					if (!Compare(denomination.Value, denomination.Type))
					{
						validationResultList.Add(new ValidationResult
						{
							Result = false,
							ValidationError = new ValidationError
							{
								ErrorCode = HttpStatusCode.ExpectationFailed,
								ErrorMessage = string.Format("[{0}] is not a valid Denomination Type", denomination.Type)
							}
						});
					}
				});
			}

			if (string.IsNullOrEmpty(deposit.UserId.ToString()) || string.IsNullOrWhiteSpace(deposit.UserId.ToString()))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
						ErrorMessage = "UserId Is Required"
					}
				});

			if (string.IsNullOrEmpty(deposit.UserReferance) || string.IsNullOrWhiteSpace(deposit.UserReferance))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
                        ErrorMessage = "UserReferance  Is Required"
					}
				});


			if (string.IsNullOrEmpty(deposit.TransactionType.ToString()) ||
			    string.IsNullOrWhiteSpace(deposit.TransactionType.ToString()))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
                        ErrorMessage = "Transaction Type  Is Required"
					}
				});


			if (string.IsNullOrEmpty(deposit.Users.User.Value) || string.IsNullOrWhiteSpace(deposit.Users.User.Value))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
						ErrorMessage = "Username and Type is Required"
					}
				});


			if (string.IsNullOrEmpty(deposit.CollectionUnits.ToString()) ||
			    string.IsNullOrWhiteSpace(deposit.CollectionUnits.ToString()))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
						ErrorMessage = "CollectionUnits  Cannot  be Null/Empty"
					}
				});

			if (string.IsNullOrEmpty(deposit.Currencies.ToString()) || string.IsNullOrWhiteSpace(deposit.Currencies.ToString()))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
						ErrorMessage = "Currencies Cannot Is Required"
					}
				});

			//if (string.IsNullOrEmpty(deposit.TransactionDate) || string.IsNullOrWhiteSpace(deposit.TransactionDate))

			//	validationResultList.Add(new ValidationResult
			//	{
			//		Result = false,
			//		ValidationError = new ValidationError
			//		{
			//			ErrorCode = HttpStatusCode.NotAcceptable,
			//			ErrorMessage = "Transaction Date Is Required"
			//		}
				//});


			if (string.IsNullOrEmpty(deposit.ItramsReference) || string.IsNullOrWhiteSpace(deposit.ItramsReference))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
						ErrorMessage = "ItramsReference Is Required"
					}
				});
            
			if (string.IsNullOrEmpty(deposit.ErrorCode) ||
			    string.IsNullOrWhiteSpace(deposit.ErrorCode))

				validationResultList.Add(new ValidationResult
				{
					Result = false,
					ValidationError = new ValidationError
					{
						ErrorCode = HttpStatusCode.NotAcceptable,
						ErrorMessage = "ErrorCode Is Required. [00] put for Default"
					}
				});
			return validationResultList;
		}

		///// <summary>
		/////     Check if current Transaction Type exist
		///// </summary>
		///// <param name="depositMessage"></param>
		///// <returns></returns>
		//public ValidationResult ValidateTransactionTypes(RequestMessage depositMessage)
		//{
		//	IEnumerable<VaultTransactionType> vaultTransactionTypes = _repository.All<VaultTransactionType>();
		//	var transactionTypes = vaultTransactionTypes.ToDictionary(t => t.Code.ToString(), t => t.Name);

		//	if (!transactionTypes.ContainsKey(depositMessage.TransactionType.Code))
		//		return new ValidationResult
		//		{
		//			Result = false,
		//			ValidationError =
		//				new ValidationError
		//				{
		//					ErrorCode = HttpStatusCode.NotFound,
		//					ErrorMessage = string.Format("Invalid Transaction Type Code [{0}]", depositMessage.TransactionType.Code)
		//				}
		//		};

		//	return new ValidationResult {Result = true, ValidationError = null};
		//}

		/// <summary>
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		private string CheckValue(string value)
		{
			if (string.IsNullOrEmpty(value)) return "NULL";
			if (string.IsNullOrWhiteSpace(value)) return "NULL";
			return value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="capturedDenomination"></param>
		/// <returns></returns>
		private decimal CalculateDenominationValue(Denomination capturedDenomination)
		{
			switch (capturedDenomination.Value)
			{
				case 20000:
					return Convert.ToDecimal(capturedDenomination.Count*200);

				case 10000:
					return Convert.ToDecimal(capturedDenomination.Count*100);

				case 5000:
					return Convert.ToDecimal(capturedDenomination.Count*50);

				case 2000:
					return Convert.ToDecimal(capturedDenomination.Count*20);

				case 1000:
					return Convert.ToDecimal(capturedDenomination.Count*10);

				case 500:
					return Convert.ToDecimal(capturedDenomination.Count*5);

				case 200:
					return Convert.ToDecimal(capturedDenomination.Count*2);

				case 100:
					return Convert.ToDecimal(capturedDenomination.Count*1);

				case 50:
					return Convert.ToDecimal(capturedDenomination.Count*0.5);

				case 20:
					return Convert.ToDecimal(capturedDenomination.Count*0.2);

				case 10:
					return Convert.ToDecimal(capturedDenomination.Count*0.1);

				case 5:
					return Convert.ToDecimal(capturedDenomination.Count*0.05);
				default:
					throw new Exception(string.Format("Invalid Denomination Type [{0}]", capturedDenomination.Value));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="value"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public bool Compare(int value, string type)
		{
			var denominations = from t in _repository.All<DenominationType>()
				join d in _repository.All<Domain.Data.Model.Denomination>() on t.DenominationTypeId equals d.DenominationTypeId
				select new
				{
					d.ValueInCents,
					Name = t.LookUpKey == "NOTES"? "Note" : "Coin"
				};

			var money = denominations.ToDictionary(denomination => denomination.ValueInCents,
				denomination => denomination.Name);
			return money.ContainsKey(value) && String.Equals(money[value], type, StringComparison.CurrentCultureIgnoreCase);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public ValidationResult ValidateGptPartialPayment(RequestMessage request)
        {

            var code = Convert.ToInt16(request.TransactionType.Code);
            var transactionTypeCode = _repository.Query<VaultTransactionType>(a => a.Code == code).FirstOrDefault();

            if (string.IsNullOrWhiteSpace(request.DeviceSerial) || transactionTypeCode == null)
            {
                return new ValidationResult { Result = true, ValidationError = null };
            }

            var device = _repository.Query<Device>(a => a.SerialNumber == request.DeviceSerial, a => a.DeviceType, a => a.DeviceType.Supplier).FirstOrDefault();

            if (transactionTypeCode.LookUpKey == "PAYMENT" && device.DeviceType.Supplier.LookUpKey == "CASH_CONNECT")
            {
                return new ValidationResult
                {
                    Result = false,
                    ValidationError =
                        new ValidationError
                        {
                            ErrorCode = HttpStatusCode.NotFound,
                            ErrorMessage = "No Payments Can be Made by Cash Connect."
                        }
                };
            }
            return new ValidationResult { Result = true, ValidationError = null };
        }

		///// <summary>
		///// 
		///// </summary>
		///// <param name="request"></param>
		///// <returns></returns>
		//public ValidationResult ValidateExcessPaymentOnCit(RequestMessage request)
		//{
		//	var code = Convert.ToInt16(request.TransactionType.Code);
		//	var transactionTypeCode = _repository.Query<VaultTransactionType>(a => a.Code == code).FirstOrDefault();
		//	if (transactionTypeCode == null)
		//	{
		//		return new ValidationResult { Result = true, ValidationError = null };
		//	}

		//	if (transactionTypeCode.LookUpKey == "CIT")
		//	{
		//		var containers = new List<Container>();
		//		foreach (var unit in request.CollectionUnits.CollectionUnit)
		//		{
		//			var container = _repository.Query<Container>(a => a.SerialNumber == unit.Value, o => o.CashDeposit).FirstOrDefault();
		//			if (container != null)
		//			{
		//				containers.Add(container);
		//			}
		//		}

		//		if (containers.Any())
		//		{
		//			var cashDeposit = containers.SingleOrDefault().CashDeposit;
		//			if (cashDeposit.VaultAmount != request.Currencies.Denominations.TotalValue)
		//			{
		//				string excessCiTmessage =
		//					string.Format(
		//						"Failed to run CIT on Amount to be paid [{0}] which is more than the Total [{1}] available funds available after making all payments.",
		//						request.Currencies.Denominations.TotalValue, cashDeposit.VaultAmount);

		//				string shortCitMessage =
		//					string.Format(
		//						"Failed to run CIT on Amount to be paid [{0}] which is less than the Total [{1}] available funds available after making all payments.",
		//						request.Currencies.Denominations.TotalValue, cashDeposit.VaultAmount);

		//				string citErrorMessage = (cashDeposit.VaultAmount < request.Currencies.Denominations.TotalValue)
		//					? excessCiTmessage
		//					: shortCitMessage;

		//				return new ValidationResult
		//				{
		//					Result = false,
		//					ValidationError =
		//						new ValidationError
		//						{
		//							ErrorCode = HttpStatusCode.ExpectationFailed,
		//							ErrorMessage = citErrorMessage
		//						}
		//				};
		//			}
		//		}
		//	}
		//	return new ValidationResult { Result = true, ValidationError = null };
		//}

		#endregion
		
	}
}