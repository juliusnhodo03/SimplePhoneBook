using SimplePhoneBook.API.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimplePhoneBook.API.DataLayer.Repositories
{
    public interface IRepository
    {
        /// <summary>
        /// Search entries with the search text provided. This can either be phoneNumber or name.
        /// </summary>
        /// <param name="text"></param>
        Task<IEnumerable<Entry>> SearchAsync(string text);

        /// <summary>
        /// Add entries to phone book.
        /// </summary>
        /// <param name="contact"></param>
        Task<bool> AddToPhoneBookAsync(Entry entry);

        /// <summary>
        /// Get Phone Book
        /// </summary>
        Task<Data.Entities.PhoneBook> GetPhoneBookAsync();

        /// <summary>
        /// Get phone entry
        /// </summary>
        /// <param name="id"></param>
        Task<Entry> GetContactAsync(int id);

        /// <summary>
        /// Update entry
        /// </summary>
        /// <param name="entry"></param>
        Task<bool> UpdateContactAsync(Entry entry);

        /// <summary>
        /// Delete entry
        /// </summary>
        /// <param name="contact"></param>
        Task<bool> DeleteContactAsync(int id);
    }
}
