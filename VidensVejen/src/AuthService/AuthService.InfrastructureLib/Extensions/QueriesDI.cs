using AuthService.FacadeLib.Queries.Interfaces;
using AuthService.InfrastructureLib.QueryHandlers;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.InfrastructureLib.Extensions;

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
        services.AddScoped<IAuthUsersQueries, AuthUserQueryHandlerIMPL>();
        services.AddScoped<IPermissionsQueries, PermissionQueryHandlerIMPL>();

        return services;
    }
}
