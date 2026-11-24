using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities;
using GameService.InfrastructureLib.Persistence;

namespace GameService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class GameSessionRepository(GameDbContext db) : IGameSessionRepository
{
    async Task IGameSessionRepository.AddAsync(GameSession session)
    {
        await db.GameSessions.AddAsync(session);
    }

    async Task IGameSessionRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
