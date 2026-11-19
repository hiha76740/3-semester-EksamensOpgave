using GameService.FacadeLib.Commands.DTOs;
using GameService.FacadeLib.Commands.Interfaces;
using GameService.FacadeLib.Queries.DTOs;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class UpdateQuizHandler : IUpdateQuizHandler
{
    Task<QuizDto> IUpdateQuizHandler.HandleAsync(UpdateQuizCommand command)
    {
        throw new NotImplementedException();
    }
}
