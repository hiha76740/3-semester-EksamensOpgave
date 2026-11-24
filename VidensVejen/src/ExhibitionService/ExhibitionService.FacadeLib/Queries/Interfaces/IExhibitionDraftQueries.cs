using ExhibitionService.FacadeLib.Queries.DTOs;

namespace ExhibitionService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IExhibitionDraftQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ExhibitionDraftDto?> GetExhibitionDraftByIdAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<ExhibitionDraftDto>> GetAllExhibitionDrafts();
}
