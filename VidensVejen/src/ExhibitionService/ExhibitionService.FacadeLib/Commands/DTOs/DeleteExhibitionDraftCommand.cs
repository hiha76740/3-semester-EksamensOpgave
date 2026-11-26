namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibitionDraftId"></param>
/// <param name="RowVersion"></param>
public record DeleteExhibitionDraftCommand(Guid ExhibitionDraftId, byte[] RowVersion);
