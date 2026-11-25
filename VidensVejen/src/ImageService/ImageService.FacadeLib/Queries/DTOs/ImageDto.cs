namespace ImageService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="StorageKey"></param>
/// <param name="Extension"></param>
public record ImageDto(
    Guid Id,
    string StorageKey,
    string Extension
    );
