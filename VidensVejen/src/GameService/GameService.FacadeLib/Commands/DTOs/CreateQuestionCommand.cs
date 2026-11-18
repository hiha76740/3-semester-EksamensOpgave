namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="Choices"></param>
/// <param name="CorrectFeedback"></param>
/// <param name="IncorrectFeedback"></param>
public record CreateQuestionCommand(
    string Text,
    List<CreateChoiceCommand> Choices,
    string? CorrectFeedback,
    string? IncorrectFeedback
    );
