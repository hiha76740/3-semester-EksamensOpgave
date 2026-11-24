//using ImageService.DomainLib.Entities;
//using VidensVejen.SharedKernelLib.Exceptions;

//namespace ImageService.DomainLib.Tests;

//public class ImageTests
//{
//    [Theory]
//    [InlineData("image/jpeg", ".jpg")]
//    [InlineData("image/jpeg", ".jpeg")]
//    [InlineData("image/png", ".png")]
//    public void Create_GivenSupportedExtension_InitializesImage(string contentType, string extension)
//    {
//        // Act
//        var image = Image.Create(contentType, extension);

//        // Assert
//        Assert.NotEqual(Guid.Empty, image.Id.Value);
//        Assert.Equal(image.Id.Value.ToString("N"), image.StorageKey);
//        Assert.Equal(contentType, image.ContentType);
//        Assert.Equal(extension, image.Extension);
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData("   ")]
//    public void Create_GivenBlankContentType_ThrowsDomainException(string? contentType)
//    {
//        // Act & Assert
//        Assert.Throws<DomainException>(() => Image.Create(contentType!, ".png"));
//    }

//    [Theory]
//    [InlineData(null)]
//    [InlineData("")]
//    [InlineData(".gif")]
//    [InlineData(".svg")]
//    [InlineData(".PNG")]
//    public void Create_GivenUnsupportedExtension_ThrowsDomainException(string? extension)
//    {
//        // Act & Assert
//        Assert.Throws<DomainException>(() => Image.Create("image/png", extension!));
//    }
//}
