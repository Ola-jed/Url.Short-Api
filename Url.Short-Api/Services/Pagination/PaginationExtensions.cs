using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Services.Pagination;

public static class PaginationExtensions
{
    public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> self,
        PageParameters parameters) where TEntity : Entity
    {
        return self.OrderBy(entity => entity.Id)
            .Skip(parameters.Offset)
            .Take(parameters.PerPage);
    }
}