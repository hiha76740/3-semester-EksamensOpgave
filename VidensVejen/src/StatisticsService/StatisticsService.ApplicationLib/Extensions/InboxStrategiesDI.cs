using Microsoft.Extensions.DependencyInjection;
using StatisticsService.ApplicationLib.Messaging.Inbox.Strategies;

namespace StatisticsService.ApplicationLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class InboxStrategiesDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddInboxStrategiesDI(this IServiceCollection services)
    {
        services.AddScoped<IInboxMessageStrategy, GameCompletedInboxStrategy>();

        return services;
    }
}
