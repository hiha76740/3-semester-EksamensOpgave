namespace ExhibitionService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
public record TopicDraft
{
    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? TitleEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? DescriptionEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Body { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? BodyEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? CalloutLabel { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? CalloutLabelEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? CalloutText { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? CalloutTextEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Quote { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public string? QuoteEn { get; init; } = string.Empty;


    internal TopicDraft(
        string? title,
        string? titleEn,
        string? description,
        string? descriptionEn,
        string? body,
        string? bodyEn,
        string? calloutLabel,
        string? calloutLabelEn,
        string? calloutText,
        string? calloutTextEn,
        string? quote,
        string? quoteEn)
    {
        Title = title;
        TitleEn = titleEn;
        Description = description;
        DescriptionEn = descriptionEn;
        Body = body;
        BodyEn = bodyEn;
        CalloutLabel = calloutLabel;
        CalloutLabelEn = calloutLabelEn;
        CalloutText = calloutText;
        CalloutTextEn = calloutTextEn;
        Quote = quote;
        QuoteEn = quoteEn;
    }
}
