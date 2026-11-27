using ExhibitionService.InfrastructureLib.Messaging.Outbox;
using Microsoft.Extensions.DependencyInjection;

namespace ExhibitionService.InfrastructureLib.Extensions;

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

        return services;
    }
}
