using ExhibitionService.DomainLib.ValueObjects;

namespace ExhibitionService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class ExhibitionDraft
{
    /// <summary>
    /// 
    /// </summary>
    public ExhibitionDraftId Id { get; init; } = new ExhibitionDraftId(Guid.NewGuid());

    /// <summary>
    /// 
    /// </summary>
    public string? Name { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public string? NameEn { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public ImageId? ThemeImageId { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<SubjectDraft> SubjectDrafts => _subjectDrafts.AsReadOnly();

    /// <summary>
    /// Gets the published exhibition being edited, when present.
    /// </summary>
    public ExhibitionId? SourceExhibitionId { get; private set; }

    /// <summary>
    /// Creates a draft linked to the published exhibition it will update.
    /// </summary>
    public static ExhibitionDraft CreateRevision(ExhibitionId sourceId)
    {
        return new ExhibitionDraft { SourceExhibitionId = sourceId };
    }

    private readonly List<SubjectDraft> _subjectDrafts = [];


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ExhibitionDraft Create()
    {
        var exhibitionDraft = new ExhibitionDraft();
        return exhibitionDraft;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibitionName"></param>
    /// <param name="exhibitionNameEn"></param>
    /// <param name="themeImageId"></param>
    /// <param name="subjectDrafts"></param>
    public void UpdateExhibitionDraft(
        string? exhibitionName,
        string? exhibitionNameEn,
        ImageId? themeImageId,
        List<NewSubjectDraft>? subjectDrafts)
    {
        Name = exhibitionName;
        NameEn = exhibitionNameEn;
        ThemeImageId = themeImageId;

        _subjectDrafts.Clear();

        foreach (var subjectDraft in subjectDrafts ?? [])
        {
            QuizId? quizId = null;
            QuizDraftId? quizDraftId = null;

            if (subjectDraft.QuizId.HasValue)
                quizId = new QuizId(subjectDraft.QuizId.Value);

            if (subjectDraft.QuizDraftId.HasValue)
                quizDraftId = new QuizDraftId(subjectDraft.QuizDraftId.Value);

            var subject = new SubjectDraft(
                subjectDraft.Title,
                subjectDraft.TitleEn,
                subjectDraft.Slug,
                subjectDraft.Description,
                subjectDraft.DescriptionEn,
                quizId,
                quizDraftId,
                subjectDraft.IsQuiz,
                _subjectDrafts.Count,
                subjectDraft.Topics
                );

            _subjectDrafts.Add(subject);
        }
    }

    // EF constructor
    private ExhibitionDraft() { }
}
