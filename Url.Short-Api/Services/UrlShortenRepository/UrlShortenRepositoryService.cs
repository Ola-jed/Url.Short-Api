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
        return await Task.Run(() => _context.UrlShortens.AsNoTracking()
            .ToUrlShortenReadDto()
            .UrlPaginate(urlPaginationParameter, u => u.Id));
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
        var urlShorten = await _context.UrlShortens.FindAsync(id);
        if (urlShorten == null)
        {
            return;
        }

        _context.UrlShortens.Remove(urlShorten);
        await _context.SaveChangesAsync();
    }
}