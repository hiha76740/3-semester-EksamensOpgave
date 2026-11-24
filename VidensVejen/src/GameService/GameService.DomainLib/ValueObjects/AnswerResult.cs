namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="IsCorrect"></param>
/// <param name="Feedback"></param>
/// <param name="CorrectAnswer"></param>
public record AnswerResult(bool IsCorrect, string? Feedback, string CorrectAnswer);
