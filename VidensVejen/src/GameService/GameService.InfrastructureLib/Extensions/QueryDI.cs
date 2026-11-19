using GameService.FacadeLib.Queries.Interfaces;
using GameService.InfrastructureLib.QueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.InfrastructureLib.Extensions;

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
        services.AddScoped<IQuizQueries, QuizQueryHandlerIMPL>();

        return services;
    }
}
