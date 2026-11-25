using Microsoft.AspNetCore.Components.Authorization;
using VidensVejen.Web.Authentication;
using VidensVejen.ContractsLib.Authentication;

namespace VidensVejen.Web.Extensions;

/// <summary>
/// 
/// </summary>
public static class AuthenticationDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddBlazorAuthenticationDI(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy("User.Edit", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Edit"));
            options.AddPolicy("User.Delete", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Delete"));
            options.AddPolicy("User.Create", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireClaim(CustomClaimTypes.Permission, "User.Create");
            });
        });
        services.AddCascadingAuthenticationState();

        services.AddScoped<CustomAuthenticationStateProvider>();

        services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<CustomAuthenticationStateProvider>());

        return services;
    }
}
