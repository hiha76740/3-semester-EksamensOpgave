namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="IsCorrect"></param>
/// <param name="Feedback"></param>
/// <param name="CorrectAnswer"></param>
/// <example>
/// {
///     "isCorrect": false,
///     "feedback": "Næsten! Den skal i mad- og drikkekartoner.",
///     "correctAnswer": "Mad- og drikkekartoner"
/// }
/// </example>
public record AnswerResultResponse(
    bool IsCorrect,
    string? Feedback,
    string CorrectAnswer
    )
{
    /// <summary>
    /// Whether the guest answered correctly.
    /// </summary>
    public bool IsCorrect { get; init; } = IsCorrect;

    /// <summary>
    /// The feedback to show the guest.
    /// </summary>
    public string? Feedback { get; init; } = Feedback;

    /// <summary>
    /// The text of the correct choice.
    /// </summary>
    public string CorrectAnswer { get; init; } = CorrectAnswer;
}
