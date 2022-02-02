using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Dto;

public record UrlShortenCustomCreateDto([Required] [Url] string Url,
    [Required] [StringLength(150)] [RegularExpression("^[a-zA-Z0-9]+")] string ShortUrl,
    int? LifetimeInHours);