namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Index"></param>
/// <param name="Text"></param>
public record PlayChoiceResponse(
    int Index,
    string Text
    )
{
    /// <summary>
    /// The position of the choice in the question. Sent back when the guest answers.
    /// </summary>
    public int Index { get; init; } = Index;

    /// <summary>
    /// The text of the choice.
    /// </summary>
    public string Text { get; init; } = Text;
}
