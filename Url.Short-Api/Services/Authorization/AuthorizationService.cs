using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Url.Short_Api.Services.Authorization;

public class AuthorizationService: IAuthorizationService
{
    private readonly string _appSecret;
    private readonly string _tokenKey;
    private const int JwtLifetimeInHours = 2;

    public AuthorizationService(IConfiguration configuration)
    {
        _appSecret = configuration["AppSecret"]!;
        _tokenKey = configuration["TokenKey"]!;
    }

    public async Task<bool> ValidateAppSecret(string secret)
    {
        return await Task.FromResult(_appSecret == secret);
    }

    public Task<string> GenerateJwt()
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new(ClaimTypes.Name, "Admin")
            }),
            Expires = DateTime.UtcNow.AddHours(JwtLifetimeInHours),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenKey)),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Task.FromResult(tokenHandler.WriteToken(token));
    }
}