using Url.Short_Api.Dto;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Mapping;

/// <summary>
/// Custom Extensions for Mapping Url Shortens
/// </summary>
public static class UrlShortenMappingExtensions
{
    public static UrlShortenReadDto ToUrlShortenReadDto(this UrlShorten urlShorten)
    {
        return new UrlShortenReadDto(urlShorten.Id, urlShorten.Url, urlShorten.ShortUrl, urlShorten.LifetimeInHours,
            urlShorten.CreatedAt);
    }

    public static UrlShorten ToUrlShorten(this UrlShortenUpdateDto urlShortenUpdateDto)
    {
        return new UrlShorten
        {
            Url = urlShortenUpdateDto.Url,
            LifetimeInHours = urlShortenUpdateDto.LifetimeInHours
        };
    }
    
    public static UrlShorten ToUrlShorten(this UrlShortenCreateDto urlShortenCreateDto)
    {
        return new UrlShorten
        {
            Url = urlShortenCreateDto.Url,
            LifetimeInHours = urlShortenCreateDto.LifetimeInHours
        };
    }
}