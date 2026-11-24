namespace ExhibitionService.FacadeLib.Queries.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="Name"></param>
/// <param name="Subjects"></param>
/// <param name="ThemeImageId"></param>
public record ExhibitionDto(
    Guid Id,
    string Name,
    IReadOnlyList<SubjectDto> Subjects,
    Guid ThemeImageId
    );
