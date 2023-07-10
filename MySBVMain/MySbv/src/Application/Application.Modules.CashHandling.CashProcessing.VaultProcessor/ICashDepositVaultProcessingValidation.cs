using Application.Dto.CashDeposit;
using Application.Dto.CashProcessing;
using Utility.Core;

namespace Application.Modules.CashHandling.CashProcessing.VaultProcessor
{
    public interface ICashDepositVaultProcessingValidation
    {
        /// <summary>
        ///     Find a Cash deposit by a seal or serial number
        /// </summary>
        /// <param name="username"></param>
        /// <param name="sealSerialNumber"></param>
        /// <returns></returns>
        MethodResult<CashProcessingDto> FindBySealSerialNumber(string sealSerialNumber, string username);


        /// <summary>
        /// Format deposit to VaultContainer
        /// </summary>
        /// <param name="deposit"></param>
        /// <returns></returns>
        VaultContainerDto FormatToVaultDeposit(CashProcessingDto deposit);

        /// <summary>
        /// Process Vault deposit
        /// </summary>
        /// <param name="cashDepositDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        MethodResult<CashProcessingDto> ProcessVault(VaultContainerDto cashDepositDto, string username); 
    }
}