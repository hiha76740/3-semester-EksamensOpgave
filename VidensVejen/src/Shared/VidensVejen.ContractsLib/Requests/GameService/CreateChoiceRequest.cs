namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="CorrectAnswer"></param>
/// <param name="Feedback"></param>
/// <param name="FeedbackEn"></param>
public record CreateChoiceRequest(
    string Text,
    string? TextEn,
    bool CorrectAnswer,
    string? Feedback,
    string? FeedbackEn
    )
{
    /// <summary>
    /// The text in danish you want the choice to have.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// The text in english you want the choice to have.
    /// </summary>
    public string? TextEn { get; init; } = TextEn;

    /// <summary>
    /// Marks the choice as correct
    /// </summary>
    public bool CorrectAnswer { get; init; } = CorrectAnswer;

    /// <summary>
    /// Feedback shown in danish when the guest answers. Optional.
    /// </summary>
    public string? Feedback { get; init; } = Feedback;

    /// <summary>
    /// Feedback shown in english when the guest answers. Optional.
    /// </summary>
    public string? FeedbackEn { get; init; } = FeedbackEn;
}
