using ImageService.DomainLib.Entities;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ImageService.DomainLib.Tests;

public class ImageTests
{
    [Theory]
    [InlineData(".jpg")]
    [InlineData(".jpeg")]
    [InlineData(".png")]
    public void Create_GivenSupportedExtension_InitializesImage(string extension)
    {
        // Act
        var image = Image.Create(extension);

        // Assert
        Assert.NotEqual(Guid.Empty, image.Id.Value);
        Assert.Equal(image.Id.Value.ToString("N"), image.StorageKey);
        Assert.Equal(extension, image.Extension);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(".gif")]
    [InlineData(".svg")]
    [InlineData(".PNG")]
    public void Create_GivenUnsupportedExtension_ThrowsDomainException(string? extension)
    {
        // Act & Assert
        Assert.Throws<DomainException>(() => Image.Create(extension!));
    }
}
