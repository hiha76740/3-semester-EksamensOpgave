using GameService.ApplicationLib.Messaging.Inbox.Strategies;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.ApplicationLib.Extensions;

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
        services.AddScoped<IInboxMessageStrategy, DeleteQuizInboxStrategy>();

        return services;
    }
}
