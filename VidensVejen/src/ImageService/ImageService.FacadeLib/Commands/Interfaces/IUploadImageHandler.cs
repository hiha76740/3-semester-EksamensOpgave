using ImageService.FacadeLib.Commands.DTOs;

namespace ImageService.FacadeLib.Commands.Interfaces;

/// <summary>
/// 
/// </summary>
public interface IUploadImageHandler
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Guid> HandleAsync(UploadImageCommand command, CancellationToken cancellationToken);
}
