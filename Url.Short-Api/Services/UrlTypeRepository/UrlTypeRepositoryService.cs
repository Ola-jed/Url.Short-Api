using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;
using Url.Short_Api.Dto;
using Url.Short_Api.Mapping;

namespace Url.Short_Api.Services.UrlTypeRepository;

public class UrlTypeRepositoryService : IUrlTypeRepositoryService
{
    private readonly AppDbContext _context;

    public UrlTypeRepositoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UrlPage<UrlTypeReadDto>> GetAll(UrlPaginationParameter urlPaginationParameter)
    {
        var page = await _context.UrlTypes
            .AsNoTracking()
            .AsyncUrlPaginate(urlPaginationParameter, u => u.Id);
        
        return page.Map(x => x.ToUrlTypeReadDto());
    }

    public async Task<UrlTypeReadDto?> GetById(int id)
    {
        return await _context.UrlTypes.AsNoTracking()
            .ToUrlTypeReadDto()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<UrlShortenReadDto>> GetUsages(int id)
    {
        var urlTypeDomain = await _context.UrlTypes.AsNoTracking()
            .Where(u => u.Id == id)
            .Select(u => u.Domain)
            .FirstOrDefaultAsync();
        if (urlTypeDomain == null)
        {
            return Enumerable.Empty<UrlShortenReadDto>();
        }

        return await _context.UrlShortens.AsNoTracking()
            .Where(u => EF.Functions.Like(u.Url, $"%{urlTypeDomain}%"))
            .ToUrlShortenReadDto()
            .ToListAsync();
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.UrlTypes.AnyAsync(x => x.Id == id);
    }

    public async Task<IEnumerable<UrlTypeReadDto>> FindByDomain(string domain)
    {
        return await _context.UrlTypes.AsNoTracking()
            .Where(u => EF.Functions.Like(u.Domain, $"%{domain}%"))
            .ToUrlTypeReadDto()
            .ToListAsync();
    }

    public async Task<IEnumerable<UrlTypeReadDto>> FindByShortName(string shortName)
    {
        return await _context.UrlTypes.AsNoTracking()
            .Where(u => EF.Functions.Like(u.ShortName, $"%{shortName}%"))
            .ToUrlTypeReadDto()
            .ToListAsync();
    }

    public async Task<UrlTypeReadDto> Create(UrlTypeCreateDto urlTypeCreateDto)
    {
        var urlType = urlTypeCreateDto.ToUrlType();
        var urlTypeEntityEntry = _context.UrlTypes.Add(urlType);
        await _context.SaveChangesAsync();
        return urlTypeEntityEntry.Entity.ToUrlTypeReadDto();
    }

    public async Task Update(int id, UrlTypeUpdateDto urlTypeUpdateDto)
    {
        await _context.UrlTypes
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(u => u.ShortName, urlTypeUpdateDto.ShortName)
                .SetProperty(u => u.Domain, urlTypeUpdateDto.Domain)
            );
    }

    public async Task Delete(int id)
    {
        await _context.UrlTypes.Where(x => x.Id == id).ExecuteDeleteAsync();
    }
}