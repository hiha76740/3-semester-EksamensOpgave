namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Index"></param>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
public record PlayChoiceDto(
    int Index,
    string Text,
    string? TextEn
    );
