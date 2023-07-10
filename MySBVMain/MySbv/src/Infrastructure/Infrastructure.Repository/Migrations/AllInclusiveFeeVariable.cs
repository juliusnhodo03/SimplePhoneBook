using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class AllInclusiveFeeVariable
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 170,
                Code = "AIFV1",
                FeeTypeId = 1,
                Value = 0,
                Name = "All Inclusive Fee Variable",
                Description = "All Inclusive Fee Variable",
                LookUpKey = "ALL_INCLUSIVE_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}