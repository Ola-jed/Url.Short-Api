using Microsoft.EntityFrameworkCore;
using Url.Short_Api.Data;

namespace Url.Short_Api.Services.ExpiredLinkDeletion;

public sealed class ExpiredLinkDeletionService : IHostedService, IDisposable
{
    private readonly ILogger<ExpiredLinkDeletionService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private Timer? _timer;

    public ExpiredLinkDeletionService(ILogger<ExpiredLinkDeletionService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{} is starting", nameof(ExpiredLinkDeletionService));
        _timer = new Timer(DeleteExpiredLinks, null, TimeSpan.Zero, TimeSpan.FromHours(1));
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("{} is stopping", nameof(ExpiredLinkDeletionService));
        _timer?.Change(Timeout.Infinite, 0);
        return Task.CompletedTask;
    }

    private void DeleteExpiredLinks(object? state)
    {
        _logger.LogInformation("{} is deleting expired links", nameof(ExpiredLinkDeletionService));
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var entriesCount = context.Database.ExecuteSqlInterpolated(
            $"DELETE FROM \"UrlShortens\" WHERE \"LifetimeInHours\" > 0 AND \"CreatedAt\" < {DateTime.Now} - (\"LifetimeInHours\" * interval '1 hour') ");
        _logger.LogInformation("{} deleted {} expired links", nameof(ExpiredLinkDeletionService), entriesCount);
    }
    
    public void Dispose()
    {
        _timer?.Dispose();
    }
}