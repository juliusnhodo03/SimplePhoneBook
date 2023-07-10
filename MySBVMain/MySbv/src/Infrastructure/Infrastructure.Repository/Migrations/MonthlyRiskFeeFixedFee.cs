using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class MonthlyRiskFeeFixedFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 172,
                Code = "MRFF",
                FeeTypeId = 1,
                Value = 0,
                Name = "Monthly Risk Fee Fixed Fee",
                Description = "Monthly Risk Fee Fixed Fee",
                LookUpKey = "MONTHLY_RISK_FEE_FIXED_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            });
        }
    }
}