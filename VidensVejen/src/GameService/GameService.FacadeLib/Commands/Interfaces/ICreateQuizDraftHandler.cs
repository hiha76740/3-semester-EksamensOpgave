using GameService.FacadeLib.Commands.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ICreateQuizDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<Guid> HandleAsync(CreateQuizCommand command);
}
