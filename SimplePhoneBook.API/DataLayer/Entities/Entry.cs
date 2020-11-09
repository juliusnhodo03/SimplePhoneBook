using System.ComponentModel.DataAnnotations;

namespace SimplePhoneBook.API.Data.Entities
{
    public class Entry
    {
        public int EntryId { get; set; }
        public int PhoneBookId { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        public string PhoneNumber { get; set; }

        public PhoneBook PhoneBook { get; set; }
    }
}
