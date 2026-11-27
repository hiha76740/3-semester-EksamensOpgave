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
    public QuizId? QuizId { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string TitleEn { get; private set; } = string.Empty;


    /// <summary>
    /// 
    /// </summary>
    public string Slug { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string Description { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string DescriptionEn { get; private set; } = string.Empty;


    private readonly List<Topic> _topics = [];
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Topic> Topics => _topics.AsReadOnly();


    internal Subject(
        string title,
        string titleEn,
        string slug,
        string description,
        string descriptionEn,
        QuizId? quizId,
        List<NewTopic> topics
        )
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Subject must have a title.");

        if (string.IsNullOrWhiteSpace(titleEn))
            throw new DomainException("Subject must have a english title.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Subject must have a description.");

        if (string.IsNullOrWhiteSpace(descriptionEn))
            throw new DomainException("Subject must have a english description.");

        if (string.IsNullOrWhiteSpace(slug))
            throw new DomainException("Could not set slug, no value.");

        if ((quizId == null && topics == null) || topics.Count == 0)
            throw new DomainException("A subject must have at least one topic");

        Id = new SubjectId(Guid.NewGuid());
        Title = title;
        TitleEn = titleEn;
        Description = description;
        DescriptionEn = descriptionEn;
        Slug = slug;
        QuizId = quizId;

        foreach (var topic in topics)
        {
            AddTopic(topic);
        }
    }


    private void AddTopic(NewTopic newTopic)
    {
        var topic = new Topic(
            newTopic.Title,
            newTopic.TitleEn,
            newTopic.Description,
            newTopic.DescriptionEn,
            newTopic.Body,
            newTopic.BodyEn,
            newTopic.CalloutLabel,
            newTopic.CalloutLabelEn,
            newTopic.CalloutText,
            newTopic.CalloutTextEn,
            newTopic.Qoute,
            newTopic.QouteEn
            );

        _topics.Add(topic);
    }




    // EF constructor
    private Subject() { }

}
