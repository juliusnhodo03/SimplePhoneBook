using Microsoft.EntityFrameworkCore;
using SimplePhoneBook.API.Data;
using System;
using System.Threading.Tasks;

namespace SimplePhoneBook.API.DataLayer
{
    public class SeedData
    {
        public static async Task SeedAsync(PhoneBookContext context)
        {
            try
            {
                // Create database schema if none exists
                context.Database.Migrate();

                if (await context.PhoneBooks.AnyAsync()) return;
                
                // Seed PhoneBook
                var phoneBook = new Data.Entities.PhoneBook
                {
                    Name = "ABSA PhoneBook"
                };

                await context.PhoneBooks.AddAsync(phoneBook);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
