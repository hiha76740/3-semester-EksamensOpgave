using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class SubjectDraft
{
    /// <summary>
    /// 
    /// </summary>
    public SubjectDraftId Id { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string? Title { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? TitleEn { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Slug { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? Description { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? DescriptionEn { get; private set; }


    private readonly List<TopicDraft> _topicDrafts = [];
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<TopicDraft> TopicDrafts => _topicDrafts.AsReadOnly();


    internal SubjectDraft(
        string? title,
        string? titleEn,
        string? slug,
        string? description,
        string? descriptionEn,
        List<NewTopicDraft>? topicDrafts)
    {

        Id = new SubjectDraftId(Guid.NewGuid());
        Title = title;
        TitleEn = titleEn;
        Description = description;
        DescriptionEn = descriptionEn;
        Slug = slug;

        foreach (var topicDraft in topicDrafts ?? [])
        {
            var topic = new TopicDraft(
                topicDraft.Title,
                topicDraft.TitleEn,
                topicDraft.Description,
                topicDraft.DescriptionEn,
                topicDraft.Body,
                topicDraft.BodyEn,
                topicDraft.CalloutLabel,
                topicDraft.CalloutLabelEn,
                topicDraft.CalloutText,
                topicDraft.CalloutTextEn,
                topicDraft.Quote,
                topicDraft.QuoteEn);

            _topicDrafts.Add(topic);
        }
    }


    // EF constructor
    private SubjectDraft() { }
}
