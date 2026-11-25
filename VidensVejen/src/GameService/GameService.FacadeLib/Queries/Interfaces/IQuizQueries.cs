using GameService.FacadeLib.Queries.DTOs;

namespace GameService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IQuizQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<QuizDto>> GetAllPublishedQuizzes();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<QuizDto?> GetQuizByIdAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibitionId"></param>
    /// <returns></returns>
    Task<List<QuizDto>> GetQuizzesByExhibitionAsync(Guid exhibitionId);
}
