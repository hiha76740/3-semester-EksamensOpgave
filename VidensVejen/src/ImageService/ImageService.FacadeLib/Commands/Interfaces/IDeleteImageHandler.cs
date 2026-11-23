using ImageService.FacadeLib.Commands.DTOs;

namespace ImageService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IDeleteImageHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    Task HandleAsync(DeleteImageCommand command);
}
