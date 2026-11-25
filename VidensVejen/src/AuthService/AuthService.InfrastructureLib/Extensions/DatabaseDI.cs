using AuthService.InfrastructureLib.Persistence;
using AuthService.InfrastructureLib.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class DatabaseDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddDatabaseDI(this IServiceCollection services, IConfiguration configuration)
    {
        var conn = configuration.GetConnectionString("AuthUserDb");
        services.AddDbContext<AuthUserDbContext>(options =>
        {
            options.UseSqlServer(conn);

            var email = configuration["BootstrapAdmin:Email"];
            if (string.IsNullOrWhiteSpace(email) == false)
            {
                options.UseSeeding((context, _) =>
                    AuthUserConfiguration.SeedAdmin((AuthUserDbContext)context, email, configuration["BootstrapAdmin:Password"]));
            }
        });

        return services;
    }

    /// <summary>
    /// Applies pending EF Core migrations to this service's database.
    /// </summary>
    /// <param name="services">The application service provider.</param>
    public static void ApplyDatabaseMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AuthUserDbContext>();
        context.Database.Migrate();
    }
}
