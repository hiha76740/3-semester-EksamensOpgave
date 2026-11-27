namespace VidensVejen.ContractsLib.Responses.ExhibitionService;

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
/// <param name="Quote"></param>
/// <param name="QuoteEn"></param>
public record TopicResponse(
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
    string? Quote,
    string? QuoteEn
    )
{
    /// <summary>
    /// The title of the topic shown in Danish.
    /// </summary>
    public string Title { get; init; } = Title;

    /// <summary>
    /// The title of the topic shown in English.
    /// </summary>
    public string TitleEn { get; init; } = TitleEn;

    /// <summary>
    /// The description of the topic shown in Danish.
    /// </summary>
    public string Description { get; init; } = Description;

    /// <summary>
    /// The description of the topic shown in English.
    /// </summary>
    public string DescriptionEn { get; init; } = DescriptionEn;

    /// <summary>
    /// The text body of the topic shown in Danish.
    /// </summary>
    public string Body { get; init; } = Body;

    /// <summary>
    /// The text body of the topic shown in English.
    /// </summary>
    public string BodyEn { get; init; } = BodyEn;

    /// <summary>
    /// The first text in the highlighted box shown in Danish.
    /// </summary>
    public string CalloutLabel { get; init; } = CalloutLabel;

    /// <summary>
    /// The first text in the highlighted box shown in English.
    /// </summary>
    public string CalloutLabelEn { get; init; } = CalloutLabelEn;

    /// <summary>
    /// The text in the highlighted box shwon in Danish.
    /// </summary>
    public string CalloutText { get; init; } = CalloutText;

    /// <summary>
    /// The text in the highlighted box shwon in English.
    /// </summary>
    public string CalloutTextEn { get; init; } = CalloutTextEn;

    /// <summary>
    /// The quote of the mascot shown in Danish
    /// </summary>
    public string? Quote { get; init; } = Quote;

    /// <summary>
    /// The quote of the mascot shown in English
    /// </summary>
    public string? QuoteEn { get; init; } = QuoteEn;
}
