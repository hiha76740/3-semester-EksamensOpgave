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
    public string ExhibitionName { get; private set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    public string ThemeImage { get; private set; } = string.Empty;

    private readonly List<Subject> _subjects = [];
    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Subject> Subjects => _subjects.AsReadOnly();



    /// <summary>
    /// 
    /// </summary>
    /// <param name="title"></param>
    /// <param name="description"></param>
    /// <param name="topics"></param>
    public void AddSubject(
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
        var subject = Subject.Create(this, title, description, topics);

        _subjects.Add(subject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="exhibitionName"></param>
    /// <param name="themeImage"></param>
    /// <param name="subjects"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Exhibition Create(
    string exhibitionName,
    string themeImage,
    List<(
        string Title,
        string Description,
        List<(
            string Title,
            string Description,
            string Body,
            string CalloutLabel,
            string CalloutText,
            string? Quote)> Topics)> subjects)
    {
        if (string.IsNullOrWhiteSpace(exhibitionName))
            throw new DomainException("Exhibition must have a name.");

        if (string.IsNullOrWhiteSpace(themeImage))
            throw new DomainException("Exhibition must have a theme.");

        if (subjects.Count == 0)
        {
            throw new DomainException("An exhibition must have at least one subject");
        }

        var exhibition = new Exhibition(exhibitionName, themeImage);

        if (subjects.Count > 0)
        {
            foreach (var (title, description, topics) in subjects)
            {
                exhibition.AddSubject(title, description, topics);
            }
        }

        return exhibition;
    }

    private Exhibition(string exhibitionName, string themeImage)
    {
        Id = new ExhibitionId(Guid.NewGuid());
        ExhibitionName = exhibitionName;
        ThemeImage = themeImage;
    }

    // EF constructor
    private Exhibition() { }

}
