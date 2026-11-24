using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
public record Choice
{
    /// <summary>
    /// 
    /// </summary>
    public string Text { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string TextEn { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsCorrect { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? Feedback { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? FeedbackEn { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="textEn"></param>
    /// <param name="isCorrect"></param>
    /// <param name="feedback"></param>
    /// <param name="feedbackEn"></param>
    /// <exception cref="DomainException"></exception>
    public Choice(string text, string textEn, bool isCorrect, string? feedback, string? feedbackEn)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("Choice text could not be set, no value");

        Text = text;
        TextEn = textEn;
        IsCorrect = isCorrect;
        Feedback = feedback;
        FeedbackEn = feedbackEn;
    }
}
