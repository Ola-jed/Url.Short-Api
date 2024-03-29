using System.Text;
using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;
using Url.Short_Api.Dto;
using Url.Short_Api.Mapping;

namespace Url.Short_Api.Services.UrlShortener;

public class UrlShortenerService: IUrlShortenerService
{
    private readonly AppDbContext _context;

    public UrlShortenerService(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<UrlShortenReadDto> ShortenUrl(UrlShortenCreateDto urlShortenCreateDto)
    {
        var urlShorten = urlShortenCreateDto.ToUrlShorten();
        urlShorten.ShortUrl = await GenerateUrlToken(urlShorten.Url);
        urlShorten.CreatedAt = DateTime.Now;
        var urlShortenEntityEntry = _context.UrlShortens.Add(urlShorten);
        await _context.SaveChangesAsync();
        return urlShortenEntityEntry.Entity.ToUrlShortenReadDto();
    }

    public async Task<UrlShortenReadDto> CustomShortenUrl(UrlShortenCustomCreateDto urlShortenCustomCreateDto)
    {
        var urlShorten = urlShortenCustomCreateDto.ToUrlShorten();
        urlShorten.CreatedAt = DateTime.Now;
        var urlShortenEntityEntry = _context.UrlShortens.Add(urlShorten);
        await _context.SaveChangesAsync();
        return urlShortenEntityEntry.Entity.ToUrlShortenReadDto();
    }

    public async Task<bool> ShortUrlExists(string shortUrl)
    {
        return await _context.UrlShortens.AnyAsync(x => x.ShortUrl == shortUrl);
    }

    public async Task UpdateShortenUrl(int id, UrlShortenUpdateDto urlShortenUpdateDto)
    {
        var shortUrl = await GenerateUrlToken(urlShortenUpdateDto.Url);
        await _context.UrlShortens
            .Where(x => x.Id == id)
            .ExecuteUpdateAsync(x => x.SetProperty(s => s.Url, urlShortenUpdateDto.Url)
                .SetProperty(s => s.ShortUrl, shortUrl)
                .SetProperty(s => s.LifetimeInHours, urlShortenUpdateDto.LifetimeInHours)
            );
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