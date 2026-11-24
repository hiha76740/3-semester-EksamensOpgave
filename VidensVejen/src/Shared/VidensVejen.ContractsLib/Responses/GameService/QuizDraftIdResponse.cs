namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="QuizDraftId"></param>
/// <example>
/// {
///     "quizDraftId": "123e4567-e89b-12d3-a456-426614174000"
/// }
/// </example>
public record QuizDraftIdResponse(
    Guid QuizDraftId
    )
{
    /// <summary>
    /// The id of the quiz draft.
    /// </summary>
    public Guid QuizDraftId { get; init; } = QuizDraftId;
}
