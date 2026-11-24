namespace ExhibitionService.DomainLib.ValueObjects;

/// <summary>
/// 
/// </summary>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="Topics"></param>
public record NewSubjectDraft(
    string? Title,
    string? TitleEn,
    string? Slug,
    string? Description,
    string? DescriptionEn,
    List<NewTopicDraft>? Topics);
