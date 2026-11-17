namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="CorrectAnswer"></param>
public record CreateChoiceRequest(
    string Text,
    bool CorrectAnswer
    )
{
    /// <summary>
    /// The text you want the choice to have.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// Marks the choice as correct
    /// </summary>
    public bool CorrectAnswer { get; init; } = CorrectAnswer;
}
