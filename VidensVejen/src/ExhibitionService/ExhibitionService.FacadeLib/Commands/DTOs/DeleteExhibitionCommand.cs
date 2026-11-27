namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibitionId"></param>
/// <param name="RowVersion"></param>
public record DeleteExhibitionCommand(
    Guid ExhibitionId,
    byte[] RowVersion
    );
