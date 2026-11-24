using GameService.DomainLib.Entities;

namespace GameService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IGameSessionRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    Task AddAsync(GameSession session);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
