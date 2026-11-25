using System.Text.Json;
using StatisticsService.ApplicationLib.Repositories;
using StatisticsService.DomainLib.Entities;
using VidensVejen.ContractsLib.Events;

namespace StatisticsService.ApplicationLib.Messaging.Inbox.Strategies;

/// <summary>
/// 
/// </summary>
/// <param name="gameResultRepo"></param>
public class GameCompletedInboxStrategy(IGameResultRepository gameResultRepo) : IInboxMessageStrategy
{
    string IInboxMessageStrategy.EventType => typeof(GameCompletedIntegrationEvent).FullName!;

    async Task IInboxMessageStrategy.ApplyAsync(Guid eventId, string content, CancellationToken cancellationToken)
    {
        var integrationEvent = JsonSerializer.Deserialize<GameCompletedIntegrationEvent>(content)
            ?? throw new InvalidOperationException($"Inbox event {eventId} is missing a event");

        var gameResult = GameResult.Create(
            integrationEvent.SessionId,
            integrationEvent.QuizId,
            integrationEvent.QuizTitle,
            integrationEvent.ExhibitionId,
            integrationEvent.CorrectCount,
            integrationEvent.TotalCount,
            integrationEvent.CompletedAt);

        await gameResultRepo.AddAsync(gameResult);
    }
}
