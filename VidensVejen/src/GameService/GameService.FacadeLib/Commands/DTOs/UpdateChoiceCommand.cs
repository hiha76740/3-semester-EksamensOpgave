namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="CorrectAnswer"></param>
public record UpdateChoiceCommand(
    Guid Id,
    string Text,
    bool CorrectAnswer
    );
