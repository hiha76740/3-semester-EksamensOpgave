using ImageService.ApplicationLib.Repositories;
using ImageService.ApplicationLib.Services;
using ImageService.ApplicationLib.Storages;
using ImageService.DomainLib.Entities;
using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ImageService.ApplicationLib.Handlers;


/// <summary>
/// 
/// </summary>
/// <param name="imageStorage"></param>
/// <param name="imageRepo"></param>
/// <param name="exhibitionService"></param>
/// <param name="logger"></param>
public class DeleteImageHandler(
    IImageStorage imageStorage,
    IImageRepository imageRepo,
    IExhibitionService exhibitionService,
    ILogger<DeleteImageHandler> logger
    ) : IDeleteImageHandler
{
    async Task IDeleteImageHandler.HandleAsync(DeleteImageCommand command)
    {
        try
        {
            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Beginning to delete image {ImageId}", command.Id);

            var imageId = new ImageId(command.Id);

            var image = await imageRepo.GetImageByIdAsync(imageId)
                ?? throw new NotFoundException("Image Id not found");

            var inUse = await exhibitionService.ImageInUseAsync(imageId);

            if (inUse == true)
                throw new InvalidOperationException("Can not delete image that is still in use");

            if (logger.IsEnabled(LogLevel.Information))
                logger.LogInformation("Deleting image {ImageId} from database", command.Id);

            imageRepo.SetOriginalRowVersion(image, command.RowVersion);

            await imageStorage.DeleteAsync(image.StorageKey, image.Extension);

            imageRepo.Delete(image);

            await imageRepo.SaveAsync();
        }
        catch (Exception ex)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {
                logger.LogError("Error {ErrorMessage}", ex.Message);

                if (ex.InnerException != null)
                    logger.LogError("InnerException Error {Error}", ex.InnerException.Message);
            }

            throw;
        }
    }
}
