using ExhibitionService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace ExhibitionService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class Exhibition
{
    /// <summary>
    /// 
    /// </summary>
    public ExhibitionId Id { get; init; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public string Name { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string NameEn { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public ImageId ThemeImageId { get; private set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Subject> Subjects => _subjects.AsReadOnly();

    private readonly List<Subject> _subjects = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="exhibitionName"></param>
    /// <param name="exhibitionNameEn"></param>
    /// <param name="themeImageId"></param>
    /// <param name="subjects"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Exhibition Create(
        ExhibitionId id,
        string exhibitionName,
        string exhibitionNameEn,
        ImageId themeImageId,
        List<NewSubject> subjects)
    {
        if (id == null)
            throw new DomainException("Could not set Id, no value");

        if (string.IsNullOrWhiteSpace(exhibitionName))
            throw new DomainException("Exhibition must have a name.");

        if (string.IsNullOrWhiteSpace(exhibitionNameEn))
            throw new DomainException("Exhibition must have a english name.");

        if (subjects.Count == 0)
            throw new DomainException("An exhibition must have at least one subject");

        var exhibition = new Exhibition(id, exhibitionName, exhibitionNameEn, themeImageId, subjects);

        return exhibition;
    }


    /// <summary>
    /// Applies validated publication content to this exhibition.
    /// </summary>
    public void ApplyRevision(Exhibition revision)
    {
        if (revision.Id != Id)
        {
            throw new DomainException("The revision belongs to a different exhibition.");
        }

        Name = revision.Name;
        NameEn = revision.NameEn;
        ThemeImageId = revision.ThemeImageId;
        _subjects.Clear();
        _subjects.AddRange(revision.Subjects);
    }

    private void AddSubject(NewSubject newSubject)
    {
        QuizId? quizId = null;

        if (newSubject.QuizId.HasValue)
        {
            quizId = new QuizId(newSubject.QuizId.Value);
        }

        var subject = new Subject(
            newSubject.Title,
            newSubject.TitleEn,
            newSubject.Slug,
            newSubject.Description,
            newSubject.DescriptionEn,
            quizId,
            _subjects.Count,
            newSubject.Topics
            );

        _subjects.Add(subject);
    }


    private Exhibition(ExhibitionId id, string exhibitionName, string exhibitionNameEn, ImageId themeImageId, List<NewSubject> subjects)
    {
        Id = id;
        Name = exhibitionName;
        NameEn = exhibitionNameEn;
        ThemeImageId = themeImageId;

        foreach (var subject in subjects)
        {
            AddSubject(subject);
        }
    }

    // EF constructor
    private Exhibition() { }
}
