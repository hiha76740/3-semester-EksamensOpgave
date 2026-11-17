namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="Questions"></param>
/// <example>
/// {
///     "id": "123e4567-e89b-12d3-a456-426614174000",
///     "title": "Affaldshierakiet",
///     "exhibitionId": "123e4567-e89b-12d3-a456-426614174000",
///     "quizLayout": "Vertikalt",
///     "questions": [
///         {
///             "id": "123e4567-e89b-12d3-a456-426614174000",
///             "text": "Hvad skal dette sorteres som?",
///             "choices": [
///                 {
///                     "id": "123e4567-e89b-12d3-a456-426614174000",
///                     "text": "Plastik",
///                     "correctAnswer": true
///                 }
///             ]
///         }
///     ]
/// }   
/// </example>
public record QuizResponse(
    Guid Id,
    string Title,
    Guid ExhibitionId,
    string QuizLayout,
    List<QuestionResponse> Questions
    )
{
    /// <summary>
    /// A unique identifier for the quiz.
    /// </summary>
    public Guid Id { get; init; } = Id;

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
    /// A list of the questions assoicated to the quiz.
    /// </summary>
    public List<QuestionResponse> Questions { get; init; } = Questions;
}
