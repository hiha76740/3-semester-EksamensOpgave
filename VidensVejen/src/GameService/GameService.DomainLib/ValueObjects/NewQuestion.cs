namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record NewQuestion(string? Text, string? TextEn, List<Choice>? Choices);
