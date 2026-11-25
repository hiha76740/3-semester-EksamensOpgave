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
        services.AddScoped<ICreateQuizDraftHandler, CreateQuizDraftHandler>();
        services.AddScoped<IUpdateQuizDraftHandler, UpdateQuizDraftHandler>();
        services.AddScoped<IPublishQuizHandler, PublishQuizHandler>();
        services.AddScoped<IStartSessionHandler, StartSessionHandler>();
        services.AddScoped<IAnswerQuestionHandler, AnswerQuestionHandler>();
        services.AddScoped<IDeleteQuizHandler, DeleteQuizHandler>();

        return services;
    }
}
