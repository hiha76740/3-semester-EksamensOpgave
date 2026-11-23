using ImageService.ApplicationLib.Repositories;
using ImageService.DomainLib.Entities;
using ImageService.InfrastructureLib.Persistence;

namespace ImageService.InfrastructureLib.Repositories;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class ImageRepository(ImageDbContext db) : IImageRepository
{
    async Task IImageRepository.AddAsync(Image image, CancellationToken cancellationToken)
    {
        await db.AddAsync(image, cancellationToken);
    }

    void IImageRepository.Delete(Image image)
    {
        db.Remove(image);
    }

    async Task<Image?> IImageRepository.GetImageByIdAsync(ImageId imageId)
    {
        return await db.Images.FindAsync(imageId);
    }

    async Task IImageRepository.SaveAsync()
    {
        await db.SaveChangesAsync();
    }
}
