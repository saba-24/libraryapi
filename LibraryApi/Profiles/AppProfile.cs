using AutoMapper;
using LibraryApi.Data.Dto;
using LibraryApi.Data.Entities;

namespace LibraryApi.Profiles;

public class AppProfile : Profile
{
    public AppProfile()
    {
        CreateMap<BookDto, Book>();
        CreateMap<Book, BookDto>();
        CreateMap<BookCopy, CopyDto>();
        CreateMap<CopyDto, BookCopy>();
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
    }
}