namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="CorrectFeedback"></param>
/// <param name="IncorrectFeedback"></param>
/// <param name="Choices"></param>
public record QuestionDto(
    Guid Id,
    string Text,
    string? CorrectFeedback,
    string? IncorrectFeedback,
    List<ChoiceDto> Choices
    );
