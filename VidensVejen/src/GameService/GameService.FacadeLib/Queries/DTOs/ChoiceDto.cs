namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="CorrectAnswer"></param>
public record ChoiceDto(
    string Text,
    bool CorrectAnswer
    );
