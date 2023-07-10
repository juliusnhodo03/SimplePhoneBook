using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class DeviceMovementFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 122,
                Code = "DM001",
                FeeTypeId = 1,
                Value = 0,
                Name = "Device Movement Fee",
                Description = "Device Movement Fee",
                LookUpKey = "DEVICE_MOVEMENT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}