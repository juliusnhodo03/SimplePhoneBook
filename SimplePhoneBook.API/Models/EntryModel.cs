namespace SimplePhoneBook.API.Models
{
    public class EntryModel
    {
        public int EntryId { get; set; }
        public int PhoneBookId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
