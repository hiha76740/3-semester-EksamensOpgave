using ExhibitionService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class Subject
{
    /// <summary>
    /// 
    /// </summary>
    public SubjectId Id { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public Exhibition Exhibition { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Title { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; private set; } = string.Empty;


    private readonly List<Topic> _topics = [];
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Topic> Topics => _topics.AsReadOnly();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="body"></param>
    /// <param name="calloutLabel"></param>
    /// <param name="calloutText"></param>
    /// <param name="quote"></param>
    internal void AddTopic(
        string title,
        string description,
        string body,
        string calloutLabel,
        string calloutText,
        string? quote)
    {
        var topic = Topic.Create(title, description, body, calloutLabel, calloutText, quote);

        _topics.Add(topic);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibition"></param>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="topics"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    internal static Subject Create(
       Exhibition exhibition,
       string title,
       string description,
       List<(
           string Title,
           string Description,
           string Body,
           string CalloutLabel,
           string CalloutText,
           string? Quote)> topics)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Subject must have a title.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Subject must have a description.");

        if (topics.Count == 0)
        {
            throw new DomainException("A subject must have at least one topic");
        }

        var subject = new Subject(exhibition, title, description);

        foreach (var topic in topics)
        {
            subject.AddTopic(
                topic.Title,
                topic.Description,
                topic.Body,
                topic.CalloutLabel,
                topic.CalloutText,
                topic.Quote);
        }
        return subject;
    }


    private Subject(Exhibition exhibition, string title, string description)
    {
        Id = new SubjectId(Guid.NewGuid());
        Exhibition = exhibition;
        Title = title;
        Description = description;
    }

    // EF constructor
    private Subject() { }

}
