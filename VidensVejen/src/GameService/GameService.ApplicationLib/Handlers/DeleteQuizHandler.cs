using GameService.FacadeLib.Commands.Interfaces;

namespace GameService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
public class DeleteQuizHandler : IDeleteQuizHandler
{
    Task IDeleteQuizHandler.HandleAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
