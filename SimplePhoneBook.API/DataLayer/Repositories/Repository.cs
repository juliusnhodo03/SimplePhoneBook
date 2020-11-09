using Microsoft.EntityFrameworkCore;
using SimplePhoneBook.API.Data;
using SimplePhoneBook.API.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePhoneBook.API.DataLayer.Repositories
{
    public class Repository : IRepository
    {
        private readonly PhoneBookContext _phoneBookContext;

        public Repository(PhoneBookContext phoneBookContext)
        {
            _phoneBookContext = phoneBookContext ?? throw new ArgumentNullException(nameof(phoneBookContext));
        }

        /// <summary>
        /// List all entries
        /// </summary>
        public async Task<IEnumerable<Entry>> ListAsync()
        {
            return await _phoneBookContext.Entries.ToListAsync();
        }

        /// <summary>
        /// Search entries with the search text provided. This can either be phoneNumber or name.
        /// </summary>
        /// <param name="searchText"></param>
        public async Task<IEnumerable<Entry>> SearchAsync(string searchText)
        {
            if (searchText == "_none__valid_")         
                return await _phoneBookContext.Entries.ToListAsync();
            
            return await _phoneBookContext.Entries
                      .Where(x => x.PhoneNumber.Contains(searchText) || 
                                  x.Name.Contains(searchText))
                      .ToListAsync();
        }

        /// <summary>
        /// Add entries to phone book.
        /// </summary>
        /// <param name="contact"></param>
        public async Task<bool> AddToPhoneBookAsync(Entry entry)
        {
            var phoneBook = await _phoneBookContext.PhoneBooks.FirstOrDefaultAsync();

            if(phoneBook != null)
            {
                _phoneBookContext.Entry(entry).State = EntityState.Added;
                return await _phoneBookContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        /// <summary>
        /// Get Phone Book
        /// </summary>
        public async Task<Data.Entities.PhoneBook> GetPhoneBookAsync()
        {
            return await _phoneBookContext.PhoneBooks
                            .Include(e => e.Entries)
                            .FirstOrDefaultAsync();
        }
    }
}
