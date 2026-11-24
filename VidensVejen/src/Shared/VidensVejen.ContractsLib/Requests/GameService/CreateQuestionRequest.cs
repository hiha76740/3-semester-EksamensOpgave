namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record CreateQuestionRequest(
    string? Text,
    string? TextEn,
    List<CreateChoiceRequest>? Choices
    )
{
    /// <summary>
    /// The text of the question in Danish.
    /// </summary>
    public string? Text { get; init; } = Text;

    /// <summary>
    /// The text of the question in English.
    /// </summary>
    public string? TextEn { get; init; } = TextEn;

    /// <summary>
    /// A list of the choices you want the question to have.
    /// </summary>
    public List<CreateChoiceRequest>? Choices { get; init; } = Choices;
}
