using Domain.Data.Model;
using Infrastructure.Repository.Database;
using System;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public static class VolumeBreakerFeeAbove
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee() 
            
            {
                FeeId = 68,
                Code = "850",
                FeeTypeId = 1,
                Value = 1,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee
            {
                FeeId = 69,
                Code = "851",
                FeeTypeId = 1,
                Value = 2,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },  new Fee()
            {
                FeeId = 70,
                Code = "852",
                FeeTypeId = 1,
                Value = 3,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 71,
                Code = "853",
                FeeTypeId = 1,
                Value = 4,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 72,
                Code = "854",
                FeeTypeId = 1,
                Value = 5,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 73,
                Code = "855",
                FeeTypeId = 1,
                Value = 6,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 74,
                Code = "856",
                FeeTypeId = 1,
                Value = 7,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 75,
                Code = "857",
                FeeTypeId = 1,
                Value = 8,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 76,
                Code = "858",
                FeeTypeId = 1,
                Value = 9,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 77,
                Code = "859",
                FeeTypeId = 1,
                Value = 10,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 78,
                Code = "860",
                FeeTypeId = 2,
                Value = 2,
                Name = "Volume Breaker Fee Above",
                Description = "Volume Breaker Fee Above",
                LookUpKey = "VOLUME_BREAKER_FEE_ABOVE",
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