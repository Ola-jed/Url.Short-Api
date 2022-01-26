using Url.Short_Api.Dto;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Mapping;

/// <summary>
/// Custom extensions for mapping UrlTypes
/// </summary>
public static class UrlTypeMappingExtensions
{
    public static UrlTypeReadDto ToUrlTypeReadDto(this UrlType urlType)
    {
        return new UrlTypeReadDto(urlType.Id,urlType.Domain,urlType.ShortName);
    }

    public static UrlType ToUrlType(this UrlTypeUpdateDto urlTypeUpdateDto)
    {
        return new UrlType
        {
            Domain = urlTypeUpdateDto.Domain,
            ShortName = urlTypeUpdateDto.ShortName
        };
    }

    public static UrlType ToUrlType(this UrlTypeCreateDto urlTypeCreateDto)
    {
        return new UrlType
        {
            Domain = urlTypeCreateDto.Domain,
            ShortName = urlTypeCreateDto.ShortName
        };
    }
}