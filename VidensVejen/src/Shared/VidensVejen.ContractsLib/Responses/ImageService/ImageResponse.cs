namespace VidensVejen.ContractsLib.Responses.ImageService;

/// <summary>
/// Contains the stored metadata needed get a image.
/// </summary>
/// <param name="Id">The unique identifier of the image.</param>
/// <param name="StorageKey">The key used to locate the image file.</param>
/// <param name="Extension">The stored file extension.</param>
/// <param name="RowVersion">The version required when deleting the image.</param>
/// <param name="Url">The url of the image</param>
public record ImageResponse(
    Guid Id,
    string StorageKey,
    string Extension,
    byte[] RowVersion,
    string Url
    );
