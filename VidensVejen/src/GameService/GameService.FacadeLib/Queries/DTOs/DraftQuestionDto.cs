namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record DraftQuestionDto(
    Guid Id,
    string? Text,
    string? TextEn,
    List<ChoiceDto> Choices
    );
