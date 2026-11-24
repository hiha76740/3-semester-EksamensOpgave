using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.ApplicationLib.Services;

/// <summary>
/// 
/// </summary>
public interface IImageService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    Task<bool> ExistsAsync(ImageId imageId);
}
