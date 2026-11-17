namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="Description"></param>
/// <param name="Body"></param>
/// <param name="CalloutLabel"></param>
/// <param name="CalloutText"></param>
/// <param name="Quote"></param>
public record TopicResponse(
    Guid Id,
    string Title,
    string Description,
    string Body,
    string CalloutLabel,
    string CalloutText,
    string Quote
    )
{
    /// <summary>
    /// A unique identifier for a topic.
    /// </summary>
    public Guid Id { get; init; } = Id;

    /// <summary>
    /// The title of the topic.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The description of the topic.
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// The text body of the topic.
    /// </summary>
    public string Body { get; init; } = Body;

    /// <summary>
    /// The first text in the highlighted box.
    /// </summary>
    public string CalloutLabel { get; init; } = CalloutLabel;

    /// <summary>
    /// The text in the highlighted box.
    /// </summary>
    public string CalloutText { get; init; } = CalloutText;

    /// <summary>
    /// The quote of the mascot
    /// </summary>
    public string Quote { get; init; } = Quote;
}
