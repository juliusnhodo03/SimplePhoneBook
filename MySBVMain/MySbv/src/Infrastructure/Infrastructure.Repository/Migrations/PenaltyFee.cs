using System;
using Infrastructure.Repository.Database;
using System.Data.Entity.Migrations;
using Domain.Data.Model;

namespace Infrastructure.Repository.Migrations
{
    public class PenaltyFee
    {
        public static void Seed(Context context)
        {
            context.Fees.AddOrUpdate(new Fee() 
            {

                FeeId = 41,
                Code = "600",
                FeeTypeId = 1,
                Value = 30,
                Name = "Penalty Fee",
                Description = "Penalty Fee",
                LookUpKey = "PENALTY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }, new Fee()
            {

                FeeId = 42,
                Code = "601",
                FeeTypeId = 1,
                Value = 40,
                Name = "Penalty Fee",
                Description = "Penalty Fee",
                LookUpKey = "PENALTY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {

                FeeId = 43,
                Code = "602",
                FeeTypeId = 1,
                Value = 60,
                Name = "Penalty Fee",
                Description = "Penalty Fee",
                LookUpKey = "PENALTY_FEE",
                IsNotDeleted = true,
                CreatedById = 1,
                LastChangedById = 1,
                LastChangedDate = DateTime.Now,
                CreateDate = DateTime.Now

            }
            , new Fee()
            {

                FeeId = 44,
                Code = "603",
                FeeTypeId = 1,
                Value = 70,
                Name = "Penalty Fee",
                Description = "Penalty Fee",
                LookUpKey = "PENALTY_FEE",
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