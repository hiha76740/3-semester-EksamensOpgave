namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="DraftId"></param>
/// <param name="RowVersion"></param>
public record DeleteDraftCommand(Guid DraftId, byte[] RowVersion);
