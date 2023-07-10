using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public static class VolumeBreakerValueMin
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee() 
            
            {
                FeeId = 175,
                Code = "VBVN1",
                FeeTypeId = 1,
                Value = 1,
                Name = "Volume Breaker Value Min",
                Description = "Volume Breaker Value Min",
                LookUpKey = "VOLUME_BREAKER_VALUE_MIN",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}