namespace GameService.DomainLib.ValueObjects.Drafts;

/// <summary>
/// 
/// </summary>
/// <param name="Text"></param>
/// <param name="TextEn"></param>
/// <param name="Choices"></param>
public record NewQuestionDraft(string? Text, string? TextEn, List<ChoiceDraft>? Choices);
