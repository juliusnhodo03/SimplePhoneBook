using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto.Product
{
    public class CitFeeDto
    {
        [Display(Name = "Public Holiday Incl")]
        public bool PublicHolidayInclInFeeFlag { get; set; }

        //[Required(ErrorMessage = "Monthly Cit Fee Code is required")]
        [Display(Name = "Monthly Cit Fee Code")]
        public string MonthlyCitFeeCode { get; set; }

        //[Required(ErrorMessage = "Monthly Cit Fee is required")]
        [Display(Name = "Monthly Cit Fee")]
        public decimal MonthlyCitFee { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Fixed Code is required")]
        [Display(Name = "Monthly Risk Fee Fixed Code")]
        public string MonthlyRiskFeeFixedCode { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Fixed Fee is required")]
        [Display(Name = "Monthly Risk Fee Fixed Fee")]
        public decimal MonthlyRiskFeeFixedFee { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Variable is required")]
        [Display(Name = "Monthly Risk Fee Variable")]
        public string MonthlyRiskFeeVariable { get; set; }

        //[Required(ErrorMessage = "Monthly Risk Fee Variable Fee is required")]
        [Display(Name = "Monthly Risk Fee Variable Fee")]
        public decimal MonthlyRiskFeeVariableFee { get; set; }

        //[Required(ErrorMessage = "Cit Ad Hoc Fee Code is required")]
        [Display(Name = "Cit Ad Hoc Fee Code")]
        public string CitAdHocFeeCode { get; set; }

        //[Required(ErrorMessage = "Cit Ad Hoc Fee is required")]
        [Display(Name = "Cit Ad Hoc Fee")]
        public decimal CitAdHocFee { get; set; }

        //[Required(ErrorMessage = "Max Cash Daily ValueCode is required")]
        [Display(Name = "Max Cash Daily ValueCode")]
        public string MaxCashDailyValueCode { get; set; }

        //[Required(ErrorMessage = "Max Cash Daily ValueFee is required")]
        [Display(Name = "Max Cash Daily ValueFee")]
        public decimal MaxCashDailyValueFee { get; set; }

        //[Required(ErrorMessage = "All Inclusive Product FeeCode is required")]
        [Display(Name = "All Inclusive Product FeeCode")]
        public string AllInclusiveProductFeeCode { get; set; }

        //[Required(ErrorMessage = "All Inclusive Product Fee is required")]
        [Display(Name = "All Inclusive Product Fee")]
        public decimal AllInclusiveProductFee { get; set; }
    }

}
