using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Url.Short_Api.Dto;
using Url.Short_Api.Services.UrlShortener;
using Url.Short_Api.Services.UrlShortenRepository;

namespace Url.Short_Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UrlShortenManagementController : ControllerBase
{
    private readonly IUrlShortenRepositoryService _repositoryService;
    private readonly IUrlShortenerService _urlShortenerService;

    public UrlShortenManagementController(IUrlShortenRepositoryService repositoryService,
        IUrlShortenerService urlShortenerService)
    {
        _repositoryService = repositoryService;
        _urlShortenerService = urlShortenerService;
    }

    /// <summary>
    /// Get and paginate the url shorten created
    /// </summary>
    /// <param name="page">The page we want to retrieve data from</param>
    /// <param name="perPage">The number of items per page</param>
    /// <returns></returns>
    [HttpGet]
    public async Task<UrlPage<UrlShortenReadDto>> GetAll([FromQuery] int page = 1, [FromQuery] int perPage = 15)
    {
        var url = HttpContext.Request.GetDisplayUrl().Split('?')[0];
        var pageParam = new UrlPaginationParameter(perPage, page, url, nameof(page), nameof(perPage));
        return await _repositoryService.GetAll(pageParam);
    }

    /// <summary>
    /// Get an existing url shorten by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UrlShortenReadDto>> GetById(int id)
    {
        var urlShorten = await _repositoryService.GetById(id);
        return urlShorten == null ? NotFound() : urlShorten;
    }

    /// <summary>
    /// Get the url shorten by its short url
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    [HttpGet("short/{shortUrl}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UrlShortenReadDto>> GetByShortUrl(string shortUrl)
    {
        var urlShorten = await _repositoryService.GetByShortUrl(shortUrl);
        return urlShorten == null ? NotFound() : urlShorten;
    }

    /// <summary>
    /// Find the url shorten by its long url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    [HttpGet("url")]
    public async Task<IEnumerable<UrlShortenReadDto>> SearchByUrl([FromQuery] string url)
    {
        return await _repositoryService.FindByUrl(url);
    }

    /// <summary>
    /// Update an existing url shorten
    /// </summary>
    /// <param name="id"></param>
    /// <param name="urlShortenUpdateDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(int id, UrlShortenUpdateDto urlShortenUpdateDto)
    {
        if (!await _repositoryService.Exists(id))
        {
            return NotFound();
        }

        await _urlShortenerService.UpdateShortenUrl(id, urlShortenUpdateDto);
        return NoContent();
    }

    /// <summary>
    /// Delete an existing url shorten by its id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _repositoryService.Exists(id))
        {
            return NotFound();
        }

        await _repositoryService.DeleteById(id);
        return NoContent();
    }
}