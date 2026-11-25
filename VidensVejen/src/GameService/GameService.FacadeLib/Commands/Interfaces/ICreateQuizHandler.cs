using GameService.FacadeLib.Commands.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ICreateQuizHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(CreateQuizCommand command);
}
