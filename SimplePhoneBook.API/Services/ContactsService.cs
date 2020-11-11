using AutoMapper;
using SimplePhoneBook.API.Data.Entities;
using SimplePhoneBook.API.DataLayer.Repositories;
using SimplePhoneBook.API.Models;
using System.Threading.Tasks;

namespace PhoneBook.API.Services
{
    public class ContactsService : IContactsService
    {
        private readonly IRepository _entryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entryRepository"></param>
        /// <param name="mapper"></param>
        public ContactsService(IRepository entryRepository, IMapper mapper)
        {
            _entryRepository = entryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get phone entry
        /// </summary>
        /// <param name="id"></param>
        public async Task<EntryModel> GetContact(int id)
        {
            var contact = await _entryRepository.GetContactAsync(id);

            var mapped = _mapper.Map<Entry, EntryModel>(contact);

            return mapped;
        }

        /// <summary>
        /// Add entry to phone book.
        /// </summary>
        /// <param name="contact"></param>
        public async Task<bool> AddContactAsync(EntryModel entry)
        {
            var contact = _mapper.Map<EntryModel, Entry>(entry);
            var result = await _entryRepository.AddToPhoneBookAsync(contact);
            return result;
        }

        /// <summary>
        /// Update entry
        /// </summary>
        /// <param name="contact"></param>
        public async Task<bool> UpdateContactAsync(EntryModel contact)
        {
            var entry = _mapper.Map<EntryModel, Entry>(contact);

            return await _entryRepository.UpdateContactAsync(entry);
        }

        /// <summary>
        /// Delete entry
        /// </summary>
        /// <param name="contact"></param>
        public async Task<bool> DeleteContactAsync(int id)
        {
            return await _entryRepository.DeleteContactAsync(id);
        }
    }
}
