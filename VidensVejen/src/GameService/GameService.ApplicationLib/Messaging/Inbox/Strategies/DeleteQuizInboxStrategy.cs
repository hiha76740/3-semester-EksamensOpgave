using System.Text.Json;
using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.ValueObjects;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.ContractsLib.Events;

namespace GameService.ApplicationLib.Messaging.Inbox.Strategies;


/// <summary>
/// 
/// </summary>
/// <param name="repo"></param>
public class DeleteQuizInboxStrategy(
    IQuizRepository repo
    ) : IInboxMessageStrategy
{
    string IInboxMessageStrategy.EventType => typeof(QuizDeletionIntegrationEvent).FullName!;

    async Task<OutboxMessage?> IInboxMessageStrategy.ApplyAsync(Guid eventId, string content, CancellationToken cancellationToken)
    {
        var integrationEvent = JsonSerializer.Deserialize<QuizDeletionIntegrationEvent>(content)
                ?? throw new InvalidOperationException($"Inbox event {eventId} is missing a event");

        var id = new ExhibitionId(integrationEvent.ExhibitionId);

        var list = await repo.GetAllForExhibitionAsync(id);

        foreach (var quiz in list)
        {
            repo.Delete(quiz);
        }

        return null;
    }
}
