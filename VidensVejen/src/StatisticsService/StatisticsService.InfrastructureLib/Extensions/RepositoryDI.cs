using Microsoft.Extensions.DependencyInjection;
using StatisticsService.ApplicationLib.Repositories;
using StatisticsService.InfrastructureLib.Repositories;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;

namespace StatisticsService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class RepositoryDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositoryDI(this IServiceCollection services)
    {
        services.AddScoped<IGameResultRepository, GameResultRepository>();
        services.AddScoped<IInboxRepository, InboxRepository>();

        return services;
    }
}
