using ExhibitionService.DomainLib.Entities;
using ExhibitionService.DomainLib.ValueObjects;

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
    /// <param name="id"></param>
    /// <returns></returns>
    Task<ExhibitionDraft?> GetExhibitionDraftByIdAsync(ExhibitionDraftId id);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibitionDraft"></param>
    void Delete(ExhibitionDraft exhibitionDraft);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();
}
