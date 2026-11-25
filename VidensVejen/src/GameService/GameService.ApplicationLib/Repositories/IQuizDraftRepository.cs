using GameService.DomainLib.Entities.Drafts;

namespace GameService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IQuizDraftRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="draft"></param>
    /// <returns></returns>
    Task AddAsync(QuizDraft draft);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<QuizDraft?> GetDraftByIdAsync(QuizDraftId id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="draft"></param>
    void Delete(QuizDraft draft);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
