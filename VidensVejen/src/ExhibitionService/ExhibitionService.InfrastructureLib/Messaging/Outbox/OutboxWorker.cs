using ExhibitionService.ApplicationLib.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;

namespace ExhibitionService.InfrastructureLib.Messaging.Outbox;

/// <summary>
/// 
/// </summary>
public sealed class OutboxWorker(
    IServiceScopeFactory scopeFactory,
    ILogger<OutboxWorker> logger
    ) : BackgroundService
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

                var repository = scope.ServiceProvider
                    .GetRequiredService<IOutboxRepository>();

                eventIds = await repository.GetPendingIdsAsync(limit: 50, cancellationToken: stoppingToken);
            }
            catch (Exception ex) when (stoppingToken.IsCancellationRequested == false)
            {
                if (logger.IsEnabled(LogLevel.Error))
                    logger.LogError(ex, "Could not get pending outbox messages");

                continue;
            }



            foreach (var eventId in eventIds)
            {
                try
                {
                    await using var scope = scopeFactory.CreateAsyncScope();

                    var handler = scope.ServiceProvider
                        .GetRequiredService<PublishOutboxMessageHandler>();

                    await handler.HandleAsync(eventId, stoppingToken);

                }
                catch (Exception ex) when (stoppingToken.IsCancellationRequested == false)
                {
                    if (logger.IsEnabled(LogLevel.Error))
                        logger.LogError(ex, "Publish of outbox-event {EventId} made an error", eventId);
                }
            }
        }
    }
}
