namespace VidensVejen.ContractsLib.Responses.StatisticsService;

/// <summary>
/// 
/// </summary>
/// <param name="QuizId"></param>
/// <param name="AveragePercentage"></param>
/// <param name="GameCount"></param>
/// <example>
/// {
///     "quizId": "123e4567-e89b-12d3-a456-426614174000",
///     "averagePercentage": 81,
///     "gameCount": 4
/// }
/// </example>
public record QuizAverageResponse(
    Guid QuizId,
    int AveragePercentage,
    int GameCount
    )
{
    /// <summary>
    /// A unique identifier for the quiz.
    /// </summary>
    public Guid QuizId { get; init; } = QuizId;

    /// <summary>
    /// The average share of correct answers in percent for all games of the quiz today.
    /// </summary>
    public int AveragePercentage { get; init; } = AveragePercentage;

    /// <summary>
    /// The number of finished games of the quiz today.
    /// </summary>
    public int GameCount { get; init; } = GameCount;
}
