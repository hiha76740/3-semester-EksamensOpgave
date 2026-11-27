using GameService.DomainLib.Entities.Quizzes;
using GameService.DomainLib.ValueObjects;

namespace GameService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IQuizRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <returns></returns>
    Task AddAsync(Quiz quiz);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    void Delete(Quiz quiz);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Quiz>> GetAllForExhibitionAsync(ExhibitionId id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Quiz?> GetQuizByIdAsync(QuizId id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="quiz"></param>
    /// <param name="rowVersion"></param>
    void SetOriginalRowVersion(Quiz quiz, byte[] rowVersion);
}
