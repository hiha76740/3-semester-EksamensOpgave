using ImageService.ApplicationLib.Handlers;
using ImageService.ApplicationLib.Repositories;
using ImageService.DomainLib.Entities;
using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Commands.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;
using ImageService.ApplicationLib.Services;
using ImageService.ApplicationLib.Storages;

namespace ImageService.ApplicationLib.Tests;

public class DeleteImageTests
{

    [Theory]
    [InlineData(true, false)]
    [InlineData(true, true)]
    [InlineData(false, false)]
    public async Task Handle_GivenImageAndUsage_DeletesOnlyUnusedImage(bool found, bool inUse)
    {
        // Arrange
        var image = Image.Create("image/png", ".png");
        var repository = new Mock<IImageRepository>();
        var storage = new Mock<IImageStorage>();
        var exhibitions = new Mock<IExhibitionService>();
        repository.Setup(r => r.GetImageByIdAsync(image.Id)).ReturnsAsync(found ? image : null);
        exhibitions.Setup(s => s.ImageInUseAsync(image.Id)).ReturnsAsync(inUse);
        byte[] version = [1, 2, 3];
        IDeleteImageHandler handler = new DeleteImageHandler(
            storage.Object, repository.Object, exhibitions.Object, new Mock<ILogger<DeleteImageHandler>>().Object);
        var command = new DeleteImageCommand(image.Id.Value, version);

        // Act & Assert
        if (found == false)
        {
            await Assert.ThrowsAsync<NotFoundException>(() => handler.HandleAsync(command));
            exhibitions.VerifyNoOtherCalls();
        }
        else if (inUse)
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(command));
        }
        else
        {
            await handler.HandleAsync(command);
            repository.Verify(r => r.SetOriginalRowVersion(image, version), Times.Once);
        }
        storage.Verify(s => s.DeleteAsync(image.StorageKey, image.Extension), found && !inUse ? Times.Once() : Times.Never());
        repository.Verify(r => r.Delete(image), found && !inUse ? Times.Once() : Times.Never());
        repository.Verify(r => r.SaveAsync(), found && !inUse ? Times.Once() : Times.Never());
    }
}
