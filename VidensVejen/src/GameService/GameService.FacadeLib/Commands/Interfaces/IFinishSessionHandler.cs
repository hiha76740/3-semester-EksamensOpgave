using GameService.FacadeLib.Queries.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IFinishSessionHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task<SessionScoreDto> HandleAsync(Guid sessionId);
}
