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
    async Task<ImageUrlComponentsDto?> IImageQueries.GetImageUrlComponentsById(Guid id)
    {
        var imageKey = new ImageId(id);

        return await db.Images.AsNoTracking()
            .Where(i => i.Id == imageKey)
            .Select(i => new ImageUrlComponentsDto(i.StorageKey, i.Extension))
            .FirstOrDefaultAsync();
    }
}
