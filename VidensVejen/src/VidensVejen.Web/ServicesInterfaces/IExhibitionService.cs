using System.Net;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Drafts;
using VidensVejen.ContractsLib.Requests.ExhibitionService.Exhibitions;
using VidensVejen.ContractsLib.Responses.ExhibitionService;

namespace VidensVejen.Web.ServicesInterfaces;

/// <summary>
/// 
/// </summary>
public interface IExhibitionService
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<ExhibitionNameResponse>> GetAllNamesAsync();

    /// <summary>Gets the exhibition drafts for the overview.</summary>
    Task<IReadOnlyList<ExhibitionDraftResponse>> GetDraftsAsync();

    /// <summary>Deletes the specified exhibition draft.</summary>
    Task DeleteDraftAsync(Guid draftId);

    /// <summary>Deletes the specified published exhibition.</summary>
    Task DeletePublishedAsync(Guid exhibitionId);

    /// <summary>Creates an exhibition with its theme image.</summary>
    /// <param name="exhibition">The exhibition and its content.</param>
    /// <returns>The status code returned by ExhibitionService.</returns>
    Task<HttpStatusCode> CreateAsync(CreateExhibitionRequest exhibition);

    /// <summary>Creates an empty exhibition draft and returns its identifier.</summary>
    Task<Guid> CreateDraftAsync();

    /// <summary>Gets the published content used to prefill the editor.</summary>
    Task<ExhibitionResponse> GetPublishedAsync(Guid exhibitionId);

    /// <summary>Saves a new edit draft with its original exhibition reference.</summary>
    Task<Guid> CreateRevisionDraftAsync(Guid exhibitionId, UpdateExhibitionDraftRequest draft);

    /// <summary>Publishes the edit draft over the original exhibition.</summary>
    Task PublishRevisionAsync(Guid draftId, CreateExhibitionRequest exhibition);

    /// <summary>Gets an existing exhibition draft.</summary>
    Task<ExhibitionDraftResponse> GetDraftAsync(Guid draftId);

    /// <summary>Replaces all saved fields of the specified draft.</summary>
    Task SaveDraftAsync(Guid draftId, UpdateExhibitionDraftRequest draft);
}
