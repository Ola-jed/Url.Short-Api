using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Url.Short_Api.Dto;

namespace Url.Short_Api.Services.UrlShortenRepository;

public interface IUrlShortenRepositoryService
{
    Task<UrlPage<UrlShortenReadDto>> GetAll(UrlPaginationParameter urlPaginationParameter);
    Task<UrlShortenReadDto?> GetById(int id);
    Task<UrlShortenReadDto?> GetByShortUrl(string shortUrl);
    Task<IEnumerable<UrlShortenReadDto>> FindByUrl(string url);
    Task<bool> Exists(int id);
    Task DeleteById(int id);
}