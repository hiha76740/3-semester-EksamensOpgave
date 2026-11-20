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
    public ImageId? Theme { get; private set; }

    private readonly List<Subject> _subjects = [];
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Subject> Subjects => _subjects.AsReadOnly();


    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibitionName"></param>
    /// <param name="theme"></param>
    /// <param name="subjects"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Exhibition Create(
    string exhibitionName,
    Guid? theme,
    List<NewSubject> subjects)
    {
        if (string.IsNullOrWhiteSpace(exhibitionName))
            throw new DomainException("Exhibition must have a name.");

        if (subjects.Count == 0)
            throw new DomainException("An exhibition must have at least one subject");

        ImageId? imageId = null;

        if (theme != null)
            imageId = new ImageId(theme.Value);

        var exhibition = new Exhibition(exhibitionName, imageId, subjects);

        return exhibition;
    }


    private void AddSubject(NewSubject newSubject)
    {
        var subject = new Subject(newSubject.Title, newSubject.Description, newSubject.Topics);

        _subjects.Add(subject);
    }


    private Exhibition(string exhibitionName, ImageId? theme, List<NewSubject> subjects)
    {
        Id = new ExhibitionId(Guid.NewGuid());
        Name = exhibitionName;
        Theme = theme;

        foreach (var subject in subjects)
        {
            AddSubject(subject);
        }
    }

    // EF constructor
    private Exhibition() { }
}
