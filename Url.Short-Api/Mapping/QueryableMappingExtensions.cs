using Url.Short_Api.Dto;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Mapping;

public static class QueryableMappingExtensions
{
    public static IQueryable<UrlShortenReadDto> ToUrlShortenReadDto(this IQueryable<UrlShorten> urlShortens)
    {
        return urlShortens.Select(urlShorten => urlShorten.ToUrlShortenReadDto());
    }
    
    public static IQueryable<UrlTypeReadDto> ToUrlTypeReadDto(this IQueryable<UrlType> urlTypes)
    {
        return urlTypes.Select(urlType => urlType.ToUrlTypeReadDto());
    }
}