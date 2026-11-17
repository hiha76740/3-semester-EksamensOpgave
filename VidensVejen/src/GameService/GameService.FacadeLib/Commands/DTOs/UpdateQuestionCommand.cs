namespace GameService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="Choices"></param>
public record UpdateQuestionCommand(
    Guid Id,
    string Text,
    List<UpdateChoiceCommand> Choices
    );
