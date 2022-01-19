using Url.Short_Api.Dto;
using Url.Short_Api.Services.Pagination;

namespace Url.Short_Api.Services.UrlTypeRepository;

public interface IUrlTypeRepositoryService
{
    Task<IEnumerable<UrlTypeReadDto>> GetAll(PageParameters pageParameters);
    Task<UrlTypeReadDto?> GetById(int id);
    Task<IEnumerable<UrlShortenReadDto>> GetUsages(int id);
    Task<bool> Exists(int id);
    Task<IEnumerable<UrlTypeReadDto>> FindByDomain(string domain);
    Task<IEnumerable<UrlTypeReadDto>> FindByShortName(string shortName);
    Task<UrlTypeReadDto> Create(UrlTypeCreateDto urlTypeCreateDto);
    Task Update(int id, UrlTypeUpdateDto urlTypeUpdateDto);
    Task Delete(int id);
}