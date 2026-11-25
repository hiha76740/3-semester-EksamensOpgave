using Microsoft.Extensions.DependencyInjection;
using StatisticsService.ApplicationLib.Messaging.Inbox.Handlers;

namespace StatisticsService.ApplicationLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class HandlerDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddHandlerDI(this IServiceCollection services)
    {
        services.AddScoped<ProcessInboxMessageHandler>();

        return services;
    }
}
