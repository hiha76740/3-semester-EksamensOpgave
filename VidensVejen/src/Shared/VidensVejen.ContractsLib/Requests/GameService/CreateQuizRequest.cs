namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="Questions"></param>
/// <example>
/// {
///     "title": "Affaldshierakiet",
///     "exhibitionId": "123e4567-e89b-12d3-a456-426614174000",
///     "quizLayout": "Vertikalt",
///     "questions": [
///         {
///             "text": "Hvad skal dette sorteres som?",
///             "choices": [
///                 {
///                     "text": "Plastik",
///                     "correctAnswer": true,
///                 }
///             ]
///         }
///     ]
/// }
/// </example>
public record CreateQuizRequest(
    string Title,
    Guid ExhibitionId,
    string QuizLayout,
    List<CreateQuestionRequest> Questions
    )
{
    /// <summary>
    /// The title of the quiz.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// A unique identifier for a exhibition the quiz associate to.
    /// </summary>
    public Guid ExhibitionId { get; init; } = ExhibitionId;

    /// <summary>
    /// The layout of the quiz.
    /// </summary>
    public string QuizLayout { get; init; } = QuizLayout;

    /// <summary>
    /// A list of the questions you want the quiz to have.
    /// </summary>
    public List<CreateQuestionRequest> Questions { get; init; } = Questions;
}
