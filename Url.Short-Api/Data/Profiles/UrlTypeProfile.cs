using AutoMapper;
using Url.Short_Api.Dto;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Data.Profiles;

public class UrlTypeProfile : Profile
{
    public UrlTypeProfile()
    {
        CreateMap<UrlType, UrlTypeReadDto>();
        CreateMap<UrlTypeCreateDto, UrlType>();
        CreateMap<UrlTypeUpdateDto, UrlType>();
    }
}