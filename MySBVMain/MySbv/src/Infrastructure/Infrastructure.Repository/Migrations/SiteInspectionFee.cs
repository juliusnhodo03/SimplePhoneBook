using System;
using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class SiteInspectionFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 121,
                Code = "SIF",
                FeeTypeId = 1,
                Value = 0,
                Name = "Site Inspection Fee",
                Description = "Site Inspection Fee",
                LookUpKey = "SITE_INSPECTION_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            });
        }
    }
}