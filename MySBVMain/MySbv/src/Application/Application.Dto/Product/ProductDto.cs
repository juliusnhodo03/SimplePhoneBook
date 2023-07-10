using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using Application.Dto.CashDeposit;
using Application.Dto.Device;
using Application.Dto.DeviceType;
using Application.Dto.ProductType;

namespace Application.Dto.Product
{
    public class ProductDto
    {
        public int ProductId { get; set; }

        [Display(Name = "Product Type")]
        public int ProductTypeId { get; set; }

        [Display(Name = "Device Name")]
        public int? DeviceId { get; set; }

        [Display(Name = "Device Name")]
        public int? DeviceTypeId { get; set; }

        [Display(Name = "Site Name")]
        public int SiteId { get; set; }

        [Display(Name = "Device Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Cit Code")]
        public string CitCode { get; set; }

        [Display(Name = "Status Name")]
        public int StatusId { get; set; }

        [Display(Name = "Service Type")]
        public int ServiceTypeId { get; set; }

        [Display(Name = "Settlement Type")]
        public int SettlementTypeId { get; set; }
       
        [Display(Name = "Description")]
        public string ProductDescription { get; set; }
        
        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Implementation Date")]
        public string ImplementationDateString { get; set; }

        public DateTime ImplementationDate { get; set; }

        //[DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Termination Date")]
        public string TerminationDateString { get; set; }

        public DateTime? TerminationDate { get; set; }

        /// <summary>
        /// CIT
        /// /// </summary>
        [Display(Name = "Public Holiday Incl")]
        public bool PublicHolidayInclInFeeFlag { get; set; }

        //[Required(ErrorMessage = "Monthly Cit Fee Code is required")]
        [Display(Name = "Monthly Cit Fee Code")]
        public string MonthlyCitFeeCode { get; set; }

