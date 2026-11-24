using System.Net;
using VidensVejen.ContractsLib.Requests.ExhibitionService;
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
}
