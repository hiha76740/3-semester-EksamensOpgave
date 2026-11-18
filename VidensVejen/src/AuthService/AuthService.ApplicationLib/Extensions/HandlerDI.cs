using AuthService.ApplicationLib.Handlers.Authentication;
using AuthService.ApplicationLib.Handlers.Users;
using AuthService.ApplicationLib.Messaging.Inbox.Handlers;
using AuthService.ApplicationLib.Messaging.Outbox;
using AuthService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace AuthService.ApplicationLib.Extensions;

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
        services.AddScoped<IRegisterUserHandler, RegisterUserHandler>();
        services.AddScoped<IDeleteUserHandler, DeleteUserHandler>();
        services.AddScoped<IUpdateUserEmailHandler, UpdateUserEmailHandler>();
        services.AddScoped<IUpdateUserPasswordHandler, UpdateUserPasswordHandler>();
        services.AddScoped<IUpdateUserPermissionsHandler, UpdateUserPermissionsHandler>();

        services.AddScoped<ILoginUserHandler, LoginUserHandler>();
        services.AddScoped<IRefreshTokenHandler, RefreshTokenHandler>();

        services.AddScoped<PublishOutboxMessageHandler>();
        services.AddScoped<ProcessInboxMessageHandler>();

        return services;
    }
}
