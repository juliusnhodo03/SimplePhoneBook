using System.Collections.Generic;

namespace SimplePhoneBook.API.Models
{
    public class PhoneBookModel
    {
        public int PhoneBookId { get; set; }

        public string Name { get; set; }

        public List<EntryModel> Entries { get; set; }
    }
}
