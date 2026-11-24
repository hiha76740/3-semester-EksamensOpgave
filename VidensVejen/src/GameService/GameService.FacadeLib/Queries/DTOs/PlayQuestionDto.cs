namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="Choices"></param>
public record PlayQuestionDto(
    Guid Id,
    string? Text,
    List<PlayChoiceDto> Choices
    );
