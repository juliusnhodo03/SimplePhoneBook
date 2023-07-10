using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class VaultCashProcessingFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee()
            {
                FeeId = 79,
                Code = "1400",
                FeeTypeId = 1,
                Value = 0,
                Name = "Vault Cash Processing Fee",
                Description = "Vault Cash Processing Fee",
                LookUpKey = "VAULT_CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()
            {
                FeeId = 80,
                Code = "1401",
                FeeTypeId = 1,
                Value = 1,
                Name = "Vault Cash Processing Fee",
                Description = "Vault Cash Processing Fee",
                LookUpKey = "VAULT_CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

            {
                FeeId = 81,
                Code = "1402",
                FeeTypeId = 1,
                Value = 2,
                Name = "Vault Cash Processinf Fee",
                Description = "Vault Cash Processing Fee",
                LookUpKey = "VAULT_CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now


            }, new Fee()
            {
                FeeId = 82,
                Code = "1403",
                FeeTypeId = 1,
                Value = 3,
                Name = "Vault Cash Processing Fee",
                Description = "Vault Cash Processing Fee",
                LookUpKey = "VAULT_CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now
            }, new Fee()

            {
                FeeId = 83,
                Code = "1404",
                FeeTypeId = 1,
                Value = 4,
                Name = "Vault Cash Processing Fee",
                Description = "Vault Cash Processing Fee",
                LookUpKey = "VAULT_CASH_PROCESSING_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()

           {
               FeeId = 84,
               Code = "1405",
               FeeTypeId = 1,
               Value = 5,
               Name = "Vault Cash Processing Fee",
               Description = "Vault Cash Processing Fee",
               LookUpKey = "VAULT_CASH_PROCESSING_FEE",
               IsNotDeleted = true,
               CreatedById = 1,
               LastChangedById = 1,
               LastChangedDate = DateTime.Now,
               CreateDate = DateTime.Now
           }, new Fee()
           {
               FeeId = 85,
               Code = "1406",
               FeeTypeId = 1,
               Value = 6,
               Name = "Vault Cash Processing Fee",
               Description = "Vault Cash Processing Fee",
               LookUpKey = "VAULT_CASH_PROCESSING_FEE",
               IsNotDeleted = true,
               CreatedById = 1,
               LastChangedById = 1,
               LastChangedDate = DateTime.Now,
               CreateDate = DateTime.Now

           }, new Fee()
           {
               FeeId = 86,
               Code = "1407",
               FeeTypeId = 1,
               Value = 7,
               Name = "Vault Cash Processing Fee",
               Description = "Vault Cash Processing Fee",
               LookUpKey = "VAULT_CASH_PROCESSING_FEE",
               IsNotDeleted = true,
               CreatedById = 1,
               LastChangedById = 1,
               LastChangedDate = DateTime.Now,
               CreateDate = DateTime.Now

           }, new Fee()
           {
               FeeId = 87,
               Code = "1408",
               FeeTypeId = 1,
               Value = 8,
               Name = "Vault Cash Processing Fee",
               Description = "Vault Cash Processing Fee",
               LookUpKey = "VAULT_CASH_PROCESSING_FEE",
               IsNotDeleted = true,
               CreatedById = 1,
               LastChangedById = 1,
               LastChangedDate = DateTime.Now,
               CreateDate = DateTime.Now

           }, new Fee()
           {
               FeeId = 88,
               Code = "1409",
               FeeTypeId = 1,
               Value = 9,
               Name = "Vault Cash Processing Fee",
               Description = "Vault Cash Processing Fee",
               LookUpKey = "VAULT_CASH_PROCESSING_FEE",
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