using GameService.InfrastructureLib.Messaging.Inbox;
using GameService.InfrastructureLib.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class BackgroundWorkerDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBackgroundWorkerDI(this IServiceCollection services)
    {
        services.AddHostedService<OutboxWorker>();
        services.AddHostedService<InboxWorker>();

        return services;
    }
}
