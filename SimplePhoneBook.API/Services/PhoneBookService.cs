using AutoMapper;
using SimplePhoneBook.API.Data.Entities;
using SimplePhoneBook.API.DataLayer.Repositories;
using SimplePhoneBook.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneBook.API.Services
{
    public class PhoneBookService : IPhoneBookService
    {
        private readonly IRepository _entryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entryRepository"></param>
        /// <param name="mapper"></param>
        public PhoneBookService(IRepository entryRepository, IMapper mapper)
        {
            _entryRepository = entryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// List all entries
        /// </summary>
        public async Task<IEnumerable<EntryModel>> ListAsync()
        {
            var entries = await _entryRepository.ListAsync();
            var contacts = entries.Select(contact => _mapper.Map<Entry, EntryModel>(contact));
            return contacts;
        }

        /// <summary>
        /// Search entries with the search text provided. This can either be phoneNumber or name.
        /// </summary>
        /// <param name="text"></param>
        public async Task<IEnumerable<EntryModel>> SearchAsync(string text)
        {
            var entries = await _entryRepository.SearchAsync(text);
            var contacts = entries.Select(contact => _mapper.Map<Entry, EntryModel>(contact));
            return contacts;
        }

        /// <summary>
        /// Add entries to phone book.
        /// </summary>
        /// <param name="contact"></param>
        public async Task<bool> AddToPhoneBookAsync(EntryModel entry)
        {
            var contact = _mapper.Map<EntryModel, Entry>(entry);
            var result = await _entryRepository.AddToPhoneBookAsync(contact);
            return result;
        }

        /// <summary>
        /// Get Phone Book
        /// </summary>
        public async Task<PhoneBookModel> GetPhoneBookAsync()
        {
            var phoneBook = await _entryRepository.GetPhoneBookAsync();
            var mapped = _mapper.Map<SimplePhoneBook.API.Data.Entities.PhoneBook, PhoneBookModel>(phoneBook);
            return mapped;
        }
    }
}
