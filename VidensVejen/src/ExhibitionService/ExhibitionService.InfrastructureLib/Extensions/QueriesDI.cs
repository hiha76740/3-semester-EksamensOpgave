using ExhibitionService.FacadeLib.Queries.Interfaces;
using ExhibitionService.InfrastructureLib.QueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace ExhibitionService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class QueriesDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddQueriesDI(this IServiceCollection services)
    {
        services.AddScoped<IExhibitionQueries, ExhibitionQueryHandlerIMPL>();

        return services;
    }
}
