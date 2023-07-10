using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class CitRiskFeeVariable
    {
        public static void Seed(Context context)
        {
             context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 150,
                Code = "1150",
                FeeTypeId = 1,
                Value = 2,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            },

            new Fee()
            {
               FeeId = 151,
                Code = "1151",
                FeeTypeId = 1,
                Value = 2.5,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 152,
                Code = "1152",
                FeeTypeId = 1,
                Value = 3,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 153,
                Code = "1153",
                FeeTypeId = 1,
                Value = 4,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 154,
                Code = "1154",
                FeeTypeId = 1,
                Value = 4.5,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 155,
                Code = "1155",
                FeeTypeId = 1,
                Value = 6,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 156,
                Code = "1200",
                FeeTypeId = 1,
                Value = 18,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 157,
                Code = "1201",
                FeeTypeId = 1,
                Value = 19,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },
            
            new Fee()
            {
               FeeId = 158,
                Code = "1202",
                FeeTypeId = 1,
                Value = 20,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 159,
                Code = "1203",
                FeeTypeId = 1,
                Value = 21,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 160,
                Code = "1204",
                FeeTypeId = 1,
                Value = 22,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 161,
                Code = "1205",
                FeeTypeId = 1,
                Value = 23,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 162,
                Code = "1206",
                FeeTypeId = 1,
                Value = 24,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 163,
                Code = "1207",
                FeeTypeId = 1,
                Value = 25,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 164,
                Code = "1208",
                FeeTypeId = 1,
                Value = 26,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 165,
                Code = "1209",
                FeeTypeId = 1,
                Value = 27,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 166,
                Code = "1210",
                FeeTypeId = 1,
                Value = 28,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 167,
                Code = "1211",
                FeeTypeId = 1,
                Value = 29,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 168,
                Code = "1212",
                FeeTypeId = 1,
                Value = 30,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            },

            new Fee()
            {
               FeeId = 169,
                Code = "1213",
                FeeTypeId = 1,
                Value = 31,
                Name = "CIT Risk Fee - Variable",
                Description = "CIT Risk Fee - Variable",
                LookUpKey = "CIT_RISK_FEE_VARIABLE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now 
            

            });
        }
    }
}