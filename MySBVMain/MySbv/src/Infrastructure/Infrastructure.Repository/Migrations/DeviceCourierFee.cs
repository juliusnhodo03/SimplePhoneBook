using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class DeviceCourierFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 123,
                Code = "DCF",
                FeeTypeId = 1,
                Value = 0,
                Name = "Device Courier Fee",
                Description = "Device Courier Fee",
                LookUpKey = "DEVICE_COURIER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}