using ImageService.ApplicationLib.Repositories;
using ImageService.DomainLib.Entities;
using ImageService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;
using VidensVejen.SharedKernelLib.Exceptions;

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
        try
        {
            await db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyConflictException("The data was changed or deleted by another request", ex);
        }
    }

    void IImageRepository.SetOriginalRowVersion(Image image, byte[] originalRowVersion)
    {
        db.Entry(image)
            .Property(u => u.RowVersion)
            .OriginalValue = originalRowVersion;
    }
}
