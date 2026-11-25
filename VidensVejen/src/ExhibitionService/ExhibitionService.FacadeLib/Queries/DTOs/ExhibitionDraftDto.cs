namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="NameEn"></param>
/// <param name="SubjectDrafts"></param>
/// <param name="ThemeImageId"></param>
public record ExhibitionDraftDto(
    Guid? Id,
    string? Name,
    string? NameEn,
    IReadOnlyList<SubjectDraftDto> SubjectDrafts,
    Guid? ThemeImageId);
