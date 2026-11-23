namespace ImageService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="RowVersion"></param>
public record DeleteImageCommand(Guid Id, byte[] RowVersion);
