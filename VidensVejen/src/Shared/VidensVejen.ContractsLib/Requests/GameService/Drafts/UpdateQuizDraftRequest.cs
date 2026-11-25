namespace VidensVejen.ContractsLib.Requests.GameService.Drafts;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="Questions"></param>
/// <param name="EndMessage"></param>
/// <param name="EndMessageEn"></param>
/// <param name="Instruction"></param>
/// <param name="InstructionEn"></param>
/// <example>
/// {
///     "title": "Affaldshierakiet",
///     "titleEn": "The waste hierarchy",
///     "exhibitionId": "123e4567-e89b-12d3-a456-426614174000",
///     "quizLayout": "Vertikalt",
///     "questions": [
///         {
///             "text": "Hvad skal dette sorteres som?",
///             "textEn": "How should this be sorted?",
///             "choices": [
///                 {
///                     "text": "Plastik",
///                     "textEn": "Plastic",
///                     "correctAnswer": true,
///                     "feedback": "Rigtigt!",
///                     "feedbackEn": "Correct!"
///                 }
///             ]
///         }
///     ],
///     "endMessage": "Godt klaret!",
///     "endMessageEn": "Well done!",
///     "instruction": "Tryk på den bedste løsning",
///     "instructionEn": "Press on the best solution"
/// }
/// </example>
public record UpdateQuizDraftRequest(
    string? Title,
    string? TitleEn,
    Guid? ExhibitionId,
    string? QuizLayout,
    List<UpdateQuestionDraftRequest>? Questions,
    string? EndMessage,
    string? EndMessageEn,
    string? Instruction,
    string? InstructionEn
    )
{
    /// <summary>
    /// The title of the quiz in Danish.
    /// </summary>
    public string? Title { get; init; } = Title;

    /// <summary>
    /// The title of the quiz in English.
    /// </summary>
    public string? TitleEn { get; init; } = TitleEn;

    /// <summary>
    /// A unique identifier for a exhibition the quiz associate to.
    /// </summary>
    public Guid? ExhibitionId { get; init; } = ExhibitionId;

    /// <summary>
    /// The layout of the quiz.
    /// </summary>
    public string? QuizLayout { get; init; } = QuizLayout;

    /// <summary>
    /// A list of the questions you want the quiz to have.
    /// </summary>
    public List<UpdateQuestionDraftRequest>? Questions { get; init; } = Questions;

    /// <summary>
    /// Message shown in Danish when the quiz is finished. Optional.
    /// </summary>
    public string? EndMessage { get; init; } = EndMessage;

    /// <summary>
    /// Message shown in English when the quiz is finished. Optional.
    /// </summary>
    public string? EndMessageEn { get; init; } = EndMessageEn;

    /// <summary>
    /// Instruction of the quiz shown in Danish
    /// </summary>
    public string? Instruction { get; init; } = Instruction;

    /// <summary>
    /// Instruction of the quiz shown in English
    /// </summary>
    public string? InstructionEn { get; init; } = InstructionEn;
}
