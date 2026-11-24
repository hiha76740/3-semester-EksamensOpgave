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
    public string? CorrectFeedback { get; private set; }
    /// <summary>
    /// 
    /// </summary>
    public string? IncorrectFeedback { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<Choice> Choices => _choices.AsReadOnly();

    private readonly List<Choice> _choices = [];


    internal DraftQuestion(string? text, List<Choice>? choices, string? correctFeedback, string? incorrectFeedback)
    {
        Id = new QuestionId(Guid.NewGuid());
        Text = text;
        CorrectFeedback = correctFeedback;
        IncorrectFeedback = incorrectFeedback;

        if (choices != null)
            _choices.AddRange(choices);
    }

    internal NewQuestion AsNewQuestion()
    {
        return new NewQuestion(Text, [.. _choices], CorrectFeedback, IncorrectFeedback);
    }

    // EF Core constructor
    private DraftQuestion() { }
}
