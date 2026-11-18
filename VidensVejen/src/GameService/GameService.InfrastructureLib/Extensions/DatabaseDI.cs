using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.InfrastructureLib.Extensions;

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
        var conn = configuration.GetConnectionString("GameDb");
        services.AddDbContext<GameDbContext>(options =>
        options.UseSqlServer(conn));

        return services;
    }
}
