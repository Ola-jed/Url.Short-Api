using FluentPaginator.Lib.Extensions;
using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;
using Url.Short_Api.Dto;
using Url.Short_Api.Mapping;

namespace Url.Short_Api.Services.UrlShortenRepository;

public class UrlShortenRepositoryService : IUrlShortenRepositoryService
{
    private readonly AppDbContext _context;

    public UrlShortenRepositoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UrlPage<UrlShortenReadDto>> GetAll(UrlPaginationParameter urlPaginationParameter)
    {
        var page = await _context.UrlShortens
            .AsNoTracking()
            .AsyncUrlPaginate(urlPaginationParameter, u => u.Id);
        
        return page.Map(x => x.ToUrlShortenReadDto());
    }

    public async Task<UrlShortenReadDto?> GetById(int id)
    {
        return await _context.UrlShortens.AsNoTracking()
            .ToUrlShortenReadDto()
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<UrlShortenReadDto?> GetByShortUrl(string shortUrl)
    {
        return await _context.UrlShortens.AsNoTracking()
            .ToUrlShortenReadDto()
            .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
    }

    public async Task<IEnumerable<UrlShortenReadDto>> FindByUrl(string url)
    {
        return await _context.UrlShortens.AsNoTracking()
            .Where(u => EF.Functions.Like(u.Url, $"%{url}%"))
            .ToUrlShortenReadDto()
            .ToListAsync();
    }

    public async Task<bool> Exists(int id)
    {
        return await _context.UrlShortens.AnyAsync(u => u.Id == id);
    }

    public async Task DeleteById(int id)
    {
        await _context.UrlShortens
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync();
    }
}