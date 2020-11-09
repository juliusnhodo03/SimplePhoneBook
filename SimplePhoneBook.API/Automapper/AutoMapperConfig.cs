using AutoMapper;
using SimplePhoneBook.API.Data.Entities;
using SimplePhoneBook.API.Models;

namespace SimplePhoneBook.API.Automapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Entry
            CreateMap<Entry, EntryModel>().ReverseMap();

            // Order
            CreateMap<Data.Entities.PhoneBook, PhoneBookModel>().ReverseMap();
        }
    }
}
