namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="QuizId"></param>
/// <example>
/// {
///     "quizId": "123e4567-e89b-12d3-a456-426614174000"
/// }
/// </example>
public record StartSessionRequest(
    Guid QuizId
    )
{
    /// <summary>
    /// A unique identifier for the published quiz the guest wants to play.
    /// </summary>
    public Guid QuizId { get; init; } = QuizId;
}
