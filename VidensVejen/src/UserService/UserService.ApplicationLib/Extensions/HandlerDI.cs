using Microsoft.Extensions.DependencyInjection;
using UserService.ApplicationLib.Handlers;
using UserService.ApplicationLib.Messaging.Inbox.Handlers;
using UserService.ApplicationLib.Messaging.Outbox;
using UserService.FacadeLib.Commands.Interfaces;

namespace UserService.ApplicationLib.Extensions;

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
        services.AddScoped<IUpdateUserHandler, UpdateUserHandler>();
        services.AddScoped<ProcessInboxMessageHandler>();
        services.AddScoped<PublishOutboxMessageHandler>();

        return services;
    }
}
