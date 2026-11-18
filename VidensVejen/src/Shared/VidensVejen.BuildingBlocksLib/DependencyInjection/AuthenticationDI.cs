using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace VidensVejen.BuildingBlocksLib.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class AuthenticationDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builder"></param>
    /// <returns></returns>
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
}
