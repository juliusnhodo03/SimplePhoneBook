using Application.Dto.CashProcessing;
using Utility.Core;

namespace Application.Modules.CashHandling.CashProcessing.WebProcessor
{
    public interface ICashDepositWebProcessingValidation
    {
        /// <summary>
        ///     Find a Cash deposit by a seal or serial number
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sealSerialNumber"></param>
        /// <returns></returns>
        MethodResult<CashProcessingDto> FindBySealSerialNumber(string sealSerialNumber, string username);

        /// <summary>
        ///     Process a deposit
        /// </summary>
        /// <param name="processingDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<bool> Process(CashProcessingDto processingDto, string username);
    }
}