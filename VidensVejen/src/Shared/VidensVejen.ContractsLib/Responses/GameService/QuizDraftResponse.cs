namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="ExhibitionId"></param>
/// <param name="QuizLayout"></param>
/// <param name="EndMessage"></param>
/// <param name="EndMessageEn"></param>
/// <param name="Questions"></param>
/// <param name="Instruction"></param>
/// <param name="InstructionEn"></param>
/// <param name="RowVersion"></param>
public record QuizDraftResponse(
    Guid Id,
    string? Title,
    string? TitleEn,
    Guid? ExhibitionId,
    string? QuizLayout,
    string? EndMessage,
    string? EndMessageEn,
    List<DraftQuestionResponse> Questions,
    string? Instruction,
    string? InstructionEn,
    byte[] RowVersion
    )
{
    /// <summary>
    /// A unique identifier for the quiz draft.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the quiz in Danish.
    /// </summary>
    public string? Title { get; init; } = Title;

    /// <summary>
    /// The title of the quiz in English.
    /// </summary>
    public string? TitleEn { get; init; } = TitleEn;

    /// <summary>
    /// A unique identifier for the exhibition the quiz is associated to.
    /// </summary>
    public Guid? ExhibitionId { get; init; } = ExhibitionId;

    /// <summary>
    /// The layout of the quiz.
    /// </summary>
    public string? QuizLayout { get; init; } = QuizLayout;

    /// <summary>
    /// Message shown in Danish when the quiz is finished.
    /// </summary>
    public string? EndMessage { get; init; } = EndMessage;

    /// <summary>
    /// Message shown in English when the quiz is finished.
    /// </summary>
    public string? EndMessageEn { get; init; } = EndMessageEn;

    /// <summary>
    /// The questions saved in the draft.
    /// </summary>
    public List<DraftQuestionResponse> Questions { get; init; } = Questions;

    /// <summary>
    /// Instruction shown in Danish.
    /// </summary>
    public string? Instruction { get; init; } = Instruction;

    /// <summary>
    /// Instruction shown in English.
    /// </summary>
    public string? InstructionEn { get; init; } = InstructionEn;

    /// <summary>
    /// The row version returned when the user was last retrieved.
    /// </summary>
    public byte[] RowVersion { get; init; } = RowVersion;
}
