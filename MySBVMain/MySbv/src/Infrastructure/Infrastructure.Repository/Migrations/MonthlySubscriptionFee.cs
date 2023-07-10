using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class MonthlySubscriptionFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 45,
                Code = "650",
                FeeTypeId = 1,
                Value = 200,
                Name = "Monthly Subscription Fee",
                Description = "Monthly Subscription Fee",
                LookUpKey = "MONTHLY_SUBSCRIPTION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 46,
                Code = "651",
                FeeTypeId = 1,
                Value = 250,
                Name = "Monthly Subscription Fee",
                Description = "Monthly Subscription Fee",
                LookUpKey = "MONTHLY_SUBSCRIPTION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 47,
                Code = "652",
                FeeTypeId = 1,
                Value = 300,
                Name = "Monthly Subscription Fee",
                Description = "Monthly Subscription Fee",
                LookUpKey = "MONTHLY_SUBSCRIPTION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 48,
                Code = "653",
                FeeTypeId = 1,
                Value = 350,
                Name = "Monthly Subscription Fee",
                Description = "Monthly Subscription Fee",
                LookUpKey = "MONTHLY_SUBSCRIPTION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            
            );
        }
    }
}