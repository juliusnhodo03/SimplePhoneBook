using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Data.Model;

namespace Application.Dto.Product
{
    public class VaultFeeDto
    {
        [Required(ErrorMessage = "Device Supplier is required")]
        [Display(Name = "Device Supplier")]
        public int DeviceSupplierId { get; set; }

        [Required(ErrorMessage = "Device Name is required")]
        [Display(Name = "Device Name")]
        public int DeviceId { get; set; }

        [Display(Name = "Deposit Reference Editable")]
        public bool DepositReferenceEditable { get; set; }

        [Required(ErrorMessage = "Site Deposit Reference is required")]
        [Display(Name = "Site Deposit Reference")]
        public string SiteDepositReference { get; set; }

        [Required(ErrorMessage = "Cash Processing Fee Code is required")]
        [Display(Name = "Cash Processing Fee Code")]
        public string CashProcessingFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Processing Fee is required")]
        [Display(Name = "Cash Processing Fee")]
        public decimal CashProcessingFee { get; set; }

        [Required(ErrorMessage = "Penalty Fee Code is required")]
        [Display(Name = "Penalty Fee Code")]
        public string PenaltyFeeCode { get; set; }

        [Required(ErrorMessage = "Penalty Fee is required")]
        [Display(Name = "Penalty Fee")]
        public decimal PenaltyFee { get; set; }

        [Required(ErrorMessage = "Monthly Subscription Fee Code is required")]
        [Display(Name = "Monthly Subscription Fee Code")]
        public string MonthlySubscriptionFeeCode { get; set; }

        [Required(ErrorMessage = "Monthly Subscription Fee is required")]
        [Display(Name = "Monthly Subscription Fee")]
        public decimal MonthlySubscriptionFee { get; set; }

        [Required(ErrorMessage = "Min Monthly Fee Code is required")]
        [Display(Name = "Min Monthly Fee Code")]
        public string MinMonthlyFeeCode { get; set; }

        [Required(ErrorMessage = "Min Monthly Fee is required")]
        [Display(Name = "Min Monthly Fee")]
        public decimal MinMonthlyFee { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Min Code is required")]
        [Display(Name = "Volume Breaker Value Min Code")]
        public string VolumeBreakerValueMinCode { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Min Fee is required")]
        [Display(Name = "Volume Breaker Value Min Fee")]
        public decimal VolumeBreakerValueMinFee { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Max Code is required")]
        [Display(Name = "Volume Breaker Value Max Code")]
        public string VolumeBreakerValueMaxCode { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Max Fee is required")]
        [Display(Name = "Volume Breaker Value Max Fee")]
        public decimal VolumeBreakerValueMaxFee { get; set; }

        [Required(ErrorMessage = "Cash Device Rental Fee Code is required")]
        [Display(Name = "Cash Device Rental Fee Code")]
        public string CashDeviceRentalFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Device Rental Fee is required")]
        [Display(Name = "Cash Device Rental Fee")]
        public decimal CashDeviceRentalFee { get; set; }

        [Required(ErrorMessage = "Cash Device Risk Cover Code is required")]
        [Display(Name = "Cash Device Risk Cover Code")]
        public string CashDeviceRiskCoverCode { get; set; }

        [Required(ErrorMessage = "Cash Device Risk Cover Fee is required")]
        [Display(Name = "Cash Device Risk Cover Fee")]
        public decimal CashDeviceRiskCoverFee { get; set; }

        [Required(ErrorMessage = "Cash In Device Risk Cover Code is required")]
        [Display(Name = "Cash In Device Risk Cover Code")]
        public string CashInDeviceRiskCoverCode { get; set; }

        [Required(ErrorMessage = "Cash In Device Risk Cover Fee is required")]
        [Display(Name = "Cash In Device Risk Cover Fee")]
        public decimal CashInDeviceRiskCoverFee { get; set; }

        [Required(ErrorMessage = "Device Training Fee Code is required")]
        [Display(Name = "Device Training Fee Code")]
        public string DeviceTrainingFeeCode { get; set; }

        [Required(ErrorMessage = "Device Training Fee is required")]
        [Display(Name = "Device Training Fee")]
        public decimal DeviceTrainingFee { get; set; }

        [Required(ErrorMessage = "Cash Installation Fee Code is required")]
        [Display(Name = "Cash Installation Fee Code")]
        public string CashInstallationFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Installation Fee is required")]
        [Display(Name = "Cash Installation Fee")]
        public decimal CashInstallationFee { get; set; }

        [Required(ErrorMessage = "Device Movement Fee Code is required")]
        [Display(Name = "Device Movement Fee Code")]
        public string DeviceMovementFeeCode { get; set; }

        [Required(ErrorMessage = "Device Movement Fee is required")]
        [Display(Name = "Device Movement Fee")]
        public decimal DeviceMovementFee { get; set; }

    }
}
