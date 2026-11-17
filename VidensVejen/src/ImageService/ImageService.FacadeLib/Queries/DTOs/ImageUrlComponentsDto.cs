namespace ImageService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="StorageKey"></param>
/// <param name="Extension"></param>
public record ImageUrlComponentsDto(
    string StorageKey,
    string Extension
    );
