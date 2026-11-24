namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="QuizLayout"></param>
/// <param name="EndMessage"></param>
/// <param name="Questions"></param>
public record PlayQuizResponse(
    Guid Id,
    string? Title,
    string? QuizLayout,
    string? EndMessage,
    List<PlayQuestionResponse> Questions
    )
{
    /// <summary>
    /// A unique identifier for the quiz.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the quiz.
    /// </summary>
    public string? Title { get; init; } = Title;

    /// <summary>
    /// The layout of the quiz.
    /// </summary>
    public string? QuizLayout { get; init; } = QuizLayout;

    /// <summary>
    /// Message shown when the quiz is finished.
    /// </summary>
    public string? EndMessage { get; init; } = EndMessage;

    /// <summary>
    /// The questions in the quiz.
    /// </summary>
    public List<PlayQuestionResponse> Questions { get; init; } = Questions;
}
