using Microsoft.AspNetCore.Mvc;
using Url.Short_Api.Dto;
using Url.Short_Api.Services.Authorization;

namespace Url.Short_Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController: ControllerBase
{
    private readonly IAuthorizationService _authorizationService;
    
    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }
    
    /// <summary>
    /// Get the jwt by using the app secret
    /// </summary>
    /// <param name="authorizationDto">Data containing the app secret</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<TokenDto>> Authorize(AuthorizationDto authorizationDto)
    {
        if (!await _authorizationService.ValidateAppSecret(authorizationDto.Secret))
        {
            return Unauthorized();
        }

        var token = await _authorizationService.GenerateJwt();
        return new TokenDto(token);
    }
}