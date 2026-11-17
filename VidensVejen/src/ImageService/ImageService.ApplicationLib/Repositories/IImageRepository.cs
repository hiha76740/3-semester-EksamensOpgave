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
}
