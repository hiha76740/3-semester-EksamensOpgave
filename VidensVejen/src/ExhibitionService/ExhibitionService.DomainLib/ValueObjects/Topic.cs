using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
public record Topic
{
    /// <summary>
    /// 
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string TitleEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string DescriptionEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string BodyEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutLabel { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutLabelEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutText { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutTextEn { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Quote { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? QuoteEn { get; init; } = string.Empty;

    /// <summary>
    /// Gets the position of the topic in its subject.
    /// </summary>
    public int Position { get; private set; }


    internal Topic(
        string title,
        string titleEn,
        string description,
        string descriptionEn,
        string body,
        string bodyEn,
        string calloutLabel,
        string calloutLabelEn,
        string calloutText,
        string calloutTextEn,
        string? quote,
        string? quoteEn,
        int position
        )
    {

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Topic must have a title.");

        if (string.IsNullOrWhiteSpace(titleEn))
            throw new DomainException("Topic must have a english title.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Topic must have a description.");

        if (string.IsNullOrWhiteSpace(descriptionEn))
            throw new DomainException("Topic must have a english description.");

        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Topic must have a body.");

        if (string.IsNullOrWhiteSpace(bodyEn))
            throw new DomainException("Topic must have a english body.");

        if (string.IsNullOrWhiteSpace(calloutLabel))
            throw new DomainException("Topic must have a calloutLabel.");

        if (string.IsNullOrWhiteSpace(calloutLabelEn))
            throw new DomainException("Topic must have a english calloutLabel.");

        if (string.IsNullOrWhiteSpace(calloutText))
            throw new DomainException("Topic must have a calloutText.");

        if (string.IsNullOrWhiteSpace(calloutTextEn))
            throw new DomainException("Topic must have a english calloutText.");

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
        Position = position;
    }
}

