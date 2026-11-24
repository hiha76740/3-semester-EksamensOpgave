using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Queries.DTOs;

namespace GameService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IAnswerQuestionHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<AnswerResultDto> HandleAsync(AnswerQuestionCommand command);
}
