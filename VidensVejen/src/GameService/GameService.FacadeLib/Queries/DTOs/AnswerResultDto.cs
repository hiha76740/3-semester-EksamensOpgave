namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="IsCorrect"></param>
/// <param name="Feedback"></param>
/// <param name="CorrectAnswer"></param>
public record AnswerResultDto(
    bool IsCorrect,
    string? Feedback,
    string CorrectAnswer
    );
