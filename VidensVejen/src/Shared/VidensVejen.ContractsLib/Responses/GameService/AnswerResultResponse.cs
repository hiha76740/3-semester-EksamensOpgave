namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="IsCorrect"></param>
/// <param name="Feedback"></param>
/// <param name="FeedbackEn"></param>
/// <param name="CorrectAnswer"></param>
/// <param name="CorrectAnswerEn"></param>
/// <example>
/// {
///     "isCorrect": false,
///     "feedback": "Næsten! Den skal i mad- og drikkekartoner.",
///     "feedbackEn": "Almost! It goes in food and drink cartons.",
///     "correctAnswer": "Mad- og drikkekartoner",
///     "correctAnswerEn": "Food and drink cartons"
/// }
/// </example>
public record AnswerResultResponse(
    bool IsCorrect,
    string? Feedback,
    string? FeedbackEn,
    string CorrectAnswer,
    string? CorrectAnswerEn
    )
{
    /// <summary>
    /// Whether the guest answered correctly.
    /// </summary>
    public bool IsCorrect { get; init; } = IsCorrect;

    /// <summary>
    /// The feedback to show the guest in Danish.
    /// </summary>
    public string? Feedback { get; init; } = Feedback;

    /// <summary>
    /// The feedback to show the guest in English.
    /// </summary>
    public string? FeedbackEn { get; init; } = FeedbackEn;

    /// <summary>
    /// The text of the correct choice in Danish.
    /// </summary>
    public string CorrectAnswer { get; init; } = CorrectAnswer;

    /// <summary>
    /// The text of the correct choice in English.
    /// </summary>
    public string? CorrectAnswerEn { get; init; } = CorrectAnswerEn;
}
