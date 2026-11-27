using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IExhibitionRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibition"></param>
    /// <returns></returns>
    Task AddAsync(Exhibition exhibition);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Exhibition?> GetExhibitionById(ExhibitionId id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibition"></param>
    void Delete(Exhibition exhibition);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibition"></param>
    /// <param name="originalRowVersion"></param>
    /// <returns></returns>
    void SetOriginalRowVersion(Exhibition exhibition, byte[] originalRowVersion);


}
