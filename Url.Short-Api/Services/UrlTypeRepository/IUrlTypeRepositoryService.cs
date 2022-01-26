using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Url.Short_Api.Dto;

namespace Url.Short_Api.Services.UrlTypeRepository;

public interface IUrlTypeRepositoryService
{
    Task<UrlPage<UrlTypeReadDto>> GetAll(UrlPaginationParameter urlPaginationParameter);
    Task<UrlTypeReadDto?> GetById(int id);
    Task<IEnumerable<UrlShortenReadDto>> GetUsages(int id);
    Task<bool> Exists(int id);
    Task<IEnumerable<UrlTypeReadDto>> FindByDomain(string domain);
    Task<IEnumerable<UrlTypeReadDto>> FindByShortName(string shortName);
    Task<UrlTypeReadDto> Create(UrlTypeCreateDto urlTypeCreateDto);
    Task Update(int id, UrlTypeUpdateDto urlTypeUpdateDto);
    Task Delete(int id);
}