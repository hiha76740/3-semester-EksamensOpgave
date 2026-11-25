using GameService.FacadeLib.Commands.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IUpdateQuizDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="draftId"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(Guid draftId, CreateQuizCommand command);
}
