using Url.Short_Api.Dto;

namespace Url.Short_Api.Services.UrlShortener;

public interface IUrlShortenerService
{
    Task<UrlShortenReadDto> ShortenUrl(UrlShortenCreateDto urlShortenCreateDto);
    Task UpdateShortenUrl(int id,UrlShortenUpdateDto urlShortenUpdateDto);
}