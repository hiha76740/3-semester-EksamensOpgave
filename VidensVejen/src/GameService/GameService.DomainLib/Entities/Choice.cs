using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.Entities;

/// <summary>
///
/// </summary>
public class Choice
{
    /// <summary>
    ///
    /// </summary>
    public ChoiceId Id { get; init; } = null!;

    /// <summary>
    ///
    /// </summary>
    public string Text { get; private set; } = string.Empty;

    /// <summary>
    ///
    /// </summary>
    public bool IsCorrect { get; private set; }

    /// <summary>
    ///
    /// </summary>
    /// <param name="text"></param>
    /// <param name="isCorrect"></param>
    /// <returns></returns>
    /// <exception cref="DomainException"></exception>
    public static Choice Create(string text, bool isCorrect)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("Choice text could not be set, no value");

        return new Choice(text, isCorrect);
    }

    private Choice(string text, bool isCorrect)
    {
        Id = new ChoiceId(Guid.NewGuid());
        Text = text;
        IsCorrect = isCorrect;
    }

    // EF Core constructor
    private Choice() { }
}
