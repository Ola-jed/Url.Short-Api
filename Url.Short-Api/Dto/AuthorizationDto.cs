using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Dto;

public record AuthorizationDto([Required] string Secret);