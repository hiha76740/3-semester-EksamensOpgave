namespace VidensVejen.ContractsLib.Responses.ImageService;

/// <summary>
/// 
/// </summary>
/// <param name="ImageId"></param>
/// <example>
/// {
///     "imageId": "123e4567-e89b-12d3-a456-426614174000"
/// }
/// </example>
public record ImageIdResponse(
    Guid ImageId
    )
{
    /// <summary>
    /// The id of the image.
    /// </summary>
    public Guid ImageId { get; init; } = ImageId;
}
