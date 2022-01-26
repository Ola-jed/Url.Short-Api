using FluentPaginator.Lib.Page;
using FluentPaginator.Lib.Parameter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Url.Short_Api.Dto;
using Url.Short_Api.Services.UrlTypeRepository;

namespace Url.Short_Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UrlTypeManagementController : ControllerBase
{
    private readonly IUrlTypeRepositoryService _urlTypeRepositoryService;

    public UrlTypeManagementController(IUrlTypeRepositoryService urlTypeRepositoryService)
    {
        _urlTypeRepositoryService = urlTypeRepositoryService;
    }

    /// <summary>
    ///     Get a list of the url types registered
    /// </summary>
    /// <param name="page"></param>
    /// <param name="perPage"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<UrlPage<UrlTypeReadDto>> GetAll([FromQuery] int page = 1, [FromQuery] int perPage = 15)
    {
        var url = HttpContext.Request.GetDisplayUrl().Split('?')[0];
        var pageParameters = new UrlPaginationParameter(perPage, page, url, nameof(page), nameof(perPage));
        return await _urlTypeRepositoryService.GetAll(pageParameters);
    }

    /// <summary>
    ///     Get an url type by id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:int}", Name = "GetUrlType")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<UrlTypeReadDto>> GetUrlType(int id)
    {
        var urlType = await _urlTypeRepositoryService.GetById(id);
        return urlType == null ? NotFound() : urlType;
    }

    /// <summary>
    ///     Get url types by domain
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("Domain")]
    public async Task<IEnumerable<UrlTypeReadDto>> GetByDomain([FromQuery] string query)
    {
        return await _urlTypeRepositoryService.FindByDomain(query);
    }

    /// <summary>
    ///     Get url types by short name
    /// </summary>
    /// <param name="query"></param>
    /// <returns></returns>
    [HttpGet("ShortName")]
    public async Task<IEnumerable<UrlTypeReadDto>> GetByShortName([FromQuery] string query)
    {
        return await _urlTypeRepositoryService.FindByShortName(query);
    }

    /// <summary>
    ///     Creates a new url type
    /// </summary>
    /// <param name="urlTypeCreateDto"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<UrlTypeReadDto>> Post(UrlTypeCreateDto urlTypeCreateDto)
    {
        var urlType = await _urlTypeRepositoryService.Create(urlTypeCreateDto);
        return CreatedAtRoute(nameof(GetUrlType), new { urlType.Id }, urlType);
    }

    /// <summary>
    ///     Update an existing url type
    /// </summary>
    /// <param name="id"></param>
    /// <param name="urlTypeUpdateDto"></param>
    /// <returns></returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(int id, UrlTypeUpdateDto urlTypeUpdateDto)
    {
        if (!await _urlTypeRepositoryService.Exists(id))
        {
            return NotFound();
        }

        await _urlTypeRepositoryService.Update(id, urlTypeUpdateDto);
        return NoContent();
    }

    /// <summary>
    ///     Deletes an url type
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        if (!await _urlTypeRepositoryService.Exists(id))
        {
            return NotFound();
        }

        await _urlTypeRepositoryService.Delete(id);
        return NoContent();
    }
}