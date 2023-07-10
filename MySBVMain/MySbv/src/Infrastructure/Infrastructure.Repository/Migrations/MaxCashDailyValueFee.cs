using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class MaxCashDailyValueFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 171,
                Code = "MCDV1",
                FeeTypeId = 1,
                Value = 0,
                Name = "Max Cash Daily Value Fee",
                Description = "Max Cash Daily Value Fee",
                LookUpKey = "MAX_CASH_DAILY_VALUE_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            });
        }
    }
}