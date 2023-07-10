using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class DeviceInstallationFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 119,
                Code = "DV001",
                FeeTypeId = 1,
                Value = 0,
                Name = "Device Installation Fee",
                Description = "Device Installation Fee",
                LookUpKey = "DEVICE_INSTALLATION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}