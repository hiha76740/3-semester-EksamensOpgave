using ExhibitionService.FacadeLib.Queries.DTOs;

namespace ExhibitionService.FacadeLib.Queries.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IExhibitionQueries
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ExhibitionDto?> GetExhibitionByIdAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyList<ExhibitionNameDto>> GetAllExhibitionNamesAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CheckIfImageInUseAsync(Guid id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CheckIfQuizInUseAsync(Guid id);
}
