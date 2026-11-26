using VidensVejen.ContractsLib.Responses.GameService;
using VidensVejen.ContractsLib.Requests.GameService.Drafts;
using VidensVejen.ContractsLib.Requests.GameService.Quizzes;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>
/// 
/// </summary>
public interface IQuizService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="draftId">The draft to publish.</param>
    /// <param name="quiz"></param>
    /// <returns></returns>
    Task<QuizIdResponse> PublishAsync(Guid draftId, CreateQuizRequest quiz);

    /// <summary>Creates an empty quiz draft and returns its identifier.</summary>
    Task<Guid> CreateDraftAsync();

    /// <summary>Replaces the editable contents of a quiz draft.</summary>
    Task SaveDraftAsync(Guid draftId, UpdateQuizDraftRequest quiz);

    /// <summary>Gets the drafts available to the current user.</summary>
    Task<IReadOnlyList<QuizDraftResponse>> GetDraftsAsync();

    /// <summary>Gets the published quizzes available for administration.</summary>
    Task<IReadOnlyList<QuizResponse>> GetPublishedAsync();

    /// <summary>Gets the contents of an existing draft.</summary>
    Task<QuizDraftResponse> GetDraftAsync(Guid id);

    /// <summary>Gets the contents of a published quiz.</summary>
    Task<QuizResponse> GetPublishedAsync(Guid id);

    /// <summary>Deletes an unpublished draft.</summary>
    Task DeleteDraftAsync(Guid id, DeleteQuizDraftRequest request);

    /// <summary>Deletes a published quiz.</summary>
    Task DeletePublishedAsync(Guid id, DeleteQuizRequest request);
}
