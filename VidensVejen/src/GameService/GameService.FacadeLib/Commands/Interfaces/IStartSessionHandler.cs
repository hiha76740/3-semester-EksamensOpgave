using GameService.FacadeLib.Queries.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IStartSessionHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
    Task<StartSessionDto> HandleAsync(Guid quizId);
}
