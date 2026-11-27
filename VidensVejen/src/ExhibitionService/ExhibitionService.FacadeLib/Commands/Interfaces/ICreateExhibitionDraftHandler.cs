using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ICreateExhibitionDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task<Guid> HandleAsync();

    /// <summary>
    /// Saves the initial content of a draft linked to a published exhibition.
    /// </summary>
    Task<Guid> CreateRevisionAsync(UpdateExhibitionDraftCommand command);
}
