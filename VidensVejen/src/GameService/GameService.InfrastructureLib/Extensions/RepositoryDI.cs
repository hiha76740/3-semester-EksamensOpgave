using GameService.ApplicationLib.Repositories;
using GameService.InfrastructureLib.Repositories;
using Microsoft.Extensions.DependencyInjection;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;

namespace GameService.InfrastructureLib.Extensions;

/// <summary>
/// 
/// </summary>
public static class RepositoryDI
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddRepositoryDI(this IServiceCollection services)
    {
        services.AddScoped<IQuizRepository, QuizRepository>();
        services.AddScoped<IQuizDraftRepository, QuizDraftRepository>();
        services.AddScoped<IGameSessionRepository, GameSessionRepository>();
        services.AddScoped<IOutboxRepository, OutboxRepository>();

        return services;
    }
}
