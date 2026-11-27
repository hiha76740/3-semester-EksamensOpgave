using ExhibitionService.InfrastructureLib.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using VidensVejen.BuildingBlocksLib.UnitOfWork;

namespace ExhibitionService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class UnitOfWorkDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddUnitOfWorkDI(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, EFUnitOfWork>();

        return services;
    }
}
