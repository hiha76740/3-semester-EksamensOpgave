using System.Net;
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
    Task<HttpStatusCode> PublishAsync(Guid draftId, CreateQuizRequest quiz);

    /// <summary>Creates an empty quiz draft and returns its identifier.</summary>
    Task<Guid> CreateDraftAsync();

    /// <summary>Replaces the editable contents of a quiz draft.</summary>
    Task SaveDraftAsync(Guid draftId, UpdateQuizDraftRequest quiz);
}
