namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record CreateQuestionCommand(
    string Text,
    string TextEn,
    List<CreateChoiceCommand> Choices
    );
