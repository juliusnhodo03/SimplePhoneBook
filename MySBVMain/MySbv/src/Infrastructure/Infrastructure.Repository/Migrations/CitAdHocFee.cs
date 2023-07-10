using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CitAdHocFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 146,
                Code = "WEEK",
                FeeTypeId = 2,
                Value = 600,
                Name = "CIT Ad hoc Fee",
                Description = "CIT Ad hoc Fee",
                LookUpKey = "CIT_ADHOC_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },
            new Fee()
            {
                FeeId = 147,
                Code = "SATD",
                FeeTypeId = 2,
                Value = 600,
                Name = "CIT Ad hoc Fee",
                Description = "CIT Ad hoc Fee",
                LookUpKey = "CIT_ADHOC_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
                
            },

             new Fee()
            {
                FeeId = 148,
                Code = "SUND",
                FeeTypeId = 2,
                Value = 600,
                Name = "CIT Ad hoc Fee",
                Description = "CIT Ad hoc Fee",
                LookUpKey = "CIT_ADHOC_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
                
            },

             new Fee()
            {
                FeeId = 149,
                Code = "PHOL",
                FeeTypeId = 2,
                Value = 600,
                Name = "CIT Ad hoc Fee",
                Description = "CIT Ad hoc Fee",
                LookUpKey = "CIT_ADHOC_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
                
            
            });
        }
    }
}