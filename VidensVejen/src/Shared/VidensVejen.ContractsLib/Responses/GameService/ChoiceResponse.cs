namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="CorrectAnswer"></param>
public record ChoiceResponse(
    string Text,
    bool CorrectAnswer
    )
{
    /// <summary>
    /// The text of the choice.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// Marks the choice as correct
    /// </summary>
    public bool CorrectAnswer { get; init; } = CorrectAnswer;
}
