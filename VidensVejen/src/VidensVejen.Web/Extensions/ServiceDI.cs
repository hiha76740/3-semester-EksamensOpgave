using VidensVejen.Web.Services;
using VidensVejen.Web.ServicesInterfaces;

namespace VidensVejen.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class ServiceDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServiceDI(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthUserService, AuthUserService>();
        services.AddScoped<IExhibitionService, ExhibitionService>();

        return services;
    }
}
