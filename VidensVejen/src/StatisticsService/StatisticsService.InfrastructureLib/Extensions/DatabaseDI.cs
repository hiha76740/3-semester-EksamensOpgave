using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StatisticsService.InfrastructureLib.Persistence;

namespace StatisticsService.InfrastructureLib.Extensions;

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
        var conn = configuration.GetConnectionString("StatisticsDb");
        services.AddDbContext<StatisticsDbContext>(options =>
        options.UseSqlServer(conn));

        return services;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public static void ApplyDatabaseMigrations(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<StatisticsDbContext>();
        context.Database.Migrate();
    }
}
