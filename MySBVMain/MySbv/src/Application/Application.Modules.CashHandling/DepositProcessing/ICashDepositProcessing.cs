using System.Collections.Generic;
using Application.Dto.CashDeposit;
using Application.Dto.CashProcessing;
using Domain.Data.Model;
using Utility.Core;

namespace Application.Modules.CashHandling.DepositProcessing
{
    public interface ICashDepositProcessing
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