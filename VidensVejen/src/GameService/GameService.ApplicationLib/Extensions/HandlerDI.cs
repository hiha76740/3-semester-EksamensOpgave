using GameService.ApplicationLib.Handlers;
using GameService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GameService.ApplicationLib.Extensions;

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
        services.AddScoped<ICreateQuizHandler, CreateQuizHandler>();
        services.AddScoped<IUpdateQuizHandler, UpdateQuizHandler>();
        services.AddScoped<IPublishQuizHandler, PublishQuizHandler>();
        services.AddScoped<IDeleteQuizHandler, DeleteQuizHandler>();

        return services;
    }
}
