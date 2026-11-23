namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="CorrectFeedback"></param>
/// <param name="IncorrectFeedback"></param>
/// <param name="Choices"></param>
public record QuestionResponse(
    Guid Id,
    string? Text,
    string? CorrectFeedback,
    string? IncorrectFeedback,
    List<ChoiceResponse> Choices
    )
{
    /// <summary>
    /// A unique identifier for the question.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The text of the question.
    /// </summary>
    public string? Text { get; init; } = Text;

    /// <summary>
    /// Feedback shown when the guest answers correctly.
    /// </summary>
    public string? CorrectFeedback { get; init; } = CorrectFeedback;

    /// <summary>
    /// Feedback shown when the guest answers incorrectly.
    /// </summary>
    public string? IncorrectFeedback { get; init; } = IncorrectFeedback;

    /// <summary>
    /// A list of the choices associated to the question.
    /// </summary>
    public List<ChoiceResponse> Choices { get; init; } = Choices;
}
