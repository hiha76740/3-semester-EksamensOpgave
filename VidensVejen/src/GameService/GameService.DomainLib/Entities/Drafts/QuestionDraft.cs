using GameService.DomainLib.ValueObjects.Drafts;

namespace GameService.DomainLib.Entities.Drafts;

/// <summary>
/// 
/// </summary>
public class QuestionDraft
{
    /// <summary>
    /// 
    /// </summary>
    public QuestionDraftId Id { get; init; } = null!;
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
    public IReadOnlyList<ChoiceDraft> Choices => _choices.AsReadOnly();

    private readonly List<ChoiceDraft> _choices = [];


    internal QuestionDraft(string? text, string? textEn, List<ChoiceDraft>? choices)
    {
        Id = new QuestionDraftId(Guid.NewGuid());
        Text = text;
        TextEn = textEn;

        if (choices != null)
            _choices.AddRange(choices);
    }

    // EF Core constructor
    private QuestionDraft() { }
}
