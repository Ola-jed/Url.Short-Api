using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Dto;

public record UrlShortenUpdateDto([Required] string Url, int? LifetimeInHours);