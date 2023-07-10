using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CashDeviceRentalFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 89,
                Code = "GEM01",
                FeeTypeId = 1,
                Value = 4550,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 90,
                Code = "GEM02",
                FeeTypeId = 1,
                Value = 4600,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

            {
                FeeId = 91,
                Code = "GEM3",
                FeeTypeId = 1,
                Value = 4650,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()

            {
                FeeId = 92,
                Code = "GEM04",
                FeeTypeId = 1,
                Value = 4700,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 93,
                Code = "GEM05",
                FeeTypeId = 1,
                Value = 4750,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 94,
                Code = "GEM06",
                FeeTypeId = 1,
                Value = 4800,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 95,
                Code = "GEM07",
                FeeTypeId = 1,
                Value = 4850,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 96,
                Code = "GEM08",
                FeeTypeId = 1,
                Value = 4900,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

            {
                FeeId = 97,
                Code = "GEM09",
                FeeTypeId = 1,
                Value = 4950,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

            {
                FeeId = 98,
                Code = "GEM10",
                FeeTypeId = 1,
                Value = 5000,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

            {
                FeeId = 99,
                Code = "GEM11",
                FeeTypeId = 1,
                Value = 5050,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 100,
                Code = "GEM12",
                FeeTypeId = 1,
                Value = 5100,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 101,
                Code = "GEM13",
                FeeTypeId = 1,
                Value = 5150,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 102,
                Code = "GEM14",
                FeeTypeId = 1,
                Value = 5200,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 103,
                Code = "GEM15",
                FeeTypeId = 1,
                Value = 5250,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 104,
                Code = "GEM16",
                FeeTypeId = 1,
                Value = 5300,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

            {
                FeeId = 105,
                Code = "GEM17",
                FeeTypeId = 1,
                Value = 5350,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 106,
                Code = "GE001",
                FeeTypeId = 1,
                Value = 4550,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 106,
                Code = "GE002",
                FeeTypeId = 1,
                Value = 4600,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {
                FeeId = 107,
                Code = "GE003",
                FeeTypeId = 1,
                Value = 4650,
                Name = "Cash Device Rental Fee",
                Description = "Cash Device Rental Fee",
                LookUpKey = "CASH_DEVICE_RENTAL_FEE",
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