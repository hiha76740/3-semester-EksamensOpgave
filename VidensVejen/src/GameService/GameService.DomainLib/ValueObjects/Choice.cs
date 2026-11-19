using VidensVejen.SharedKernelLib.Exceptions;

namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
public record Choice
{
    /// <summary>
    /// 
    /// </summary>
    public string Text { get; init; }
    /// <summary>
    /// 
    /// </summary>
    public bool IsCorrect { get; init; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="text"></param>
    /// <param name="isCorrect"></param>
    /// <exception cref="DomainException"></exception>
    public Choice(string text, bool isCorrect)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new DomainException("Choice text could not be set, no value");

        Text = text;
        IsCorrect = isCorrect;
    }
}
