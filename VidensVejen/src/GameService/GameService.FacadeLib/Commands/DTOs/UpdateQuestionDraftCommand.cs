namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record UpdateQuestionDraftCommand(
    string? Text,
    string? TextEn,
    List<UpdateChoiceDraftCommand>? Choices
    );
