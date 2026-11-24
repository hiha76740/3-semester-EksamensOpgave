using ImageService.ApplicationLib.Repositories;
using ImageService.ApplicationLib.Storages;
using ImageService.DomainLib.Entities;
using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;

namespace ImageService.ApplicationLib.Handlers;

/// <summary>
/// 
/// </summary>
/// <param name="imageStorage"></param>
/// <param name="imageRepo"></param>
/// <param name="logger"></param>
public class UploadImageHandler(
    IImageStorage imageStorage,
    IImageRepository imageRepo,
    ILogger<UploadImageHandler> logger
    ) : IUploadImageHandler
{
    async Task<Guid> IUploadImageHandler.HandleAsync(
        UploadImageCommand command,
        CancellationToken cancellationToken
        )
    {
        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Beginning uploading new image");

        var image = Image.Create(command.Extension);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Image identifier created with id {ImageId}", image.Id.Value);

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Saving image with id {ImageId} to directory", image.Id.Value);

        await imageStorage.SaveAsync(
            image.StorageKey,
            command.Content,
            command.Extension,
            cancellationToken
            );


        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Image {ImageId} saved to directory successfully", image.Id.Value);
            logger.LogInformation("Saving image identifier with id {ImageId} to database", image.Id.Value);
        }

        await imageRepo.AddAsync(
            image,
            cancellationToken
            );

        await imageRepo.SaveAsync();

        if (logger.IsEnabled(LogLevel.Information))
            logger.LogInformation("Image {ImageId} saved to database successfully", image.Id.Value);


        return image.Id.Value;
    }
}
