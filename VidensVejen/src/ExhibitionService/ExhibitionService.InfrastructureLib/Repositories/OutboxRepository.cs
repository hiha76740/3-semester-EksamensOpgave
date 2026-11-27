using ExhibitionService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

using VidensVejen.BuildingBlocksLib.Messaging.Outbox;

namespace ExhibitionService.InfrastructureLib.Repositories;


/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class OutboxRepository(ExhibitionDbContext db) : IOutboxRepository
{
    async Task IOutboxRepository.AddAsync(OutboxMessage outboxMessage)
    {
        await db.Outbox.AddAsync(outboxMessage);
    }

    async Task<OutboxMessage?> IOutboxRepository.FindAsync(Guid eventId, CancellationToken cancellationToken)
    {
        return await db.Outbox.FindAsync(
            keyValues: [eventId],
            cancellationToken: cancellationToken);
    }

    async Task<IReadOnlyList<Guid>> IOutboxRepository.GetPendingIdsAsync(int limit, CancellationToken cancellationToken)
    {
        return await db.Outbox
            .Where(m => m.PublishedAtUtc == null)
            .OrderBy(m => m.CreatedAtUtc)
            .ThenBy(m => m.Id)
            .Take(limit)
            .Select(m => m.Id)
            .ToListAsync(cancellationToken);
    }
}
