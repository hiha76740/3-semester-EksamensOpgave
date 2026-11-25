using GameService.DomainLib.Entities.Sessions;

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
    /// <param name="id"></param>
    /// <returns></returns>
    Task<GameSession?> GetSessionByIdAsync(GameSessionId id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
