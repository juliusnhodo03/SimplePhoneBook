using System;
using Infrastructure.Repository.Database;
using Domain.Data.Model;
using System.Data.Entity.Migrations;

namespace Infrastructure.Repository.Migrations
{
    public class CashProcessingFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee() 
            {
                FeeId = 1,
                Code = "100",
                FeeTypeId = 1,
                Value = 0,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
           
            {
                FeeId = 2,
                Code = "101",
                FeeTypeId = 1,
                Value = 1,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }
            , new Fee()

            {
                FeeId = 3,
                Code = "102",
                FeeTypeId = 1,
                Value = 2,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 4,
                Code = "103",
                FeeTypeId = 1,
                Value = 3,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

              

            }
            , new Fee()

            {
                FeeId = 5,
                Code = "104",
                FeeTypeId = 1,
                Value = 4,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 6,
                Code = "105",
                FeeTypeId = 1,
                Value = 5,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 7,
                Code = "106",
                FeeTypeId = 1,
                Value = 6,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 8,
                Code = "107",
                FeeTypeId = 1,
                Value = 7,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 9,
                Code = "108",
                FeeTypeId = 1,
                Value = 8,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 10,
                Code = "109",
                FeeTypeId = 1,
                Value = 9,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 11,
                Code = "110",
                FeeTypeId = 1,
                Value = 10,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 12,
                Code = "111",
                FeeTypeId = 1,
                Value = 11,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 13,
                Code = "112",
                FeeTypeId = 1,
                Value = 12,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 14,
                Code = "114",
                FeeTypeId = 1,
                Value = 14,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 15,
                Code = "115",
                FeeTypeId = 1,
                Value = 15,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 16,
                Code = "116",
                FeeTypeId = 1,
                Value = 16,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 17,
                Code = "117",
                FeeTypeId = 1,
                Value = 17,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 18,
                Code = "118",
                FeeTypeId = 1,
                Value = 18,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 19,
                Code = "119",
                FeeTypeId = 1,
                Value = 19,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now



            }
            , new Fee()

            {
                FeeId = 20,
                Code = "120",
                FeeTypeId = 1,
                Value = 20,
                Name = "Cash Processing Fee",
                Description = "Cash Processing Fee",
                LookUpKey = "CASH_PROCESSING_FEE",
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