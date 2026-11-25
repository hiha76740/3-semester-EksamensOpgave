namespace GameService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="CorrectAnswer"></param>
/// <param name="Feedback"></param>
/// <param name="FeedbackEn"></param>
public record ChoiceDraftDto(
    string? Text,
    string? TextEn,
    bool CorrectAnswer,
    string? Feedback,
    string? FeedbackEn);
