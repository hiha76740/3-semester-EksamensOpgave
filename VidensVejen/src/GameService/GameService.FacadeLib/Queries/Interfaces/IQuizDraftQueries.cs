using GameService.FacadeLib.Queries.DTOs;

namespace GameService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IQuizDraftQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<QuizDraftDto?> GetQuizDraftByIdAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<List<QuizDraftDto>> GetQuizDraftsAsync();
}
