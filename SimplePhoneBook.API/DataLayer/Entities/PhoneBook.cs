using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SimplePhoneBook.API.Data.Entities
{
    public class PhoneBook
    {
        public int PhoneBookId { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public ICollection<Entry> Entries { get; set; }
    }
}
