using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class DeviceDeinstallationFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 120,
                Code = "DDF001",
                FeeTypeId = 1,
                Value = 0,
                Name = "Device Deinstallation Fee",
                Description = "Device Deinstallation Fee",
                LookUpKey = "DEVICE_DEINSTALLATION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}