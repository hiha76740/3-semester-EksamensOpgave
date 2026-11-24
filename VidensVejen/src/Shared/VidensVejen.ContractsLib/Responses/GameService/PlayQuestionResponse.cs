namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="Choices"></param>
public record PlayQuestionResponse(
    Guid Id,
    string? Text,
    List<PlayChoiceResponse> Choices
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
    /// The choices the guest can pick from. The correct answer is not included.
    /// </summary>
    public List<PlayChoiceResponse> Choices { get; init; } = Choices;
}
