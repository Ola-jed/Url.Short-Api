namespace Url.Short_Api.Services.Authorization;

public interface IAuthorizationService
{
    Task<bool> ValidateAppSecret(string secret);
    Task<string> GenerateJwt();
}