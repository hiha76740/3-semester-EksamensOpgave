using ExhibitionService.DomainLib.Entities;

namespace ExhibitionService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IExhibitionDraftRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibitionDraft"></param>
    /// <returns></returns>
    Task AddAsync(ExhibitionDraft exhibitionDraft);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
