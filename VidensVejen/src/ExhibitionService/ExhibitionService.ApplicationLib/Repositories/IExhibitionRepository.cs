using ExhibitionService.DomainLib.Entities;

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
    /// <returns></returns>
    Task SaveAsync();
}
