using StatisticsService.DomainLib.Entities;

namespace StatisticsService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IGameResultRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gameResult"></param>
    /// <returns></returns>
    Task AddAsync(GameResult gameResult);
}
