using System.Text.Json;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.Messaging.Publisher;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.ContractsLib;

namespace GameService.ApplicationLib.Messaging.Outbox;

/// <summary>
/// 
/// </summary>
/// <param name="outbox"></param>
/// <param name="unitOfWork"></param>
/// <param name="eventPublisher"></param>
public sealed class PublishOutboxMessageHandler(
    IOutboxRepository outbox,
    IUnitOfWork unitOfWork,
    IOutboxEventPublisher eventPublisher
    )
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandleAsync(
        Guid eventId,
        CancellationToken cancellationToken
        )
    {
        var message = await outbox.FindAsync(eventId, cancellationToken);

        if (message == null || message.PublishedAtUtc.HasValue)
        {
            return;
        }

        var messageType = AssemblyReference.Assembly.GetType(message.Type)!;
        var deserializedMessage = JsonSerializer.Deserialize(message.Content, messageType)!;

        await eventPublisher.PublishAsync(message.Topic, deserializedMessage);

        message.SetPublishAtUtc();

        await unitOfWork.SaveChangesAsync();
    }
}
