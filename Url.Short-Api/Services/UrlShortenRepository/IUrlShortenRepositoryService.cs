using Url.Short_Api.Dto;
using Url.Short_Api.Services.Pagination;

namespace Url.Short_Api.Services.UrlShortenRepository;

public interface IUrlShortenRepositoryService
{
    Task<IEnumerable<UrlShortenReadDto>> GetAll(PageParameters pageParameters);
    Task<UrlShortenReadDto?> GetById(int id);
    Task<UrlShortenReadDto?> GetByShortUrl(string shortUrl);
    Task<IEnumerable<UrlShortenReadDto>> FindByUrl(string url);
    Task<bool> Exists(int id);
    Task DeleteById(int id);
}