        //[Required(ErrorMessage = "Monthly Cit Fee is required")]
        [Display(Name = "Monthly Cit Fee")]
        public double MonthlyCitFee { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Fixed Code is required")]
        [Display(Name = "Monthly Risk Fee Fixed Code")]
        public string MonthlyRiskFeeFixedCode { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Fixed Fee is required")]
        [Display(Name = "Monthly Risk Fee Fixed Fee")]
        public double MonthlyRiskFeeFixedFee { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Variable is required")]
        [Display(Name = "Monthly Risk Fee Variable")]
        public string MonthlyRiskFeeVariable { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Variable Fee is required")]
        [Display(Name = "Monthly Risk Fee Variable Fee")]
        public double MonthlyRiskFeeVariableFee { get; set; }

        //[Required(ErrorMessage = "Cit Ad Hoc Fee Code is required")]
        [Display(Name = "Cit Ad Hoc Fee Code")]
        public string CitAdHocFeeCode { get; set; }

        //[Required(ErrorMessage = "Cit Ad Hoc Fee is required")]
        [Display(Name = "Cit Ad Hoc Fee")]
        public double CitAdHocFee { get; set; }

        //[Required(ErrorMessage = "Max Cash Daily ValueCode is required")]
        [Display(Name = "Max Cash Daily ValueCode")]
        public string MaxCashDailyValueCode { get; set; }

        //[Required(ErrorMessage = "Max Cash Daily ValueFee is required")]
        [Display(Name = "Max Cash Daily ValueFee")]
        public double MaxCashDailyValueFee { get; set; }

        //[Required(ErrorMessage = "All Inclusive Product FeeCode is required")]
        [Display(Name = "All Inclusive Product FeeCode")]
        public string AllInclusiveProductFeeCode { get; set; }

        //[Required(ErrorMessage = "All Inclusive Product Fee is required")]
        [Display(Name = "All Inclusive Product Fee")]
        public double   AllInclusiveProductFee { get; set; }
        


        /// <summary>
        /// VAULT
        /// </summary>
        [Required(ErrorMessage = "Device Supplier is required")]
        [Display(Name = "Device Supplier")]
        public int DeviceSupplierId { get; set; }
        
        [Display(Name = "Deposit Reference Editable")]
        public bool DepositReferenceEditable { get; set; }

        [Required(ErrorMessage = "Site Deposit Reference is required")]
        [Display(Name = "Site Deposit Reference")]
        public string SiteDepositReference { get; set; }

        [Required(ErrorMessage = "Cash Processing Fee Code is required")]
        [Display(Name = "Cash Processing Fee Code")]
        public string CashProcessingFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Processing Fee is required")]
        [Display(Name = "Vault Cash Processing Fee")]
        public Double VaultCashProcessingFee { get; set; }

        [Required(ErrorMessage = "Cash Processing Fee Code is required")]
        [Display(Name = "Vault Cash Processing Fee Code")]
        public string VaultCashProcessingFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Processing Fee is required")]
        [Display(Name = "Cash Processing Fee")]
        public Double CashProcessingFee { get; set; }

        [Required(ErrorMessage = "Penalty Fee Code is required")]
        [Display(Name = "Penalty Fee Code")]
        public string PenaltyFeeCode { get; set; }

        [Required(ErrorMessage = "Penalty Fee is required")]
        [Display(Name = "Penalty Fee")]
        public double PenaltyFee { get; set; }

        [Required(ErrorMessage = "Monthly Subscription Fee Code is required")]
        [Display(Name = "Monthly Subscription Fee Code")]
        public string MonthlySubscriptionFeeCode { get; set; }

        [Required(ErrorMessage = "Monthly Subscription Fee is required")]
        [Display(Name = "Monthly Subscription Fee")]
        public double MonthlySubscriptionFee { get; set; }

        [Required(ErrorMessage = "Min Monthly Fee Code is required")]
        [Display(Name = "Min Monthly Fee Code")]
        public string MinMonthlyFeeCode { get; set; }

        [Required(ErrorMessage = "Min Monthly Fee is required")]
        [Display(Name = "Min Monthly Fee")]
        public double MinMonthlyFee { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Min Code is required")]
        [Display(Name = "Volume Breaker Value Min Code")]
        public string VolumeBreakerValueMinCode { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Min Fee is required")]
        [Display(Name = "Volume Breaker Value Min Fee")]
        public double VolumeBreakerValueMinFee { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Max Code is required")]
        [Display(Name = "Volume Breaker Value Max Code")]
        public string VolumeBreakerValueMaxCode { get; set; }

        [Required(ErrorMessage = "Volume Breaker Value Max Fee is required")]
        [Display(Name = "Volume Breaker Value Max Fee")]
        public double VolumeBreakerValueMaxFee { get; set; }

        [Required(ErrorMessage = "Cash Device Rental Fee Code is required")]
        [Display(Name = "Cash Device Rental Fee Code")]
        public string CashDeviceRentalFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Device Rental Fee is required")]
        [Display(Name = "Cash Device Rental Fee")]
        public double CashDeviceRentalFee { get; set; }

        [Required(ErrorMessage = "Cash Device Risk Cover Code is required")]
        [Display(Name = "Cash Device Risk Cover Code")]
        public string CashDeviceRiskCoverCode { get; set; }

        [Required(ErrorMessage = "Cash Device Risk Cover Fee is required")]
        [Display(Name = "Cash Device Risk Cover Fee")]
        public double CashDeviceRiskCoverFee { get; set; }

        [Required(ErrorMessage = "Cash In Device Risk Cover Code is required")]
        [Display(Name = "Cash In Device Risk Cover Code")]
        public string CashInDeviceRiskCoverCode { get; set; }

        [Required(ErrorMessage = "Cash In Device Risk Cover Fee is required")]
        [Display(Name = "Cash In Device Risk Cover Fee")]
        public double CashInDeviceRiskCoverFee { get; set; }

        [Required(ErrorMessage = "Device Training Fee Code is required")]
        [Display(Name = "Device Training Fee Code")]
        public string DeviceTrainingFeeCode { get; set; }

        [Required(ErrorMessage = "Device Training Fee is required")]
        [Display(Name = "Device Training Fee")]
        public double DeviceTrainingFee { get; set; }

        [Required(ErrorMessage = "Cash Installation Fee Code is required")]
        [Display(Name = "Cash Installation Fee Code")]
        public string CashInstallationFeeCode { get; set; }

        [Required(ErrorMessage = "Cash Installation Fee is required")]
        [Display(Name = "Cash Installation Fee")]
        public double CashInstallationFee { get; set; }

        [Required(ErrorMessage = "Device Movement Fee Code is required")]
        [Display(Name = "Device Movement Fee Code")]
        public string DeviceMovementFeeCode { get; set; }

        [Required(ErrorMessage = "Device Movement Fee is required")]
        [Display(Name = "Device Movement Fee")]
        public double DeviceMovementFee { get; set; }



        /// <summary>
        /// DEPOSIT
        /// </summary>

        //[Required(ErrorMessage = "Cash Order Fee Code is required")]
        [Display(Name = "Cash Order Fee Code")]
        public string CashOrderFeeCode { get; set; }

        //[Required(ErrorMessage = "Cash Order Fee is required")]
        [Display(Name = "Cash Order Fee")]
        public double CashOrderFee { get; set; }

        //[Required(ErrorMessage = "Training Fee Code is required")]
        [Display(Name = "Training Fee Code")]
        public string TrainingFeeCode { get; set; }

        //[Required(ErrorMessage = "Training Fee is required")]
        [Display(Name = "Training Fee")]
        public double TrainingFee { get; set; }


        //[Required(ErrorMessage = "Volume Breaker Fee Below Code is required")]
        [Display(Name = "Volume Breaker Fee Below Code")]
        public string VolumeBreakerFeeBelowCode { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Below is required")]
        [Display(Name = "Volume Breaker Fee Below")]
        public double VolumeBreakerFeeBelowFee { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Above Code is required")]
        [Display(Name = "Volume Breaker Fee Above Code")]
        public string VolumeBreakerFeeAboveCode { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Above is required")]
        [Display(Name = "Volume Breaker Fee Above")]
        public double VolumeBreakerFeeAboveFee { get; set; }


        
        [Display(Name = "Product Type")]
        public string ProductTypeName { get; set; }

        [Display(Name = "Service Type")]
        public string ServiceTypeName { get; set; }

        [Display(Name = "Settlement Type")]
        public string SettlementTypeName { get; set; }

        [Display(Name = "Status")]
        public string StatusName { get; set; }

        [Display(Name = "Site")]
        public string SiteName { get; set; }

        public DeviceTypeDto  DeviceType { get; set; }
        public DeviceDto Device { get; set; }
        public ProductTypeDto ProductType { get; set; }
        public ServiceTypeDto ServiceType { get; set; }
        public SettlementTypeDto SettlementType { get; set; }
        public StatusDto Status { get; set; }
        public List<string> FeeCodes { get; set; }
        public List<ProductFeeDto> ProductFees { get; set; }
    }
}
