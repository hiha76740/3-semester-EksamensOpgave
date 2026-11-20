using ExhibitionService.FacadeLib.Commands.DTOs;

namespace ExhibitionService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface ICreateExhibitionHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(CreateExhibitionCommand command);
}
