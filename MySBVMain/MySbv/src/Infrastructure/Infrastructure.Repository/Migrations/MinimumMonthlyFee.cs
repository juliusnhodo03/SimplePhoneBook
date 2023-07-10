using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class MinimumMonthlyFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {

                FeeId = 49,
                Code = "700",
                FeeTypeId = 1,
                Value = 3000,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee
            {

                FeeId = 50,
                Code = "701",
                FeeTypeId = 1,
                Value = 3200,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee
            {

                FeeId = 51,
                Code = "702",
                FeeTypeId = 1,
                Value = 3300,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee
            {

                FeeId = 52,
                Code = "703",
                FeeTypeId = 1,
                Value = 3400,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee
            {

                FeeId = 53,
                Code = "704",
                FeeTypeId = 1,
                Value = 3700,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee
            {

                FeeId = 54,
                Code = "705",
                FeeTypeId = 1,
                Value = 3900,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee
            {

                FeeId = 55,
                Code = "706",
                FeeTypeId = 2,
                Value = 400,
                Name = "Minimum Monthly Fee",
                Description = "Minimum Monthly Fee",
                LookUpKey = "MINIMUM_MONTHLY_FEE",
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