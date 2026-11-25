using StatisticsService.FacadeLib.Queries.DTOs;

namespace StatisticsService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IGameResultQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
    Task<QuizAverageDto> GetTodayAverageAsync(Guid quizId);
}
