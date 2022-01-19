using System.ComponentModel.DataAnnotations;

namespace Url.Short_Api.Dto;

public record UrlTypeUpdateDto([Required] [StringLength(100)] string Domain,
    [Required] [StringLength(100)] string ShortName);