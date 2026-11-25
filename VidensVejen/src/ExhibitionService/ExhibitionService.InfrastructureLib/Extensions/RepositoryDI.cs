using ExhibitionService.ApplicationLib.Repositories;
using ExhibitionService.InfrastructureLib.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace ExhibitionService.InfrastructureLib.Extensions;

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
        services.AddScoped<IExhibitionRepository, ExhibitionRepository>();
        services.AddScoped<IExhibitionDraftRepository, ExhibitionDraftRepository>();

        return services;
    }
}
