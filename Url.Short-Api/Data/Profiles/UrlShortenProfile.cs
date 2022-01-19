using AutoMapper;
using Url.Short_Api.Dto;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Data.Profiles;

public class UrlShortenProfile : Profile
{
    public UrlShortenProfile()
    {
        CreateMap<UrlShorten, UrlShortenReadDto>();
        CreateMap<UrlShortenCreateDto, UrlShorten>();
        CreateMap<UrlShortenUpdateDto, UrlShorten>();
    }
}