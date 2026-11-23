using ImageService.DomainLib.Entities;

namespace ImageService.ApplicationLib.Repositories;

/// <summary>
/// 
/// </summary>
public interface IImageRepository
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task AddAsync(Image image, CancellationToken cancellationToken);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <returns></returns>
    void Delete(Image image);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageId"></param>
    /// <returns></returns>
    Task<Image?> GetImageByIdAsync(ImageId imageId);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    Task SaveAsync();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="rowVersion"></param>
    void SetOriginalRowVersion(Image image, byte[] rowVersion);
}
