using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDeleteExibitionHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(DeleteExhibitionCommand command);
}
