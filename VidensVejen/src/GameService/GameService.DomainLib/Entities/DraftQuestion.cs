using GameService.DomainLib.ValueObjects;

namespace GameService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class DraftQuestion
{
    /// <summary>
    /// 
    /// </summary>
    public QuestionId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string? Text { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? TextEn { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Choice> Choices => _choices.AsReadOnly();

    private readonly List<Choice> _choices = [];


    internal DraftQuestion(string? text, string? textEn, List<Choice>? choices)
    {
        Id = new QuestionId(Guid.NewGuid());
        Text = text;
        TextEn = textEn;

        if (choices != null)
            _choices.AddRange(choices);
    }

    // EF Core constructor
    private DraftQuestion() { }
}
