namespace ExhibitionService.FacadeLib.Commands.DTOs;

/// <summary>
/// 
/// </summary>
/// <param name="ExhibtionName"></param>
/// <param name="ImageStream"></param>
/// <param name="Extension"></param>
/// <param name="Subjects"></param>
public record CreateExhibitionCommand(
    string ExhibtionName,
    Stream ImageStream,
    string Extension,
    List<CreateSubjectCommand> Subjects
    );
