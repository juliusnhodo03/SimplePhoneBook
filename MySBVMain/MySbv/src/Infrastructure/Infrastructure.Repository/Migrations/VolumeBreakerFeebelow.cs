using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class VolumeBreakerFeebelow
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee() 
            
            {
                FeeId = 56,
                Code = "800",
                FeeTypeId = 1,
                Value = 1,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            
            } , new Fee()

            {
                FeeId = 57,
                Code = "801",
                FeeTypeId = 1,
                Value = 2,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            } , new Fee()
             {
                FeeId =58,
                Code = "802",
                FeeTypeId = 1,
                Value = 3,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 59,
                Code = "803",
                FeeTypeId = 1,
                Value = 4,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 61,
                Code = "804",
                FeeTypeId = 1,
                Value = 5,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 62,
                Code = "805",
                FeeTypeId = 1,
                Value = 6,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 63,
                Code = "806",
                FeeTypeId = 1,
                Value = 7,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 64,
                Code = "807",
                FeeTypeId = 1,
                Value = 8,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 65,
                Code = "808",
                FeeTypeId = 1,
                Value = 9,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 66,
                Code = "809",
                FeeTypeId = 1,
                Value = 10,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {
                FeeId = 67,
                Code = "810",
                FeeTypeId = 2,
                Value = 2,
                Name = "Volume Breaker Fee below",
                Description = "Volume Breaker Fee below",
                LookUpKey = "VOULME_BREAKER_FEE_BELOW",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            );
        }
    }
}