using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class DeviceTrainingFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 124,
                Code = "DTF1",
                FeeTypeId = 1,
                Value = 0,
                Name = "Device Training Fee",
                Description = "Device Training Fee",
                LookUpKey = "DEVICE_TRAINING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}