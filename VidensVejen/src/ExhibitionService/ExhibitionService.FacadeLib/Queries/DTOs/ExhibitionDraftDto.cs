namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="SubjectDrafts"></param>
/// <param name="ThemeImageId"></param>
public record ExhibitionDraftDto(
    Guid? Id,
    string? Name,
    IReadOnlyList<SubjectDto>? SubjectDrafts,
    Guid? ThemeImageId);
