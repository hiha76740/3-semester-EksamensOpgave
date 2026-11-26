using GameService.DomainLib.Entities.Quizzes;

namespace GameService.ApplicationLib.Services;

/// <summary>
/// 
/// </summary>
public interface IExhibitionService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quizId"></param>
    /// <returns></returns>
    Task<bool> QuizInUseAsync(QuizId quizId);
}
