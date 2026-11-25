using ImageService.DomainLib.Entities;
using ImageService.FacadeLib.Queries.DTOs;
using ImageService.FacadeLib.Queries.Interfaces;
using ImageService.InfrastructureLib.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ImageService.InfrastructureLib.QueryHandlers;

/// <summary>
/// 
/// </summary>
/// <param name="db"></param>
public class ImageQueryHandlerIMPL(ImageDbContext db) : IImageQueries
{
    async Task<bool> IImageQueries.CheckIfExistsAsync(Guid id)
    {
        var imageKey = new ImageId(id);

        return await db.Images.AnyAsync(i => i.Id == imageKey);
    }

    async Task<IReadOnlyList<ImageDto>> IImageQueries.GetAllAsync()
    {
        return await db.Images
            .AsNoTracking()
            .Select(i => new ImageDto(
                i.Id.Value,
                i.StorageKey,
                i.Extension,
                i.RowVersion
                ))
            .ToListAsync();
    }

    async Task<ImageUrlComponentsDto?> IImageQueries.GetImageUrlComponentsById(Guid id)
    {
        var imageKey = new ImageId(id);

        return await db.Images
            .AsNoTracking()
            .Where(i => i.Id == imageKey)
            .Select(i => new ImageUrlComponentsDto(i.StorageKey, i.Extension))
            .FirstOrDefaultAsync();
    }
}
