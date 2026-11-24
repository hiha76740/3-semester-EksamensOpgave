namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibtionName"></param>
/// <param name="ExhibitionNameEn"></param>
/// <param name="ThemeImageId"></param>
/// <param name="Subjects"></param>
public record CreateExhibitionCommand(
    string ExhibtionName,
    string ExhibitionNameEn,
    Guid ThemeImageId,
    List<CreateSubjectCommand> Subjects
    );
