using System;
using System.Threading;
using System.Threading.Tasks;
using api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class RefreshTokenCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<RefreshTokenCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval;

    public RefreshTokenCleanupService(
        IServiceScopeFactory scopeFactory,
        ILogger<RefreshTokenCleanupService> logger
    )
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _cleanupInterval = TimeSpan.FromMinutes(5); // Default interval, can be configurable
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("RefreshTokenCleanupService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await DoWorkAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during refresh token cleanup.");
            }

            await Task.Delay(_cleanupInterval, stoppingToken);
        }

        _logger.LogInformation("RefreshTokenCleanupService stopped.");
    }

    private async Task DoWorkAsync(CancellationToken stoppingToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

            // Log the cleanup process
            _logger.LogInformation("Starting refresh token cleanup at {Time}.", DateTime.UtcNow);

            // Delete expired tokens
            int deletedCount = await context
                .RefreshTokens.Where(rt => rt.ExpiresOnUtc < DateTime.UtcNow)
                .ExecuteDeleteAsync(stoppingToken);

            _logger.LogInformation("Deleted {Count} expired refresh tokens.", deletedCount);

            await context.SaveChangesAsync(stoppingToken);
        }
    }
}
