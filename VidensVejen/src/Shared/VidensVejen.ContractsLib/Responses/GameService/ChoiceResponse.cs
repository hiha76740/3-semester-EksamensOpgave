namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Text"></param>
/// <param name="CorrectAnswer"></param>
public record ChoiceResponse(
    Guid Id,
    string Text,
    bool CorrectAnswer
    )
{
    /// <summary>
    /// A unique identifier for the choice.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The text you want the choice to have.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// Marks the choice as correct
    /// </summary>
    public bool CorrectAnswer { get; init; } = CorrectAnswer;
}
