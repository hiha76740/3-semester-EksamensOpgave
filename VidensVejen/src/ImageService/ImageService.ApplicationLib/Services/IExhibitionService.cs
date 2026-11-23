using ImageService.DomainLib.Entities;

namespace ImageService.ApplicationLib.Services;

/// <summary>
/// 
/// </summary>
public interface IExhibitionService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    Task<bool> ImageInUseAsync(ImageId imageId);
}
