using GameService.FacadeLib.Commands.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IPublishQuizHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="draftId"></param>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<Guid> HandleAsync(Guid draftId, CreateQuizCommand command);
}
