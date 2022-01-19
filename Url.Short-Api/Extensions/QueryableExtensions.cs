using AutoMapper;
using AutoMapper.QueryableExtensions;
using Url.Short_Api.Data.Profiles;

namespace Url.Short_Api.Extensions;

public static class QueryableExtensions
{
    private static readonly MapperConfiguration MapperConfig = new(m =>
    {
        m.AddProfile<UrlShortenProfile>();
        m.AddProfile<UrlTypeProfile>();
    });

    public static IQueryable<TDestination> MapTo<TSource, TDestination>(this IQueryable<TSource> source)
    {
        return source.ProjectTo<TDestination>(MapperConfig);
    }
}