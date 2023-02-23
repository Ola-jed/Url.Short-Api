using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public DbSet<UrlShorten> UrlShortens { get; set; } = null!;
    public DbSet<UrlType> UrlTypes { get; set; } = null!;
}