using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Product
{
    public class DepositFeeDto
    {

        [Display(Name = "Deposit Reference Editable")]
        public bool DepositReferenceEditable { get; set; }

        [Display(Name = "Approval Required")]
        public bool ApprovalRequiredFlag { get; set; }

        //[Required(ErrorMessage = "Deposit Reference is required")]
        [Display(Name = "Deposit Reference")]
        public string SiteDepositReference { get; set; }

        //[Required(ErrorMessage = "Settlement Time is required")]
        [Display(Name = "Settlement Time")]
        public DateTime SettlementTime { get; set; }

        //[Required(ErrorMessage = "Cash Processing Fee Code is required")]
        [Display(Name = "Cash Processing Fee Code")]
        public string CashProcessingFeeCode { get; set; }

        //[Required(ErrorMessage = "Cash Processing Fee is required")]
        [Display(Name = "Cash Processing Fee")]
        public decimal CashProcessingFee { get; set; }

        //[Required(ErrorMessage = "Cash Order Fee Code is required")]
        [Display(Name = "Cash Order Fee Code")]
        public string CashOrderFeeCode { get; set; }

        //[Required(ErrorMessage = "Cash Order Fee is required")]
        [Display(Name = "Cash Order Fee")]
        public decimal CashOrderFee { get; set; }

        //[Required(ErrorMessage = "Monthly Subscription Fee Code is required")]
        [Display(Name = "Monthly Subscription Fee Code")]
        public string MonthlySubscriptionFeeCode { get; set; }

        //[Required(ErrorMessage = "Monthly Subscription Fee is required")]
        [Display(Name = "Monthly Subscription Fee")]
        public decimal MonthlySubscriptionFee { get; set; }

        //[Required(ErrorMessage = "MinMonthly Fee Code is required")]
        [Display(Name = "MinMonthly Fee Code")]
        public string MinMonthlyFeeCode { get; set; }

        //[Required(ErrorMessage = "MinMonthly Fee is required")]
        [Display(Name = "MinMonthly Fee")]
        public decimal MinMonthlyFee { get; set; }

        //[Required(ErrorMessage = "Training Fee Code is required")]
        [Display(Name = "Training Fee Code")]
        public string TrainingFeeCode { get; set; }

        //[Required(ErrorMessage = "Training Fee is required")]
        [Display(Name = "Training Fee")]
        public decimal TrainingFee { get; set; }

        //[Required(ErrorMessage = "Penalty Fee Code is required")]
        [Display(Name = "Penalty Fee Code")]
        public string PenaltyFeeCode { get; set; }

        //[Required(ErrorMessage = "Penalty Fee is required")]
        [Display(Name = "Penalty Fee")]
        public decimal PenaltyFee { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Value MinCode is required")]
        [Display(Name = "Volume Breaker Value MinCode")]
        public string VolumeBreakerValueMinCode { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Value MinFee is required")]
        [Display(Name = "Volume Breaker Value MinFee")]
        public decimal VolumeBreakerValueMinFee { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Value MaxCode is required")]
        [Display(Name = "Volume Breaker Value MaxCode")]
        public string VolumeBreakerValueMaxCode { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Value MaxFee is required")]
        [Display(Name = "Volume Breaker Value MaxFee")]
        public decimal VolumeBreakerValueMaxFee { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Below Code is required")]
        [Display(Name = "Volume Breaker Fee BelowCode")]
        public string VolumeBreakerFeeBelowCode { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Below is required")]
        [Display(Name = "Volume Breaker FeeBelow")]
        public decimal VolumeBreakerFeeBelowFee { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Above Code is required")]
        [Display(Name = "Volume Breaker Fee AboveCode")]
        public string VolumeBreakerFeeAboveCode { get; set; }

        //[Required(ErrorMessage = "Volume Breaker Fee Above is required")]
        [Display(Name = "Volume Breaker FeeAbove")]
        public decimal VolumeBreakerFeeAboveFee { get; set; }
    }

}
