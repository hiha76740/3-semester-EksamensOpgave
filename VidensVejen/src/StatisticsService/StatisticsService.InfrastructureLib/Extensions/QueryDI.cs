using Microsoft.Extensions.DependencyInjection;
using StatisticsService.FacadeLib.Queries.Interfaces;
using StatisticsService.InfrastructureLib.QueryHandlers;

namespace StatisticsService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class QueryDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddQueryDI(this IServiceCollection services)
    {
        services.AddScoped<IGameResultQueries, GameResultQueryHandlerIMPL>();

        return services;
    }
}
