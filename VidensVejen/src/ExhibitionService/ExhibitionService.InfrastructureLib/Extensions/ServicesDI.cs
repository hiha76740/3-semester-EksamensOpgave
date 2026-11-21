using ExhibitionService.ApplicationLib.Services;
using ExhibitionService.InfrastructureLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ExhibitionService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServicesDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServicesDI(this IServiceCollection services)
    {
        services.AddScoped<IImageService, ImageService>();

        return services;
    }
}
