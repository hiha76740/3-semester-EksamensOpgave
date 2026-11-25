using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.InfrastructureLib.Persistence;
using UserService.InfrastructureLib.Persistence.Configurations;

namespace UserService.InfrastructureLib.Extensions;

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
        var conn = configuration.GetConnectionString("UserDb");
        services.AddDbContext<UserDbContext>(options =>
        {
            options.UseSqlServer(conn);

            var email = configuration["BootstrapAdmin:Email"];
            if (string.IsNullOrWhiteSpace(email) == false)
            {
                options.UseSeeding((context, _) =>
                    UserConfiguration.SeedAdmin((UserDbContext)context, email));
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
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        context.Database.Migrate();
    }
}
