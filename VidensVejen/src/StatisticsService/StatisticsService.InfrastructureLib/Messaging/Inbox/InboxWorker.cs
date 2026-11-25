using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StatisticsService.ApplicationLib.Messaging.Inbox.Handlers;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;

namespace StatisticsService.InfrastructureLib.Messaging.Inbox;

/// <summary>
/// 
/// </summary>
/// <param name="scopeFactory"></param>
/// <param name="logger"></param>
public class InboxWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<InboxWorker> logger) : BackgroundService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(5));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            IReadOnlyList<Guid> eventIds;

            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var repository = scope.ServiceProvider.GetRequiredService<IInboxRepository>();

                eventIds = await repository.GetPendingIdsAsync(limit: 50, cancellationToken: stoppingToken);
            }
            catch (Exception ex) when (stoppingToken.IsCancellationRequested == false)
            {
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError(ex, "Unable to get inbox-messages");

                continue;
            }

            foreach (var eventId in eventIds)
            {
                try
                {
                    await using var scope = scopeFactory.CreateAsyncScope();
                    var handler = scope.ServiceProvider.GetRequiredService<ProcessInboxMessageHandler>();

                    await handler.HandleAsync(eventId, stoppingToken);
                }
                catch (Exception ex) when (stoppingToken.IsCancellationRequested == false)
                {
                    if (logger.IsEnabled(LogLevel.Error))
                        logger.LogError(ex, "Process of inbox-message {EventId} failed", eventId);
                }
            }
        }
    }
}
