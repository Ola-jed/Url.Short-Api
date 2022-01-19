using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;
using Url.Short_Api.Dto;
using Url.Short_Api.Services.Pagination;

namespace Url.Short_Api.Services.UrlShortenRepository;

public class UrlShortenRepositoryService : IUrlShortenRepositoryService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public UrlShortenRepositoryService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UrlShortenReadDto>> GetAll(PageParameters pageParameters)
    {
        var urlShortens = await _context.UrlShortens.AsNoTracking()
            .Paginate(pageParameters);
        return _mapper.Map<IEnumerable<UrlShortenReadDto>>(urlShortens);
    }

    public async Task<UrlShortenReadDto?> GetById(int id)
    {
        var urlShorten = await _context.UrlShortens.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);
        return urlShorten == null ? null : _mapper.Map<UrlShortenReadDto>(urlShorten);
    }

    public async Task<UrlShortenReadDto?> GetByShortUrl(string shortUrl)
    {
        var urlShorten = await _context.UrlShortens.AsNoTracking()
            .FirstOrDefaultAsync(u => u.ShortUrl == shortUrl);
        return urlShorten == null ? null : _mapper.Map<UrlShortenReadDto>(urlShorten);
    }

    public async Task<IEnumerable<UrlShortenReadDto>> FindByUrl(string url)
    {
        var urls = await _context.UrlShortens.AsNoTracking()
            .Where(u => EF.Functions.Like(u.Url, $"%{url}%"))
            .ToListAsync();
        return _mapper.Map<IEnumerable<UrlShortenReadDto>>(urls);
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