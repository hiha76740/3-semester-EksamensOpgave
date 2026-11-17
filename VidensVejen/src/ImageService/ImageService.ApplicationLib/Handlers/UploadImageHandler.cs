using ImageService.ApplicationLib.Repositories;
using ImageService.ApplicationLib.Storages;
using ImageService.DomainLib.Entities;
using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Commands.Interfaces;

namespace ImageService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="imageStorage"></param>
/// <param name="imageRepo"></param>
public class UploadImageHandler(
    IImageStorage imageStorage,
    IImageRepository imageRepo
    ) : IUploadImageHandler
{
    async Task<Guid> IUploadImageHandler.HandleAsync(
        UploadImageCommand command,
        CancellationToken cancellationToken
        )
    {
        var image = Image.Create(command.ContentType, command.Extension);

        await imageStorage.SaveAsync(
            image.StorageKey,
            command.Content,
            command.Extension,
            cancellationToken
            );

        await imageRepo.AddAsync(
            image,
            cancellationToken
            );

        return image.Id.Value;
    }
}
