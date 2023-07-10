
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Dto.VaultPayment;
using Utility.Core;
using Application.Dto.Device;
using Application.Dto.Site;
using Domain.Data.Model;
using Application.Dto.Account;

namespace Application.Modules.VaultPayment
{
    public interface IVaultPaymentValidation
    {
        /// <summary> 
        /// Build payment model.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isRetailSupervisor"></param>
        /// <returns></returns>
        Task<PaymentModel> GeneratePaymentModel(User user, bool isRetailSupervisor);


        /// <summary>
        /// Get deposited amount in the Device.
        /// this retrieves only for an open bag
        /// </summary>
        /// <param name="bagNumber"></param>
        /// <returns></returns>
        Task<decimal> GetAmountInDevice(string bagNumber);


        /// <summary>
        /// Save Device Transaction
        /// </summary>
        /// <param name="vaultPaymentDto"></param>
        /// <param name="userId"></param>
        /// <param name="userRole"></param>
        /// <returns></returns>
        Task<MethodResult<PaymentResponseDto>> ReleasePayment(VaultPaymentDto vaultPaymentDto, int userId, string userRole);  
        

        /// <summary>
        /// get site devices
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<IEnumerable<DeviceDto>> GetSiteDevices(int siteId);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="merchantId"></param>
        /// <returns></returns>
        Task<List<SiteDto>> GetSites(int merchantId);


        /// <summary>
        /// get all accounts on site
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<List<AccountDto>> GetSiteAccounts(int siteId);


        /// <summary>
        /// get site information
        /// </summary>
        /// <param name="siteId"></param>
        /// <returns></returns>
        Task<SiteDto> GetSiteInfo(int siteId);


        /// <summary>
        /// get open bag number in device.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        Task<string> GetOpenBagNumber(int deviceId);


        /// <summary>
        /// Get Current LoggedIn User Information
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<User> GetLoggedUser(string username);

        /// <summary>
        /// Get Site Contact Email Address
        /// </summary>
        /// <param name="citCode"></param>
        /// <returns></returns>
        Task<string> GetSiteContactPersonEmail(string citCode);

    }
}