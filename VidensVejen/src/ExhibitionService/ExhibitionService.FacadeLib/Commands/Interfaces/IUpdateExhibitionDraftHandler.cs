using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IUpdateExhibitionDraftHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task<byte[]> HandleAsync(UpdateExhibitionDraftCommand command);
}
