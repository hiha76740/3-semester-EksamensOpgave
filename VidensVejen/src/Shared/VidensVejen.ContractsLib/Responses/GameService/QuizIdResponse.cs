namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="QuizId"></param>
/// <example>
/// {
///     "quizId": "123e4567-e89b-12d3-a456-426614174000"
/// }
/// </example>
public record QuizIdResponse(
    Guid QuizId
    )
{
    /// <summary>
    /// The id of the quiz.
    /// </summary>
    public Guid QuizId { get; init; } = QuizId;
}
