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
    public ImageId Id { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string StorageKey { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Extension { get; set; } = string.Empty;


    /// <summary>
    /// 
    /// </summary>
    public byte[] RowVersion { get; private set; } = [];


    /// <summary>
    /// 
    /// </summary>
    /// <param name="extension"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Image Create(string extension)
    {
        if (extension != ".jpg" &&
                extension != ".jpeg" &&
                extension != ".png")
        {
            throw new DomainException("Only jpg- and png-image files are allowed");
        }

        var image = new Image(extension);

        return image;
    }


    private Image(string extension)
    {
        Id = new ImageId(Guid.NewGuid());
        StorageKey = $"{Id.Value:N}";
        Extension = extension;
    }

    // EF Constructor
    private Image() { }
}
