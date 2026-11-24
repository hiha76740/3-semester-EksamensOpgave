namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="QuestionId"></param>
/// <param name="ChoiceIndex"></param>
/// <example>
/// {
///     "questionId": "123e4567-e89b-12d3-a456-426614174000",
///     "choiceIndex": 1
/// }
/// </example>
public record AnswerQuestionRequest(
    Guid QuestionId,
    int ChoiceIndex
    )
{
    /// <summary>
    /// A unique identifier for the question the guest answers.
    /// </summary>
    public Guid QuestionId { get; init; } = QuestionId;

    /// <summary>
    /// The index of the choice the guest picked.
    /// </summary>
    public int ChoiceIndex { get; init; } = ChoiceIndex;
}
