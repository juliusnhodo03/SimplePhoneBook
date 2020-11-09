using AutoMapper;
using SimplePhoneBook.API.Data.Entities;
using SimplePhoneBook.API.Extensions;
using SimplePhoneBook.API.Models;

namespace SimplePhoneBook.API.Automapper
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            // Entry
            CreateMap<Entry, EntryModel>().ReverseMap();
           
            CreateMap<Entry, EntryModel>()
                    .ForMember(dest => dest.Name, src => src.MapFrom(o => o.Name.ToProperCase()));

            // Order
            CreateMap<Data.Entities.PhoneBook, PhoneBookModel>().ReverseMap();
        }
    }
}
