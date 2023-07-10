using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public static class VolumeBreakerValueMax
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee() 
            
            {
                FeeId = 176,
                Code = "VBVX1",
                FeeTypeId = 1,
                Value = 1,
                Name = "Volume Breaker Value Max",
                Description = "Volume Breaker Value Max",
                LookUpKey = "VOLUME_BREAKER_VALUE_MAX",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}