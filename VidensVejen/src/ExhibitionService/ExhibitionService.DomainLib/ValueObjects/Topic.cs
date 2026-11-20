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
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Body { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutLabel { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutText { get; init; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Quote { get; init; } = string.Empty;


    internal Topic(string title, string description, string body, string calloutLabel, string calloutText, string? quote)
    {

        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Topic must have a title.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Topic must have a description.");

        if (string.IsNullOrWhiteSpace(body))
            throw new DomainException("Topic must have a body.");

        if (string.IsNullOrWhiteSpace(calloutLabel))
            throw new DomainException("Topic must have a calloutLabel.");

        if (string.IsNullOrWhiteSpace(calloutText))
            throw new DomainException("Topic must have a calloutText.");

        Title = title;
        Description = description;
        Body = body;
        CalloutLabel = calloutLabel;
        CalloutText = calloutText;
        Quote = quote;
    }
}

