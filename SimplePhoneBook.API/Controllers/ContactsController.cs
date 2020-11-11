using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.API.Services;
using SimplePhoneBook.API.Models;

namespace SimplePhoneBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactsService _contactsService;

        public ContactsController(IContactsService contactsService)
        {
            _contactsService = contactsService;
        }

        /// <summary>
        /// Get a phone entry. 
        /// </summary>
        /// <param name="text"></param>
        [HttpGet("{id}")]
        public async Task<EntryModel> Get(int id)
        {
            return await _contactsService.GetContact(id);
        }


        /// <summary>
        /// Add entry in phone book.
        /// </summary>
        /// <param name="contact"></param>
        [HttpPost]
        public async Task<bool> Post([FromBody] EntryModel contact)
        {
            return await _contactsService.AddContactAsync(contact);
        }


        /// <summary>
        /// update entry in phone book.
        /// </summary>
        /// <param name="contact"></param>
        [HttpPut]
        public async Task<bool> Put([FromBody] EntryModel contact)
        {
            return await _contactsService.UpdateContactAsync(contact);
        }


        /// <summary>
        /// delete entry in phone book.
        /// </summary>
        /// <param name="contact"></param>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _contactsService.DeleteContactAsync(id);
        }
    }
}
