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
        /// Search entries with the search text provided. This can either be phoneNumber or name.
        /// </summary>
        /// <param name="searchText"></param>
        public async Task<IEnumerable<Entry>> SearchAsync(string searchText)
        {
            if (searchText == "_none__valid_")         
                return await _phoneBookContext.Entries
                    .OrderBy(e => e.Name).ThenBy(e => e.PhoneNumber)
                    .ToListAsync();
            
            return await _phoneBookContext.Entries
                        .OrderBy(e => e.Name).ThenBy(e => e.PhoneNumber)
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
            var phoneBook =  await _phoneBookContext.PhoneBooks.Include(e => e.Entries).FirstOrDefaultAsync();

            phoneBook.Entries = phoneBook.Entries.OrderBy(e => e.Name).ThenBy(e => e.PhoneNumber).ToList();

            return phoneBook;
        }

        /// <summary>
        /// Get phone entry
        /// </summary>
        /// <param name="id"></param>
        public async Task<Entry> GetContactAsync(int id)
        {
            var entry = await _phoneBookContext.Entries.FirstOrDefaultAsync(e => e.EntryId == id);
            return entry;
        }

        /// <summary>
        /// Update entry
        /// </summary>
        /// <param name="entry"></param>
        public async Task<bool> UpdateContactAsync(Entry entry)
        {
            _phoneBookContext.Entry(entry).State = EntityState.Modified;
            return await _phoneBookContext.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Delete entry
        /// </summary>
        /// <param name="contact"></param>
        public async Task<bool> DeleteContactAsync(int id)
        {
            var contact = await _phoneBookContext.Entries.FirstOrDefaultAsync(e => e.EntryId == id);
            _phoneBookContext.Entry(contact).State = EntityState.Deleted;
            return await _phoneBookContext.SaveChangesAsync() > 0;
        }
    }
}
