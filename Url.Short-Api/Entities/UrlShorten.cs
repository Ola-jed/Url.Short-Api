using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Url.Short_Api.Entities;

[Index(nameof(ShortUrl), IsUnique = true)]
public class UrlShorten: Entity
{
    [Required] [Url] public string Url { get; set; } = string.Empty;

    [Required] [StringLength(150)] public string ShortUrl { get; set; } = string.Empty;

    public int? LifetimeInHours { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; }
}