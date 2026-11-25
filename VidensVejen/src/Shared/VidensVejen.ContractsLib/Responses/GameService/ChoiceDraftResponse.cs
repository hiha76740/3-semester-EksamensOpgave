namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="CorrectAnswer"></param>
/// <param name="Feedback"></param>
/// <param name="FeedbackEn"></param>
public record ChoiceDraftResponse(
    string? Text,
    string? TextEn,
    bool CorrectAnswer,
    string? Feedback,
    string? FeedbackEn
    )
{
    /// <summary>
    /// The text of the choice in Danish.
    /// </summary>
    public string? Text { get; init; } = Text;

    /// <summary>
    /// The text of the choice in English.
    /// </summary>
    public string? TextEn { get; init; } = TextEn;

    /// <summary>
    /// Marks the choice as correct
    /// </summary>
    public bool CorrectAnswer { get; init; } = CorrectAnswer;

    /// <summary>
    /// The feedback of the choice in Danish.
    /// </summary>
    public string? Feedback { get; init; } = Feedback;

    /// <summary>
    /// The feedback of the choice in English.
    /// </summary>
    public string? FeedbackEn { get; init; } = FeedbackEn;
}
