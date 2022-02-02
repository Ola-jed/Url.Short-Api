using Url.Short_Api.Dto;

namespace Url.Short_Api.Services.UrlShortener;

public interface IUrlShortenerService
{
    Task<UrlShortenReadDto> ShortenUrl(UrlShortenCreateDto urlShortenCreateDto);
    Task<UrlShortenReadDto> CustomShortenUrl(UrlShortenCustomCreateDto urlShortenCustomCreateDto);
    Task<bool> ShortUrlExists(string shortUrl);
    Task UpdateShortenUrl(int id,UrlShortenUpdateDto urlShortenUpdateDto);
}