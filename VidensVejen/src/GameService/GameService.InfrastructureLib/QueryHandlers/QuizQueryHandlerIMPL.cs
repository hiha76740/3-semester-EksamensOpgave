using GameService.FacadeLib.Queries.DTOs;
using GameService.FacadeLib.Queries.Interfaces;

namespace GameService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
public class QuizQueryHandlerIMPL : IQuizQueries
{
    Task<QuizDto?> IQuizQueries.GetQuizByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
