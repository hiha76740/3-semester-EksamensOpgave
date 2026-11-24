namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="Choices"></param>
/// <param name="CorrectFeedback"></param>
/// <param name="IncorrectFeedback"></param>
public record CreateQuestionRequest(
    string? Text,
    List<CreateChoiceRequest>? Choices,
    string? CorrectFeedback,
    string? IncorrectFeedback
    )
{
    /// <summary>
    /// The text you want the choice to have.
    /// </summary>
    public string? Text { get; init; } = Text;

    /// <summary>
    /// A list of the choices you want the question to have.
    /// </summary>
    public List<CreateChoiceRequest>? Choices { get; init; } = Choices;

    /// <summary>
    /// Feedback shown when the guest answers correctly. Optional.
    /// </summary>
    public string? CorrectFeedback { get; init; } = CorrectFeedback;

    /// <summary>
    /// Feedback shown when the guest answers incorrectly. Optional.
    /// </summary>
    public string? IncorrectFeedback { get; init; } = IncorrectFeedback;
}
