using SimplePhoneBook.API.Models;
using System.Threading.Tasks;

namespace PhoneBook.API.Services
{
    public interface IContactsService
    {
        /// <summary>
        /// Get phone entry
        /// </summary>
        /// <param name="id"></param>
        Task<EntryModel> GetContact(int id);

        /// <summary>
        /// Add entry.
        /// </summary>
        /// <param name="contact"></param>
        Task<bool> AddContactAsync(EntryModel entry);

        /// <summary>
        /// Update entry
        /// </summary>
        /// <param name="contact"></param>
        Task<bool> UpdateContactAsync(EntryModel contact);

        /// <summary>
        /// Delete entry
        /// </summary>
        /// <param name="contact"></param>
        Task<bool> DeleteContactAsync(int id);
    }
}
