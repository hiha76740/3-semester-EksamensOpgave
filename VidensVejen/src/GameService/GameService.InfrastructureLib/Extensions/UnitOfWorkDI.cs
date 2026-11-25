using GameService.InfrastructureLib.UnitOfWorks;
using Microsoft.Extensions.DependencyInjection;
using VidensVejen.BuildingBlocksLib.UnitOfWork;

namespace GameService.InfrastructureLib.Extensions;

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
