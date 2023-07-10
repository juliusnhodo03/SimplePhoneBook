using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Application.Dto.Account;
using Application.Dto.Device;
using Application.Dto.Merchant;
using Application.Dto.Site;

namespace Application.Dto.VaultPayment
{
    public class PaymentModel
    {
        public VaultPaymentDto Payment { get; set; }

        [NotMapped]
        public List<MerchantDto> Merchants { get; set; }

        [NotMapped]
        public List<SiteDto> Sites { get; set; }

        [NotMapped]
        public List<DeviceDto> Devices { get; set; }

        [NotMapped]
        public List<AccountDto> Accounts { get; set; }
    }
}