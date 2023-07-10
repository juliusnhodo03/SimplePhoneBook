using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CashDeviceRiskCover
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 108,
                Code = "950",
                FeeTypeId = 1,
                Value = 240,
                Name = " Cash Device Risk Cover",
                Description = " Cash Device Risk Cover",
                LookUpKey = "CASH_DEVICE_RISK_COVER",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 109,
                Code = "951",
                FeeTypeId = 1,
                Value = 300,
                Name = " Cash Device Risk Cover",
                Description = " Cash Device Risk Cover",
                LookUpKey = "CASH_DEVICE_RISK_COVER",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 110,
                Code = "952",
                FeeTypeId = 1,
                Value = 200,
                Name = " Cash Device Risk Cover",
                Description = " Cash Device Risk Cover",
                LookUpKey = "CASH_DEVICE_RISK_COVER",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 111,
                Code = "953",
                FeeTypeId = 1,
                Value = 340,
                Name = " Cash Device Risk Cover",
                Description = " Cash Device Risk Cover",
                LookUpKey = "CASH_DEVICE_RISK_COVER",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee() 
            
            {

                FeeId = 112,
                Code = "954",
                FeeTypeId = 1,
                Value = 400,
                Name = " Cash Device Risk Cover",
                Description = " Cash Device Risk Cover",
                LookUpKey = "CASH_DEVICE_RISK_COVER",
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