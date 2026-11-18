using ImageService.ApplicationLib.Handlers;
using ImageService.ApplicationLib.Repositories;
using ImageService.ApplicationLib.Storages;
using ImageService.DomainLib.Entities;
using ImageService.FacadeLib.Commands.DTOs;
using ImageService.FacadeLib.Commands.Interfaces;
using Moq;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ImageService.ApplicationLib.Tests;

public class UploadImageTests
{
    [Theory]
    [InlineData("image/jpeg", ".jpg")]
    [InlineData("image/jpeg", ".jpeg")]
    [InlineData("image/png", ".png")]
    public async Task Handle_GivenValidImage_SavesContentAndRegistersMatchingImage(
        string contentType, string extension)
    {
        // Arrange
        using var content = new MemoryStream([1, 2, 3]);
        var token = TestContext.Current.CancellationToken;
        var command = new UploadImageCommand(content, contentType, extension);
        var storage = new Mock<IImageStorage>();
        var repository = new Mock<IImageRepository>();
        Image? registeredImage = null;
        string? storedKey = null;

        storage.Setup(s => s.SaveAsync(It.IsAny<string>(), content, extension, token))
            .Callback<string, Stream, string, CancellationToken>(
                (key, stream, fileExtension, cancellationToken) => storedKey = key);
        repository.Setup(r => r.AddAsync(It.IsAny<Image>(), token))
            .Callback<Image, CancellationToken>((image, cancellationToken) => registeredImage = image);
        IUploadImageHandler handler = new UploadImageHandler(storage.Object, repository.Object);

        // Act
        var id = await handler.HandleAsync(command, token);

        // Assert
        Assert.NotNull(registeredImage);
        Assert.NotEqual(Guid.Empty, id);
        Assert.Equal(id, registeredImage.Id.Value);
        Assert.Equal(registeredImage.StorageKey, storedKey);
        Assert.Equal(contentType, registeredImage.ContentType);
        Assert.Equal(extension, registeredImage.Extension);
        storage.Verify(s => s.SaveAsync(registeredImage.StorageKey, content, extension, token), Times.Once);
        repository.Verify(r => r.AddAsync(registeredImage, token), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }

    [Theory]
    [InlineData("", ".png")]
    [InlineData("image/png", ".gif")]
    public async Task Handle_GivenInvalidMetadata_ThrowsBeforeWriting(string contentType, string extension)
    {
        // Arrange
        using var content = new MemoryStream([1, 2, 3]);
        var token = TestContext.Current.CancellationToken;
        var storage = new Mock<IImageStorage>();
        var repository = new Mock<IImageRepository>();
        IUploadImageHandler handler = new UploadImageHandler(storage.Object, repository.Object);
        var command = new UploadImageCommand(content, contentType, extension);

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.HandleAsync(command, token));

        storage.VerifyNoOtherCalls();
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenStorageFailure_RethrowsWithoutRegisteringImage()
    {
        // Arrange
        using var content = new MemoryStream([1, 2, 3]);
        var token = TestContext.Current.CancellationToken;
        var storage = new Mock<IImageStorage>();
        var repository = new Mock<IImageRepository>();
        var expected = new IOException("Storage failed");
        storage.Setup(s => s.SaveAsync(It.IsAny<string>(), content, ".png", token))
            .ThrowsAsync(expected);
        IUploadImageHandler handler = new UploadImageHandler(storage.Object, repository.Object);
        var command = new UploadImageCommand(content, "image/png", ".png");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<IOException>(() => handler.HandleAsync(command, token));

        Assert.Same(expected, exception);
        repository.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_GivenRepositoryFailure_RethrowsOriginalException()
    {
        // Arrange
        using var content = new MemoryStream([1, 2, 3]);
        var token = TestContext.Current.CancellationToken;
        var storage = new Mock<IImageStorage>();
        var repository = new Mock<IImageRepository>();
        var expected = new InvalidOperationException("Repository failed");
        repository.Setup(r => r.AddAsync(It.IsAny<Image>(), token)).ThrowsAsync(expected);
        IUploadImageHandler handler = new UploadImageHandler(storage.Object, repository.Object);
        var command = new UploadImageCommand(content, "image/png", ".png");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(command, token));

        Assert.Same(expected, exception);
        storage.Verify(s => s.SaveAsync(It.IsAny<string>(), content, ".png", token), Times.Once);
        repository.Verify(r => r.AddAsync(It.IsAny<Image>(), token), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Never);
    }

    [Fact]
    public async Task Handle_GivenSaveFailure_RethrowsOriginalException()
    {
        // Arrange
        using var content = new MemoryStream([1, 2, 3]);
        var token = TestContext.Current.CancellationToken;
        var storage = new Mock<IImageStorage>();
        var repository = new Mock<IImageRepository>();
        var expected = new InvalidOperationException("Save failed");
        repository.Setup(r => r.SaveAsync()).ThrowsAsync(expected);
        IUploadImageHandler handler = new UploadImageHandler(storage.Object, repository.Object);
        var command = new UploadImageCommand(content, "image/png", ".png");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<InvalidOperationException>(
            () => handler.HandleAsync(command, token));

        Assert.Same(expected, exception);
        storage.Verify(s => s.SaveAsync(It.IsAny<string>(), content, ".png", token), Times.Once);
        repository.Verify(r => r.AddAsync(It.IsAny<Image>(), token), Times.Once);
        repository.Verify(r => r.SaveAsync(), Times.Once);
    }
}
