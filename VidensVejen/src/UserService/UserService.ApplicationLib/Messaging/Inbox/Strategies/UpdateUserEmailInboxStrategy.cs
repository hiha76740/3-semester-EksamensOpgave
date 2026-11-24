using System.Text.Json;
using UserService.ApplicationLib.Repositories;
using UserService.DomainLib.Entities;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.ContractsLib.Events;
using VidensVejen.SharedKernelLib.Exceptions;

namespace UserService.ApplicationLib.Messaging.Inbox.Strategies;

/// <summary>
/// 
/// </summary>
/// <param name="userRepo"></param>
public class UpdateUserEmailInboxStrategy(IUserRepository userRepo) : IInboxMessageStrategy
{
    string IInboxMessageStrategy.EventType => typeof(UserUpdateEmailIntegrationEvent).FullName!;

    async Task<OutboxMessage?> IInboxMessageStrategy.ApplyAsync(Guid eventId, string content, CancellationToken cancellationToken)
    {
        var integrationEvent = JsonSerializer.Deserialize<UserUpdateEmailIntegrationEvent>(content)
                 ?? throw new InvalidOperationException($"Inbox event {eventId} is missing a event");

        var userId = new UserId(integrationEvent.UserId);

        var user = await userRepo.GetUserByIdAsync(userId)
            ?? throw new NotFoundException("Email update error, User not found");

        user.ChangeEmail(integrationEvent.Email);

        var userActivateIntegrationEvent = new UserActivateIntegrationEvent(
                eventId,
                "user-activate",
                user.Id.Value
                );

        var outboxMessage = new OutboxMessage
        {
            Id = userActivateIntegrationEvent.EventId,
            Type = userActivateIntegrationEvent.GetType().FullName!,
            Topic = userActivateIntegrationEvent.EventTopic,
            Content = JsonSerializer.Serialize(userActivateIntegrationEvent),
            CreatedAtUtc = DateTime.UtcNow
        };

        return outboxMessage;
    }
}
