namespace VidensVejen.ContractsLib.Requests.GameService;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="Choices"></param>
public record CreateQuestionRequest(
    string Text,
    List<CreateChoiceRequest> Choices
    )
{
    /// <summary>
    /// The text you want the choice to have.
    /// </summary>
    public string Text { get; init; } = Text;

    /// <summary>
    /// A list of the choices you want the question to have.
    /// </summary>
    public List<CreateChoiceRequest> Choices { get; init; } = Choices;
}
