using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CitFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 125,
                Code = "1W001",
                FeeTypeId = 1,
                Value = 1220.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },
            new Fee()
            {
                FeeId = 126,
                Code = "1W002",
                FeeTypeId = 1,
                Value = 1250.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },
                new Fee()
            {
                FeeId = 127,
                Code = "1W003",
                FeeTypeId = 1,
                Value = 1300.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 128,
                Code = "1W004",
                FeeTypeId = 1,
                Value = 1350.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 130,
                Code = "1W005",
                FeeTypeId = 1,
                Value = 1400.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 131,
                Code = "1W006",
                FeeTypeId = 1,
                Value = 1450.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 132,
                Code = "1W007",
                FeeTypeId = 1,
                Value = 1500.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 133,
                Code = "1W008",
                FeeTypeId = 1,
                Value = 1550.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 134,
                Code = "1W009",
                FeeTypeId = 1,
                Value = 1600.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 135,
                Code = "1W010",
                FeeTypeId = 1,
                Value = 1650.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 136,
                Code = "1W011",
                FeeTypeId = 1,
                Value = 1700.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 137,
                Code = "1W012",
                FeeTypeId = 1,
                Value = 1750.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 138,
                Code = "1W013",
                FeeTypeId = 1,
                Value = 1800.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

             new Fee()
            {
                FeeId = 139,
                Code = "2W001",
                FeeTypeId = 1,
                Value = 1540.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            new Fee()
            {
                FeeId = 140,
                Code = "2W002",
                FeeTypeId = 1,
                Value = 1600.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            new Fee()
            {
                FeeId = 141,
                Code = "2W003",
                FeeTypeId = 1,
                Value = 1650.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            
            new Fee()
            {
                FeeId = 142,
                Code = "2W004",
                FeeTypeId = 1,
                Value = 1710.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            
            new Fee()
            {
                FeeId = 143,
                Code = "2W005",
                FeeTypeId = 1,
                Value = 1750.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            
            new Fee()
            {
                FeeId = 144,
                Code = "2W006",
                FeeTypeId = 1,
                Value = 1800.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            
            new Fee()
            {
                FeeId = 145,
                Code = "2W007",
                FeeTypeId = 1,
                Value = 1850.00,
                Name = "CIT Fee",
                Description = "CIT Fee",
                LookUpKey = "CIT_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            


            });
        }
    }
}