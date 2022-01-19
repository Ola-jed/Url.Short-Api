using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Entities;

namespace Url.Short_Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UrlShorten>()
            .HasIndex(u => u.ShortUrl)
            .IsUnique();
        modelBuilder.Entity<UrlType>()
            .HasIndex(u => u.Domain)
            .IsUnique();
        modelBuilder.Entity<UrlType>()
            .HasIndex(u => u.ShortName)
            .IsUnique();
    }
    
    public DbSet<UrlShorten> UrlShortens { get; set; } = null!;
    public DbSet<UrlType> UrlTypes { get; set; } = null!;
}