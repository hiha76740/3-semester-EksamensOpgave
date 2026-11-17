using ImageService.ApplicationLib.Repositories;
using ImageService.InfrastructureLib.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ImageService.InfrastructureLib.Extensions;

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
        services.AddScoped<IImageRepository, ImageRepository>();

        return services;
    }
}
