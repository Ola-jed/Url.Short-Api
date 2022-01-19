using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Services.Pagination;

public static class PaginationExtensions
{
    public static async Task<IEnumerable<TEntity>> Paginate<TEntity>(this IQueryable<TEntity> self,
        PageParameters parameters) where TEntity : Entity
    {
        return await self.OrderBy(entity => entity.Id)
            .Skip(parameters.Offset)
            .Take(parameters.PerPage)
            .ToListAsync();
    }
}