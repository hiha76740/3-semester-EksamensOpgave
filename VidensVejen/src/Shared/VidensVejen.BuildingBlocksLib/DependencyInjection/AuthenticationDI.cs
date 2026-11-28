using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VidensVejen.ContractsLib.Authentication;

namespace VidensVejen.BuildingBlocksLib.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class AuthenticationDI
{
    /// <summary>
    /// Adds VidensVejen JWT bearer authentication to the service collection.
    /// </summary>
    /// <remarks>Configures issuer, audience, lifetime, and signing key validation. The access token may be
    /// supplied through the accessToken cookie.</remarks>
    /// <param name="services">The service collection to configure.</param>
    /// <param name="builder">The web application builder used to read JWT configuration.</param>
    /// <returns>The configured service collection.</returns>
    public static IServiceCollection AddVidensVejenAuthentication(this IServiceCollection services, WebApplicationBuilder builder)
    {
        _ = services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)),
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = ctx =>
            {
                _ = ctx.Request.Cookies.TryGetValue("accessToken", out var accessToken);
                if (string.IsNullOrEmpty(accessToken) == false)
                    ctx.Token = accessToken;

                return Task.CompletedTask;
            }
        };
    });

        return services;
    }

    /// <summary>
    /// Adds VidensVejen authorization policies for application permissions.
    /// </summary>
    /// <param name="services">The service collection to which the authorization policies are added.</param>
    /// <returns>The service collection for method chaining.</returns>
    public static IServiceCollection AddVidensVejenAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
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
                options.AddPolicy(permission, policy =>
                {
                    policy.RequireAuthenticatedUser()
                    .RequireClaim(CustomClaimTypes.Permission, permission, permission);
                });
            }

            options.AddPolicy("Users.Access", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Read", "User.Create", "User.Edit", "User.Delete"));
            options.AddPolicy("Users.List", policy => policy.RequireAuthenticatedUser()
                .RequireClaim(CustomClaimTypes.Permission, "User.Read", "User.Edit", "User.Delete"));
        });

        return services;
    }
}
