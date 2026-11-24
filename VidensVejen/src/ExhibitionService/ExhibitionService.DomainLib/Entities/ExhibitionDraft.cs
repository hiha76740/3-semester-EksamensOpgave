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

    private readonly List<SubjectDraft> _subjectDrafts = [];


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ExhibitionDraftId Create()
    {
        var exhibitionDraft = new ExhibitionDraft();
        return exhibitionDraft.Id;
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

        foreach (var subjectDraft in subjectDrafts ?? [])
        {
            var subject = new SubjectDraft(
                subjectDraft.Title,
                subjectDraft.TitleEn,
                subjectDraft.Slug,
                subjectDraft.Description,
                subjectDraft.DescriptionEn,
                subjectDraft.Topics);

            _subjectDrafts.Add(subject);
        }
    }

    // EF constructor
    private ExhibitionDraft() { }
}
