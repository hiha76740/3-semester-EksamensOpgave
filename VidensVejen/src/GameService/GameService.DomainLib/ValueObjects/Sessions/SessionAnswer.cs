namespace GameService.DomainLib.ValueObjects.Sessions;

/// <summary>
/// 
/// </summary>
/// <param name="QuestionId"></param>
/// <param name="ChoiceIndex"></param>
/// <param name="IsCorrect"></param>
public record SessionAnswer(Guid QuestionId, int ChoiceIndex, bool IsCorrect);
