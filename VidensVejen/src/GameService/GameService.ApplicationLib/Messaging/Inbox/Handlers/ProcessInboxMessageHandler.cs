using System.Data;
using GameService.ApplicationLib.Messaging.Inbox.Strategies;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;
using VidensVejen.BuildingBlocksLib.Messaging.Outbox;
using VidensVejen.BuildingBlocksLib.UnitOfWork;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.ApplicationLib.Messaging.Inbox.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="inbox"></param>
/// <param name="outbox"></param>
/// <param name="unitOfWork"></param>
/// <param name="strategies"></param>
public class ProcessInboxMessageHandler(
    IInboxRepository inbox,
    IOutboxRepository outbox,
    IUnitOfWork unitOfWork,
    IEnumerable<IInboxMessageStrategy> strategies
    )
{

    private readonly Dictionary<string, IInboxMessageStrategy> _strategiesByEventType =
        strategies.ToDictionary(
            strategy => strategy.EventType,
            StringComparer.Ordinal
            );

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    /// <exception cref="NotFoundException"></exception>
    public async Task HandleAsync(
        Guid eventId,
        CancellationToken cancellationToken
        )
    {
        unitOfWork.BeginTransaction(IsolationLevel.Serializable);

        try
        {
            var message = await inbox.FindAsync(eventId, cancellationToken);

            if (message == null || message.ProccessedOnUtc.HasValue)
            {
                unitOfWork.Commit();
                return;
            }

            if (_strategiesByEventType.TryGetValue(message.Type, out var strategy) == false)
            {
                throw new InvalidOperationException($"No inbox-strategy for the type '{message.Type}' on event '{eventId}'.");
            }

            var outboxMessage = await strategy.ApplyAsync(message.Id, message.Content, cancellationToken);


            message.SetProccesedAtUtc();

            if (outboxMessage != null)
                await outbox.AddAsync(outboxMessage);

            await unitOfWork.SaveChangesAsync();
            unitOfWork.Commit();

        }
        catch (Exception)
        {
            unitOfWork.Rollback();
            throw;
        }

    }
}
