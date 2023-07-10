using System.Collections.Generic;
using Vault.Integration.DataContracts;

namespace Vault.Integration.FailedTransactions.MessageProcessor
{
    public interface IRequestValidation
    {
        /// <summary>
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        ValidationResult ValidateCitCode(string siteId);

        /// <summary>
        /// </summary>
        /// <param name="depositMessage"></param>
        /// <returns></returns>
        ValidationResult ValidateIsBagOpened(RequestMessage depositMessage);

        /// <summary>
        /// </summary>
        /// <param name="beneficiaryCode"></param>
        /// <returns></returns>
        ValidationResult ValidateBeneficiary(string beneficiaryCode);

        /// <summary>
        /// </summary>
        /// <param name="deviceSerialNumber"></param>
        /// <returns></returns>
        ValidationResult ValidateDevice(string deviceSerialNumber);

        /// <summary>
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        List<ValidationResult> ValidateXml(RequestMessage deposit);

        /// <summary>
        /// </summary>
        /// <param name="depositMessage"></param>
        /// <returns></returns>
        ValidationResult ValidateTotals(RequestMessage depositMessage);

        ///// <summary>
        ///// </summary>
        ///// <param name="depositMessage"></param>
        ///// <returns></returns>
        //ValidationResult ValidateTransactionTypes(RequestMessage depositMessage);

        /// <summary>
        /// </summary>
        /// <param name="depositMessage"></param>
        /// <returns></returns>
        ValidationResult ValidatePayment(RequestMessage depositMessage);

        /// <summary>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ValidationResult ValidateGptPartialPayment(RequestMessage request);

        /// <summary>
        ///     Validates for valid Country Currency Code
        /// </summary>
        /// <param name="depositMessage"></param>
        /// <returns></returns>
        ValidationResult ValidateCurrencyCode(RequestMessage depositMessage);

        ///// <summary>
        ///// Validates excess payment on CIT Operation
        ///// </summary>
        ///// <param name="request"></param>
        ///// <returns></returns>
        //ValidationResult ValidateExcessPaymentOnCit(RequestMessage request);

        /// <summary>
        ///     Validates a Bag Number
        /// </summary>
        /// <param name="bagNumber"></param>
        /// <returns></returns>
        ValidationResult ValidateBagNumber(string bagNumber);
    }
}