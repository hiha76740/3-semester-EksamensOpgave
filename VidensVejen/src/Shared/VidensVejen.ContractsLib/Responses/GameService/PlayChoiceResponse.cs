namespace VidensVejen.ContractsLib.Responses.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Index"></param>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
public record PlayChoiceResponse(
    int Index,
    string Text,
    string? TextEn
    )
{
    /// <summary>
    /// The position of the choice in the question. Sent back when the guest answers.
    /// </summary>
    public int Index { get; init; } = Index;

    /// <summary>
    /// The text of the choice in Danish.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// The text of the choice in English.
    /// </summary>
    public string? TextEn { get; init; } = TextEn;
}
