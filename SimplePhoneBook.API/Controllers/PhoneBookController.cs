using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PhoneBook.API.Services;
using SimplePhoneBook.API.Models;

namespace SimplePhoneBook.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhoneBookController : ControllerBase
    {
        private readonly IPhoneBookService _phoneBookService;

        public PhoneBookController(IPhoneBookService phoneBookService)
        {
            _phoneBookService = phoneBookService;
        }


        /// <summary>
        /// Get phone book
        /// </summary>
        [HttpGet]
        public async Task<PhoneBookModel> Get()
        {
            return await _phoneBookService.GetPhoneBookAsync();
        }


        /// <summary>
        /// Search phone book for entries with the search text provided. 
        /// This can either be phoneNumber or name. 
        /// </summary>
        /// <param name="text"></param>
        [HttpGet("Search/{text}")]
        public async Task<IEnumerable<EntryModel>> SearchAsync(string text)
        {
            return await _phoneBookService.SearchAsync(text);
        }
    }
}
