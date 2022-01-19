using System.Text;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;
using Url.Short_Api.Dto;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Services.UrlShortener;

public class UrlShortenerService: IUrlShortenerService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public UrlShortenerService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<UrlShortenReadDto> ShortenUrl(UrlShortenCreateDto urlShortenCreateDto)
    {
        var urlShorten = _mapper.Map<UrlShorten>(urlShortenCreateDto);
        urlShorten.ShortUrl = await GenerateUrlToken(urlShorten.Url);
        urlShorten.CreatedAt = DateTime.Now;
        var urlShortenEntityEntry = _context.UrlShortens.Add(urlShorten);
        await _context.SaveChangesAsync();
        return _mapper.Map<UrlShortenReadDto>(urlShortenEntityEntry.Entity);
    }

    public async Task UpdateShortenUrl(int id, UrlShortenUpdateDto urlShortenUpdateDto)
    {
        var urlShorten = await _context.UrlShortens.FindAsync(id);
        if (urlShorten == null)
        {
            return;
        }
        _mapper.Map(urlShortenUpdateDto, urlShorten);
        urlShorten.ShortUrl = await GenerateUrlToken(urlShorten.Url);
        _context.Update(urlShorten);
        await _context.SaveChangesAsync();
    }

    private async Task<string> GenerateUrlToken(string url)
    {
        var domain = new Uri(url).Host;
        var existingDomainMapping = await _context.UrlTypes.AsNoTracking()
            .FirstOrDefaultAsync(urlType => domain.Contains(urlType.Domain));
        string urlShortened;
        do
        {
            var urlBuilder = new StringBuilder();
            var urlSecondPart = Guid.NewGuid().ToString().Remove(7);
            urlShortened = existingDomainMapping == null
                ? urlBuilder.Append(urlSecondPart).ToString()
                : urlBuilder.Append(existingDomainMapping.ShortName).Append('-').Append(urlSecondPart).ToString();
        } while (await _context.UrlShortens.AnyAsync(u => u.ShortUrl == urlShortened));
        return urlShortened;
    }
}