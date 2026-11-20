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


    internal Subject(string title, string description, List<NewTopic> topics)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Subject must have a title.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Subject must have a description.");

        if (topics == null || topics.Count == 0)
            throw new DomainException("A subject must have at least one topic");

        Id = new SubjectId(Guid.NewGuid());
        Title = title;
        Description = description;

        foreach (var topic in topics)
        {
            AddTopic(topic);
        }
    }


    private void AddTopic(NewTopic newTopic)
    {
        var topic = new Topic(newTopic.Title, newTopic.Description, newTopic.Body, newTopic.CalloutLabel, newTopic.CalloutText, newTopic.Quote);

        _topics.Add(topic);
    }




    // EF constructor
    private Subject() { }

}
