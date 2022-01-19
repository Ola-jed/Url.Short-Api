using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Dto;

public record UrlShortenCreateDto([Required] [Url] string Url, int? LifetimeInHours);