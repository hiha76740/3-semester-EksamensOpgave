namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="IsCorrect"></param>
/// <param name="Feedback"></param>
/// <param name="FeedbackEn"></param>
/// <param name="CorrectAnswer"></param>
/// <param name="CorrectAnswerEn"></param>
public record AnswerResult(bool IsCorrect, string? Feedback, string? FeedbackEn, string CorrectAnswer, string? CorrectAnswerEn);
