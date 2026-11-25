using Microsoft.Extensions.DependencyInjection;
using StatisticsService.InfrastructureLib.Messaging.Inbox;

namespace StatisticsService.InfrastructureLib.Extensions;

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
        services.AddHostedService<InboxWorker>();

        return services;
    }
}
