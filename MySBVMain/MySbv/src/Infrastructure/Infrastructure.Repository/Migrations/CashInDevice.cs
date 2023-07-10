using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CashInDevice
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 113,
                Code = "1000",
                FeeTypeId = 1,
                Value = 2,
                Name = "Cash in Device",
                Description = "Cash in Device",
                LookUpKey = "CASH_IN_DEVICE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()

            {
                FeeId = 114,
                Code = "1001",
                FeeTypeId = 1,
                Value = 2.5,
                Name = "Cash in Device",
                Description = "Cash in Device",
                LookUpKey = "CASH_IN_DEVICE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 115,
                Code = "1002",
                FeeTypeId = 1,
                Value = 3,
                Name = "Cash in Device",
                Description = "Cash in Device",
                LookUpKey = "CASH_IN_DEVICE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 116,
                Code = "1003",
                FeeTypeId = 1,
                Value = 4,
                Name = "Cash in Device",
                Description = "Cash in Device",
                LookUpKey = "CASH_IN_DEVICE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 117,
                Code = "1004",
                FeeTypeId = 1,
                Value = 4.5,
                Name = "Cash in Device",
                Description = "Cash in Device",
                LookUpKey = "CASH_IN_DEVICE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee() 
            {
                FeeId = 118,
                Code = "1005",
                FeeTypeId = 1,
                Value = 6,
                Name = "Cash in Device",
                Description = "Cash in Device",
                LookUpKey = "CASH_IN_DEVICE",
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