using StatisticsService.ApplicationLib.Repositories;
using StatisticsService.DomainLib.Entities;
using StatisticsService.InfrastructureLib.Persistence;

namespace StatisticsService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class GameResultRepository(StatisticsDbContext db) : IGameResultRepository
{
    async Task IGameResultRepository.AddAsync(GameResult gameResult)
    {
        await db.GameResults.AddAsync(gameResult);
    }
}
