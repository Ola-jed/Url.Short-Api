namespace Url.Short_Api.Dto;

public record UrlShortenReadDto(int Id, string Url, string ShortUrl, int? LifetimeInHours, DateTime CreatedAt);