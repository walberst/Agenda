using Agenda.Application.DTOs;
using Agenda.Domain.Entities;
using AutoMapper;

namespace Agenda.Application.Mapping;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Contact, ContactDto>().ReverseMap();
        CreateMap<User, UserDto>().ReverseMap();
    }
}
