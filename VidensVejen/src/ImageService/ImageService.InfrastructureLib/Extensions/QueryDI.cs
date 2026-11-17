using ImageService.FacadeLib.Queries.Interfaces;
using ImageService.InfrastructureLib.QueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.InfrastructureLib.Extensions;

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
        services.AddScoped<IImageQueries, ImageQueryHandlerIMPL>();

        return services;
    }
}
