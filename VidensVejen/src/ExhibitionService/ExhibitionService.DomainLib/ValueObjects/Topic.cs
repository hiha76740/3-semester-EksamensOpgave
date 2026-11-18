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
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Body { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutLabel { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string CalloutText { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string? Quote { get; private set; } = string.Empty;


    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="body"></param>
    /// <param name="calloutLabel"></param>
    /// <param name="calloutText"></param>
    /// <param name="quote"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    internal static Topic Create(
       string title,
       string description,
       string body,
       string calloutLabel,
       string calloutText,
       string? quote)
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

        return new Topic(title, description, body, calloutLabel, calloutText, quote);
    }
    private Topic(string title, string description, string body, string calloutLabel, string calloutText, string? quote)
    {
        Title = title;
        Description = description;
        Body = body;
        CalloutLabel = calloutLabel;
        CalloutText = calloutText;
        Quote = quote;
    }

    // EF constructor
    private Topic() { }
}

