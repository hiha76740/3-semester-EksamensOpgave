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


    internal Question(string? text, string? textEn, List<Choice>? choices)
    {
        Id = new QuestionId(Guid.NewGuid());
        Text = text;
        TextEn = textEn;

        if (choices != null)
            _choices.AddRange(choices);
    }

    internal void EnsureCanBePublished()
    {
        if (string.IsNullOrWhiteSpace(Text))
            throw new DomainException("Question text could not be set, no value");

        if (_choices.Count < 2)
            throw new DomainException("A question must have at least two choices");

        if (_choices.Count(c => c.IsCorrect) != 1)
            throw new DomainException("A question must have exactly one correct choice");
    }

    // EF Core constructor
    private Question() { }
}
