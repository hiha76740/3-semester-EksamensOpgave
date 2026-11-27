namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="Id"></param>
/// <param name="ExhibtionName"></param>
/// <param name="ExhibitionNameEn"></param>
/// <param name="ThemeImageId"></param>
/// <param name="Subjects"></param>
/// <param name="DraftRowVersion"></param>
public record CreateExhibitionCommand(
    Guid Id,
    string ExhibtionName,
    string ExhibitionNameEn,
    Guid ThemeImageId,
    List<CreateSubjectCommand> Subjects,
    byte[] DraftRowVersion
    );
