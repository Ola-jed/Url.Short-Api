using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Url.Short_Api.Entities;

[Index(nameof(Domain), IsUnique = true)]
[Index(nameof(ShortName), IsUnique = true)]
public class UrlType: Entity
{
    [Required] [StringLength(100)] public string Domain { get; set; } = null!;

    [Required] [StringLength(100)] public string ShortName { get; set; } = null!;
}