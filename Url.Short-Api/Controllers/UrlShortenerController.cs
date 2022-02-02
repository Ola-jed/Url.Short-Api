using Microsoft.AspNetCore.Mvc;
using Url.Short_Api.Dto;
using Url.Short_Api.Services.UrlShortener;
using Url.Short_Api.Services.UrlShortenRepository;

namespace Url.Short_Api.Controllers;

[Route("url.short")]
[ApiController]
public class UrlShortenerController : ControllerBase
{
    private readonly IUrlShortenerService _urlShortenerService;
    private readonly IUrlShortenRepositoryService _shortenRepositoryService;

    public UrlShortenerController(IUrlShortenerService urlShortenerService,
        IUrlShortenRepositoryService shortenRepositoryService)
    {
        _urlShortenerService = urlShortenerService;
        _shortenRepositoryService = shortenRepositoryService;
    }

    /// <summary>
    /// Get a shorten url by its token
    /// </summary>
    /// <param name="urlToken">The url token schema</param>
    /// <returns></returns>
    [HttpGet("{urlToken}", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status301MovedPermanently)]
    [ProducesResponseType(StatusCodes.Status302Found)]
    [ProducesResponseType(StatusCodes.Status307TemporaryRedirect)]
    [ProducesResponseType(StatusCodes.Status308PermanentRedirect)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string urlToken)
    {
        var shortenUrl = await _shortenRepositoryService.GetByShortUrl(urlToken);
        return shortenUrl == null ? NotFound() : new RedirectResult(shortenUrl.Url);
    }

    /// <summary>
    /// Shorten the url given in the request body
    /// </summary>
    /// <param name="urlShortenCreateDto">Data for the shortened url</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<UrlShortenReadDto>> Post(UrlShortenCreateDto urlShortenCreateDto)
    {
        var shorten = await _urlShortenerService.ShortenUrl(urlShortenCreateDto);
        return CreatedAtRoute(nameof(Get), new { UrlToken = shorten.ShortUrl }, shorten);
    }

    /// <summary>
    /// Create a shorten url with a custom url
    /// </summary>
    /// <param name="urlShortenCustomCreateDto">The custom data</param>
    /// <returns></returns>
    [HttpPost("Custom")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<UrlShortenCustomCreateDto>> CreateCustom(
        UrlShortenCustomCreateDto urlShortenCustomCreateDto)
    {
        if (await _urlShortenerService.ShortUrlExists(urlShortenCustomCreateDto.ShortUrl))
        {
            return BadRequest();
        }

        var shorten = await _urlShortenerService.CustomShortenUrl(urlShortenCustomCreateDto);
        return CreatedAtRoute(nameof(Get), new { UrlToken = shorten.ShortUrl }, shorten);
    }
}