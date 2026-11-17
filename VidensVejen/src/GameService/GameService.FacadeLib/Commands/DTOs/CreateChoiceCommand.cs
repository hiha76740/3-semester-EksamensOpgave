namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="CorrectAnswer"></param>
public record CreateChoiceCommand(
    string Text,
    bool CorrectAnswer
    );
