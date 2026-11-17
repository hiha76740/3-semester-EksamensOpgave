namespace ImageService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Content"></param>
/// <param name="ContentType"></param>
/// <param name="Extension"></param>
public record UploadImageCommand(
    Stream Content,
    string ContentType,
    string Extension);
