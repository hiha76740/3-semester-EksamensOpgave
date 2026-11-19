using GameService.DomainLib.ValueObjects;
using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities;

/// <summary>
/// 
/// </summary>
public class Question
{
    /// <summary>
    /// 
    /// </summary>
    public QuestionId Id { get; init; } = null!;
    /// <summary>
    /// 
    /// </summary>
    public string Text { get; private set; } = string.Empty;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="choices"></param>
    /// <param name="correctFeedback"></param>
    /// <param name="incorrectFeedback"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    internal static Question Create(string text, List<Choice> choices, string? correctFeedback, string? incorrectFeedback)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("Question text could not be set, no value");

        if (choices == null || choices.Count < 2)
            throw new DomainException("A question must have at least two choices");

        if (choices.Count(c => c.IsCorrect) != 1)
            throw new DomainException("A question must have exactly one correct choice");

        return new Question(text, choices, correctFeedback, incorrectFeedback);
    }

    private Question(string text, List<Choice> choices, string? correctFeedback, string? incorrectFeedback)
    {
        Id = new QuestionId(Guid.NewGuid());
        Text = text;
        CorrectFeedback = correctFeedback;
        IncorrectFeedback = incorrectFeedback;
        _choices.AddRange(choices);
    }

    // EF Core constructor
    private Question() { }
}
