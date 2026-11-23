namespace GameService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="Choices"></param>
/// <param name="CorrectFeedback"></param>
/// <param name="IncorrectFeedback"></param>
public record NewQuestion(string? Text, List<Choice>? Choices, string? CorrectFeedback, string? IncorrectFeedback);
