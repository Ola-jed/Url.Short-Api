using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;
using Url.Short_Api.Dto;
using Url.Short_Api.Entities;
using Url.Short_Api.Extensions;
using Url.Short_Api.Services.Pagination;

namespace Url.Short_Api.Services.UrlTypeRepository;

public class UrlTypeRepositoryService : IUrlTypeRepositoryService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;
    
    public UrlTypeRepositoryService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UrlTypeReadDto>> GetAll(PageParameters pageParameters)
    {
        return await _context.UrlTypes
            .AsNoTracking()
            .Paginate(pageParameters)
            .MapTo<UrlType,UrlTypeReadDto>()
            .ToListAsync();
    }

    public async Task<UrlTypeReadDto?> GetById(int id)
    {
        return await _context.UrlTypes.AsNoTracking()
            .MapTo<UrlType,UrlTypeReadDto>()
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
            .MapTo<UrlShorten,UrlShortenReadDto>()
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
            .MapTo<UrlType,UrlTypeReadDto>()
            .ToListAsync();
    }

    public async Task<IEnumerable<UrlTypeReadDto>> FindByShortName(string shortName)
    {
        return await _context.UrlTypes.AsNoTracking()
            .Where(u => EF.Functions.Like(u.ShortName, $"%{shortName}%"))
            .MapTo<UrlType,UrlTypeReadDto>()
            .ToListAsync();
    }

    public async Task<UrlTypeReadDto> Create(UrlTypeCreateDto urlTypeCreateDto)
    {
        var urlType = _mapper.Map<UrlType>(urlTypeCreateDto);
        var urlTypeEntityEntry = _context.UrlTypes.Add(urlType);
        await _context.SaveChangesAsync();
        return _mapper.Map<UrlTypeReadDto>(urlTypeEntityEntry.Entity);
    }

    public async Task Update(int id, UrlTypeUpdateDto urlTypeUpdateDto)
    {
        var urlType = await _context.UrlTypes.FindAsync(id);
        if (urlType == null)
        {
            return;
        }

        _mapper.Map(urlTypeUpdateDto, urlType);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var urlType = await _context.UrlTypes.FindAsync(id);
        if (urlType == null)
        {
            return;
        }

        _context.UrlTypes.Remove(urlType);
        await _context.SaveChangesAsync();
    }
}