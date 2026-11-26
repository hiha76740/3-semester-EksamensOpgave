using GameService.ApplicationLib.Services;
using GameService.InfrastructureLib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.InfrastructureLib.Extensions;

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
    public static IServiceCollection AddInternalServicesDI(this IServiceCollection services)
    {
        services.AddScoped<IExhibitionService, ExhibitionService>();

        return services;
    }
}
