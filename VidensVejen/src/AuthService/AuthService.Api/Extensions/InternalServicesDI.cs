using AuthService.Api.Services;

namespace AuthService.Api.Extensions;

internal static class InternalServicesDI
{
    internal static IServiceCollection AddInternalServicesDI(this IServiceCollection services)
    {
        services.AddScoped<ICookieService, CookieService>();

        return services;
    }
}
