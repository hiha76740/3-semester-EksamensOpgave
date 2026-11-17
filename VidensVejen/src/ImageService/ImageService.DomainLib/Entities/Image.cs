using VidensVejen.SharedKernelLib.Exceptions;

namespace ImageService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class Image
{
    /// <summary>
    /// 
    /// </summary>
    public ImageId Id { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string StorageKey { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string ContentType { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string Extension { get; set; }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="contentType"></param>
    /// <param name="extension"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Image Create(string contentType, string extension)
    {
        if (string.IsNullOrWhiteSpace(contentType))
            throw new DomainException("Content type could not be set, no value");

        if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png")
        {
            throw new DomainException("Only jpg- and png-image files are allowed");
        }

        var image = new Image(contentType, extension);

        return image;
    }


    private Image(string contentType, string extension)
    {
        Id = new ImageId(Guid.NewGuid());
        StorageKey = $"{Id.Value:N}";
        ContentType = contentType;
        Extension = extension;
    }
}
