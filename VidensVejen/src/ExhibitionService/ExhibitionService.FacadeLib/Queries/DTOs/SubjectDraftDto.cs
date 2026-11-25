namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Title"></param>
/// <param name="TitleEn"></param>
/// <param name="Slug"></param>
/// <param name="Description"></param>
/// <param name="DescriptionEn"></param>
/// <param name="Topics"></param>
public record SubjectDraftDto(
    Guid? Id,
    string? Title,
    string? TitleEn,
    string? Slug,
    string? Description,
    string? DescriptionEn,
    List<TopicDraftDto> Topics);
