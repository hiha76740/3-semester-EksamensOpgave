namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="ExhibtionName"></param>
/// <param name="ExhibitionNameEn"></param>
/// <param name="ThemeImageId"></param>
/// <param name="SubjectDrafts"></param>
public record UpdateExhibitionDraftCommand(
    Guid Id,
    string? ExhibtionName,
    string? ExhibitionNameEn,
    Guid? ThemeImageId,
    List<UpdateSubjectDraftCommand> SubjectDrafts
    );
