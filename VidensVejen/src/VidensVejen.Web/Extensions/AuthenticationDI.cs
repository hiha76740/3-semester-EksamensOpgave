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
    /// Adds the Blazor authentication and authorization services to the dependency injection container.
    /// </summary>
    /// <remarks>Registers authorization policies, cascading authentication state, and the custom
    /// authentication state provider.</remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <returns>The same service collection so additional registrations can be chained.</returns>
    public static IServiceCollection AddBlazorAuthenticationDI(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            string[] permissions =
            [
                "User.Create", "User.Edit", "User.Delete", "User.Read",
                "Exhibition.Publish", "Exhibition.Edit", "Exhibition.Delete", "Exhibition.Read",
                "Game.Publish", "Game.Edit", "Game.Delete", "Game.Read",
                "Image.Read", "Image.Delete"
            ];
            foreach (var permission in permissions)
            {
                options.AddPolicy(permission, policy => policy.RequireAuthenticatedUser()
                    .RequireClaim(CustomClaimTypes.Permission, permission));
            }

            options.AddPolicy("Images.Access", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "Image.Read", "Image.Delete"));
            options.AddPolicy("Users.Access", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Read", "User.Create", "User.Edit", "User.Delete"));
            options.AddPolicy("Users.Profile", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Read", "User.Edit"));
            options.AddPolicy("Users.List", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Read", "User.Edit", "User.Delete"));
            options.AddPolicy("Exhibitions.Access", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "Exhibition.Read", "Exhibition.Edit", "Exhibition.Publish", "Exhibition.Delete"));
            options.AddPolicy("Exhibitions.Editor", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "Exhibition.Edit", "Exhibition.Publish"));
            options.AddPolicy("Games.Access", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "Game.Read", "Game.Edit", "Game.Publish", "Game.Delete"));
            options.AddPolicy("Games.Editor", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "Game.Edit", "Game.Publish"));
        });
        services.AddCascadingAuthenticationState();

        services.AddScoped<CustomAuthenticationStateProvider>();

        services.AddScoped<AuthenticationStateProvider>(serviceProvider =>
            serviceProvider.GetRequiredService<CustomAuthenticationStateProvider>());

        return services;
    }
}
