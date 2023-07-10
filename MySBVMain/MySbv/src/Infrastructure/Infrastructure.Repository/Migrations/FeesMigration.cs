using Infrastructure.Repository.Database;

namespace Infrastructure.Repository.Migrations
{
    public static class FeesMigration
    {
        public static void SeedFees(Context context)
        {
            //// J
            CashProcessingFee.Seed(context);
            CashOrderFee.Seed(context);
            PenaltyFee.Seed(context);
            MonthlySubscriptionFee.Seed(context);
            MinimumMonthlyFee.Seed(context);
            VolumeBreakerFeebelow.Seed(context);
            VolumeBreakerFeeAbove.Seed(context);

            ////// N
            VaultCashProcessingFee.Seed(context);
            CashDeviceRentalFee.Seed(context);
            CashDeviceRiskCover.Seed(context);
            CashInDevice.Seed(context);
            DeviceInstallationFee.Seed(context);
            DeviceDeinstallationFee.Seed(context);
            SiteInspectionFee.Seed(context);

            ////// T
            DeviceMovementFee.Seed(context);
            DeviceCourierFee.Seed(context);
            DeviceTrainingFee.Seed(context);
            CitFee.Seed(context);
            CitAdHocFee.Seed(context);
            CitRiskFeeVariable.Seed(context);
            AllInclusiveFeeVariable.Seed(context);
            MonthlyRiskFeeFixedFee.Seed(context);
            MaxCashDailyValueFee.Seed(context);
            TrainingFee.Seed(context);
            VolumeBreakerValueMin.Seed(context);
            VolumeBreakerValueMax.Seed(context);
        }
    }
}