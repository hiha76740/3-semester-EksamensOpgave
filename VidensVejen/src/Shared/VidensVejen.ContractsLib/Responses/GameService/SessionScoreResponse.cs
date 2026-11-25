namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="CorrectCount"></param>
/// <param name="TotalCount"></param>
/// <param name="Percentage"></param>
/// <example>
/// {
///     "correctCount": 5,
///     "totalCount": 8,
///     "percentage": 63
/// }
/// </example>
public record SessionScoreResponse(
    int CorrectCount,
    int TotalCount,
    int Percentage
    )
{
    /// <summary>
    /// The number of questions the guest answered correctly.
    /// </summary>
    public int CorrectCount { get; init; } = CorrectCount;

    /// <summary>
    /// The number of questions in the quiz.
    /// </summary>
    public int TotalCount { get; init; } = TotalCount;

    /// <summary>
    /// The share of correct answers in percent, rounded to a whole number.
    /// </summary>
    public int Percentage { get; init; } = Percentage;
}
