using GameService.ApplicationLib.Repositories;
using GameService.DomainLib.Entities.Sessions;
using GameService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

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

    async Task<GameSession?> IGameSessionRepository.GetSessionByIdAsync(GameSessionId id)
    {
        return await db.GameSessions
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    async Task IGameSessionRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
