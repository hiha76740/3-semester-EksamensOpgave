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
}
