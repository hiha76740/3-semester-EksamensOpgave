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

    /// <summary>Creates an exhibition with its theme image.</summary>
    /// <param name="exhibition">The exhibition and its content.</param>
    /// <returns>The status code returned by ExhibitionService.</returns>
    Task<HttpStatusCode> CreateAsync(CreateExhibitionRequest exhibition);

    /// <summary>Creates an empty exhibition draft and returns its identifier.</summary>
    Task<Guid> CreateDraftAsync();

    /// <summary>Replaces all saved fields of the specified draft.</summary>
    Task SaveDraftAsync(Guid draftId, SaveExhibitionDraftRequest draft);
}
