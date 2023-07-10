using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CashOrderFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 21,
                Code = "400",
                FeeTypeId = 1,
                Value = 0,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
             {
                FeeId = 22,
                Code = "401",
                FeeTypeId = 1,
                Value = 1,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 23,
                Code = "402",
                FeeTypeId = 1,
                Value = 2,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 23,
                Code = "403",
                FeeTypeId = 1,
                Value = 3,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 24,
                Code = "404",
                FeeTypeId = 1,
                Value = 4,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 25,
                Code = "405",
                FeeTypeId = 1,
                Value = 5,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 26,
                Code = "406",
                FeeTypeId = 1,
                Value = 6,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 27,
                Code = "407",
                FeeTypeId = 1,
                Value = 7,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 28,
                Code = "408",
                FeeTypeId = 1,
                Value = 8,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 29,
                Code = "409",
                FeeTypeId = 1,
                Value = 9,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 30,
                Code = "410",
                FeeTypeId = 1,
                Value = 10,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 31,
                Code = "411",
                FeeTypeId = 1,
                Value = 11,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 32,
                Code = "412",
                FeeTypeId = 1,
                Value = 12,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 33,
                Code = "413",
                FeeTypeId = 1,
                Value = 13,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 34,
                Code = "414",
                FeeTypeId = 1,
                Value = 14,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 35,
                Code = "415",
                FeeTypeId = 1,
                Value = 15,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 36,
                Code = "416",
                FeeTypeId = 1,
                Value = 16,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 37,
                Code = "417",
                FeeTypeId = 1,
                Value = 17,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 38,
                Code = "418",
                FeeTypeId = 1,
                Value = 18,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 39,
                Code = "419",
                FeeTypeId = 1,
                Value = 19,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()
            {
                FeeId = 40,
                Code = "420",
                FeeTypeId = 1,
                Value = 20,
                Name = "Cash Order Fee",
                Description = "Cash Order Fee",
                LookUpKey = "CASH_ORDER_FEE",
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