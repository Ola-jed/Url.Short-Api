using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Entities;

public class UrlType: Entity
{
    [Required] [StringLength(100)] public string Domain { get; set; } = string.Empty;

    [Required] [StringLength(100)] public string ShortName { get; set; } = string.Empty;
}