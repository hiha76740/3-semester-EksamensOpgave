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


    /// <summary>
    /// Gets the published quiz identifier.
    /// </summary>
    public QuizId? QuizId { get; private set; }

    /// <summary>
    /// Gets the quiz draft identifier.
    /// </summary>
    public QuizDraftId? QuizDraftId { get; private set; }

    /// <summary>
    /// Gets whether the subject is a quiz.
    /// </summary>
    public bool IsQuiz { get; private set; }

    /// <summary>
    /// Gets the position of the subject in its exhibition draft.
    /// </summary>
    public int Position { get; private set; }

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
        QuizId? quizId,
        QuizDraftId? quizDraftId,
        bool isQuiz,
        int position,
        List<NewTopicDraft>? topicDrafts)
    {

        Id = new SubjectDraftId(Guid.NewGuid());
        Title = title;
        TitleEn = titleEn;
        Slug = slug;
        Description = description;
        DescriptionEn = descriptionEn;
        QuizId = quizId;
        QuizDraftId = quizDraftId;
        IsQuiz = isQuiz;
        Position = position;

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
                topicDraft.QuoteEn,
                _topicDrafts.Count
                );

            _topicDrafts.Add(topic);
        }
    }


    // EF constructor
    private SubjectDraft() { }
}
