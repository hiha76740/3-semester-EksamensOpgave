using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using VidensVejen.BuildingBlocksLib.Messaging.Inbox;

namespace GameService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
public class InboxRepository(GameDbContext db) : IInboxRepository
{
    async Task<InboxMessage?> IInboxRepository.FindAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await db.Inbox.FindAsync(
            keyValues: [eventId],
            cancellationToken: cancellationToken);
    }

    async Task<IReadOnlyList<Guid>> IInboxRepository.GetPendingIdsAsync(int limit, CancellationToken cancellationToken)
    {
        return await db.Inbox
            .Where(message => message.ProccessedOnUtc == null)
            .OrderBy(message => message.RecievedOnUtc)
            .ThenBy(message => message.Id)
            .Take(limit)
            .Select(message => message.Id)
            .ToListAsync(cancellationToken);
    }

    async Task IInboxRepository.StoreIfMissingAsync(InboxMessage message, CancellationToken cancellationToken)
    {
        db.Inbox.Add(message);

        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException)
        {
            db.Entry(message).State = EntityState.Detached;

            var alreadyStored = await db.Inbox
                .AnyAsync(e => e.Id == message.Id, cancellationToken);

            if (alreadyStored == false)
            {
                throw;
            }
        }
    }
}
