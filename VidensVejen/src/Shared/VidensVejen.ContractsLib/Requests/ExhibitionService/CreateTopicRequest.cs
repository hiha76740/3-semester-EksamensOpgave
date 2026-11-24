namespace VidensVejen.ContractsLib.Requests.ExhibitionService;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="Body"></param>
/// <param name="BodyEn"></param>
/// <param name="CalloutLabel"></param>
/// <param name="CalloutLabelEn"></param>
/// <param name="CalloutText"></param>
/// <param name="CalloutTextEn"></param>
/// <param name="Qoute"></param>
/// <param name="QouteEn"></param>
public record CreateTopicRequest(
    string Title,
    string TitleEn,
    string Description,
    string DescriptionEn,
    string Body,
    string BodyEn,
    string CalloutLabel,
    string CalloutLabelEn,
    string CalloutText,
    string CalloutTextEn,
    string? Qoute,
    string? QouteEn
    )
{
    /// <summary>
    /// The title of the topic.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The english title of the topic.
    /// </summary>
    public string TitleEn { get; init; } = TitleEn;

    /// <summary>
    /// The description of the topic.
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// The english description of the topic.
    /// </summary>
    public string DescriptionEn { get; init; } = DescriptionEn;

    /// <summary>
    /// The text body of the topic.
    /// </summary>
    public string Body { get; init; } = Body;

    /// <summary>
    /// The text english body of the topic.
    /// </summary>
    public string BodyEn { get; init; } = BodyEn;

    /// <summary>
    /// The first text in the highlighted box.
    /// </summary>
    public string CalloutLabel { get; init; } = CalloutLabel;

    /// <summary>
    /// The english first text in the highlighted box.
    /// </summary>
    public string CalloutLabelEn { get; init; } = CalloutLabelEn;

    /// <summary>
    /// The text in the highlighted box.
    /// </summary>
    public string CalloutText { get; init; } = CalloutText;

    /// <summary>
    /// The english text in the highlighted box.
    /// </summary>
    public string CalloutTextEn { get; init; } = CalloutTextEn;
    /// <summary>
    /// The qoute of the mascot
    /// </summary>
    public string? Qoute { get; init; } = Qoute;

    /// <summary>
    /// The english qoute of the mascot
    /// </summary>
    public string? QouteEn { get; init; } = QouteEn;

}
