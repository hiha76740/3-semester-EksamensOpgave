using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDeleteExhibitionDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(DeleteExhibitionDraftCommand command);
}
