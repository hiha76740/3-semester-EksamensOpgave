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
    /// Applies database migrations and initializes the administrator when configured.
    /// </summary>
    /// <param name="services">The application service provider.</param>
    /// <param name="configuration">The application configuration.</param>
    public static void InitializeAdminDatabase(this IServiceProvider services, IConfiguration configuration)
    {
        if (string.IsNullOrWhiteSpace(configuration["BootstrapAdmin:Email"]))
        {
            return;
        }

        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<UserDbContext>();
        context.Database.Migrate();
    }
}
