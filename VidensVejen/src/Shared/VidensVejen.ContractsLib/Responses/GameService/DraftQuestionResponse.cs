namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record DraftQuestionResponse(
    Guid Id,
    string? Text,
    string? TextEn,
    List<ChoiceResponse> Choices
    )
{
    /// <summary>
    /// A unique identifier for the question.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The text of the question in Danish.
    /// </summary>
    public string? Text { get; init; } = Text;

    /// <summary>
    /// The text of the question in English.
    /// </summary>
    public string? TextEn { get; init; } = TextEn;

    /// <summary>
    /// The choices saved in the draft.
    /// </summary>
    public List<ChoiceResponse> Choices { get; init; } = Choices;
}
