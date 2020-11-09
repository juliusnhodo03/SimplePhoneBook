using SimplePhoneBook.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PhoneBook.API.Services
{
    public interface IPhoneBookService
    {
        /// <summary>
        /// List all entries
        /// </summary>
        Task<IEnumerable<EntryModel>> ListAsync();

        /// <summary>
        /// Search entries with the search text provided. This can either be phoneNumber or name.
        /// </summary>
        /// <param name="text"></param>
        Task<IEnumerable<EntryModel>> SearchAsync(string text);

        /// <summary>
        /// Add entries to phone book.
        /// </summary>
        /// <param name="contact"></param>
        Task<bool> AddToPhoneBookAsync(EntryModel entry);

        /// <summary>
        /// Get Phone Book
        /// </summary>
        Task<PhoneBookModel> GetPhoneBookAsync();
    }
}
